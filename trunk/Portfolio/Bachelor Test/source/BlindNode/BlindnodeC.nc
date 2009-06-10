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

#include "printf.h"
#include "Timer.h"
#include "Blindnode.h"
#include <UserButton.h>

module BlindnodeC
{
  uses {
    interface Boot;
    interface SplitControl as RadioControl;
    //interface AMSend;
    interface Receive;
    //interface Timer<TMilli> as Timer;
    //interface Read<uint16_t>;
    interface Leds;
    //interface voor de RSSI
    interface CC2420Packet;
    interface SplitControl as PrintfControl;
    interface PrintfFlush;	
    interface Notify<button_state_t>;
    //interfaces voor de user button
    //interface Get<button_state_t>;
    
  }
}
implementation
{
  int8_t RSSIvals[NREADINGS];
  int8_t reading = 0;
  int8_t RSSIval_filter_old;
  int8_t RSSIval_filter_new;
  int8_t direction = 1;
  bool wait = FALSE;
  bool first = TRUE;

  int8_t i;

  /* Current local state - interval, version and accumulated readings */
  oscilloscope_t local;

  // Use LEDs to report various status issues.
  void report_problem() { call Leds.led0On(); }
  void report_sent() { call Leds.led1Toggle(); }
  void report_received() { call Leds.led2Toggle(); }

  event void Boot.booted() {
    if (call RadioControl.start() != SUCCESS)
      report_problem();
    if (call PrintfControl.start() != SUCCESS)
      report_problem();

    call Notify.enable();
  }


  event void RadioControl.startDone(error_t error) {
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


  event void Notify.notify( button_state_t state ) {
    if ( state == BUTTON_PRESSED ) {
      wait = FALSE;  
      printf("Userbutton is pressed\n");
	 call PrintfFlush.flush();
    }
  }

  void filter(){
    int16_t temp_total = 0;
    for(i=0;i<NREADINGS;i++){
        temp_total += RSSIvals[i]; 
    }
    RSSIval_filter_new = temp_total / NREADINGS;  
  }

  void movement(){  
  if (first){
	RSSIval_filter_old = RSSIval_filter_new;
	first = FALSE;
  }
  printf("RSSIval_filter_new = %i\n", RSSIval_filter_new);  

  if (RSSIval_filter_new < RSSIval_filter_old){
	printf("RSSI is verslechterd!\n");
	direction++;
	if (direction >= 5)
		direction = 1;
  }	 
  else {
	printf("RSSI is verbeterd!\n"); 
  }

  printf("Druk op de knop als je klaar bent!\n");
  if (direction == 1)
	printf("Vooruit\n"); 
  else if (direction == 2)
	printf("Achteruit\n");
  else if (direction == 3)
	printf("Links\n");
  else if (direction == 4)
	printf("Rechts\n");

  RSSIval_filter_old = RSSIval_filter_new;
  }

 

  event message_t* Receive.receive(message_t* msg, void* payload, uint8_t len) {
    oscilloscope_t *omsg = payload;
    report_received();

    if ( !wait ){
     local.warning = omsg->warning;

     if ( local.warning == 1 )
     	 RSSIvals[reading++] = call CC2420Packet.getRssi(msg);

     if ( reading == NREADINGS ){
   	 filter();
	 reading = 0; 
	
         //distance();
     	 wait = TRUE;
         
         if (RSSIval_filter_new > 10){
	 	printf("Doel bereikt!\n");
	        printf("Proficiat!\n");
         }
	 else
	        movement(); 
	 call PrintfFlush.flush();
     }
    }
    return msg;
    
  }




}
