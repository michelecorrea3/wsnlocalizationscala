#This page describes all new XML formats

# GUI -> WSN #
## WSNActionRequest ##
```
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

              <xs:element name="active" minOccurs="0" maxOccurs="1" />

              <xs:element name="AN" minOccurs="0" maxOccurs="1" />

              <xs:element name="X" minOccurs="0" maxOccurs="1" />

              <xs:element name="Y" minOccurs="0" maxOccurs="1" />

              <xs:element name="Samplerate" minOccurs="0" maxOccurs="1" />

              <xs:element name="LocRate" minOccurs="0" maxOccurs="1" />

              <xs:element name="Leds" minOccurs="0" maxOccurs="1" />

              <xs:element name="Power" minOccurs="0" maxOccurs="1" />

              <xs:element name="Frequency" minOccurs="0" maxOccurs="1" />

              <xs:element name="Discovery" minOccurs="0" maxOccurs="1" /><!--1 for discovery, otherwise an error happened-->

            </xs:sequence>
          </xs:complexType>

        </xs:element>
      </xs:sequence>
    </xs:complexType>

  </xs:element>
</xs:schema>
```
## Replie ##
Voor alle requests behalve Discovery verwachten we het volgende formaat:

```
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
        <xs:element name="RequestReply">

          <xs:complexType>
            <xs:sequence>

              <xs:element name="NodeID" minOccurs="1" maxOccurs="1" />

              <xs:element name="active" minOccurs="0" maxOccurs="1" />

              <xs:element name="AN" minOccurs="0" maxOccurs="1" />

              <xs:element name="X" minOccurs="0" maxOccurs="1" />

              <xs:element name="Y" minOccurs="0" maxOccurs="1" />

              <xs:element name="Samplerate" minOccurs="0" maxOccurs="1" />

              <xs:element name="LocRate" minOccurs="0" maxOccurs="1" />

              <xs:element name="Leds" minOccurs="0" maxOccurs="1" />

              <xs:element name="Power" minOccurs="0" maxOccurs="1" />

              <xs:element name="Frequency" minOccurs="0" maxOccurs="1" />

              <xs:element name="Conn" minOccurs="0" maxOccurs="1" />

            </xs:sequence>
          </xs:complexType>

        </xs:element>
      </xs:sequence>
    </xs:complexType>

  </xs:element>
</xs:schema>
```

Voor de discovery request sturen we het volgende XML formaat terug:

```
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
```