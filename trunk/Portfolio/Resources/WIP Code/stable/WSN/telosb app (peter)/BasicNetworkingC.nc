#include <Timer.h>
#include <UserButton.h>
#include "printf.h"
#include "BasicNetworking.h"

module BasicNetworkingC {
  uses {
    // initialization
    interface Boot;
	interface SplitControl as RadioControl;
	interface SplitControl as SerialControl;
	interface StdControl as RoutingControl;
	interface StdControl as DisseminationControl; // BroadcastControl ???
	
	// Dissemination
	interface DisseminationUpdate<DissMsg_t> as RequestUpdate; // for root
	interface DisseminationValue<DissMsg_t> as RequestValue; // for motes
	
	// Collection
	interface Send as CollectSend; // to send data to root
	interface Receive as Snoop; // forwarding
	interface Receive as CollectReceive; // data acquired by root
	interface RootControl; // denote root
    
	// serial
	interface AMSend as SerialSend;
	interface Receive as SerialReceive;
	interface Packet as UartPacket;
	
	// sensors
	interface Read<uint16_t> as ReadLight;
	interface Read<uint16_t> as ReadTemp;
	interface Read<uint16_t> as ReadHumidity;
	
	// pushbutton
	interface Notify<button_state_t>;
	
	// memory
	interface Queue<message_t *> as UartQueue; // FIFO
	interface Pool<message_t> as UartPool; // get & put
	interface Queue<message_t *> as RadioQueue; // FIFO
	interface Pool<message_t> as RadioPool; // get & put
	
	// printf
	interface SplitControl as PrintfControl;
	interface PrintfFlush;

	// other
	interface Timer<TMilli>;
	interface Timer<TMilli> as BroadcastTimer;
	interface Leds;
	
	//AN receive & RSSI
	interface Receive;
	interface CC2420Packet;
	
	//AN Transmit
	interface AMSend;
	
	interface Packet as RadioPacket;
  }
}
implementation {
  //declaration of functions
  void status();
  task void RadioSendTask();
  
  static void ReportProblem();
  static void ReportSent();
  static void ReportReceive();
  static void FatalProblem();
  
  // variables for proces
  uint16_t Alpha = 0;
  uint16_t MaxNumMotes = MAXIMUM_NUMBER_MOTES;
  uint16_t WaitS = 0;
  
  //Lenght to determine the type of packets
  uint8_t UartLen;
  uint8_t RadioLen;
  
  //message buffers
  message_t FwdMsg, SndMsg, UartBuf;
  
  //check radio status
  bool FwdBusy, SendBusy, UartBusy, TaskBusy;
  
  //root or not?
  bool Root = FALSE;
  
  // bools for data
  bool LightIsRead = FALSE;
  bool TempIsRead = FALSE;
  bool HumidityIsRead = FALSE;
  bool ButtonIsPressed = FALSE;
  
  //Message structs
  CollMsg_t LocalCollMsg;
  LocMsg_t LocalLocMsg;
  StatMsg_t LocalStatMsg;
  broadcast_t local;
  
  // reports
  static void ReportProblem() { call Leds.led0Toggle(); }
  static void ReportSent() { call Leds.led1Toggle(); }
  static void ReportReceive() { call Leds.led2Toggle(); }
  static void FatalProblem() { call Leds.led0Toggle();
							   call Leds.led1Toggle();
							   call Leds.led2Toggle();
							   call Timer.stop(); }
  
  //OK
  // initialize radio, serial, network
  event void Boot.booted() {
  
    //initiate other data: Philippe
	//collection message
    LocalCollMsg.moteid = TOS_NODE_ID;
	LocalCollMsg.lightreading = 0;
	LocalCollMsg.tempreading = 0;
	LocalCollMsg.humidityreading = 0;
	LocalCollMsg.button = FALSE;
	LocalCollMsg.type = 1;
	LocalCollMsg.battery = 0;
	
	LocalStatMsg.moteid = TOS_NODE_ID;
	LocalStatMsg.type = 3;
	LocalStatMsg.active = FALSE;
	LocalStatMsg.AN = TRUE;
	LocalStatMsg.posx = 0;
	LocalStatMsg.posy = 0;
	LocalStatMsg.sampleRate = DEFAULT_SAMPLING_PERIOD;
	LocalStatMsg.locRate = 1000;
	LocalStatMsg.leds = 0;
	LocalStatMsg.power = 0;
	LocalStatMsg.frequency = 0;
	LocalStatMsg.conn = 0;
	
	LocalLocMsg.moteid = TOS_NODE_ID;
	LocalLocMsg.ANmoteid = TOS_NODE_ID;
	LocalLocMsg.type = 2;
	LocalLocMsg.posx = 0;
	LocalLocMsg.posy = 0;
	LocalLocMsg.RSSI = 0;
	
	SendBusy = FALSE;
	UartBusy = FALSE;
	
	call Leds.led0Off();
	call Leds.led1Off();
	call Leds.led2Off();
	
	if (call RadioControl.start() != SUCCESS)
	  FatalProblem();
	
	//Routing for Collection protocol
	if (call RoutingControl.start() != SUCCESS)
	  FatalProblem();
	  
	//Routing for Dissemination protocol
	if (call DisseminationControl.start() != SUCCESS)
	  FatalProblem();
	
	if (TOS_NODE_ID == 0) { // if mote is root
	  Root = TRUE;
	  if (call SerialControl.start() != SUCCESS)
	    FatalProblem();
	}
	else { // other motes active pushbutton
	  call Notify.enable();
	}
	call PrintfControl.start();
  }
  
  //OK
  event void RadioControl.startDone(error_t err) {
    if (err != SUCCESS)
	  FatalProblem();
	else {
	  if (Root)
	    call RootControl.setRoot();
		//stop running Timers
      if (call BroadcastTimer.isRunning())
	    call BroadcastTimer.stop();
      if (call Timer.isRunning())
	    call Timer.stop();
		//start the sensor timer
	  call Timer.startPeriodic(LocalStatMsg.sampleRate);
	}
  }
  
  //OK
  event void RadioControl.stopDone(error_t err) { }
  
  //OK
  event void SerialControl.startDone(error_t err) {
    if (err != SUCCESS)
	  FatalProblem();
  }
  
  event void PrintfFlush.flushDone(error_t error){ }
  event void PrintfControl.startDone(error_t error) { }
  event void PrintfControl.stopDone(error_t error) {}
  
  //OK
  event void SerialControl.stopDone(error_t err) { }
  
  //OK
  // pushbutton
  event void Notify.notify(button_state_t state) {
    if (state == BUTTON_PRESSED)
	  LocalCollMsg.button = TRUE;
  }
 

  void Status(){
    	if (!Root) {
	  
	    StatMsg_t *out;
		if (!TaskBusy) {
		  out = (StatMsg_t *)call CollectSend.getPayload(&SndMsg);
		  memcpy(out, &LocalStatMsg, sizeof(StatMsg_t));
		  post RadioSendTask();
		  TaskBusy = TRUE;
		}
		else {
		  message_t *NewMsg = call RadioPool.get();
		  if (NewMsg == NULL)
		    ReportProblem();
		  out = (StatMsg_t *)call CollectSend.getPayload(NewMsg);
		  memcpy(out, &LocalStatMsg, sizeof(StatMsg_t));
		  if (call RadioQueue.enqueue(NewMsg) != SUCCESS) {
		    call RadioPool.put(NewMsg);
			FatalProblem();
		  }
		}
	  }
  } 
  //Works, no real functionality yet
  // action after dissemination
  void ProcessRequest(DissMsg_t *NewRequest) {
    // if target mote or broadcast to all motes
	// if ((NewRequest->targetid == TOS_NODE_ID) || (NewRequest->targetid == 0xFFFF)) {
	  // switch (NewRequest->request) {
	  
	    // case SAMPLING_PERIOD:
		  // //LocalStatMsg.samplerate = NewRequest->parameter;
		  // //call Timer.stop();
		  // //Alpha = SamplingPeriod/(MaxNumMotes+1);
		  // //WaitS = (1+TOS_NODE_ID)*Alpha;
		  // //call WaitTimer.startOneShot(WaitS);
		  // status();
		  // break;
		  
		// //case LOC_PERIOD:
			// //LocalStatMsg.locRate = NewRequest->parameter;
			// //status();
			// //break:
			
		// case ACTIVE_REQUEST:
			// LocalStatMsg.active = NewRequest->parameter;
			// status();
			// break;
			
		// case AN_REQUEST:
			// LocalStatMsg.AN = NewRequest->parameter;
			// status();
			// break;
		
		// case LED_REQUEST:
		  // call Leds.set(NewRequest->parameter);
		  // LocalStatMsg.leds = NewRequest->parameter;
		  // status();
		  // break;
		  
		// case SLEEP_REQUEST:
		  // status();
		  // break;
		 
		// case POWER_REQUEST:
			// LocalStatMsg.power = NewRequest->parameter;
			// status();
			// break;
			
		// case FREQUENCY_REQUEST:
			// LocalStatMsg.frequency = NewRequest->parameter;
			// status();
			// break;
			
		// case POSX_REQUEST:
			// LocalStatMsg.posx = NewRequest->parameter;
			// status();
			// break;
			
		// case POSY_REQUEST:
			// LocalStatMsg.posy = NewRequest->parameter;
			// status();
			// break;
		
		// case STATUS_REQUEST:
			// status();
			// break;
			
		// default:
		  // break;
	  // }
	// }
  }
  

  
  // when root receives requests to disseminate
  event message_t* SerialReceive.receive(message_t* Msg, void* Payload, uint8_t Length) {
	DissMsg_t *NewRequest = Payload;
	if (Length == sizeof(DissMsg_t))
	   call RequestUpdate.change(NewRequest);
	return Msg;
  }
  
  // TASK: Motes pass on msg to root
  task void RadioSendTask() {
    if (!Root && !SendBusy) {
		RadioLen = call RadioPacket.payloadLength(&SndMsg);
	
	  if (call CollectSend.send(&SndMsg, RadioLen) == SUCCESS){
	    SendBusy = TRUE;
		TaskBusy = FALSE;
		}
      else
	    ReportProblem();
	}
	LocalCollMsg.button = FALSE;
  }
  
  // TASK: root sends data over serial port
  task void SerialSendTask() { 
	
	UartLen = call UartPacket.payloadLength(&UartBuf);
	
	if (call SerialSend.send(0xFFFF, &UartBuf, UartLen) == SUCCESS)
	  UartBusy = TRUE;
	else
	  ReportProblem();
  }
  
  // when done with passing on msg
  event void CollectSend.sendDone(message_t* Msg, error_t err) {
	if (err != SUCCESS)
	  ReportProblem();
	  
	SendBusy = FALSE; // Just finished CollectSend, if radioqueue is not empty
	if (call RadioQueue.empty() == FALSE) {
	  message_t *QueueMsg = call RadioQueue.dequeue();
	  if (QueueMsg == NULL) {
	    FatalProblem();
		return;
	  }
	  memcpy(&SndMsg, QueueMsg, sizeof(message_t));
	  if (call RadioPool.put(QueueMsg) != SUCCESS) {
	    FatalProblem();
		return;
	  }
	  post RadioSendTask();
	}
	ReportSent();
  }
  
  // when done forwarding data over serial port (root only)
  event void SerialSend.sendDone(message_t* Msg, error_t err) {
	UartBusy = FALSE; //Just finished SerialSend, if uartqueue is not empty
	if (call UartQueue.empty() == FALSE) {
	  message_t *QueueMsg = call UartQueue.dequeue();
	  if (QueueMsg == NULL) {
	    FatalProblem();
		return;
	  }
	  memcpy(&UartBuf, QueueMsg, sizeof(message_t));
	  if (call UartPool.put(QueueMsg) != SUCCESS) {
	    FatalProblem();
		return;
	  }
	  post SerialSendTask();
	}
	ReportSent();
  }
  
  // when dissemination passed around a message
  event void RequestValue.changed() {
    DissMsg_t *NewRequest = (DissMsg_t *)call RequestValue.get();
	ProcessRequest(NewRequest);
  }
  
  event void Timer.fired() {
  
    if (call ReadLight.read() == SUCCESS)
	  LightIsRead = TRUE;
	if (call ReadTemp.read() == SUCCESS)
	  TempIsRead = TRUE;
	if (call ReadHumidity.read() == SUCCESS)
      HumidityIsRead = TRUE;
	  
	// when all measurements are done
	if (LightIsRead && TempIsRead && HumidityIsRead) {
	  if (!Root) {
	  
	    CollMsg_t *out;
		if (!TaskBusy) {
		  out = (CollMsg_t *)call CollectSend.getPayload(&SndMsg);
		  memcpy(out, &LocalCollMsg, sizeof(CollMsg_t));
		  post RadioSendTask();
		  TaskBusy = TRUE;
		}
		else {
		  message_t *NewMsg = call RadioPool.get();
		  if (NewMsg == NULL)
		    ReportProblem();
		  out = (CollMsg_t *)call CollectSend.getPayload(NewMsg);
		  memcpy(out, &LocalCollMsg, sizeof(CollMsg_t));
		  if (call RadioQueue.enqueue(NewMsg) != SUCCESS) {
		    call RadioPool.put(NewMsg);
			FatalProblem();
		  }
		}
	  }
	}
  }
  
  // when sensor has been read
  event void ReadLight.readDone(error_t err, uint16_t reading) {
    if (err != SUCCESS) {
	  reading = 0xFFFF;
	  ReportProblem();
	}
	else {
	  LocalCollMsg.lightreading = reading;
	}    
  }
  
  // when sensor has been read
  event void ReadTemp.readDone(error_t err, uint16_t reading) {
    if (err != SUCCESS) {
	  reading = 0xFFFF;
	  ReportProblem();
	}
	else {
	  LocalCollMsg.tempreading = reading;
	}
  }

  // when sensor has been read
  event void ReadHumidity.readDone(error_t err, uint16_t reading) {
    if (err != SUCCESS) {
	  reading = 0xFFFF;
	  ReportProblem();
	}
	else {
	  LocalCollMsg.humidityreading = reading;
	}
  }  
  
  // when receiving collected messages, forward over serial port
  event message_t* CollectReceive.receive(message_t* Msg, void* Payload, uint8_t Length) {

    int packetType = 0;
	CollMsg_t* inColl; 
	CollMsg_t* outColl;
	StatMsg_t* inStat; 
	StatMsg_t* outStat;
	LocMsg_t* inLoc;
    LocMsg_t* outLoc;
  
	if ( Length == sizeof(CollMsg_t)){
		inColl = (CollMsg_t*)Payload;
		packetType = 1;
	}
	else if ( Length == sizeof(StatMsg_t)){
		inStat = (StatMsg_t*)Payload;
		packetType = 2;
	}
	else if ( Length == sizeof(LocMsg_t)){
		inLoc = (LocMsg_t*)Payload;
		packetType = 3;
	}

	if (UartBusy == FALSE){
		if (packetType == 1){
		outColl = (CollMsg_t*)call SerialSend.getPayload(&UartBuf);

		memcpy(outColl, inColl, sizeof(CollMsg_t));
		}
		else if (packetType == 2){
		outStat = (StatMsg_t*)call SerialSend.getPayload(&UartBuf);

		memcpy(outStat, inStat, sizeof(StatMsg_t));
		}
		else if (packetType == 3){
		outLoc = (LocMsg_t*)call SerialSend.getPayload(&UartBuf);

		memcpy(outLoc, inLoc, sizeof(LocMsg_t));
		}
		UartLen = Length;
		post SerialSendTask();
	}
	else {
	  // when busy, queue up
	  message_t *NewMsg = call UartPool.get();
	  if (NewMsg == NULL) {
	    ReportProblem();
		return Msg;
	  }
		if (packetType == 1){
		outColl = (CollMsg_t*)call SerialSend.getPayload(&UartBuf);

		memcpy(outColl, inColl, sizeof(CollMsg_t));
		}
		else if (packetType == 2){
		outStat = (StatMsg_t*)call SerialSend.getPayload(&UartBuf);

		memcpy(outStat, inStat, sizeof(StatMsg_t));
		}
		else if (packetType == 3){
		outLoc = (LocMsg_t*)call SerialSend.getPayload(&UartBuf);

		memcpy(outLoc, inLoc, sizeof(LocMsg_t));
		}
	  if (call UartQueue.enqueue(NewMsg) != SUCCESS) {
	    call UartPool.put(NewMsg);
		FatalProblem();
		return Msg;
	  }
	}
	return Msg;
  }
  
  //when receiving a broadcasted message
  event message_t* Receive.receive(message_t* msg, void* payload, uint8_t len) {
  broadcast_t *omsg = payload;
  LocalLocMsg.ANmoteid = omsg->id;
  LocalLocMsg.RSSI = call CC2420Packet.getRssi(msg);
  LocalLocMsg.posx = 100;
  LocalLocMsg.posy = 100;
  
  	  if (!Root && LocalStatMsg.AN) {
	  
	    LocMsg_t *out;
		if (!TaskBusy) {
		  out = (LocMsg_t *)call CollectSend.getPayload(&SndMsg);
		  memcpy(out, &LocalLocMsg, sizeof(LocMsg_t));
		  post RadioSendTask();
		  TaskBusy = TRUE;
		}
		else {
		  message_t *NewMsg = call RadioPool.get();
		  if (NewMsg == NULL)
		    ReportProblem();
		  out = (LocMsg_t *)call CollectSend.getPayload(NewMsg);
		  memcpy(out, &LocalLocMsg, sizeof(LocMsg_t));
		  if (call RadioQueue.enqueue(NewMsg) != SUCCESS) {
		    call RadioPool.put(NewMsg);
			FatalProblem();
		  }
		}
	  }
  
  return msg;
  }
  
  event void BroadcastTimer.fired() {
	if (!SendBusy && sizeof local <= call AMSend.maxPayloadLength())
	  {
	    memcpy(call AMSend.getPayload(&SndMsg), &local, sizeof local);
	    if (call AMSend.send(AM_BROADCAST_ADDR, &SndMsg, sizeof local) == SUCCESS)
	      SendBusy = TRUE;
	  }
	if (!SendBusy)
	  ReportProblem();
  }

  event void AMSend.sendDone(message_t* msg, error_t error) {
    if (error == SUCCESS)
      ReportSent();
    else
      ReportProblem();

    SendBusy = FALSE;
  }
  
  // when forwarding
  event message_t* Snoop.receive(message_t* Msg, void* Payload, uint8_t Length) {
	return Msg;
  }


}