a:15:{i:0;a:3:{i:0;s:14:"document_start";i:1;a:0:{}i:2;i:0;}i:1;a:3:{i:0;s:6:"p_open";i:1;a:0:{}i:2;i:0;}i:2;a:3:{i:0;s:7:"p_close";i:1;a:0:{}i:2;i:1;}i:3;a:3:{i:0;s:12:"section_edit";i:1;a:4:{i:0;i:-1;i:1;i:0;i:2;i:1;i:3;s:0:"";}i:2;i:1;}i:4;a:3:{i:0;s:6:"header";i:1;a:3:{i:0;s:12:"Senseless V2";i:1;i:2;i:2;i:1;}i:2;i:1;}i:5;a:3:{i:0;s:12:"section_open";i:1;a:1:{i:0;i:2;}i:2;i:1;}i:6;a:3:{i:0;s:4:"code";i:1;a:2:{i:0;s:3537:"
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
              <xs:element name="RSSI"  minOccurs="0" maxOccurs="1" />
              <xs:element name="LQI"  minOccurs="0" maxOccurs="1" />
              <xs:element name="X"  minOccurs="0" maxOccurs="1" />
              <xs:element name="Y"  minOccurs="0" maxOccurs="1" />
              <xs:element name="Z"  minOccurs="0" maxOccurs="1" />
              <xs:element name="extra1"  minOccurs="0" maxOccurs="1" />
              <xs:element name="extra2"  minOccurs="0" maxOccurs="1" />
              <xs:element name="extra3"  minOccurs="0" maxOccurs="1" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>
";i:1;s:3:"xml";}i:2;i:29;}i:7;a:3:{i:0;s:13:"section_close";i:1;a:0:{}i:2;i:3580;}i:8;a:3:{i:0;s:12:"section_edit";i:1;a:4:{i:0;i:1;i:1;i:3579;i:2;i:2;i:3;s:12:"Senseless V2";}i:2;i:3580;}i:9;a:3:{i:0;s:6:"header";i:1;a:3:{i:0;s:12:"Senseless V1";i:1;i:2;i:2;i:3580;}i:2;i:3580;}i:10;a:3:{i:0;s:12:"section_open";i:1;a:1:{i:0;i:2;}i:2;i:3580;}i:11;a:3:{i:0;s:4:"code";i:1;a:2:{i:0;s:2981:"
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
";i:1;s:3:"xml";}i:2;i:3609;}i:12;a:3:{i:0;s:13:"section_close";i:1;a:0:{}i:2;i:6602;}i:13;a:3:{i:0;s:12:"section_edit";i:1;a:4:{i:0;i:3580;i:1;i:0;i:2;i:2;i:3;s:12:"Senseless V1";}i:2;i:6602;}i:14;a:3:{i:0;s:12:"document_end";i:1;a:0:{}i:2;i:6602;}}