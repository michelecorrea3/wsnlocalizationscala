/*
 * Copyright (c) 2006 Intel Corporation
 * All rights reserved.
 *
 * This file is distributed under the terms in the attached INTEL-LICENSE     
 * file. If you do not find these files, copies can be found by writing to
 * Intel Research Berkeley, 2150 Shattuck Avenue, Suite 1300, Berkeley, CA, 
 * 94704.  Attention:  Intel License Inquiry.
 */
 
 /**
 * MultihopOscilloscope demo application using the collection layer. 
 * See README.txt file in this directory and TEP 119: Collection.
 *
 * @author David Gay
 * @author Kyle Jamieson
 * @author Raja Jurdak, Antonio Ruzzelli, and Samuel Boivineau
 */

#include "Timer.h"
#include "MultihopOscilloscope.h"


module MultihopOscilloscopeC 
{
  uses 
  {
    // Interfaces voor initialisatie:
	//-------------------------------------------------------------------------------------------------------------------------------
    interface Boot;
    interface SplitControl as RadioControl;
    interface SplitControl as SerialControl;
    interface StdControl as RoutingControl;		// used for reception of data collected by the root of the network 
    
    // Interfaces voor communicatie, multihop en serieel:
	//--------------------------------------------------------------------------------------------------------------------------------
    interface Send as CollectSend;			// used by a node which want to send data to the root
    interface Receive as Snoop;				// used when we have to forward data
    interface Receive as CollectReceive;	// used for reception of data collected by the root of the network
	interface CollectionPacket; 
	interface CtpInfo as CollectInfo;		// specific to the ctp protocol, used to get some data (parent and quality here)
    interface AMSend as SerialSend;			// Serial
    
 
	//Interface for the root 
	//--------------------------------------------------------------------------------------------------------------------------------
	interface RootControl;

	//Memory
	//--------------------------------------------------------------------------------------------------------------------------------
    interface Queue<message_t *> as UARTQueue;		//FIFO (QueueC)
    interface Pool<message_t> as UARTMessagePool;	//GET & PUT (PoolC)

	//Memory2
	//--------------------------------------------------------------------------------------------------------------------------------
    interface Queue<message_t *> as RadioQueue;		//FIFO (QueueC)
    interface Pool<message_t> as RadioMessagePool;	//GET & PUT (PoolC)

	
    // various
	//-------------------------------------------------------------------------------------------------------------------------------
    interface Timer<TMilli>;
    interface Leds;
	
	//Read off sensors
	//-------------------------------------------------------------------------------------------------------------------------------------
		interface Read<uint16_t> as ReadVoltage;
		interface Read<uint16_t> as ReadLight;
		interface Read<uint16_t> as ReadTemperature;
		interface Read<uint16_t> as ReadHumidity;
	
	// RSSI
	//------------------------------------------------------------------------------------------------------------------------------------
		interface CC2420Packet;
	
	//Broadcast
	//------------------------------------------------------------------------------------------------------------------------------------
		interface Receive as MobileReceive;
	
	// Interfaces for printf:
	// -------------------------------------------------------------------------------------------------------------------------------------
		interface SplitControl as PrintfControl;
		interface PrintfFlush;
  }
}

implementation {
  task void uartSendTask();
  static void startTimer();
  static void fatal_problem();
  static void report_problem();
  static void report_sent();
  static void report_received();

  uint8_t uartlen;
  message_t sendbuf;
  message_t uartbuf;
  bool sendBusy=FALSE, uartbusy=FALSE;
  
  	uint16_t getRssi(message_t *Msg);
  
  //BOOLS for the data
  //------------------------------------------------------------------------------------------------------------------------------------
  	bool voltageIsRead = FALSE;
	bool lightIsRead = FALSE;
	bool tempIsRead = FALSE;
	bool humidityIsRead = FALSE;
	bool rssiIsRead = FALSE;
	
	
  // BOOLS for confirming root
  //-----------------------------------------------------------------------------------------------------------------------------------
	bool root = FALSE;
  
  oscilloscope_t local;

  /* When we head an Oscilloscope message, we check it's sample count. If
     it's ahead of ours, we "jump" forwards (set our count to the received
     count). However, we must then suppress our next count increment. This
     is a very simple form of "time" synchronization (for an abstract
     notion of time). */
	 
  bool suppress_count_change;

  // 
  // On bootup, initialize radio and serial communications, and our
  // own state variables.
  //
  event void Boot.booted(){
    local.moteId = TOS_NODE_ID;
	local.reply = NO_REPLY;
	
	call PrintfControl.start();
	
	if(TOS_NODE_ID == 0)
		root = TRUE;

    // Initialisation phase:
    if (call RadioControl.start() != SUCCESS)
      fatal_problem();

    if (call RoutingControl.start() != SUCCESS)
      fatal_problem();
  }
  
	event void PrintfFlush.flushDone(error_t error){ }
	event void PrintfControl.startDone(error_t error) { }
	event void PrintfControl.stopDone(error_t error) {}

  event void RadioControl.startDone(error_t error){
    if (error != SUCCESS)
      fatal_problem();

    if (sizeof(local) > call CollectSend.maxPayloadLength())
      fatal_problem();
	  
	if (root)
	{
		call RootControl.setRoot();
		if (call SerialControl.start() != SUCCESS)
			fatal_problem();
	}
  }

  event void SerialControl.startDone(error_t error){
    if (error != SUCCESS)
      fatal_problem();

    startTimer();
  }

  static void startTimer(){
    if (call Timer.isRunning()) 
	call Timer.stop();
	
    call Timer.startPeriodic(DEFAULT_INTERVAL);
  }

  event void RadioControl.stopDone(error_t error) { }
  event void SerialControl.stopDone(error_t error) { }

  //
  // Only the root will receive messages from this interface; its job
  // is to forward them to the serial uart for processing on the pc
  // connected to the sensor network.
  //

  event message_t*
  CollectReceive.receive(message_t* msg, void *payload, uint8_t len) { 
 
    oscilloscope_t* in = (oscilloscope_t*)payload;
    oscilloscope_t* out;
    if (uartbusy == FALSE) 				
	{
		out = (oscilloscope_t*)call SerialSend.getPayload(&uartbuf);
		if (len != sizeof(oscilloscope_t)) 
		{
			return msg;
		}
		else 
		{
			memcpy(out, in, sizeof(oscilloscope_t));
		}
		
		uartlen = sizeof(oscilloscope_t);
		post uartSendTask();		
    } 
	else 							
	{
      // The UART is busy; queue up messages and service them when the
      // UART becomes free.
      message_t *newmsg = call UARTMessagePool.get();
      if (newmsg == NULL) 
	  {
      //Serial port busy, so enqueue.
        report_problem();
        return msg;
      }

      out = (oscilloscope_t*)call SerialSend.getPayload(newmsg);
      memcpy(out, in, sizeof(oscilloscope_t));

      if (call UARTQueue.enqueue(newmsg) != SUCCESS) 
	  {
        // drop the message on the floor and hang if we run out of
        // queue space without running out of queue space first (this
        // should not occur).
        call UARTMessagePool.put(newmsg);
        fatal_problem();
        return msg;
      }
    }

    return msg;
  }

  task void uartSendTask(){
    if (call SerialSend.send(0xffff, &uartbuf, uartlen) != SUCCESS) {
      report_problem();
    } 
	else 
	{
      uartbusy = TRUE;
    }
  }

	task void collectSendTask() {
		if (!root) {
			if (call CollectSend.send(&sendbuf, sizeof(oscilloscope_t)) == SUCCESS)
			{
				oscilloscope_t *o;
				sendBusy = TRUE;
				printf("Collectsend van Mobile is SUCCESS\n");
				o = (oscilloscope_t *)call CollectSend.getPayload(&sendbuf);
				printf("De id = %u\n",o->moteId);
				call PrintfFlush.flush();
			}
			else
				report_problem();
		}
	}

  event void SerialSend.sendDone(message_t *msg, error_t error){
    uartbusy = FALSE;
    if (call UARTQueue.empty() == FALSE) 
	{
      // We just finished a UART send, and the uart queue is
      // non-empty.  Let's start a new one.
      message_t *queuemsg = call UARTQueue.dequeue();
      if (queuemsg == NULL) 
	  {
        fatal_problem();
        return;
      }
      memcpy(&uartbuf, queuemsg, sizeof(message_t));
      if (call UARTMessagePool.put(queuemsg) != SUCCESS) 
	  {
        fatal_problem();
        return;
      }
      post uartSendTask();
    }
  }
  
	void fillPacket() {
		uint16_t tmp;
		call CollectInfo.getEtx(&tmp);
		local.quality = tmp;
		call CollectInfo.getParent(&tmp);
		local.parentId = tmp;
	}

  event message_t* 
  Snoop.receive(message_t* msg, void* payload, uint8_t len){
    return msg;
  }

  event void Timer.fired(){
			if (call ReadVoltage.read() == SUCCESS) voltageIsRead = TRUE;				
			if (call ReadLight.read() == SUCCESS) lightIsRead = TRUE;				
			if (call ReadTemperature.read() == SUCCESS) tempIsRead = TRUE;				
			if (call ReadHumidity.read() == SUCCESS) humidityIsRead = TRUE;
			getRssi(&sendbuf);
			
			
			if(voltageIsRead && lightIsRead && tempIsRead && humidityIsRead && rssiIsRead)
			{
				local.count++;
				if(!root)				
				{
					oscilloscope_t *out;
					if (!sendBusy){
						out = (oscilloscope_t *)call CollectSend.getPayload(&sendbuf);
						memcpy(out, &local, sizeof(oscilloscope_t));		
						post collectSendTask();
					}	
					else
					{
						message_t *newmsg = call RadioMessagePool.get();
						if (newmsg == NULL) 
						{
							report_problem();
						}
						
						out = (oscilloscope_t*)call CollectSend.getPayload(newmsg);
						memcpy(out, &local, sizeof(oscilloscope_t));
						
						if (call RadioQueue.enqueue(newmsg) != SUCCESS) 
						{
							// drop the message on the floor and hang if we run out of
							// queue space without running out of queue space first (this
							// should not occur).
					        call RadioMessagePool.put(newmsg);
							fatal_problem();
						}
					}		
						
				}
			}
	}

	event void CollectSend.sendDone(message_t* msg, error_t error){
		if (error != SUCCESS)
			report_problem();
			
		sendBusy = FALSE;
		
		if (call RadioQueue.empty() == FALSE)
		{
			message_t *queuemsg = call RadioQueue.dequeue();
			if (queuemsg == NULL) 
			{
		        fatal_problem();
				return;
	        }
			memcpy(&sendbuf, queuemsg, sizeof(message_t));
			if (call RadioMessagePool.put(queuemsg) != SUCCESS) 
			{
				fatal_problem();
				return;
			}
			post collectSendTask();
			
		}
		
		local.reply = NO_REPLY;

		report_sent();
  }

	event void ReadVoltage.readDone(error_t ok, uint16_t data) { 
	if (ok == SUCCESS)
	{
		fillPacket();
		local.Voltreading = data;
	}
	else
	{
		data = 0xffff;
		report_problem();
    }
  }

	event void ReadLight.readDone(error_t ok, uint16_t data) {
	if (ok == SUCCESS) 
	{
		fillPacket();
		local.Lightreading = data;
	}
	else
	{
      data = 0xffff;
      report_problem();
    }
  }
  
	event void ReadTemperature.readDone(error_t ok, uint16_t data) { 
	if (ok == SUCCESS) 
	{
		fillPacket();
		local.Tempreading = data;
	}
	else
	{
      data = 0xffff;
      report_problem();
    }
	
  }
  
	event void ReadHumidity.readDone(error_t ok, uint16_t data) {
	if (ok == SUCCESS) 
	{
		fillPacket();
		local.Humidityreading = data;
	}
	else
	{
      data = 0xffff;
      report_problem();
    }
	
  }
  
	uint16_t getRssi(message_t *msg){
		local.RSSI = (uint16_t) call CC2420Packet.getRssi(msg);
		rssiIsRead = TRUE;
		return local.RSSI;
	}
  
 	event message_t* MobileReceive.receive(message_t* msg, void* payload, uint8_t len) {
	
		oscilloscope_t *in = (oscilloscope_t*)payload;
		oscilloscope_t *out; 
		
		report_received();
		if(!sendBusy)
		{
			out = (oscilloscope_t *)call CollectSend.getPayload(&sendbuf);
			if (len != sizeof(oscilloscope_t)) 
			{
				return msg;
			}
			else 
			{
				memcpy(out, in, sizeof(oscilloscope_t));
			}
			
			post collectSendTask();
		}
		else
		{
			message_t *newmsg = call RadioMessagePool.get();
			if (newmsg == NULL) 
			{
				report_problem();
				return msg;
			}
			
			out = (oscilloscope_t*)call CollectSend.getPayload(newmsg);
			memcpy(out, in, sizeof(oscilloscope_t));
			
			if (call RadioQueue.enqueue(newmsg) != SUCCESS) 
			{
				// drop the message on the floor and hang if we run out of
				// queue space without running out of queue space first (this
				// should not occur).
		        call RadioMessagePool.put(newmsg);
				fatal_problem();
				return msg;
			}
		} 
		sendBusy = TRUE;
		return msg;
	}

  // Use LEDs to report various status issues.
  static void fatal_problem(){ 
    call Leds.led0On(); 
    call Leds.led1On();
    call Leds.led2On();
    call Timer.stop(); 

  }

  static void report_problem() {
	call Leds.led0Toggle(); 
  }
  
  static void report_sent(){ 
	call Leds.led1Toggle(); 
  }
  
  static void report_received(){ 
	call Leds.led2Toggle(); 
  }
}
