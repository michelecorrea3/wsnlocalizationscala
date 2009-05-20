#include <Timer.h>
#include <UserButton.h>
#include "printf.h"
#include "Senseless.h"

module SenselessC {
  uses {
  
    // Initialization
	//---------------------------------------------------------------------------
    interface Boot;
	interface SplitControl as RadioControl;			// Used to start the radio
	interface SplitControl as SerialControl;		// Used to start the serial interface
	interface StdControl as RoutingControlC;		// Used to start data collection service
	interface StdControl as RoutingControlL;		// Used to start data collection service
	interface StdControl as RoutingControlS;		// Used to start data collection service
	interface StdControl as DisseminationControl; 	// Interface to the dissemination protocol
	
	// Dissemination
	//---------------------------------------------------------------------------
	interface DisseminationUpdate<DissMsg_t> as RequestUpdate; 	// Used to update a network shared (disseminated) value
	interface DisseminationValue<DissMsg_t> as RequestValue; 	// Used to read a network shared (disseminated) variable
	
	// Collection
	//---------------------------------------------------------------------------
	interface Send as CollectSendC; 				// Used to send with the collection protocol
	interface Send as CollectSendL; 				// Used to send with the collection protocol
	interface Send as CollectSendS; 				// Used to send with the collection protocol
	interface Receive as SnoopC; 					// Used to overhear messages
	interface Receive as SnoopL; 					// Used to overhear messages
	interface Receive as SnoopS; 					// Used to overhear messages
	interface Receive as CollectReceiveC; 			// Used to receive messeges with collection protocol
	interface Receive as CollectReceiveL; 			// Used to receive messeges with collection protocol
	interface Receive as CollectReceiveS; 			// Used to receive messeges with collection protocol
	interface RootControl as RootControlC; 			// Used to control whether the current node is the root of the tree 
	interface RootControl as RootControlS; 			// Used to control whether the current node is the root of the tree 
	interface RootControl as RootControlL; 			// Used to control whether the current node is the root of the tree 
    
	// Serial
	//---------------------------------------------------------------------------
	interface AMSend as SerialSendC;				// Used to transmit serial
	interface AMSend as SerialSendL;				// Used to transmit serial
	interface AMSend as SerialSendS;				// Used to transmit serial
	interface Receive as SerialReceive;				// Used to receive serial
	interface Packet as UartPacket;					// Message data type accessors
	
	// Sensors
	//---------------------------------------------------------------------------
	interface Read<uint16_t> as ReadLight;		// Used to measure the light intensity
	interface Read<uint16_t> as ReadTemp;		// Used to measure the temperature
	interface Read<uint16_t> as ReadHumidity;	// Used to measure the humidty
	interface Read<uint16_t> as ReadBattery;	// Used to measure the voltage of the mote
	
	// Pushbutton
	//---------------------------------------------------------------------------
	interface Notify<button_state_t>;			// Used for getting values from self-triggered devices like pushbutton
	
	// Memory
	//---------------------------------------------------------------------------
	interface Queue<message_t *> as UartQueueC; 	// FIFO list
	interface Pool<message_t> as UartPoolC; 		// Get & put
		
	interface Queue<message_t *> as UartQueueL; 	// FIFO
	interface Pool<message_t> as UartPoolL; 		// Get & put
	
	interface Queue<message_t *> as UartQueueS; 	// FIFO
	interface Pool<message_t> as UartPoolS; 		// Get & put
	
	interface Queue<message_t *> as RadioQueueC; 	// FIFO
	interface Pool<message_t> as RadioPoolC; 		// Get & put
	
	interface Queue<message_t *> as RadioQueueL; 	// FIFO
	interface Pool<message_t> as RadioPoolL; 		// Get & put
	
	interface Queue<message_t *> as RadioQueueS; 	// FIFO
	interface Pool<message_t> as RadioPoolS; 		// Get & put
	
	// Printf
	//---------------------------------------------------------------------------
	interface SplitControl as PrintfControl;		// Printf
	interface PrintfFlush;							// Printf

	// Other
	//---------------------------------------------------------------------------
	interface Timer<TMilli>;						// Timer (for sensors)
	interface Timer<TMilli> as BroadcastTimer;		// Broadcast Timer
	interface Leds;									// Leds
	interface Packet as RadioPacket;				// Message data type accessors
	
	// RSSI & LQI & Power level & Frequency
	//---------------------------------------------------------------------------
	interface CC2420Packet;							// Used to get RSSI/LQI & set/get power level
	interface CC2420Config;							// An HAL abstraction of the ChipCon CC2420 radio (setting frequency)
	
	// Broadcast
	//---------------------------------------------------------------------------
	interface AMSend as BroadcastSend;				// Used to transmit beacon of anchor
	interface Receive as BroadcastReceive;			// Used to receive the beacons
	
  }
}
implementation {

  // Declaration of functions
  //---------------------------------------------------------------------------
  void Status();						// Function to transmit status message to the root
  task void RadioSendTaskL();			// Task for transmitting location message with collection protocol
  task void RadioSendTaskC();			// Task for transmitting sensor message with collection protocol
  task void RadioSendTaskS();			// Task for transmitting status message with collection protocol
  task void BroadcastTask();			// Task for transmitting broadcast message
  
  task void SerialSendTaskL();			// Tast for transmitting location message over the serial port
  task void SerialSendTaskC();			// Tast for transmitting sensor message over the serial port
  task void SerialSendTaskS();			// Tast for transmitting status message over the serial port
  
  static void ReportProblem();			// Red led
  static void ReportSent();				// Green led
  static void ReportReceive();			// Blue led
  static void FatalProblem();			// All leds + stop timer
  
  // Declaration of variables:
  //---------------------------------------------------------------------------
  
  // Lenght to determine the type of packets
  uint8_t UartLen;
  uint8_t RadioLen;
  uint8_t i;
  
  // Message buffers
  message_t SndMsgC, SndMsgL, SndMsgS, SndMsgB, UartBufC, UartBufL, UartBufS;
  
  // Check radio status
  bool FwdBusy, SendBusy, UartBusy, TaskBusy;
  
  // Check root
  bool Root = FALSE;
  
  // Check frequency request
  bool FreqReq = FALSE;
  int Freq = 26;
  
  // Check if sensormeasurements are done
  bool LightIsRead = FALSE;
  bool TempIsRead = FALSE;
  bool HumidityIsRead = FALSE;
  bool ButtonIsPressed = FALSE;
  bool VoltageIsRead = FALSE;
  
  // Message structs
  CollMsg_t LocalCollMsg;
  LocMsg_t LocalLocMsg;
  StatMsg_t LocalStatMsg;
  broadcast_t local;
  
  // Reports
  static void ReportProblem() 
  { 
	//call Leds.led0Toggle(); 
	printf("Report Problem!\n");
  }
  static void ReportSent() 
  { 
	printf("Report Sent!\n");
    //call Leds.led1Toggle(); 
  }
  static void ReportReceive() 
  { 
  printf("Report Receive!\n");
  //call Leds.led2Toggle(); 
  }
  static void FatalProblem() 
  {
  printf("Report fatal!\n");
   //call Leds.led0Toggle();
   //call Leds.led1Toggle();
   //call Leds.led2Toggle();
   //call Timer.stop(); 
   }
							   
	
	struct node
	{
	uint8_t node_ID;
	uint8_t VAN;
	struct node *next;
	};
	
	struct node *n = NULL;

 
  // Booting process (initialize data, network, radio & serial interface, root & collection tree)
  //---------------------------------------------------------------------------
  event void Boot.booted() 
  {
  
	// Sensor message
    LocalCollMsg.moteid = TOS_NODE_ID;
	LocalCollMsg.lightreading = 0;
	LocalCollMsg.tempreading = 0;
	LocalCollMsg.humidityreading = 0;
	LocalCollMsg.button = FALSE;
	LocalCollMsg.type = 1;
	LocalCollMsg.battery = 0;
	
	// Status message
	LocalStatMsg.moteid = TOS_NODE_ID;
	LocalStatMsg.type = 0;
	LocalStatMsg.active = TRUE;
	LocalStatMsg.AN = FALSE;
	LocalStatMsg.posx = 0;
	LocalStatMsg.posy = 0;
	LocalStatMsg.sampleRate = DEFAULT_SAMPLING_PERIOD;
	LocalStatMsg.locRate = DEFAULT_LOC_PERIOD;
	LocalStatMsg.leds = 0;
	LocalStatMsg.power = 0;
	LocalStatMsg.frequency = 26;
	LocalStatMsg.conn = 0;
	
	// Localization message
	LocalLocMsg.moteid = TOS_NODE_ID;
	LocalLocMsg.ANmoteid = TOS_NODE_ID;
	LocalLocMsg.VANs = 0;
	LocalLocMsg.VANr = 0;
	LocalLocMsg.hopCount = 0;
	LocalLocMsg.RSSI = 0;
	
	// Broadcast message
	local.id = TOS_NODE_ID;
	local.hopCount = 1;
	local.VAN = 0;
	
	if(TOS_NODE_ID == 2 || TOS_NODE_ID == 3)
	{
	LocalStatMsg.AN = TRUE;
	local.VAN = 1;
	LocalLocMsg.VANr = 1;
	//call BroadcastTimer.startPeriodic(10000);
	}
	
	// Set that radio & serial port aren't busy
	SendBusy = FALSE;
	UartBusy = FALSE;
	
	// Set the leds off
	call Leds.led0Off();
	call Leds.led1Off();
	call Leds.led2Off();
	
	// Start the radio
	if (call RadioControl.start() != SUCCESS)
	  FatalProblem();
	
	// Start the data collection service for sensor message
	if (call RoutingControlC.start() != SUCCESS)
	  FatalProblem();
	
	// Start the data collection service for location message	
	if (call RoutingControlL.start() != SUCCESS)
	  FatalProblem();
	  
	// Start the data collection service for status message
	if (call RoutingControlS.start() != SUCCESS)
	  FatalProblem();
	  
	// Start the service for Dissemination protocol
	if (call DisseminationControl.start() != SUCCESS)
	  FatalProblem();
	
	// Setting the root of the network and starting the serial interface
	if (TOS_NODE_ID == 0) 
	{ // if mote is root
	  Root = TRUE;
	  if (call SerialControl.start() != SUCCESS)
	    FatalProblem();
	}

	// Activate pushbutton
	  call Notify.enable();
	
	// Start printf interface
	call PrintfControl.start();
	
	// Send status message if the node is rebooted
	//LocalStatMsg.type = 3;
	//Status(3);
	
	//Test channel
	//call CC2420Config.setChannel(20);
	
  }
  
  struct node *addToList(struct node **p, uint8_t id, uint8_t VANs)
{
	struct node *n = malloc(sizeof(struct node));

	if (n == NULL)
	{
		ReportProblem();
		ReportSent();
		ReportReceive();
	}
	//	return NULL;
 
	n->next = *p; /* the previous element (*p) now becomes the "next" element */
	*p = n;       /* add new empty element to the front (head) of the list */
	n->node_ID = id;
	n->VAN = VANs;
 
	return *p;
}

struct node **VAN(struct node **n)
{
	uint8_t ans = 0;
	uint8_t vans = 0;
	
	while (*n != NULL)
	{
		if ((*n)->VAN == 1) 
		{
			ans++;
			printf("VAN: Found anchornode %u in list \n", (*n)->node_ID);
		}
		else if ((*n)->VAN == 2)
		{
			vans++;
			printf("VAN: Found Virtual anchornode %u in list \n", (*n)->node_ID);
		}
		n = &(*n)->next;
	}
	if ( (ans + vans) >= 3)
	return 2;
	else
	return 0;
	
}

	struct node **list_search(struct node **n, uint8_t id, uint8_t VANs)
	{
		while (*n != NULL)
		{
			if ((*n)->node_ID == id)
			{
				(*n)->VAN = VANs;
				return n;
			}
			n = &(*n)->next;
		}
		return NULL;
	}

	void list_print(struct node *n)
	{
		if (n == NULL)
		{
			printf("list is empty\n");
		}
		while (n != NULL)
		{
			printf("Data in list %u\n", n->node_ID);
			n = n->next;
		}
		call PrintfFlush.flush();
	}


  
  // An event that is called when the radio is started:
  // * Set the node with id 0 as root of all the collection trees
  // * Start timer for sensors
  // * Start broadcast timer
  // * Transmit a status message
  //---------------------------------------------------------------------------
  event void RadioControl.startDone(error_t err) {
    if (err != SUCCESS)
	  FatalProblem();
	else 
	{
	  if (Root)
	  {
	    call RootControlL.setRoot();
		call RootControlS.setRoot();
		call RootControlC.setRoot();
	  }
		//stop running Timers
      if (call BroadcastTimer.isRunning())
	    call BroadcastTimer.stop();
      if (call Timer.isRunning())
	    call Timer.stop();
		//start the sensor timer
	call Timer.startPeriodic(LocalStatMsg.sampleRate); //sensoren
		//if(LocalStatMsg.AN == 1)
		if(!Root)
    call BroadcastTimer.startPeriodic(LocalStatMsg.locRate);
	
	Status();

	}
  }
  
  // Event handled when radio is stopped
  event void RadioControl.stopDone(error_t err) { }
  
  // Event handled when we execute the CC2420Config.sync() command
  event void CC2420Config.syncDone(error_t error) {}
  
  // Event handled when the serial interface is successfully started
  event void SerialControl.startDone(error_t err) 
  {
    if (err != SUCCESS)
	  FatalProblem();
  }
  
  // Event handled for the printf interface
  event void PrintfFlush.flushDone(error_t error){ }
  event void PrintfControl.startDone(error_t error) { }
  event void PrintfControl.stopDone(error_t error) {}
  
  // Event handled when the serial interface is stopped
  event void SerialControl.stopDone(error_t err) { }
  
  // Event handled when the pushbutton is pressed
  //---------------------------------------------------------------------------
  event void Notify.notify(button_state_t state) {
    if (state == BUTTON_PRESSED)
	  LocalCollMsg.button = TRUE;
  }
  
  
 
  // Function to transmit the status of the node to the root:
  // * Checks if there isn't a task busy
  // * Adds the content of the LocalStatMsg struct as the payload of the SndMsgS payload
  // * If task is busy then the data will be put on a Queue
  //---------------------------------------------------------------------------
  void Status()
  {

    	if (!Root) 
		{
		
		StatMsg_t *out;
			  
			if (!TaskBusy) 
			{
			  out = (StatMsg_t *)call CollectSendS.getPayload(&SndMsgS);
			  memcpy(out, &LocalStatMsg, sizeof(StatMsg_t));
			  
				//printf("Status(): !root && !Taskbusy\n");
				if (call CollectSendS.send(&SndMsgS, sizeof(StatMsg_t)) == SUCCESS)
				{
					//SendBusy = TRUE;
					//TaskBusy = FALSE;		
					printf("StatMsg wordt verzonden\n");
					call PrintfFlush.flush();
				}
				else
				ReportProblem();
				  //TaskBusy = TRUE;
				  //post RadioSendTaskS();
			}
			else 
			{
			  message_t *NewMsg = call RadioPoolS.get();
			  if (NewMsg == NULL)
				ReportProblem();
			  out = (StatMsg_t *)call CollectSendS.getPayload(NewMsg);
			  memcpy(out, &LocalStatMsg, sizeof(StatMsg_t));
			  if (call RadioQueueS.enqueue(NewMsg) != SUCCESS) {
				call RadioPoolS.put(NewMsg);
				FatalProblem();
			  }
		}
	  }
  } 
  
  // Function to process the request received with dissemination:
  // * Checks if the request is meant for this node or for every node
  // * Checks which request it is and executes the requests
  // * Sends a status message as reply
  //---------------------------------------------------------------------------
  void ProcessRequest(DissMsg_t *NewRequest) {
    //if target mote or broadcast to all motes
	if ((NewRequest->targetid == TOS_NODE_ID) || (NewRequest->targetid == 0xFFFF)) {
	  switch (NewRequest->request) {
	  
	    case SAMPLING_PERIOD:  //rate that sensors get measured
		  LocalStatMsg.sampleRate = NewRequest->parameter;
		  call Timer.stop();
//		  Alpha = SamplingPeriod/(MaxNumMotes+1);
//		  WaitS = (1+TOS_NODE_ID)*Alpha;
//		  call WaitTimer.startOneShot(WaitS);
		  call Timer.startPeriodic(LocalStatMsg.sampleRate);
		  //LocalStatMsg.type = 4;
		  //Status();
		  break;
		  
		case LOC_PERIOD: //rate that RSSI is transmitted
			if (LocalStatMsg.AN == TRUE)
			{						
				call BroadcastTimer.stop();
				call BroadcastTimer.startPeriodic(LocalStatMsg.locRate);
				
				LocalStatMsg.locRate = NewRequest->parameter;
			}
			//Status();
			break;
			
		case ACTIVE_REQUEST: //Put a node in the active mode
			if(NewRequest->parameter == 1)
			{
				LocalStatMsg.active = TRUE;
				
				if(LocalStatMsg.AN == TRUE)
				call BroadcastTimer.startPeriodic(LocalStatMsg.locRate);
				
			}
			else if(NewRequest->parameter == 0)
			{
				LocalStatMsg.active = FALSE;
				
				if(LocalStatMsg.AN == TRUE)
				call BroadcastTimer.stop();
				
			}
			//Status();
			break;
			
		case AN_REQUEST: //Set the node as anchor node
			if(NewRequest->parameter == 1)
			{
				LocalStatMsg.AN = TRUE;
				LocalLocMsg.VANr = 1;
				local.VAN = 1;
			}
			else if(NewRequest->parameter == 0)
			{
				if (call BroadcastTimer.isRunning())
				call BroadcastTimer.stop();
				
				LocalStatMsg.AN = FALSE;
			}
			//Status();
			break;
		
		case LED_REQUEST: //set the leds on
		  call Leds.set(NewRequest->parameter);
		  LocalStatMsg.leds = NewRequest->parameter;
		  //LocalStatMsg.type = 4;
		  //Status();
		  break;
		 
		case POWER_REQUEST: //sets the power level
			LocalStatMsg.power = NewRequest->parameter;
			//LocalStatMsg.type = 4;
			//Status();
			break;
			
		case FREQUENCY_REQUEST: //sets the frequency
			LocalStatMsg.frequency = NewRequest->parameter;
			call CC2420Config.setChannel(LocalStatMsg.frequency);
			//LocalStatMsg.type = 4;
			  /**
			   * Sync must be called to commit software parameters configured on
			   * the microcontroller (through the CC2420Config interface) to the
			   * CC2420 radio chip.
			   */
			call CC2420Config.sync();
			//Status();
			break;
			
		case POSX_REQUEST: // sets the x coordinate of the node
			LocalStatMsg.posx = NewRequest->parameter;
			//LocalStatMsg.type = 4;
			//Status();
			break;
			
		case POSY_REQUEST: // sets the y coordinate of the node
			LocalStatMsg.posy = NewRequest->parameter;
			//LocalStatMsg.type = 4;
			//Status();
			break;
		
		case STATUS_REQUEST: //discovery messages
			//LocalStatMsg.type = 4;
			for ( i = 0; i < 5; i++)
			Status();
			break;
			
		default:
		  break;
	  }
	}
  }
  
  
  // Event handled when the root receives requests to disseminate:
  // * Checks if the message had the correct message, if so then it disseminates the request to his children
  //---------------------------------------------------------------------------
  event message_t* SerialReceive.receive(message_t* Msg, void* Payload, uint8_t Length) {
	DissMsg_t *NewRequest = Payload;
	
			printf("SerialReceive.Receive\n");
			
	
	if (Length == sizeof(DissMsg_t))
	   call RequestUpdate.change(NewRequest);
	   
	
	   
	/*   
	if (NewRequest->targetid == 0xFFFF && NewRequest->request == FREQUENCY_REQUEST)
	{
		FreqReq = TRUE;
		Freq = NewRequest->parameter;
		//call CC2420Config.setChannel(NewRequest->parameter);
		//call CC2420Config.sync();
	}
	  */  
	call PrintfFlush.flush();
	return Msg;
  }

  
  // Event handled when a sensor message is successfully sent with the collection protocol:
  // * Checks if there is still data on the Queue to be transmitted
  //---------------------------------------------------------------------------
  event void CollectSendC.sendDone(message_t* Msg, error_t err) {
	if (err == SUCCESS)
	{
	  	  printf("CollectSendC.sendDone == SUCCESS\n");

	}
	else
	ReportProblem();
	  
	SendBusy = FALSE; // Just finished CollectSend, if radioqueue is not empty
	
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
			
	  post RadioSendTaskC();
	}
	
	call PrintfFlush.flush();
	ReportSent();
  }
  
  // Event handled when a location message is successfully sent with the collection protocol:
  // * Checks if there is still data on the Queue to be transmitted
  //---------------------------------------------------------------------------
  event void CollectSendL.sendDone(message_t* Msg, error_t err) {
	if (err == SUCCESS)
	{
	  	  printf("CollectSendL.sendDone == SUCCESS\n");
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
	  post RadioSendTaskL();
	}
	
	call PrintfFlush.flush();
	ReportSent();
  }
  
  // Event handled when a status message is successfully send with the collection protocol:
  // * Checks if there is still data on the Queue to be transmitted
  //---------------------------------------------------------------------------
  event void CollectSendS.sendDone(message_t* Msg, error_t err) {
    	//call Leds.led0On();
	if (err == SUCCESS)
	{
	  	  printf("CollectSendS.sendDone == SUCCESS\n");
		    //call Leds.led1On();
	}
	else if (err == EBUSY)
	{
		ReportProblem();
	  //call Leds.led2On();
	}
	  
	SendBusy = FALSE; // Just finished CollectSend, if radioqueue is not empty
	
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
	  
	  post RadioSendTaskS();
	}
	
	call PrintfFlush.flush();
	ReportSent();
  }
  
  // Event handled when a sensor message is successfully transmited over the serial port:
  // * Checks if there is still data on the Queue to be transmitted
  //---------------------------------------------------------------------------
  event void SerialSendC.sendDone(message_t* Msg, error_t err) {
  	if (err == SUCCESS)
	{
			ReportReceive();
	  	  printf("SerialSendC.sendDone == SUCCESS\n");
		  //call PrintfFlush.flush();
	}
	else
	ReportProblem();
	UartBusy = FALSE; //Just finished SerialSend, if uartqueue is not empty
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
	ReportSent();
	call PrintfFlush.flush();
  }
  
  // Event handled when a location message is successfully transmitted over the serial port:
  // * Checks if there is still data on the Queue to be transmitted
  //---------------------------------------------------------------------------
  event void SerialSendL.sendDone(message_t* Msg, error_t err) {
  	if (err == SUCCESS)
	{
			ReportReceive();
	  	  printf("SerialSendL.sendDone == SUCCESS\n");
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
	ReportSent();
	call PrintfFlush.flush();
  }
  
  // Event handled when a status message is successfully transmitted over the serial port:
  // * Checks if there is still data on the Queue to be transmitted
  //---------------------------------------------------------------------------
    event void SerialSendS.sendDone(message_t* Msg, error_t err) {
  	if (err == SUCCESS)
	{
			ReportReceive();
	  	  printf("SerialSendS.sendDone == SUCCESS\n");
		  //call PrintfFlush.flush();
	}
	else
	ReportProblem();
	UartBusy = FALSE; //Just finished SerialSend, if uartqueue is not empty
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
  
  // Event handled when dissemination valua is changed:
  // * Executes the request with the "processRequest" function
  //---------------------------------------------------------------------------
  event void RequestValue.changed() {
    DissMsg_t *NewRequest = (DissMsg_t *)call RequestValue.get();
	ProcessRequest(NewRequest);
  }
  
  // Task for sending a status message with the collection protocol to the root:
  // * Checks if the radio is free and that the note isn't the root node
  // * When the message is being sent, then the radio is busy and the task is finished
  //---------------------------------------------------------------------------
  task void RadioSendTaskS() 
	{
    if (!Root && !SendBusy) 
	{
		//RadioLen = call RadioPacket.payloadLength(&SndMsg);
		
		if (call CollectSendS.send(&SndMsgS, sizeof(StatMsg_t)) == SUCCESS)
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
	
  // Task for sending a sensor message with the collection protocol to the root:
  // * Checks if the radio is free and that the note isn't the root node
  // * When the message is being sent, then the radio is busy and the task is finished
  //---------------------------------------------------------------------------
  task void RadioSendTaskC() 
	{
    if (!Root && !SendBusy) 
	{
		//RadioLen = call RadioPacket.payloadLength(&SndMsg);
		
		if (call CollectSendC.send(&SndMsgC, sizeof(CollMsg_t)) == SUCCESS)
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
  
  // Task for sending a location message with the collection protocol to the root:
  // * Checks if the radio is free and that the note isn't the root node
  // * When the message is being sent, then the radio is busy and the task is finished
  //---------------------------------------------------------------------------
  task void RadioSendTaskL() 
	{
    if (!Root && !SendBusy) 
	{
		//RadioLen = call RadioPacket.payloadLength(&SndMsg);
		//call CC2420Packet.setPower(&SndMsgL,12);
		if (call CollectSendL.send(&SndMsgL, sizeof(LocMsg_t)) == SUCCESS)
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
  
  // Event handled when the timer fires:
  // * Waits until all the sensor measurements are taken
  // * Transmits the sensor data with the collection protocol
  // * If the radio is busy then the data will be put on a Queue
  //---------------------------------------------------------------------------
  event void Timer.fired() {
  
  /*
  if( FreqReq == TRUE )
  {
		FreqReq == FALSE;
		call CC2420Config.setChannel(Freq);
		call CC2420Config.sync();
  }
  */
  	call Leds.led0Off();
	call Leds.led1Off();
	call Leds.led2Off();
  
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
		  out = (CollMsg_t *)call CollectSendC.getPayload(&SndMsgC);
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
		  out = (CollMsg_t *)call CollectSendC.getPayload(NewMsg);
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
  
  // Event handled when light measurement is taken
  //---------------------------------------------------------------------------
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
  
  // Event handled when temperature measurement is taken
  //---------------------------------------------------------------------------
  event void ReadTemp.readDone(error_t err, uint16_t reading) {
    if (err != SUCCESS) {
	  reading = 0xFFFF;
	  ReportProblem();
	}
	else {
	  LocalCollMsg.tempreading = reading;
	}
  }

  // Event handled when humidity measurement is taken
  //---------------------------------------------------------------------------
  event void ReadHumidity.readDone(error_t err, uint16_t reading) {
    if (err != SUCCESS) {
	  reading = 0xFFFF;
	  ReportProblem();
	}
	else {
	  LocalCollMsg.humidityreading = reading;
	}
  }  
  
  // Event handled when voltage measurement is taken
  //---------------------------------------------------------------------------
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
  
  // Task for sending a sensor message serial to the root:
  // * When the message is being sent, then the serial interface is busy
  //---------------------------------------------------------------------------
  task void SerialSendTaskC() { 
	
	//UartLen = call UartPacket.payloadLength(&UartBuf);
	UartLen = sizeof(CollMsg_t);
	if (call SerialSendC.send(0xFFFF, &UartBufC, UartLen) == SUCCESS)
	{
		UartBusy = TRUE;
		printf("SerialSend van CollMsg \n");
		call PrintfFlush.flush();
	}
	else
	  ReportProblem();
  }
  
  // Task for sending a location message serial to the root:
  // * When the message is being sent, then the serial interface is busy
  //---------------------------------------------------------------------------
  task void SerialSendTaskL() { 
	
	UartLen = sizeof(LocMsg_t);
	if (call SerialSendL.send(0xFFFF, &UartBufL, UartLen) == SUCCESS)
	{
		UartBusy = TRUE;
		printf("SerialSend van LocMsg \n");
		call PrintfFlush.flush();
	}
	else
	  ReportProblem();
  }
  
  // Task for sending a status message serial to the root:
  // * When the message is being sent, then the serial interface is busy
  //---------------------------------------------------------------------------
  task void SerialSendTaskS() 
	{ 
	
	UartLen = sizeof(StatMsg_t);
	if (call SerialSendS.send(0xFFFF, &UartBufS, UartLen) == SUCCESS)
	{
		UartBusy = TRUE;
		printf("SerialSend van StatMsg \n");

	}
	else
	  ReportProblem();
	  
	call PrintfFlush.flush();
  }
  
  // Event handled when the root receives sensor messages:
  // * Checks if the serial interface is free to transmit the message over the serial interface
  // * Else the message is put on a queue
  //---------------------------------------------------------------------------
  event message_t* CollectReceiveC.receive(message_t* Msg, void* Payload, uint8_t Length) {

    //int packetType = 0;
	CollMsg_t* inColl = (CollMsg_t*)Payload;
	CollMsg_t* outColl;

		printf("CollectReceiveC.receive:\n");
		printf("ID is: %u\n", inColl->moteid);
		printf("Power is: %u\n", call CC2420Packet.getPower(Msg));
		printf("Channel: %u\n",call CC2420Config.getChannel());
		if (UartBusy == FALSE)
		{
			outColl = (CollMsg_t*)call SerialSendC.getPayload(&UartBufC);

			  if (Length != sizeof(CollMsg_t))
				return Msg;
			  else
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
		  outColl = (CollMsg_t*)call SerialSendC.getPayload(NewMsg);
		  memcpy(outColl, inColl, sizeof(CollMsg_t));
		  if (call UartQueueC.enqueue(NewMsg) != SUCCESS) {
			call UartPoolC.put(NewMsg);
			FatalProblem();
			return Msg;
		  }
		}
	call PrintfFlush.flush();
	return Msg;
		
  }
  
  // Event handled when the root receives location messages:
  // * Checks if the serial interface is free to transmit the message over the serial interface
  // * Else the message is put on a queue
  //---------------------------------------------------------------------------
  event message_t* CollectReceiveL.receive(message_t* Msg, void* Payload, uint8_t Length) {

    //int packetType = 0;
	LocMsg_t* inLoc = (LocMsg_t*)Payload;
	LocMsg_t* outLoc;

		printf("CollectReceiveL.receive:\n");
		printf("ID is: %u\n", inLoc->moteid);
		printf("Power level is: %u\n", call CC2420Packet.getPower(Msg));
		printf("Channel: %u\n",call CC2420Config.getChannel());
		
		if (UartBusy == FALSE)
		{
			outLoc = (LocMsg_t*)call SerialSendL.getPayload(&UartBufL);

			  if (Length != sizeof(LocMsg_t))
				return Msg;
			  else
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
		  outLoc = (LocMsg_t*)call SerialSendL.getPayload(NewMsg);
		  memcpy(outLoc, inLoc, sizeof(LocMsg_t));
		  if (call UartQueueL.enqueue(NewMsg) != SUCCESS) {
			call UartPoolL.put(NewMsg);
			FatalProblem();
			return Msg;
		  }
		}
	call PrintfFlush.flush();
	return Msg;
		
  }
  
  // Event handled when the root receives status messages:
  // * Checks if the serial interface is free to transmit the message over the serial interface
  // * Else the message is put on a queue
  //---------------------------------------------------------------------------
  event message_t* CollectReceiveS.receive(message_t* Msg, void* Payload, uint8_t Length) {

    //int packetType = 0;
	StatMsg_t* inStat = (StatMsg_t*)Payload;
	StatMsg_t* outStat;

		printf("CollectReceiveS.receive:\n");
		printf("ID is: %u\n", inStat->moteid);
		printf("Status: type %u\n", inStat->type);
		
		if (UartBusy == FALSE)
		{
			outStat = (StatMsg_t*)call SerialSendS.getPayload(&UartBufS);

			  if (Length != sizeof(StatMsg_t))
				return Msg;
			  else
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
		  outStat = (StatMsg_t*)call SerialSendS.getPayload(NewMsg);
		  memcpy(outStat, inStat, sizeof(StatMsg_t));
		  if (call UartQueueL.enqueue(NewMsg) != SUCCESS) {
			call UartPoolL.put(NewMsg);
			FatalProblem();
			return Msg;
		  }
		}
	call PrintfFlush.flush();
	return Msg;
		
  }
  
  // Event handled when a blind node receives a broadcast message from the anchors:
  // * Checks if the radio is free to transmit the message with the collection protocol
  // * Else the message is put on a queue
  //---------------------------------------------------------------------------
  event message_t* BroadcastReceive.receive(message_t* msg, void* payload, uint8_t len) {
  
  broadcast_t *omsg = (broadcast_t*)payload;
  //FatalProblem();
  /*
  broadcast_t *omsg = (broadcast_t*)payload;
  LocalLocMsg.ANmoteid = omsg->id;
  LocalLocMsg.VANs = omsg->VAN;
  LocalLocMsg.RSSI = call CC2420Packet.getRssi(msg);
  LocalLocMsg.hopCount = omsg->hopCount;
  //LocalLocMsg.posx = omsg->posx;
  //LocalLocMsg.posy = omsg->posy;
  //call CC2420Packet.getLqi(msg);
  LocalStatMsg.power = call CC2420Packet.getPower(msg);
  */

	printf(" Received broadcast message from node: %u\n", omsg->id);
	printf("RSSI: %d\n",(LocalLocMsg.RSSI - 45) );
	//printf("Power: %u\n",call CC2420Packet.getPower(msg));
	printf("Channel: %u\n",call CC2420Config.getChannel());
	printf("Power: %u\n",LocalStatMsg.power);
	printf("VANr van deze node is: %u\n",LocalLocMsg.VANr);
	
			/*
			if(LocalLocMsg.VANr > 1 && LocalStatMsg.active)
			{
				if (list_search( &n, LocalLocMsg.ANmoteid, LocalLocMsg.VANs) == NULL ) 
					addToList(&n, LocalLocMsg.ANmoteid, LocalLocMsg.VANs);
			}
			  */
			//  list_print(n);
  
  		  //printf("BroadcastReceive.receive\n");
		  //call PrintfFlush.flush();
  
		//alleen blindnode verzend location message
  	  //if (!Root && !LocalStatMsg.AN && LocalStatMsg.active) 
	  if (!Root && LocalStatMsg.active && (omsg->VAN > 0) ) 
	  {
		//broadcast_t *omsg = (broadcast_t*)payload;
		LocalLocMsg.ANmoteid = omsg->id;
		LocalLocMsg.VANs = omsg->VAN;
		LocalLocMsg.RSSI = call CC2420Packet.getRssi(msg);
		LocalLocMsg.hopCount = omsg->hopCount;
		  //LocalLocMsg.posx = omsg->posx;
		  //LocalLocMsg.posy = omsg->posy;
		  //call CC2420Packet.getLqi(msg);
		LocalStatMsg.power = call CC2420Packet.getPower(msg);
		
		if( (LocalLocMsg.VANr == 0 || LocalLocMsg.VANr == 2) && LocalStatMsg.active)
		{
			if (list_search( &n, LocalLocMsg.ANmoteid, LocalLocMsg.VANs) == NULL ) 
				addToList(&n, LocalLocMsg.ANmoteid, LocalLocMsg.VANs);
		}
	  
		if(LocalLocMsg.VANr == 1 && LocalLocMsg.VANs !=1)
		printf("Anchor received message from a non anchor node\n");
		else
		{
			LocMsg_t *out;
			if (!SendBusy) 
			{
			  out = (LocMsg_t *)call CollectSendL.getPayload(&SndMsgL);

			  
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
			  out = (LocMsg_t *)call CollectSendL.getPayload(NewMsg);
			  memcpy(out, &LocalLocMsg, sizeof(LocMsg_t));
			  if (call RadioQueueL.enqueue(NewMsg) != SUCCESS) 
			  {
				call RadioPoolL.put(NewMsg);
				FatalProblem();
			  }
			  
			  //if(!LocalStatMsg.AN)
			  //addToList(&n, LocalLocMsg.ANmoteid, LocalLocMsg.VANs);
			  
			  //list_print(&n);
			  
			}
		}
	  }
  call PrintfFlush.flush();
  return msg;
  }
  
  // Task for sending a broadcast message to the blind nodes:
  // * When the message is being sent, then the radio is busy and the task is finished
  //---------------------------------------------------------------------------
  task void BroadcastTask()
  {
		//call CC2420Packet.setPower(&SndMsgB,LocalStatMsg.power);
		//call CC2420Packet.setPower(&SndMsgB,11);
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
  
  // Event handled when the Broadcasttimer fires:
  // * Transmits the broadcast data
  //---------------------------------------------------------------------------
  event void BroadcastTimer.fired() {
	//if (!SendBusy && sizeof local <= call BroadcastSend.maxPayloadLength())
		printf("BroadcastTimer.fired\n");

		  //ReportReceive();
	if(!Root && !LocalStatMsg.AN)
	{
	LocalLocMsg.VANr = VAN(&n);
	local.VAN = LocalLocMsg.VANr;
	
	printf("De var van node %u: %u\n",TOS_NODE_ID, LocalLocMsg.VANr);
	}
	
	
	
	if ( !SendBusy && !Root && LocalStatMsg.active && (LocalLocMsg.VANr > 0) )
	  {
	  
	  	broadcast_t * o = (broadcast_t *)call BroadcastSend.getPayload(&SndMsgB);
			
		memcpy(o, &local, sizeof(broadcast_t));
		// call CC2420Packet.setPower(&SndMsgB,11);
		TaskBusy = TRUE;
		post BroadcastTask();
			
	  }
	else
	  ReportProblem();
	  
	  call PrintfFlush.flush();	
  }

  // Event handled when the broadcastsend is successfully done:
  //---------------------------------------------------------------------------
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
  
  // Events used to listen to transmitted messages
  //---------------------------------------------------------------------------
  event message_t* SnoopC.receive(message_t* Msg, void* Payload, uint8_t Length) {
	return Msg;
  }
    event message_t* SnoopL.receive(message_t* Msg, void* Payload, uint8_t Length) {
	return Msg;
  }
    event message_t* SnoopS.receive(message_t* Msg, void* Payload, uint8_t Length) {
	return Msg;
  }

}