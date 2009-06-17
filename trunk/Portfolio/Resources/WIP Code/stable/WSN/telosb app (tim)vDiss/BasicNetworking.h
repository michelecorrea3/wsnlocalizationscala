#ifndef BASIC_NETWORKING_H
#define BASIC_NETWORKING_H
#include <AM.h>

enum {
  AM_COLLMSG = 0x93,
  AM_LOCMSG = 0x90,
  AM_STATMSG = 0x82,
  AM_DISSMSG = 0x71,
  AM_OSCILLOSCOPE = 0x94,
  BROADCAST_DIS_KEY = 42,
  DEFAULT_SAMPLING_PERIOD = 100000,
  DEFAULT_LOC_PERIOD = 10000,
  MAXIMUM_NUMBER_MOTES = 5,
  LOC_PERIOD = 0x11,
  SAMPLING_PERIOD = 0x22,
  ACTIVE_REQUEST = 0x33,
  AN_REQUEST = 0x44,
  LED_REQUEST = 0x55,
  SLEEP_REQUEST = 0x66,
  POWER_REQUEST = 0x77,
  FREQUENCY_REQUEST = 0x88,
  POSX_REQUEST = 0x99,
  POSY_REQUEST = 0xAA,
  STATUS_REQUEST = 0xBB
};

typedef nx_struct CollMsg { 
  nx_am_addr_t moteid; // moteID of sending mote
  nx_uint8_t type; //type of message: eg. 1 for sens data, 2 for localization data, 3 for status
  nx_uint16_t battery; //demosensor/battery level
  nx_uint16_t lightreading; // sense
  nx_uint16_t tempreading;
  nx_uint16_t humidityreading;
  nx_bool button;
} CollMsg_t;

typedef nx_struct StatMsg { 
  nx_am_addr_t moteid; // moteID of sending mote
  nx_uint8_t type; //type of message: eg. 1 for sens data, 2 for localization data, 3 for status
  nx_bool active; //participating in the localization or not?
  nx_bool AN; //AN or BN / TRUE or FALSE
  nx_uint16_t posx; /*x coordinate of the anchor mote*/
  nx_uint16_t posy; /*x coordinate of the anchor mote*/  
  nx_uint16_t sampleRate;
  nx_uint16_t locRate;
  nx_uint8_t leds;
  nx_uint8_t power;
  nx_uint8_t frequency;
  nx_uint8_t conn; //containts the number of motes the BN is connected to... EXP 
} StatMsg_t;

typedef nx_struct LocMsg { 
  nx_am_addr_t moteid; // moteID of sending mote
  nx_am_addr_t ANmoteid; //type of message: eg. 1 for sens data, 2 for localization data, 3 for status
  nx_uint8_t type; //type of message: eg. 1 for sens data, 2 for localization data, 3 for status
  nx_uint16_t posx; /*x coordinate of the blind mote*/
  nx_uint16_t posy; /*y coordinate of the blind mote*/
  nx_int8_t RSSI;
} LocMsg_t;

/* POSSIBLE REQUESTS
 * 
 * CHANGE_SAMPLING_PERIOD 
 * request = 0xAA
 * parameter = <TMilli>
 *
 * LED_REQUEST
 * request = 0x55
 * parameter = 	0 (no leds)
 *                      	1 (R)
 *		    	2 (Y)
 *			3 (RY)
 *			4 (B)
 *			5 (BR)
 *			6 (BY)
 *			7 (RYB)
 *
 * SLEEP_REQUEST
 * request = 0x66
 * parameter = 
 */

typedef nx_struct DissMsg {
  nx_am_addr_t targetid; // moteID of targetmote
  nx_uint8_t request; // see above
  nx_uint16_t parameter; // parameter for request
} DissMsg_t;

typedef nx_struct broadcast {
  nx_am_addr_t id; /* Mote id of sending mote. */
  nx_uint16_t posx; /*x coordinate of the anchor mote*/
  nx_uint16_t posy; /*x coordinate of the anchor mote*/
} broadcast_t;
#endif BASIC_NETWORKING_H