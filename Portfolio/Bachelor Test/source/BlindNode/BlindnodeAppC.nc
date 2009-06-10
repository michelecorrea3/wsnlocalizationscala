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
#include <UserButton.h>

configuration BlindnodeAppC { }
implementation
{
  components BlindnodeC, MainC, ActiveMessageC, LedsC,
    PrintfC, CC2420ActiveMessageC as Radio,
    //new DemoSensorC() as Sensor, 
    //new AMSenderC(AM_OSCILLOSCOPE), 
    //  new TimerMilliC(),
    new AMReceiverC(AM_OSCILLOSCOPE);
    components UserButtonC;

  BlindnodeC.Boot -> MainC;
  BlindnodeC.RadioControl -> ActiveMessageC;
  //BlindnodeC.AMSend -> AMSenderC;
  BlindnodeC.Receive -> AMReceiverC;
  //BlindnodeC.Read -> Sensor;
  BlindnodeC.Leds -> LedsC;
  BlindnodeC.PrintfControl -> PrintfC;
  BlindnodeC.PrintfFlush -> PrintfC;	
  BlindnodeC.CC2420Packet -> Radio;
  //BlindnodeC.Timer -> TimerMilliC;
  //BlindnodeC.Get -> UserButtonC;
  BlindnodeC.Notify -> UserButtonC;
  
}
