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

configuration AnchorNodeAppC { }
implementation
{
  components AnchorNodeC, MainC, ActiveMessageC, LedsC,
    new TimerMilliC(), new DemoSensorC() as Sensor, 
    new AMSenderC(AM_OSCILLOSCOPE), PrintfC; 

  AnchorNodeC.Boot -> MainC;
  AnchorNodeC.RadioControl -> ActiveMessageC;
  AnchorNodeC.AMSend -> AMSenderC;
  AnchorNodeC.Timer -> TimerMilliC;
  AnchorNodeC.Read -> Sensor;
  AnchorNodeC.Leds -> LedsC;
  AnchorNodeC.PrintfControl -> PrintfC;
  AnchorNodeC.PrintfFlush -> PrintfC;	 
}
