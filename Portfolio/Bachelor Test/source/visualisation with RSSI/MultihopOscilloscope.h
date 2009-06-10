#ifndef MULTIHOP_OSCILLOSCOPE_H
	#define MULTIHOP_OSCILLOSCOPE_H
	#include "AM.h"
//	#define COLLECTION_PROTOCOL 		//  cf TEP 119: Collection.
//	#define DISSEMINATION_PROTOCOL		//  cf TEP 118: Dissemination.

enum {
  /* Number of readings per message. If you increase this, you may have to
     increase the message_t size. */
//  NREADINGS = 5,
  /* Default sampling period. */
  DEFAULT_INTERVAL = 5024,
  AM_OSCILLOSCOPE = 0x93,
  AM_BROADCAST = 0x72,
  NO_REPLY = 0x00,
};

typedef nx_struct oscilloscope {
  		nx_am_addr_t moteId; 			/* Mote id of sending mote. */
		nx_uint16_t count; 				/* The readings are samples count * NREADINGS onwards */
		nx_uint16_t quality;			/* The quality of the link */
		nx_am_addr_t parentId;			/* The parent id of the node */
		nx_uint8_t reply;
 
		nx_uint16_t Voltreading;
		nx_uint16_t Lightreading;
		nx_uint16_t Tempreading;
		nx_uint16_t Humidityreading;
		nx_uint16_t RSSI;
		
} oscilloscope_t;

#endif
