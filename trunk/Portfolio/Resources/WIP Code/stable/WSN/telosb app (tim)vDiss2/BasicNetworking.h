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
  nx_uint8_t type; //empty field
  nx_uint16_t battery; //demosensor/battery level
  nx_uint16_t lightreading; // Light meas.
  nx_uint16_t tempreading;  // Temp. meas.
  nx_uint16_t humidityreading; // Humidity meas.
  nx_bool button;			// Button pressed (1=YES / 0=NO
} CollMsg_t;

typedef nx_struct StatMsg { 
  nx_am_addr_t moteid; // moteID of sending mote
  nx_uint8_t type; // Request reply or Reboot reply
  nx_bool active; // Participating in the localization or not?
  nx_bool AN; // AN=TRUE / BN=FALSE
  nx_uint16_t posx; // x coordinate of the node
  nx_uint16_t posy; // y coordinate of the node
  nx_uint16_t sampleRate; // Period of sensor message
  nx_uint16_t locRate; // Period of location message
  nx_uint8_t leds; // Leds (0 to 7)
  nx_uint8_t power; // Power (0 to 31)
  nx_uint8_t frequency; // Frequency (11 to 26)
  nx_uint8_t conn; // Containts the number of motes the BN is connected to... EXP 
} StatMsg_t;

typedef nx_struct LocMsg { 
  nx_am_addr_t moteid; // moteID of sending mote
  nx_am_addr_t ANmoteid; // moteID of anchor node
  nx_uint8_t type; //empty field
  nx_uint16_t posx; // x coordinate of the blind mote
  nx_uint16_t posy; // y coordinate of the blind mote
  nx_int8_t RSSI; // Received Signal Strength Indication
} LocMsg_t;

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