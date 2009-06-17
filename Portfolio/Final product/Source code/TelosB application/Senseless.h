#ifndef SENSELESS_H
#define SENSELESS_H
#include <AM.h>

enum {
  AM_COLLMSG = 0x93,					// acts as the port which the sensor message will be sent to
  AM_LOCMSG = 0x90,						// acts as the port which the location message will be sent to
  AM_STATMSG = 0x82,					// acts as the port which the status message will be sent to
  AM_DISSMSG = 0x71,					// acts as the port which the dissemination message will be sent to
  AM_OSCILLOSCOPE = 0x94,				// acts as the port which the beacon/broadcast message will be sent to
  BROADCAST_DIS_KEY = 42,				// key for dissemination
  DEFAULT_SAMPLING_PERIOD = 10000,		// period for sampling the sensors of the node
  DEFAULT_LOC_PERIOD = 10000,			// period for broadcasting beacon message
  //MAXIMUM_NUMBER_MOTES = 5,
  LOC_PERIOD = 0x11,					// request to change location period
  SAMPLING_PERIOD = 0x22,				// request to change sampling period of the sensors
  ACTIVE_REQUEST = 0x33,				// request to change the status of active
  AN_REQUEST = 0x44,					// request to change the node to an anchor or blind node
  LED_REQUEST = 0x55,					// request to change the leds
  //SLEEP_REQUEST = 0x66,	
  POWER_REQUEST = 0x77,					// request to change the power level of transmission
  FREQUENCY_REQUEST = 0x88,				// request to change the frequency
  POSX_REQUEST = 0x99,					// request to change the x coordinate of the node
  POSY_REQUEST = 0xAA,					// request to change the y coordinate of the node
  STATUS_REQUEST = 0xBB					// request to receive a status report
};

// Struct for sensor measurements
typedef nx_struct CollMsg { 
  nx_am_addr_t moteid; 				// MoteID of sending mote
  nx_uint8_t type; 					// Empty field
  nx_uint16_t battery; 				// Demosensor: voltage of node
  nx_uint16_t lightreading; 		// Light measurement
  nx_uint16_t tempreading;  		// Temperature measurement
  nx_uint16_t humidityreading; 		// Humidity measurement
  nx_bool button;					// Button pressed (1=YES / 0=NO)
} CollMsg_t;

// Struct for the status of the node
typedef nx_struct StatMsg { 
  nx_am_addr_t moteid; 				// MoteID of sending mote
  nx_uint8_t type; 					// Empty field
  nx_bool active; 					// Active in the localization process? (TRUE = yes & FALSE = no)
  nx_bool AN; 						// Anchor Node? (TRUE = Anchor Node & FALSE = Blind Node)
  nx_uint16_t posx; 				// x coordinate of the node
  nx_uint16_t posy; 				// y coordinate of the node
  nx_uint32_t sampleRate; 			// Period of sensor message
  nx_uint16_t locRate; 				// Period of location message
  nx_uint8_t leds; 					// Leds (0 to 7)
  nx_uint8_t power; 				// Power level (0 to 31)
  nx_uint8_t frequency; 			// Frequency (11 to 26)
  nx_uint8_t conn; 					// Contains the number of motes the BN is connected to
} StatMsg_t;

// Struct for data that is needed for the localization process
typedef nx_struct LocMsg { 
  nx_am_addr_t moteid; 				// MoteID of sending mote
  nx_am_addr_t ANmoteid; 			// MoteID of anchor node
  nx_uint8_t VANs;					// Grade of the anchor node of the source (0= BN, 1=AN and 2=VAN)
  nx_uint8_t VANr;					// Grade of the anchor node of the receiver (0= BN, 1=AN and 2=VAN)  
  nx_uint8_t hopCount;				// Hop count
  nx_int8_t RSSI; 					// Received Signal Strength Indication
} LocMsg_t;

// Struct for the message that will be disseminated through the network
typedef nx_struct DissMsg {
  nx_am_addr_t targetid; 			// MoteID of targetmote
  nx_uint8_t request; 				// Code for possible request
  nx_uint16_t parameter; 			// Parameter for the request
} DissMsg_t;

// Struct for the beacon that the anchor nodes transmit to the blind nodes
typedef nx_struct broadcast {
  nx_am_addr_t id; 					// MoteID of sending mote.
  nx_uint16_t hopCount; 			// hopCount
  nx_uint16_t VAN	 				// grade of anchor
} broadcast_t;

#endif SENSELESS_H