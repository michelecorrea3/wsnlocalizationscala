/*
 * Copyright (c) 2006 Intel Corporation
 * All rights reserved.
 *
 * This file is distributed under the terms in the attached INTEL-LICENSE     
 * file. If you do not find these files, copies can be found by writing to
 * Intel Research Berkeley, 2150 Shattuck Avenue, Suite 1300, Berkeley, CA, 
 * 94704.  Attention:  Intel License Inquiry.
 */

// @author David Gay
//@author Peter De Cauwer & Tim van Overtveldt


#include "Timer.h"
#include "AnchorNode.h"
#include "printf.h"

module AnchorNodeC
{
  uses {
    interface Boot;
    interface SplitControl as RadioControl;
    interface AMSend;
    interface Timer<TMilli>;
    interface Read<uint16_t>;
    interface Leds;
    interface SplitControl as PrintfControl;
    interface PrintfFlush;
  }
}
implementation
{
  message_t sendbuf;
  bool sendbusy;

  /* Current local state - warning, node id */
  oscilloscope_t local;

   
  // Use LEDs to report various status issues.
  void report_problem() { call Leds.led0Toggle(); }
  void report_sent() { call Leds.led1Toggle(); }
  void report_event() { call Leds.led2Toggle(); }

  event void Boot.booted() {
    local.id = TOS_NODE_ID;
    if (call RadioControl.start() != SUCCESS)
      report_problem();
    if (call PrintfControl.start() != SUCCESS)
      report_problem();
  }

  void startTimer() {
    call Timer.startPeriodic(DEFAULT_INTERVAL);
  }

  event void RadioControl.startDone(error_t error) {
    startTimer();
  }

  event void RadioControl.stopDone(error_t error) {
  }

  event void PrintfControl.startDone(error_t error) {
  	printf("Hi I am writing to you from my TinyOS application!!\n");
    	call PrintfFlush.flush();
  }

  event void PrintfControl.stopDone(error_t error) {
    	printf("This should not be printed...");
  	call PrintfFlush.flush();
  }
  
  event void PrintfFlush.flushDone(error_t error) {
  	    }

  
  event void Timer.fired() {
 	if ( local.warning == 1 )
   	 {
	if (!sendbusy && sizeof local <= call AMSend.maxPayloadLength())
	  {
	    memcpy(call AMSend.getPayload(&sendbuf), &local, sizeof local);
	    if (call AMSend.send(AM_BROADCAST_ADDR, &sendbuf, sizeof local) == SUCCESS)
	      sendbusy = TRUE;
	  }
	if (!sendbusy)
	  report_problem();
      }
    if (call Read.read() != SUCCESS)
      report_problem();
  }

  event void AMSend.sendDone(message_t* msg, error_t error) {
    if (error == SUCCESS)
      report_sent();
    else
      report_problem();

    sendbusy = FALSE;
  }

  event void Read.readDone(error_t result, uint16_t data) {
    if (result != SUCCESS)
      {
	data = 0xffff;
	report_problem();
      }
      if ( data > MAX_SENS_VAL )
	local.warning = 1;
      printf("Sensorwaarde = %u\n",data);
      printf("Warning = %u\n",local.warning);
      call PrintfFlush.flush();
  }
}
