a:118:{i:0;a:3:{i:0;s:14:"document_start";i:1;a:0:{}i:2;i:0;}i:1;a:3:{i:0;s:6:"p_open";i:1;a:0:{}i:2;i:0;}i:2;a:3:{i:0;s:7:"p_close";i:1;a:0:{}i:2;i:1;}i:3;a:3:{i:0;s:12:"section_edit";i:1;a:4:{i:0;i:-1;i:1;i:0;i:2;i:1;i:3;s:0:"";}i:2;i:1;}i:4;a:3:{i:0;s:6:"header";i:1;a:3:{i:0;s:11:"XML schemas";i:1;i:1;i:2;i:1;}i:2;i:1;}i:5;a:3:{i:0;s:12:"section_open";i:1;a:1:{i:0;i:1;}i:2;i:1;}i:6;a:3:{i:0;s:13:"section_close";i:1;a:0:{}i:2;i:26;}i:7;a:3:{i:0;s:12:"section_edit";i:1;a:4:{i:0;i:1;i:1;i:25;i:2;i:1;i:3;s:11:"XML schemas";}i:2;i:26;}i:8;a:3:{i:0;s:6:"header";i:1;a:3:{i:0;s:12:"Senseless V4";i:1;i:2;i:2;i:26;}i:2;i:26;}i:9;a:3:{i:0;s:12:"section_open";i:1;a:1:{i:0;i:2;}i:2;i:26;}i:10;a:3:{i:0;s:13:"section_close";i:1;a:0:{}i:2;i:52;}i:11;a:3:{i:0;s:12:"section_edit";i:1;a:4:{i:0;i:26;i:1;i:51;i:2;i:2;i:3;s:12:"Senseless V4";}i:2;i:52;}i:12;a:3:{i:0;s:6:"header";i:1;a:3:{i:0;s:17:"WSN to Controller";i:1;i:3;i:2;i:52;}i:2;i:52;}i:13;a:3:{i:0;s:12:"section_open";i:1;a:1:{i:0;i:3;}i:2;i:52;}i:14;a:3:{i:0;s:13:"section_close";i:1;a:0:{}i:2;i:79;}i:15;a:3:{i:0;s:6:"header";i:1;a:3:{i:0;s:37:"status message (send if node reboots)";i:1;i:4;i:2;i:79;}i:2;i:79;}i:16;a:3:{i:0;s:12:"section_open";i:1;a:1:{i:0;i:4;}i:2;i:79;}i:17;a:3:{i:0;s:4:"code";i:1;a:2:{i:0;s:1485:"
<?xml version="1.0" encoding="utf-8"?>
<xs:schema id="StatusMessage"
    targetNamespace="http://tempuri.org/StatusMessage.xsd"
    elementFormDefault="qualified"
    xmlns="http://tempuri.org/StatusMessage.xsd"
    xmlns:mstns="http://tempuri.org/StatusMessage.xsd"
    xmlns:xs="http://www.w3.org/2001/XMLSchema"
>
  <xs:element name="StatusMessage"> <!-- Root of the xml-file-->
    <xs:complexType>
      <xs:sequence>
        <xs:element name="Status" maxOccurs="unbounded"> <!-- Each sensor has a single entry with all the measurements of that sensor -->
          <xs:complexType>
            <xs:sequence>
              <xs:element name="ID" /> <!-- ID of node -->
              <xs:element name="Time" /> <!-- Timestamp of the measurements -->
			  <xs:element name="Active" /> <!-- Boolean participating in the localization or not? -->
			  <xs:element name="AN" /> <!-- AN or BN -->
			  <xs:element name="X" /> <!-- x coordinate of the anchor mote -->
			  <xs:element name="Y" /> <!-- y coordinate of the anchor mote -->
			  <xs:element name="SampleRate" /> 
			  <xs:element name="LocRate" /> 
			  <xs:element name="Leds" /> 
			  <xs:element name="Power" />  <!-- Power level -->
			  <xs:element name="Frequency" /> 
			  <xs:element name="Conn" /> <!--containts the number of motes the BN is connected to... EXP -->
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>
";i:1;s:3:"xml";}i:2;i:131;}i:18;a:3:{i:0;s:13:"section_close";i:1;a:0:{}i:2;i:1630;}i:19;a:3:{i:0;s:6:"header";i:1;a:3:{i:0;s:20:"Localization message";i:1;i:4;i:2;i:1630;}i:2;i:1630;}i:20;a:3:{i:0;s:12:"section_open";i:1;a:1:{i:0;i:4;}i:2;i:1630;}i:21;a:3:{i:0;s:4:"code";i:1;a:2:{i:0;s:1391:"
<?xml version="1.0" encoding="utf-8"?>
<xs:schema id="LocationMessage"
    targetNamespace="http://tempuri.org/LocationMessage.xsd"
    elementFormDefault="qualified"
    xmlns="http://tempuri.org/LocationMessage.xsd"
    xmlns:mstns="http://tempuri.org/LocationMessage.xsd"
    xmlns:xs="http://www.w3.org/2001/XMLSchema"
>
  <xs:element name="LocationMessage"> <!-- Root of the xml-file-->
    <xs:complexType>
      <xs:sequence>
        <xs:element name="Location" maxOccurs="unbounded"> <!-- Each sensor has a single entry with all the measurements of that sensor -->
          <xs:complexType>
            <xs:sequence>
              <xs:element name="Node" /> <!-- ID of node -->
			  <xs:element name="ANode" /> <!-- ID of the anchor node -->
                          <xs:element name="Time" /> <!-- Timestamp of the measurements -->
			  <xs:element name="VARs" /> <!-- grade of anchor for the source 0: BN, 1: True AN, 2: 2AN, 1VAN, 3: 1AN, 2VAN, 4: 3VAN-->
			  <xs:element name="VARr" /> <!-- grade of anchor for the receiver -->
			  <xs:element name="RSSI" /> 
			  <xs:element name="hopCount" />  <!-- hop count to the anchor node -->
			  <xs:element name="X" /> 
			  <xs:element name="Y" /> 
			  <xs:element name="Z" /> 
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>
";i:1;s:3:"xml";}i:2;i:1665;}i:22;a:3:{i:0;s:13:"section_close";i:1;a:0:{}i:2;i:3070;}i:23;a:3:{i:0;s:12:"section_edit";i:1;a:4:{i:0;i:52;i:1;i:3069;i:2;i:3;i:3;s:17:"WSN to Controller";}i:2;i:3070;}i:24;a:3:{i:0;s:6:"header";i:1;a:3:{i:0;s:12:"Senseless V3";i:1;i:2;i:2;i:3070;}i:2;i:3070;}i:25;a:3:{i:0;s:12:"section_open";i:1;a:1:{i:0;i:2;}i:2;i:3070;}i:26;a:3:{i:0;s:13:"section_close";i:1;a:0:{}i:2;i:3095;}i:27;a:3:{i:0;s:12:"section_edit";i:1;a:4:{i:0;i:3070;i:1;i:3094;i:2;i:2;i:3;s:12:"Senseless V3";}i:2;i:3095;}i:28;a:3:{i:0;s:6:"header";i:1;a:3:{i:0;s:17:"WSN to Controller";i:1;i:3;i:2;i:3095;}i:2;i:3095;}i:29;a:3:{i:0;s:12:"section_open";i:1;a:1:{i:0;i:3;}i:2;i:3095;}i:30;a:3:{i:0;s:13:"section_close";i:1;a:0:{}i:2;i:3122;}i:31;a:3:{i:0;s:6:"header";i:1;a:3:{i:0;s:37:"status message (send if node reboots)";i:1;i:4;i:2;i:3122;}i:2;i:3122;}i:32;a:3:{i:0;s:12:"section_open";i:1;a:1:{i:0;i:4;}i:2;i:3122;}i:33;a:3:{i:0;s:4:"code";i:1;a:2:{i:0;s:1485:"
<?xml version="1.0" encoding="utf-8"?>
<xs:schema id="StatusMessage"
    targetNamespace="http://tempuri.org/StatusMessage.xsd"
    elementFormDefault="qualified"
    xmlns="http://tempuri.org/StatusMessage.xsd"
    xmlns:mstns="http://tempuri.org/StatusMessage.xsd"
    xmlns:xs="http://www.w3.org/2001/XMLSchema"
>
  <xs:element name="StatusMessage"> <!-- Root of the xml-file-->
    <xs:complexType>
      <xs:sequence>
        <xs:element name="Status" maxOccurs="unbounded"> <!-- Each sensor has a single entry with all the measurements of that sensor -->
          <xs:complexType>
            <xs:sequence>
              <xs:element name="ID" /> <!-- ID of node -->
              <xs:element name="Time" /> <!-- Timestamp of the measurements -->
			  <xs:element name="Active" /> <!-- Boolean participating in the localization or not? -->
			  <xs:element name="AN" /> <!-- AN or BN -->
			  <xs:element name="X" /> <!-- x coordinate of the anchor mote -->
			  <xs:element name="Y" /> <!-- y coordinate of the anchor mote -->
			  <xs:element name="SampleRate" /> 
			  <xs:element name="LocRate" /> 
			  <xs:element name="Leds" /> 
			  <xs:element name="Power" />  <!-- Power level -->
			  <xs:element name="Frequency" /> 
			  <xs:element name="Conn" /> <!--containts the number of motes the BN is connected to... EXP -->
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>
";i:1;s:3:"xml";}i:2;i:3174;}i:34;a:3:{i:0;s:13:"section_close";i:1;a:0:{}i:2;i:4673;}i:35;a:3:{i:0;s:6:"header";i:1;a:3:{i:0;s:25:"Sensormeasurments message";i:1;i:4;i:2;i:4673;}i:2;i:4673;}i:36;a:3:{i:0;s:12:"section_open";i:1;a:1:{i:0;i:4;}i:2;i:4673;}i:37;a:3:{i:0;s:4:"code";i:1;a:2:{i:0;s:2990:"
<?xml version="1.0" encoding="utf-8"?>
<xs:schema id="SensorMeasurements"
    targetNamespace="http://tempuri.org/SensorMeasurements.xsd"
    elementFormDefault="qualified"
    xmlns="http://tempuri.org/SensorMeasurements.xsd"
    xmlns:mstns="http://tempuri.org/SensorMeasurements.xsd"
    xmlns:xs="http://www.w3.org/2001/XMLSchema"
>
  <xs:element name="SensorMeasurements"> <!-- Root of the xml-file-->
    <xs:complexType>
      <xs:sequence>
        <xs:element name="Sensor" maxOccurs="unbounded"> <!-- Each sensor has a single entry with all the measurements of that sensor -->
          <xs:complexType>
            <xs:sequence>
              <xs:element name="ID" /> <!-- ID of node -->
              <xs:element name="Type" /> <!-- Type of node (e.g. TelosB, SunSpot) (0 & 1) --> 
              <xs:element name="Time" /> <!-- Timestamp of the measurements -->
              <xs:element name="Temp" /> <!-- Measurement of temperature -->
              <xs:element name="Humidity" /> <!-- Measurement of humidity -->
              <xs:element name="Light" /> <!-- Measurement of light -->
              <xs:element name="Power" /> <!-- Measurement of power -->
              <xs:element name="AccX"  minOccurs="0" maxOccurs="1" /> <!-- Measurement of X-axis acceleration (SunSpot) -->
              <xs:element name="AccY"  minOccurs="0" maxOccurs="1" /> <!-- Measurement of Y-axis acceleration (SunSpot) -->
              <xs:element name="AccZ"  minOccurs="0" maxOccurs="1" /> <!-- Measurement of Z-axis acceleration (SunSpot) -->
              <xs:element name="TiltX"  minOccurs="0" maxOccurs="1" /> <!-- Measurement of X-axis tilt-angle (SunSpot) -->
              <xs:element name="TiltY"  minOccurs="0" maxOccurs="1" /> <!-- Measurement of Y-axis tilt-angle (SunSpot) -->
              <xs:element name="TiltZ"  minOccurs="0" maxOccurs="1" /> <!-- Measurement of Z-axis tilt-angle (SunSpot) -->
              <xs:element name="LED1"  minOccurs="0" maxOccurs="1" />
              <xs:element name="LED2"  minOccurs="0" maxOccurs="1" />
              <xs:element name="LED3"  minOccurs="0" maxOccurs="1" />
              <xs:element name="LED4"  minOccurs="0" maxOccurs="1" />
              <xs:element name="LED5"  minOccurs="0" maxOccurs="1" />
              <xs:element name="LED6"  minOccurs="0" maxOccurs="1" />
              <xs:element name="LED7"  minOccurs="0" maxOccurs="1" />
              <xs:element name="LED8"  minOccurs="0" maxOccurs="1" />
              <xs:element name="Polling"  minOccurs="0" maxOccurs="1" />
              <xs:element name="Button1" minOccurs="1" maxOccurs="1" /> <!-- Status of button1 of the node  (1 = pressed, 0 = not pressed)-->
              <xs:element name="Button2" minOccurs="1" maxOccurs="1" /> <!-- Status of button2 of the node  (1 = pressed, 0 = not pressed) (SunSpot)-->
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>
";i:1;s:3:"xml";}i:2;i:4712;}i:38;a:3:{i:0;s:13:"section_close";i:1;a:0:{}i:2;i:7716;}i:39;a:3:{i:0;s:6:"header";i:1;a:3:{i:0;s:20:"Localization message";i:1;i:4;i:2;i:7716;}i:2;i:7716;}i:40;a:3:{i:0;s:12:"section_open";i:1;a:1:{i:0;i:4;}i:2;i:7716;}i:41;a:3:{i:0;s:4:"code";i:1;a:2:{i:0;s:1116:"
<?xml version="1.0" encoding="utf-8"?>
<xs:schema id="LocationMessage"
    targetNamespace="http://tempuri.org/LocationMessage.xsd"
    elementFormDefault="qualified"
    xmlns="http://tempuri.org/LocationMessage.xsd"
    xmlns:mstns="http://tempuri.org/LocationMessage.xsd"
    xmlns:xs="http://www.w3.org/2001/XMLSchema"
>
  <xs:element name="LocationMessage"> <!-- Root of the xml-file-->
    <xs:complexType>
      <xs:sequence>
        <xs:element name="Location" maxOccurs="unbounded"> <!-- Each sensor has a single entry with all the measurements of that sensor -->
          <xs:complexType>
            <xs:sequence>
              <xs:element name="Node" /> <!-- ID of node -->
			  <xs:element name="ANode" /> <!-- ID of the anchor node -->
                          <xs:element name="Time" /> <!-- Timestamp of the measurements -->
			  <xs:element name="RSSI" /> 
			  <xs:element name="X" /> 
			  <xs:element name="Y" /> 
			  <xs:element name="Z" /> 
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>
";i:1;s:3:"xml";}i:2;i:7751;}i:42;a:3:{i:0;s:13:"section_close";i:1;a:0:{}i:2;i:8881;}i:43;a:3:{i:0;s:12:"section_edit";i:1;a:4:{i:0;i:3095;i:1;i:8880;i:2;i:3;i:3;s:17:"WSN to Controller";}i:2;i:8881;}i:44;a:3:{i:0;s:6:"header";i:1;a:3:{i:0;s:17:"GUI to Controller";i:1;i:3;i:2;i:8881;}i:2;i:8881;}i:45;a:3:{i:0;s:12:"section_open";i:1;a:1:{i:0;i:3;}i:2;i:8881;}i:46;a:3:{i:0;s:13:"section_close";i:1;a:0:{}i:2;i:8907;}i:47;a:3:{i:0;s:6:"header";i:1;a:3:{i:0;s:10:"DB Request";i:1;i:4;i:2;i:8907;}i:2;i:8907;}i:48;a:3:{i:0;s:12:"section_open";i:1;a:1:{i:0;i:4;}i:2;i:8907;}i:49;a:3:{i:0;s:13:"section_close";i:1;a:0:{}i:2;i:8923;}i:50;a:3:{i:0;s:6:"header";i:1;a:3:{i:0;s:13:"getAllSensors";i:1;i:5;i:2;i:8923;}i:2;i:8923;}i:51;a:3:{i:0;s:12:"section_open";i:1;a:1:{i:0;i:5;}i:2;i:8923;}i:52;a:3:{i:0;s:4:"code";i:1;a:2:{i:0;s:71:"
  <Request>
    <RequestName>getAllSensors</RequestName>
  </Request>
";i:1;s:3:"xml";}i:2;i:8946;}i:53;a:3:{i:0;s:6:"p_open";i:1;a:0:{}i:2;i:8946;}i:54;a:3:{i:0;s:5:"cdata";i:1;a:1:{i:0;s:9:"
=Reply=
";}i:2;i:9029;}i:55;a:3:{i:0;s:7:"p_close";i:1;a:0:{}i:2;i:9043;}i:56;a:3:{i:0;s:4:"code";i:1;a:2:{i:0;s:584:"
<?xml version="1.0" encoding="utf-8"?>
  <xs:element name="MapSensors">
    <xs:complexType>
      <xs:sequence>
        <xs:element name="Sensor" maxOccurs="unbounded">
          <xs:complexType>
            <xs:sequence>
              <xs:element name="nodeid" />
              <xs:element name="nodetype" />
              <xs:element name="name" />
              <xs:element name="posx" />
              <xs:element name="posy" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>
";i:1;s:3:"xml";}i:2;i:9043;}i:57;a:3:{i:0;s:13:"section_close";i:1;a:0:{}i:2;i:9641;}i:58;a:3:{i:0;s:6:"header";i:1;a:3:{i:0;s:20:"Sensor Data (detail)";i:1;i:5;i:2;i:9641;}i:2;i:9641;}i:59;a:3:{i:0;s:12:"section_open";i:1;a:1:{i:0;i:5;}i:2;i:9641;}i:60;a:3:{i:0;s:4:"code";i:1;a:2:{i:0;s:91:"
  <Request>
    <RequestName>getNodeInfo</RequestName>
    <arg>nodeid</arg>
  </Request>
";i:1;s:3:"xml";}i:2;i:9671;}i:61;a:3:{i:0;s:6:"p_open";i:1;a:0:{}i:2;i:9774;}i:62;a:3:{i:0;s:5:"cdata";i:1;a:1:{i:0;s:8:"=Reply=
";}i:2;i:9776;}i:63;a:3:{i:0;s:7:"p_close";i:1;a:0:{}i:2;i:9789;}i:64;a:3:{i:0;s:4:"code";i:1;a:2:{i:0;s:2683:"
<?xml version="1.0" encoding="utf-8"?>
  <xs:element name="SensorMeasurements"> <!-- Root of the xml-file-->
    <xs:complexType>
      <xs:sequence>
        <xs:element name="Sensor" maxOccurs="unbounded"> <!-- Each sensor has a single entry with all the measurements of that sensor -->
          <xs:complexType>
            <xs:sequence>
              <xs:element name="ID" /> <!-- ID of node -->
              <xs:element name="Type" /> <!-- Type of node (e.g. TelosB, SunSpot) -->
              <xs:element name="Time" /> <!-- Timestamp of the measurements -->
              <xs:element name="Temp" /> <!-- Measurement of temperature -->
              <xs:element name="Humidity" /> <!-- Measurement of humidity -->
              <xs:element name="Light" /> <!-- Measurement of light -->
              <xs:element name="Power" /> <!-- Measurement of power -->
              <xs:element name="AccX"  minOccurs="0" maxOccurs="1" /> <!-- Measurement of X-axis acceleration (SunSpot) -->
              <xs:element name="AccY"  minOccurs="0" maxOccurs="1" /> <!-- Measurement of Y-axis acceleration (SunSpot) -->
              <xs:element name="AccZ"  minOccurs="0" maxOccurs="1" /> <!-- Measurement of Z-axis acceleration (SunSpot) -->
              <xs:element name="TiltX"  minOccurs="0" maxOccurs="1" /> <!-- Measurement of X-axis tilt-angle (SunSpot) -->
              <xs:element name="TiltY"  minOccurs="0" maxOccurs="1" /> <!-- Measurement of Y-axis tilt-angle (SunSpot) -->
              <xs:element name="TiltZ"  minOccurs="0" maxOccurs="1" /> <!-- Measurement of Z-axis tilt-angle (SunSpot) -->
              <xs:element name="LED1"  minOccurs="0" maxOccurs="1" />
              <xs:element name="LED2"  minOccurs="0" maxOccurs="1" />
              <xs:element name="LED3"  minOccurs="0" maxOccurs="1" />
              <xs:element name="LED4"  minOccurs="0" maxOccurs="1" />
              <xs:element name="LED5"  minOccurs="0" maxOccurs="1" />
              <xs:element name="LED6"  minOccurs="0" maxOccurs="1" />
              <xs:element name="LED7"  minOccurs="0" maxOccurs="1" />
              <xs:element name="LED8"  minOccurs="0" maxOccurs="1" />
              <xs:element name="Polling"  minOccurs="0" maxOccurs="1" />
              <xs:element name="Button1" minOccurs="1" maxOccurs="1" /> <!-- Status of button1 of the node  (1 = pressed, 0 = not pressed)-->
              <xs:element name="Button2" minOccurs="1" maxOccurs="1" /> <!-- Status of button2 of the node  (1 = pressed, 0 = not pressed) (SunSpot)-->
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>
";i:1;s:3:"xml";}i:2;i:9789;}i:65;a:3:{i:0;s:13:"section_close";i:1;a:0:{}i:2;i:12486;}i:66;a:3:{i:0;s:6:"header";i:1;a:3:{i:0;s:26:"Sensor Data (geschiedenis)";i:1;i:5;i:2;i:12486;}i:2;i:12486;}i:67;a:3:{i:0;s:12:"section_open";i:1;a:1:{i:0;i:5;}i:2;i:12486;}i:68;a:3:{i:0;s:4:"code";i:1;a:2:{i:0;s:233:"
  <Request>
    <RequestName>getHistorylast</RequestName>
    <arg>nodeid</arg>
    <arg>MeasurementType</arg> <!-- zie schema van de meetwaarden om te zien wat die allemaal kunnen zijn -->
    <arg>Last X values<arg>
  </Request> 
";i:1;s:3:"xml";}i:2;i:12522;}i:69;a:3:{i:0;s:6:"p_open";i:1;a:0:{}i:2;i:12767;}i:70;a:3:{i:0;s:5:"cdata";i:1;a:1:{i:0;s:8:"=Reply=
";}i:2;i:12769;}i:71;a:3:{i:0;s:7:"p_close";i:1;a:0:{}i:2;i:12782;}i:72;a:3:{i:0;s:4:"code";i:1;a:2:{i:0;s:1074:"
<?xml version="1.0" encoding="utf-8"?>
  <xs:element name="History"> <!-- Root of the xml-file -->
    <xs:complexType>
      <xs:sequence>
        <xs:element name="Sensor" maxOccurs="1"> <!-- Just an extra hint that it are values of just one sensor over a certain timespan -->
          <xs:complexType>
            <xs:sequence>
              <xs:element name="ID" /> <!-- ID of node -->
              <xs:element name="MeasurementType" /> <!-- Type of measurement (e.g. Temperature, Humidity, ...) -->
              <xs:element name="Measurement" maxOccurs="unbounded">
                <xs:complexType>
                  <xs:sequence>
                    <xs:element name="MeasurementValue" /><!-- Actual measured value -->
                    <xs:element name="MeasurementTime" /><!-- Timestamp of the value in question -->
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>
";i:1;s:3:"xml";}i:2;i:12782;}i:73;a:3:{i:0;s:13:"section_close";i:1;a:0:{}i:2;i:13871;}i:74;a:3:{i:0;s:6:"header";i:1;a:3:{i:0;s:11:"GetNode IDs";i:1;i:5;i:2;i:13871;}i:2;i:13871;}i:75;a:3:{i:0;s:12:"section_open";i:1;a:1:{i:0;i:5;}i:2;i:13871;}i:76;a:3:{i:0;s:4:"code";i:1;a:2:{i:0;s:178:"
  <Request>
    <RequestName>getWSNID</RequestName>
    <arg>Nodeid</arg>
  </Request> 

  <Request>
    <RequestName>getNodeid</RequestName>
    <arg>WSNID</arg>
  </Request> 
";i:1;s:3:"xml";}i:2;i:13892;}i:77;a:3:{i:0;s:6:"p_open";i:1;a:0:{}i:2;i:14082;}i:78;a:3:{i:0;s:5:"cdata";i:1;a:1:{i:0;s:29:"=Reply=
NodeIDs
NodeID
idnode";}i:2;i:14084;}i:79;a:3:{i:0;s:7:"p_close";i:1;a:0:{}i:2;i:14113;}i:80;a:3:{i:0;s:6:"p_open";i:1;a:0:{}i:2;i:14113;}i:81;a:3:{i:0;s:5:"cdata";i:1;a:1:{i:0;s:21:"
WSNIDs
WSNID
sensor
";}i:2;i:14115;}i:82;a:3:{i:0;s:7:"p_close";i:1;a:0:{}i:2;i:14136;}i:83;a:3:{i:0;s:13:"section_close";i:1;a:0:{}i:2;i:14136;}i:84;a:3:{i:0;s:6:"header";i:1;a:3:{i:0;s:16:"WSNActionRequest";i:1;i:4;i:2;i:14136;}i:2;i:14136;}i:85;a:3:{i:0;s:12:"section_open";i:1;a:1:{i:0;i:4;}i:2;i:14136;}i:86;a:3:{i:0;s:4:"code";i:1;a:2:{i:0;s:1609:"
<?xml version="1.0" encoding="utf-8"?>
<xs:schema id="WSNActionReq"
    targetNamespace="http://tempuri.org/WSNActionReq.xsd"
    elementFormDefault="qualified"
    xmlns="http://tempuri.org/WSNActionReq.xsd"
    xmlns:mstns="http://tempuri.org/WSNActionReq.xsd"
    xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:element name="WSNReq">
    <xs:complexType>
      <xs:sequence>
        <xs:element name="RequestAction">
          <xs:complexType>
            <xs:sequence>
              <xs:element name="NodeID" minOccurs="1" maxOccurs="1" />
              <xs:element name="active" minOccurs="0" maxOccurs="1" /><!-- values: 0 / 1 -->
              <xs:element name="AN" minOccurs="0" maxOccurs="1" /><!-- values: 0 (BN) / 1 (AN) -->
              <xs:element name="X" minOccurs="0" maxOccurs="1" />
              <xs:element name="Y" minOccurs="0" maxOccurs="1" />
              <xs:element name="Samplerate" minOccurs="0" maxOccurs="1" /> <!-- values in ms -->
              <xs:element name="LocRate" minOccurs="0" maxOccurs="1" /> <!-- values in ms -->
              <xs:element name="Leds" minOccurs="0" maxOccurs="1" /><!-- values: 0 to 7 -->
              <xs:element name="Power" minOccurs="0" maxOccurs="1" /><!-- values: 0 to 31 -->
              <xs:element name="Frequency" minOccurs="0" maxOccurs="1" /><!-- values: 11 to 26 -->
              <xs:element name="Discovery" minOccurs="0" maxOccurs="1" /><!--1 for discovery, 0 for nothing-->
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>
";i:1;s:3:"xml";}i:2;i:14164;}i:87;a:3:{i:0;s:13:"section_close";i:1;a:0:{}i:2;i:15786;}i:88;a:3:{i:0;s:6:"header";i:1;a:3:{i:0;s:5:"Reply";i:1;i:5;i:2;i:15786;}i:2;i:15786;}i:89;a:3:{i:0;s:12:"section_open";i:1;a:1:{i:0;i:5;}i:2;i:15786;}i:90;a:3:{i:0;s:6:"p_open";i:1;a:0:{}i:2;i:15795;}i:91;a:3:{i:0;s:5:"cdata";i:1;a:1:{i:0;s:72:"Voor alle requests behalve Discovery verwachten we het volgende formaat:";}i:2;i:15796;}i:92;a:3:{i:0;s:7:"p_close";i:1;a:0:{}i:2;i:15868;}i:93;a:3:{i:0;s:4:"code";i:1;a:2:{i:0;s:1552:"
<?xml version="1.0" encoding="utf-8"?>
<xs:schema id="WSNActionReq"
    targetNamespace="http://tempuri.org/WSNActionReq.xsd"
    elementFormDefault="qualified"
    xmlns="http://tempuri.org/WSNActionReq.xsd"
    xmlns:mstns="http://tempuri.org/WSNActionReq.xsd"
    xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:element name="WSNReply">
    <xs:complexType>
      <xs:sequence>
  <xs:element name="StatusMessage"> <!-- Root of the xml-file-->
    <xs:complexType>
      <xs:sequence>
        <xs:element name="Status" maxOccurs="unbounded"> <!-- Each sensor has a single entry with all the measurements of that sensor -->
          <xs:complexType>
            <xs:sequence>
              <xs:element name="ID" /> <!-- ID of node -->
              <xs:element name="Time" /> <!-- Timestamp of the measurements -->
			  <xs:element name="Active" /> <!-- Boolean participating in the localization or not? -->
			  <xs:element name="AN" /> <!-- AN or BN -->
			  <xs:element name="X" /> <!-- x coordinate of the anchor mote -->
			  <xs:element name="Y" /> <!-- y coordinate of the anchor mote -->
			  <xs:element name="SampleRate" /> 
			  <xs:element name="LocRate" /> 
			  <xs:element name="Leds" /> 
			  <xs:element name="Power" />  <!-- Power level -->
			  <xs:element name="Frequency" /> 
			  <xs:element name="Conn" /> <!--containts the number of motes the BN is connected to... EXP -->
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>
";i:1;s:3:"xml";}i:2;i:15875;}i:94;a:3:{i:0;s:6:"p_open";i:1;a:0:{}i:2;i:17439;}i:95;a:3:{i:0;s:5:"cdata";i:1;a:1:{i:0;s:49:"Voor de discovery request sturen we het volgende ";}i:2;i:17441;}i:96;a:3:{i:0;s:7:"acronym";i:1;a:1:{i:0;s:3:"XML";}i:2;i:17490;}i:97;a:3:{i:0;s:5:"cdata";i:1;a:1:{i:0;s:15:" formaat terug:";}i:2;i:17493;}i:98;a:3:{i:0;s:7:"p_close";i:1;a:0:{}i:2;i:17508;}i:99;a:3:{i:0;s:4:"code";i:1;a:2:{i:0;s:543:"
<?xml version="1.0" encoding="utf-8"?>
<xs:schema id="WSNActionReq"
    targetNamespace="http://tempuri.org/WSNActionReq.xsd"
    elementFormDefault="qualified"
    xmlns="http://tempuri.org/WSNActionReq.xsd"
    xmlns:mstns="http://tempuri.org/WSNActionReq.xsd"
    xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:element name="DiscorveryReply">
    <xs:complexType>
      <xs:sequence>
        <xs:element name="Nodeid" maxOccurs="unbounded">
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>
";i:1;s:3:"xml";}i:2;i:17515;}i:100;a:3:{i:0;s:13:"section_close";i:1;a:0:{}i:2;i:18072;}i:101;a:3:{i:0;s:12:"section_edit";i:1;a:4:{i:0;i:8881;i:1;i:18071;i:2;i:3;i:3;s:17:"GUI to Controller";}i:2;i:18072;}i:102;a:3:{i:0;s:6:"header";i:1;a:3:{i:0;s:6:"Legacy";i:1;i:3;i:2;i:18072;}i:2;i:18072;}i:103;a:3:{i:0;s:12:"section_open";i:1;a:1:{i:0;i:3;}i:2;i:18072;}i:104;a:3:{i:0;s:6:"p_open";i:1;a:0:{}i:2;i:18086;}i:105;a:3:{i:0;s:5:"cdata";i:1;a:1:{i:0;s:1:"
";}i:2;i:18087;}i:106;a:3:{i:0;s:12:"internallink";i:1;a:2:{i:0;s:14:"xml_schema_old";i:1;s:15:"old XML schemas";}i:2;i:18088;}i:107;a:3:{i:0;s:7:"p_close";i:1;a:0:{}i:2;i:18122;}i:108;a:3:{i:0;s:6:"p_open";i:1;a:0:{}i:2;i:18122;}i:109;a:3:{i:0;s:12:"internallink";i:1;a:2:{i:0;s:23:"Requests van themaweken";i:1;N;}i:2;i:18124;}i:110;a:3:{i:0;s:7:"p_close";i:1;a:0:{}i:2;i:18151;}i:111;a:3:{i:0;s:6:"p_open";i:1;a:0:{}i:2;i:18151;}i:112;a:3:{i:0;s:12:"internallink";i:1;a:2:{i:0;s:22:"Replies van themaweken";i:1;N;}i:2;i:18153;}i:113;a:3:{i:0;s:5:"cdata";i:1;a:1:{i:0;s:1:"
";}i:2;i:18179;}i:114;a:3:{i:0;s:7:"p_close";i:1;a:0:{}i:2;i:18179;}i:115;a:3:{i:0;s:13:"section_close";i:1;a:0:{}i:2;i:18179;}i:116;a:3:{i:0;s:12:"section_edit";i:1;a:4:{i:0;i:18072;i:1;i:0;i:2;i:3;i:3;s:6:"Legacy";}i:2;i:18179;}i:117;a:3:{i:0;s:12:"document_end";i:1;a:0:{}i:2;i:18179;}}