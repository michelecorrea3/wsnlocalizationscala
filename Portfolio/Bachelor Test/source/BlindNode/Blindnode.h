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

#ifndef Blindnode_H
#define Blindnode_H

enum {
  /* Default sampling period. */
   NREADINGS = 10,

  AM_OSCILLOSCOPE = 0x93,

  MAX_SENS_VAL = 1000	
};

typedef nx_struct oscilloscope {
  nx_uint16_t id; /* Mote id of sending mote. */
  nx_bool	warning;  /* Warning or Not */
} oscilloscope_t;

#endif
