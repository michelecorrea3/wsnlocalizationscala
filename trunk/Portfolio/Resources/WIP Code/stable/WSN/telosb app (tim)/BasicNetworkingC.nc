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
	interface Read<uint16_t> as ReadBattery;
	
	// pushbutton
	interface Notify<button_state_t>;
	
	// memory
	interface Queue<message_t *> as UartQueueC; // FIFO
	interface Pool<message_t> as UartPoolC; // get & put
		
	interface Queue<message_t *> as UartQueueL; // FIFO
	interface Pool<message_t> as UartPoolL; // get & put
	
	interface Queue<message_t *> as UartQueueS; // FIFO
	interface Pool<message_t> as UartPoolS; // get & put
	
	interface Queue<message_t *> as RadioQueueC; // FIFO
	interface Pool<message_t> as RadioPoolC; // get & put
	
	interface Queue<message_t *> as RadioQueueL; // FIFO
	interface Pool<message_t> as RadioPoolL; // get & put
	
	interface Queue<message_t *> as RadioQueueS; // FIFO
	interface Pool<message_t> as RadioPoolS; // get & put
	
	// printf
	interface SplitControl as PrintfControl;
	interface PrintfFlush;

	// other
	interface Timer<TMilli>;
	interface Timer<TMilli> as BroadcastTimer;
	interface Leds;
	
	//AN receive & RSSI

	interface CC2420Packet;
	
	//Broadcast
	interface AMSend as BroadcastSend;
	interface Receive as BroadcastReceive;
	
	//Other
	interface Packet as RadioPacket;
  }
}
implementation {
  //declaration of functions
  void status();
  task void RadioSendTaskL();
  task void RadioSendTaskC();
  task void RadioSendTaskS();
  task void BroadcastTask();
  
  task void SerialSendTaskL();
  task void SerialSendTaskC();
  task void SerialSendTaskS();
  
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
  message_t SndMsgC, SndMsgL, SndMsgS, SndMsgB, UartBufC, UartBufL, UartBufS;
  
  //check radio status
  bool FwdBusy, SendBusy, UartBusy, TaskBusy;
  
  //root or not?
  bool Root = FALSE;
  
  // bools for data
  bool LightIsRead = FALSE;
  bool TempIsRead = FALSE;
  bool HumidityIsRead = FALSE;
  bool ButtonIsPressed = FALSE;
  bool VoltageIsRead = FALSE;
  
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
  event void Boot.booted() 
  {
  
    //initiate other data: Philippe
	//collection message
    LocalCollMsg.moteid = TOS_NODE_ID;
	LocalCollMsg.lightreading = 0;
	LocalCollMsg.tempreading = 0;
	LocalCollMsg.humidityreading = 0;
	LocalCollMsg.button = FALSE;
	LocalCollMsg.type = 1;
	LocalCollMsg.battery = 0;
	
	//Status message
	LocalStatMsg.moteid = TOS_NODE_ID;
	LocalStatMsg.type = 3;
	LocalStatMsg.active = FALSE;
	LocalStatMsg.AN = FALSE;
	LocalStatMsg.posx = 0;
	LocalStatMsg.posy = 0;
	LocalStatMsg.sampleRate = DEFAULT_SAMPLING_PERIOD;
	LocalStatMsg.locRate = 1000;
	LocalStatMsg.leds = 0;
	LocalStatMsg.power = 0;
	LocalStatMsg.frequency = 0;
	LocalStatMsg.conn = 0;
	if(TOS_NODE_ID == 2)
	{
	LocalStatMsg.AN = TRUE;
	//call BroadcastTimer.startPeriodic(10000);
	}
	
	
	
	//Localization message
	LocalLocMsg.moteid = TOS_NODE_ID;
	LocalLocMsg.ANmoteid = TOS_NODE_ID;
	LocalLocMsg.type = 2;
	LocalLocMsg.posx = 0;
	LocalLocMsg.posy = 0;
	LocalLocMsg.RSSI = 0;
	
	//Broadcast message
	local.id = TOS_NODE_ID;
	local.posx = 13;
	local.posy = 37;
	
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
	
	if (TOS_NODE_ID == 0) 
	{ // if mote is root
	  Root = TRUE;
	  if (call SerialControl.start() != SUCCESS)
	    FatalProblem();
	}
	else 
	{ // other motes active pushbutton
	  call Notify.enable();
	}
	call PrintfControl.start();
  }
  
  //OK
  event void RadioControl.startDone(error_t err) {
    if (err != SUCCESS)
	  FatalProblem();
	else 
	{
	  if (Root)
	    call RootControl.setRoot();
		//stop running Timers
      if (call BroadcastTimer.isRunning())
	    call BroadcastTimer.stop();
      if (call Timer.isRunning())
	    call Timer.stop();
		//start the sensor timer
	call Timer.startPeriodic(LocalStatMsg.sampleRate); //sensoren
	//  call BroadcastTimer.startPeriodic(LocalStatMsg.locRate);
	call BroadcastTimer.startPeriodic(10000);
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
 

  void Status()
  {
    	if (!Root) 
		{
		StatMsg_t *out;
			if (!TaskBusy) 
			{
			  out = (StatMsg_t *)call CollectSend.getPayload(&SndMsgS);
			  memcpy(out, &LocalStatMsg, sizeof(StatMsg_t));
			  
				printf("Status(): !root && !Taskbusy\n");
				call PrintfFlush.flush();
			  
			  TaskBusy = TRUE;
			  post RadioSendTaskS();
			}
			else 
			{
			  message_t *NewMsg = call RadioPoolS.get();
			  if (NewMsg == NULL)
				ReportProblem();
			  out = (StatMsg_t *)call CollectSend.getPayload(NewMsg);
			  memcpy(out, &LocalStatMsg, sizeof(StatMsg_t));
			  if (call RadioQueueS.enqueue(NewMsg) != SUCCESS) {
				call RadioPoolS.put(NewMsg);
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
	
			printf("SerialReceive.Receive\n");
			call PrintfFlush.flush();
	
	if (Length == sizeof(DissMsg_t))
	   call RequestUpdate.change(NewRequest);
	return Msg;
  }
  
  
  // when done with passing on msg
  event void CollectSend.sendDone(message_t* Msg, error_t err) {
	if (err == SUCCESS)
	{
	  	  printf("CollectSend.sendDone == SUCCESS\n");
		  call PrintfFlush.flush();
	}
	else
	ReportProblem();
	  
	SendBusy = FALSE; // Just finished CollectSend, if radioqueue is not empty
	if (call RadioQueueL.empty() == FALSE) 
	{
	  message_t *QueueMsg = call RadioQueueL.dequeue();
	  if (QueueMsg == NULL) {
	    FatalProblem();
		return;
	  }
	  memcpy(&SndMsgL, QueueMsg, sizeof(LocMsg_t));
	  if (call RadioPoolL.put(QueueMsg) != SUCCESS) {
	    FatalProblem();
		return;
	  }
	  	printf("RadioSendTaskL van de radioQueue\n");
		  call PrintfFlush.flush();	
	  
	  post RadioSendTaskL();
	}
	
	if (call RadioQueueC.empty() == FALSE) 
	{
	  message_t *QueueMsg = call RadioQueueC.dequeue();
	  if (QueueMsg == NULL) {
	    FatalProblem();
		return;
	  }
	  memcpy(&SndMsgC, QueueMsg, sizeof(CollMsg_t));
	  if (call RadioPoolC.put(QueueMsg) != SUCCESS) {
	    FatalProblem();
		return;
	  }
	  	printf("RadioSendTaskC van radioqueue\n");
		call PrintfFlush.flush();	
	  post RadioSendTaskC();
	}
	
	if (call RadioQueueS.empty() == FALSE) 
	{
	  message_t *QueueMsg = call RadioQueueS.dequeue();
	  if (QueueMsg == NULL) {
	    FatalProblem();
		return;
	  }
	  memcpy(&SndMsgS, QueueMsg, sizeof(StatMsg_t));
	  if (call RadioPoolS.put(QueueMsg) != SUCCESS) {
	    FatalProblem();
		return;
	  }
	  	printf("RadioSendTaskS van radioqueue\n");
		call PrintfFlush.flush();	
	  
	  post RadioSendTaskS();
	}
	
	ReportSent();
  }
  
  // when done forwarding data over serial port (root only)
  event void SerialSend.sendDone(message_t* Msg, error_t err) {
  	if (err == SUCCESS)
	{
			ReportReceive();
	  	  printf("SerialSend.sendDone == SUCCESS\n");
		  //call PrintfFlush.flush();
	}
	else
	ReportProblem();
	UartBusy = FALSE; //Just finished SerialSend, if uartqueue is not empty
	if (call UartQueueL.empty() == FALSE) {
	  message_t *QueueMsg = call UartQueueL.dequeue();
	  if (QueueMsg == NULL) {
	    FatalProblem();
		return;
	  }
	  memcpy(&UartBufL, QueueMsg, sizeof(LocMsg_t));
	  if (call UartPoolL.put(QueueMsg) != SUCCESS) {
	    FatalProblem();
		return;
	  }
	  post SerialSendTaskL();
	}
	if (call UartQueueC.empty() == FALSE) {
	  message_t *QueueMsg = call UartQueueC.dequeue();
	  if (QueueMsg == NULL) {
	    FatalProblem();
		return;
	  }
	  memcpy(&UartBufC, QueueMsg, sizeof(CollMsg_t));
	  if (call UartPoolC.put(QueueMsg) != SUCCESS) {
	    FatalProblem();
		return;
	  }
	  post SerialSendTaskC();
	}
	
	if (call UartQueueS.empty() == FALSE) {
	  message_t *QueueMsg = call UartQueueS.dequeue();
	  if (QueueMsg == NULL) {
	    FatalProblem();
		return;
	  }
	  memcpy(&UartBufS, QueueMsg, sizeof(StatMsg_t));
	  if (call UartPoolS.put(QueueMsg) != SUCCESS) {
	    FatalProblem();
		return;
	  }
	  post SerialSendTaskS();
	}
	
	ReportSent();
	call PrintfFlush.flush();
  }
  
  // when dissemination passed around a message
  event void RequestValue.changed() {
    DissMsg_t *NewRequest = (DissMsg_t *)call RequestValue.get();
	ProcessRequest(NewRequest);
  }
  
    task void RadioSendTaskS() 
	{
    if (!Root && !SendBusy) 
	{
		//RadioLen = call RadioPacket.payloadLength(&SndMsg);
		
		if (call CollectSend.send(&SndMsgS, sizeof(StatMsg_t)) == SUCCESS)
		  {
			SendBusy = TRUE;
			TaskBusy = FALSE;			
			printf("StatMsg wordt verzonden\n");
			call PrintfFlush.flush();
		  }
		else
			ReportProblem();
	}
	//LocalCollMsg.button = FALSE;
  }
	
	task void RadioSendTaskC() 
	{
    if (!Root && !SendBusy) 
	{
		//RadioLen = call RadioPacket.payloadLength(&SndMsg);
		
		if (call CollectSend.send(&SndMsgC, sizeof(CollMsg_t)) == SUCCESS)
		  {
			SendBusy = TRUE;
			TaskBusy = FALSE;			
			//printf("CollMsg wordt verzonden\n");
			//call PrintfFlush.flush();
			  //ReportReceive();
		  }
		else
			ReportProblem();
	}
	LocalCollMsg.button = FALSE;
  }
  
  event void Timer.fired() {
  
	  printf("Timer.fired\n");
	  call PrintfFlush.flush();		
	  
    if (call ReadLight.read() == SUCCESS)
	  LightIsRead = TRUE;
	if (call ReadTemp.read() == SUCCESS)
	  TempIsRead = TRUE;
	if (call ReadHumidity.read() == SUCCESS)
      HumidityIsRead = TRUE;
	if (call ReadBattery.read() == SUCCESS)
      VoltageIsRead = TRUE;
	  
	// when all measurements are done
	if (LightIsRead && TempIsRead && HumidityIsRead && VoltageIsRead) {
	  if (!Root) {
	  
		CollMsg_t *out;
	    if (!SendBusy) 
		{
		  out = (CollMsg_t *)call CollectSend.getPayload(&SndMsgC);
		  memcpy(out, &LocalCollMsg, sizeof(CollMsg_t));
		  
		  //printf("RadioSendTaskC\n");
		  //call PrintfFlush.flush();		
		  
		  TaskBusy = TRUE;
		  post RadioSendTaskC();

		}
		else 
		{
		  message_t *NewMsg = call RadioPoolC.get();
		  if (NewMsg == NULL)
		    ReportProblem();
		  out = (CollMsg_t *)call CollectSend.getPayload(NewMsg);
		  memcpy(out, &LocalCollMsg, sizeof(CollMsg_t));
		  if (call RadioQueueC.enqueue(NewMsg) != SUCCESS) 
		  {
		    call RadioPoolC.put(NewMsg);
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
	else 
	{
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
  
    // when sensor has been read
  event void ReadBattery.readDone(error_t err, uint16_t reading) {
    if (err != SUCCESS) {
	  reading = 0xFFFF;
	  ReportProblem();
	}
	else 
	{
	  LocalCollMsg.battery = reading;
	}
  } 
  
    task void RadioSendTaskL() 
	{
    if (!Root && !SendBusy) 
	{
		//RadioLen = call RadioPacket.payloadLength(&SndMsg);
		
		if (call CollectSend.send(&SndMsgL, sizeof(LocMsg_t)) == SUCCESS)
		  {
			SendBusy = TRUE;
			TaskBusy = FALSE;			
			printf("LocMsg wordt verzonden\n");
			call PrintfFlush.flush();
		  }
		else
			ReportProblem();
	}
	//LocalCollMsg.button = FALSE;
  }
  
    task void SerialSendTaskC() { 
	
	//UartLen = call UartPacket.payloadLength(&UartBuf);
	UartLen = sizeof(CollMsg_t);
	if (call SerialSend.send(0xFFFF, &UartBufC, UartLen) == SUCCESS)
	{
		UartBusy = TRUE;
		printf("SerialSend van CollMsg \n");
		call PrintfFlush.flush();
	}
	else
	  ReportProblem();
  }
  
    task void SerialSendTaskL() { 
	
	UartLen = sizeof(LocMsg_t);
	if (call SerialSend.send(0xFFFF, &UartBufL, UartLen) == SUCCESS)
	{
		UartBusy = TRUE;
		printf("SerialSend van LocMsg \n");
		call PrintfFlush.flush();
	}
	else
	  ReportProblem();
  }
  
    task void SerialSendTaskS() 
	{ 
	
	UartLen = sizeof(StatMsg_t);
	if (call SerialSend.send(0xFFFF, &UartBufS, UartLen) == SUCCESS)
	{
		UartBusy = TRUE;
		printf("SerialSend van StatMsg \n");
		call PrintfFlush.flush();
	}
	else
	  ReportProblem();
  }
  
  /*
  // when receiving collected messages, forward over serial port
  event message_t* CollectReceive.receive(message_t* Msg, void* Payload, uint8_t Length) {

    int packetType = 0;
	CollMsg_t* inColl; 
	CollMsg_t* outColl;
	StatMsg_t* inStat; 
	StatMsg_t* outStat;
	LocMsg_t* inLoc;
    LocMsg_t* outLoc;
	
		printf("CollectReceive.receive:\n");
		
	
	if ( Length == sizeof(CollMsg_t))
	{
		inColl = (CollMsg_t*)Payload;
		printf("Length of CollMsg\n");
		printf("ID is: %u\n", inColl->moteid);
		packetType = 1;
	}
	else if ( Length == sizeof(StatMsg_t))
	{
		inStat = (StatMsg_t*)Payload;
		printf("Length of StatMsg\n");
		printf("ID is: %u\n", inStat->moteid);
		packetType = 2;
	}
	else if ( Length == sizeof(LocMsg_t))
	{
		inLoc = (LocMsg_t*)Payload;
		printf("Length of StatMsg\n");
		printf("ID is: %u\n", inLoc->moteid);
		packetType = 3;
	}

	if (UartBusy == FALSE)
	{
		//printf("SerialSendTask\n");
		//call PrintfFlush.flush();
		
		if (packetType == 1)
		{
			outColl = (CollMsg_t*)call SerialSend.getPayload(&UartBufC);

			//memcpy(outColl, inColl, sizeof(CollMsg_t));
			printf("SerialSendTaskC: UartBusy == FALSE\n");
			//call PrintfFlush.flush();
			
			UartBusy = TRUE;
			post SerialSendTaskC();

		}
		else if (packetType == 2)
		{
			outStat = (StatMsg_t*)call SerialSend.getPayload(&UartBufS);
			//memcpy(outStat, inStat, sizeof(StatMsg_t));
			
			printf("SerialSendTaskS: UartBusy == FALSE\n");
			//call PrintfFlush.flush();
			
			UartBusy = TRUE;
			post SerialSendTaskS();

		}
		else if (packetType == 3)
		{
			outLoc = (LocMsg_t*)call SerialSend.getPayload(&UartBufL);
			//memcpy(outLoc, inLoc, sizeof(LocMsg_t));
			
			printf("SerialSendTaskL: UartBusy == FALSE\n");
			//call PrintfFlush.flush();
			
			UartBusy = TRUE;
			post SerialSendTaskL();
		}
		else
		  ReportProblem();
	}
	else 
	{

		//	printf("CollecteREceive: UartBusy == TRUE\n");
		//	call PrintfFlush.flush();
	
	  // when busy, queue up
	  message_t *NewMsg;
	  if (packetType == 1)
	  NewMsg = call UartPoolC.get();
	  else if (packetType == 2)
	  NewMsg = call UartPoolS.get();
	  else if (packetType == 3)
	  NewMsg = call UartPoolL.get();
	  else
		  ReportProblem();
	  
	  if (NewMsg == NULL) 
	  {
	    ReportProblem();
		return Msg;
	  }
	  
		if (packetType == 1)
		{
		outColl = (CollMsg_t*)call SerialSend.getPayload(&UartBufC);
		memcpy(outColl, inColl, sizeof(CollMsg_t));
		}
		else if (packetType == 2)
		{
		outStat = (StatMsg_t*)call SerialSend.getPayload(&UartBufS);
		memcpy(outStat, inStat, sizeof(StatMsg_t));
		}
		else if (packetType == 3)
		{
		outLoc = (LocMsg_t*)call SerialSend.getPayload(&UartBufL);

		memcpy(outLoc, inLoc, sizeof(LocMsg_t));
		}
		else
		  ReportProblem();
		

	if (packetType == 1)
	{
		if (call UartQueueC.enqueue(NewMsg) != SUCCESS) 
		{
		call UartPoolC.put(NewMsg);
		FatalProblem();
		return Msg;
		}
	}
	else if (packetType == 2)
	{
		if (call UartQueueS.enqueue(NewMsg) != SUCCESS) 
		{
		call UartPoolS.put(NewMsg);
		FatalProblem();
		return Msg;
		}
	}
	else if (packetType == 3)
	{
		if (call UartQueueL.enqueue(NewMsg) != SUCCESS) 
		{
		call UartPoolL.put(NewMsg);
		FatalProblem();
		return Msg;
		}
	}
	else
	  ReportProblem();

	  
	}
	call PrintfFlush.flush();
	return Msg;
		
  }
  */
  
    event message_t* CollectReceive.receive(message_t* Msg, void* Payload, uint8_t Length) {

    //int packetType = 0;
	CollMsg_t* inColl; 
	CollMsg_t* outColl;
	StatMsg_t* inStat; 
	StatMsg_t* outStat;
	LocMsg_t* inLoc;
    LocMsg_t* outLoc;
	
		printf("CollectReceive.receive:\n");
		
	
	if ( Length == sizeof(CollMsg_t))
	{
		inColl = (CollMsg_t*)Payload;
		printf("Length of CollMsg\n");
		printf("ID is: %u\n", inColl->moteid);
		
		if (UartBusy == FALSE)
		{
			outColl = (CollMsg_t*)call SerialSend.getPayload(&UartBufC);

			memcpy(outColl, inColl, sizeof(CollMsg_t));
			
			post SerialSendTaskC();
		}
		else 
		{
		  // when busy, queue up
		  message_t *NewMsg = call UartPoolC.get();
		  if (NewMsg == NULL) 
		  {
			ReportProblem();
			return Msg;
		  }
		  outColl = (CollMsg_t*)call SerialSend.getPayload(NewMsg);
		  memcpy(outColl, inColl, sizeof(CollMsg_t));
		  if (call UartQueueC.enqueue(NewMsg) != SUCCESS) {
			call UartPoolC.put(NewMsg);
			FatalProblem();
			return Msg;
		  }
		}

	}
	else if ( Length == sizeof(StatMsg_t))
	{
		inStat = (StatMsg_t*)Payload;
		printf("Length of StatMsg\n");
		printf("ID is: %u\n", inStat->moteid);
		
		if (UartBusy == FALSE)
		{
			outStat = (StatMsg_t*)call SerialSend.getPayload(&UartBufS);

			memcpy(outStat, inStat, sizeof(StatMsg_t));
			
			post SerialSendTaskS();
		}
		else 
		{
		  // when busy, queue up
		  message_t *NewMsg = call UartPoolS.get();
		  if (NewMsg == NULL) 
		  {
			ReportProblem();
			return Msg;
		  }
		  outStat = (StatMsg_t*)call SerialSend.getPayload(NewMsg);
		  memcpy(outStat, inStat, sizeof(StatMsg_t));
		  if (call UartQueueS.enqueue(NewMsg) != SUCCESS) {
			call UartPoolS.put(NewMsg);
			FatalProblem();
			return Msg;
		  }
		}

	}
	else if ( Length == sizeof(LocMsg_t))
	{
		inLoc = (LocMsg_t*)Payload;
		printf("Length of LocMsg\n");
		printf("ID is: %u\n", inLoc->moteid);
		
		if (UartBusy == FALSE)
		{
			outLoc = (LocMsg_t*)call SerialSend.getPayload(&UartBufL);

			memcpy(outLoc, inLoc, sizeof(LocMsg_t));
			
			post SerialSendTaskL();
		}
		else 
		{
		  // when busy, queue up
		  message_t *NewMsg = call UartPoolS.get();
		  if (NewMsg == NULL) 
		  {
			ReportProblem();
			return Msg;
		  }
		  outLoc = (LocMsg_t*)call SerialSend.getPayload(NewMsg);
		  memcpy(outLoc, inLoc, sizeof(LocMsg_t));
		  if (call UartQueueL.enqueue(NewMsg) != SUCCESS) {
			call UartPoolL.put(NewMsg);
			FatalProblem();
			return Msg;
		  }
		}

	}


	call PrintfFlush.flush();
	return Msg;
		
  }
  
  //when receiving a broadcasted message
  event message_t* BroadcastReceive.receive(message_t* msg, void* payload, uint8_t len) {
  
  //FatalProblem();
  
  broadcast_t *omsg = (broadcast_t*)payload;
  LocalLocMsg.ANmoteid = omsg->id;
  LocalLocMsg.RSSI = call CC2420Packet.getRssi(msg);
  LocalLocMsg.posx = omsg->posx;
  LocalLocMsg.posy = omsg->posy;

	printf("x: %u\n",LocalLocMsg.posx);
	call PrintfFlush.flush();
  
  		  //printf("BroadcastReceive.receive\n");
		  //call PrintfFlush.flush();
  
		//alleen blindnode verzend location message
  	  if (!Root && !LocalStatMsg.AN) 
	  {
		LocMsg_t *out;
	    if (!SendBusy) 
		{
		  out = (LocMsg_t *)call CollectSend.getPayload(&SndMsgL);
		  memcpy(out, &LocalLocMsg, sizeof(LocMsg_t));
		  TaskBusy = TRUE;
		  post RadioSendTaskL();
		  //TaskBusy = TRUE;
		}
		else 
		{
		  message_t *NewMsg = call RadioPoolL.get();
		  if (NewMsg == NULL)
		    ReportProblem();
		  out = (LocMsg_t *)call CollectSend.getPayload(NewMsg);
		  memcpy(out, &LocalLocMsg, sizeof(LocMsg_t));
		  if (call RadioQueueL.enqueue(NewMsg) != SUCCESS) 
		  {
		    call RadioPoolL.put(NewMsg);
			FatalProblem();
		  }
		}
	  }
  
  return msg;
  }
  
  task void BroadcastTask()
  {
		if (call BroadcastSend.send(AM_BROADCAST_ADDR, &SndMsgB, sizeof(broadcast_t)) == SUCCESS)
		{
		  SendBusy = TRUE;
		  TaskBusy = FALSE;
		  printf("BroadcastSend.send == SUCCESS\n");
		  call PrintfFlush.flush();
			  //FatalProblem();
		}
		else 
			ReportProblem();

  }
  
  event void BroadcastTimer.fired() {
	//if (!SendBusy && sizeof local <= call BroadcastSend.maxPayloadLength())
		printf("BroadcastTimer.fired\n");
		  call PrintfFlush.flush();	
		  ReportReceive();
	if (!SendBusy && !Root && LocalStatMsg.AN)
	  {
	  	broadcast_t * o = (broadcast_t *)call BroadcastSend.getPayload(&SndMsgB);
			
		memcpy(o, &local, sizeof(broadcast_t));
	  
		TaskBusy = TRUE;
		post BroadcastTask();
			
	  }
	else
	  ReportProblem();
  }

  event void BroadcastSend.sendDone(message_t* msg, error_t error) {
    if (error == SUCCESS)
	{
      ReportSent();
		printf("BroadcastSend.sendDone\n");
		call PrintfFlush.flush();
	}
    else
      ReportProblem();
    SendBusy = FALSE;
  }
  
  // when forwarding
  event message_t* Snoop.receive(message_t* Msg, void* Payload, uint8_t Length) {
	return Msg;
  }
  

}