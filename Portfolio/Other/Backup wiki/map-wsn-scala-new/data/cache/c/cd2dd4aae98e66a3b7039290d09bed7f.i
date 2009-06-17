a:92:{i:0;a:3:{i:0;s:14:"document_start";i:1;a:0:{}i:2;i:0;}i:1;a:3:{i:0;s:6:"p_open";i:1;a:0:{}i:2;i:0;}i:2;a:3:{i:0;s:7:"p_close";i:1;a:0:{}i:2;i:1;}i:3;a:3:{i:0;s:12:"section_edit";i:1;a:4:{i:0;i:-1;i:1;i:0;i:2;i:1;i:3;s:0:"";}i:2;i:1;}i:4;a:3:{i:0;s:6:"header";i:1;a:3:{i:0;s:12:"Messages WSN";i:1;i:1;i:2;i:1;}i:2;i:1;}i:5;a:3:{i:0;s:12:"section_open";i:1;a:1:{i:0;i:1;}i:2;i:1;}i:6;a:3:{i:0;s:13:"section_close";i:1;a:0:{}i:2;i:29;}i:7;a:3:{i:0;s:12:"section_edit";i:1;a:4:{i:0;i:1;i:1;i:28;i:2;i:1;i:3;s:12:"Messages WSN";}i:2;i:29;}i:8;a:3:{i:0;s:6:"header";i:1;a:3:{i:0;s:12:"Senseless v4";i:1;i:2;i:2;i:29;}i:2;i:29;}i:9;a:3:{i:0;s:12:"section_open";i:1;a:1:{i:0;i:2;}i:2;i:29;}i:10;a:3:{i:0;s:6:"p_open";i:1;a:0:{}i:2;i:53;}i:11;a:3:{i:0;s:5:"cdata";i:1;a:1:{i:0;s:38:"Inserted calibration & MAC & hop count";}i:2;i:54;}i:12;a:3:{i:0;s:7:"p_close";i:1;a:0:{}i:2;i:92;}i:13;a:3:{i:0;s:13:"section_close";i:1;a:0:{}i:2;i:94;}i:14;a:3:{i:0;s:12:"section_edit";i:1;a:4:{i:0;i:29;i:1;i:93;i:2;i:2;i:3;s:12:"Senseless v4";}i:2;i:94;}i:15;a:3:{i:0;s:6:"header";i:1;a:3:{i:0;s:7:"StatMsg";i:1;i:3;i:2;i:94;}i:2;i:94;}i:16;a:3:{i:0;s:12:"section_open";i:1;a:1:{i:0;i:3;}i:2;i:94;}i:17;a:3:{i:0;s:4:"code";i:1;a:2:{i:0;s:633:"
typedef nx_struct StatMsg { 
  nx_am_addr_t moteid; // moteID of sending mote
  nx_uint8_t MAC; // MAC address
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
";i:1;N;}i:2;i:118;}i:18;a:3:{i:0;s:13:"section_close";i:1;a:0:{}i:2;i:761;}i:19;a:3:{i:0;s:12:"section_edit";i:1;a:4:{i:0;i:94;i:1;i:760;i:2;i:3;i:3;s:7:"StatMsg";}i:2;i:761;}i:20;a:3:{i:0;s:6:"header";i:1;a:3:{i:0;s:6:"LocMsg";i:1;i:3;i:2;i:761;}i:2;i:761;}i:21;a:3:{i:0;s:12:"section_open";i:1;a:1:{i:0;i:3;}i:2;i:761;}i:22;a:3:{i:0;s:4:"code";i:1;a:2:{i:0;s:344:"
typedef nx_struct LocMsg { 
  nx_am_addr_t moteid; // moteID of sending mote
  nx_am_addr_t ANmoteid; // moteID of the reference node
  nx_uint8_t ANs; // grade of anchor of the source
  nx_uint8_t ANr; // grade of anchor of the receiver
  nx_uint8_t hopCount; // hop count
  nx_int8_t RSSI; // Received Signal Strength Indication
} LocMsg_t;
";i:1;N;}i:2;i:784;}i:23;a:3:{i:0;s:13:"section_close";i:1;a:0:{}i:2;i:1138;}i:24;a:3:{i:0;s:12:"section_edit";i:1;a:4:{i:0;i:761;i:1;i:1137;i:2;i:3;i:3;s:6:"LocMsg";}i:2;i:1138;}i:25;a:3:{i:0;s:6:"header";i:1;a:3:{i:0;s:12:"BroadcastMsg";i:1;i:3;i:2;i:1138;}i:2;i:1138;}i:26;a:3:{i:0;s:12:"section_open";i:1;a:1:{i:0;i:3;}i:2;i:1138;}i:27;a:3:{i:0;s:4:"code";i:1;a:2:{i:0;s:167:"
typedef nx_struct broadcast {
  nx_am_addr_t id; /* Mote id of sending mote. */
  nx_uint8_t hopCount // hop count
  nx_uint8_t VAR // grade of anchor
} broadcast_t;
";i:1;N;}i:2;i:1167;}i:28;a:3:{i:0;s:13:"section_close";i:1;a:0:{}i:2;i:1344;}i:29;a:3:{i:0;s:12:"section_edit";i:1;a:4:{i:0;i:1138;i:1;i:1343;i:2;i:3;i:3;s:12:"BroadcastMsg";}i:2;i:1344;}i:30;a:3:{i:0;s:6:"header";i:1;a:3:{i:0;s:12:"Senseless v3";i:1;i:2;i:2;i:1344;}i:2;i:1344;}i:31;a:3:{i:0;s:12:"section_open";i:1;a:1:{i:0;i:2;}i:2;i:1344;}i:32;a:3:{i:0;s:13:"section_close";i:1;a:0:{}i:2;i:1370;}i:33;a:3:{i:0;s:12:"section_edit";i:1;a:4:{i:0;i:1344;i:1;i:1369;i:2;i:2;i:3;s:12:"Senseless v3";}i:2;i:1370;}i:34;a:3:{i:0;s:6:"header";i:1;a:3:{i:0;s:22:"Possible future fields";i:1;i:3;i:2;i:1370;}i:2;i:1370;}i:35;a:3:{i:0;s:12:"section_open";i:1;a:1:{i:0;i:3;}i:2;i:1370;}i:36;a:3:{i:0;s:10:"listu_open";i:1;a:0:{}i:2;i:1402;}i:37;a:3:{i:0;s:13:"listitem_open";i:1;a:1:{i:0;i:1;}i:2;i:1402;}i:38;a:3:{i:0;s:16:"listcontent_open";i:1;a:0:{}i:2;i:1402;}i:39;a:3:{i:0;s:5:"cdata";i:1;a:1:{i:0;s:9:" Hopcount";}i:2;i:1406;}i:40;a:3:{i:0;s:17:"listcontent_close";i:1;a:0:{}i:2;i:1415;}i:41;a:3:{i:0;s:14:"listitem_close";i:1;a:0:{}i:2;i:1415;}i:42;a:3:{i:0;s:13:"listitem_open";i:1;a:1:{i:0;i:1;}i:2;i:1415;}i:43;a:3:{i:0;s:16:"listcontent_open";i:1;a:0:{}i:2;i:1415;}i:44;a:3:{i:0;s:5:"cdata";i:1;a:1:{i:0;s:40:" Counter for the dissemination from the ";}i:2;i:1419;}i:45;a:3:{i:0;s:7:"acronym";i:1;a:1:{i:0;s:3:"GUI";}i:2;i:1459;}i:46;a:3:{i:0;s:17:"listcontent_close";i:1;a:0:{}i:2;i:1462;}i:47;a:3:{i:0;s:14:"listitem_close";i:1;a:0:{}i:2;i:1462;}i:48;a:3:{i:0;s:13:"listitem_open";i:1;a:1:{i:0;i:1;}i:2;i:1462;}i:49;a:3:{i:0;s:16:"listcontent_open";i:1;a:0:{}i:2;i:1462;}i:50;a:3:{i:0;s:5:"cdata";i:1;a:1:{i:0;s:12:" MAC address";}i:2;i:1466;}i:51;a:3:{i:0;s:17:"listcontent_close";i:1;a:0:{}i:2;i:1478;}i:52;a:3:{i:0;s:14:"listitem_close";i:1;a:0:{}i:2;i:1478;}i:53;a:3:{i:0;s:13:"listitem_open";i:1;a:1:{i:0;i:1;}i:2;i:1478;}i:54;a:3:{i:0;s:16:"listcontent_open";i:1;a:0:{}i:2;i:1478;}i:55;a:3:{i:0;s:5:"cdata";i:1;a:1:{i:0;s:9:" ParentID";}i:2;i:1482;}i:56;a:3:{i:0;s:17:"listcontent_close";i:1;a:0:{}i:2;i:1491;}i:57;a:3:{i:0;s:14:"listitem_close";i:1;a:0:{}i:2;i:1491;}i:58;a:3:{i:0;s:13:"listitem_open";i:1;a:1:{i:0;i:1;}i:2;i:1491;}i:59;a:3:{i:0;s:16:"listcontent_open";i:1;a:0:{}i:2;i:1491;}i:60;a:3:{i:0;s:5:"cdata";i:1;a:1:{i:0;s:4:" LQI";}i:2;i:1495;}i:61;a:3:{i:0;s:17:"listcontent_close";i:1;a:0:{}i:2;i:1499;}i:62;a:3:{i:0;s:14:"listitem_close";i:1;a:0:{}i:2;i:1499;}i:63;a:3:{i:0;s:11:"listu_close";i:1;a:0:{}i:2;i:1499;}i:64;a:3:{i:0;s:13:"section_close";i:1;a:0:{}i:2;i:1501;}i:65;a:3:{i:0;s:12:"section_edit";i:1;a:4:{i:0;i:1370;i:1;i:1500;i:2;i:3;i:3;s:22:"Possible future fields";}i:2;i:1501;}i:66;a:3:{i:0;s:6:"header";i:1;a:3:{i:0;s:7:"CollMsg";i:1;i:3;i:2;i:1501;}i:2;i:1501;}i:67;a:3:{i:0;s:12:"section_open";i:1;a:1:{i:0;i:3;}i:2;i:1501;}i:68;a:3:{i:0;s:4:"code";i:1;a:2:{i:0;s:424:"
typedef nx_struct CollMsg { 
  nx_am_addr_t moteid; // moteID of sending mote
  nx_uint8_t type; //type of message: eg. 1 for sens data, 2 for localization data, 3 for status
  nx_uint16_t battery; //demosensor/battery level
  nx_uint16_t lightreading; // Light meas.
  nx_uint16_t tempreading;  // Temp. meas.
  nx_uint16_t humidityreading; // Humidity meas.
  nx_bool button;	// Button pressed (1=YES / 0=NO
} CollMsg_t;
";i:1;N;}i:2;i:1525;}i:69;a:3:{i:0;s:13:"section_close";i:1;a:0:{}i:2;i:1959;}i:70;a:3:{i:0;s:12:"section_edit";i:1;a:4:{i:0;i:1501;i:1;i:1958;i:2;i:3;i:3;s:7:"CollMsg";}i:2;i:1959;}i:71;a:3:{i:0;s:6:"header";i:1;a:3:{i:0;s:7:"StatMsg";i:1;i:3;i:2;i:1959;}i:2;i:1959;}i:72;a:3:{i:0;s:12:"section_open";i:1;a:1:{i:0;i:3;}i:2;i:1959;}i:73;a:3:{i:0;s:4:"code";i:1;a:2:{i:0;s:652:"
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
";i:1;N;}i:2;i:1983;}i:74;a:3:{i:0;s:13:"section_close";i:1;a:0:{}i:2;i:2645;}i:75;a:3:{i:0;s:12:"section_edit";i:1;a:4:{i:0;i:1959;i:1;i:2644;i:2;i:3;i:3;s:7:"StatMsg";}i:2;i:2645;}i:76;a:3:{i:0;s:6:"header";i:1;a:3:{i:0;s:6:"LocMsg";i:1;i:3;i:2;i:2645;}i:2;i:2645;}i:77;a:3:{i:0;s:12:"section_open";i:1;a:1:{i:0;i:3;}i:2;i:2645;}i:78;a:3:{i:0;s:4:"code";i:1;a:2:{i:0;s:338:"
typedef nx_struct LocMsg { 
  nx_am_addr_t moteid; // moteID of sending mote
  nx_am_addr_t ANmoteid; // moteID of anchor node
  nx_uint8_t type; //empty field
  nx_uint16_t posx; // x coordinate of the blind mote
  nx_uint16_t posy; // y coordinate of the blind mote
  nx_int8_t RSSI; // Received Signal Strength Indication
} LocMsg_t;
";i:1;N;}i:2;i:2668;}i:79;a:3:{i:0;s:13:"section_close";i:1;a:0:{}i:2;i:3016;}i:80;a:3:{i:0;s:12:"section_edit";i:1;a:4:{i:0;i:2645;i:1;i:3015;i:2;i:3;i:3;s:6:"LocMsg";}i:2;i:3016;}i:81;a:3:{i:0;s:6:"header";i:1;a:3:{i:0;s:7:"DissMsg";i:1;i:3;i:2;i:3016;}i:2;i:3016;}i:82;a:3:{i:0;s:12:"section_open";i:1;a:1:{i:0;i:3;}i:2;i:3016;}i:83;a:3:{i:0;s:4:"code";i:1;a:2:{i:0;s:176:"
typedef nx_struct DissMsg {
  nx_am_addr_t targetid; // moteID of targetmote
  nx_uint8_t request; // see above
  nx_uint16_t parameter; // parameter for request
} DissMsg_t;
";i:1;N;}i:2;i:3040;}i:84;a:3:{i:0;s:13:"section_close";i:1;a:0:{}i:2;i:3226;}i:85;a:3:{i:0;s:12:"section_edit";i:1;a:4:{i:0;i:3016;i:1;i:3225;i:2;i:3;i:3;s:7:"DissMsg";}i:2;i:3226;}i:86;a:3:{i:0;s:6:"header";i:1;a:3:{i:0;s:12:"BroadcastMsg";i:1;i:3;i:2;i:3226;}i:2;i:3226;}i:87;a:3:{i:0;s:12:"section_open";i:1;a:1:{i:0;i:3;}i:2;i:3226;}i:88;a:3:{i:0;s:4:"code";i:1;a:2:{i:0;s:208:"
typedef nx_struct broadcast {
  nx_am_addr_t id; /* Mote id of sending mote. */
  nx_uint16_t posx; /*x coordinate of the anchor mote*/
  nx_uint16_t posy; /*x coordinate of the anchor mote*/
} broadcast_t;
";i:1;N;}i:2;i:3255;}i:89;a:3:{i:0;s:13:"section_close";i:1;a:0:{}i:2;i:3472;}i:90;a:3:{i:0;s:12:"section_edit";i:1;a:4:{i:0;i:3226;i:1;i:0;i:2;i:3;i:3;s:12:"BroadcastMsg";}i:2;i:3472;}i:91;a:3:{i:0;s:12:"document_end";i:1;a:0:{}i:2;i:3472;}}