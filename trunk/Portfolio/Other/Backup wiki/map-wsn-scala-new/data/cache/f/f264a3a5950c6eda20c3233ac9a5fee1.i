a:132:{i:0;a:3:{i:0;s:14:"document_start";i:1;a:0:{}i:2;i:0;}i:1;a:3:{i:0;s:6:"header";i:1;a:3:{i:0;s:9:"Opmerking";i:1;i:4;i:2;i:1;}i:2;i:1;}i:2;a:3:{i:0;s:12:"section_open";i:1;a:1:{i:0;i:4;}i:2;i:1;}i:3;a:3:{i:0;s:6:"p_open";i:1;a:0:{}i:2;i:16;}i:4;a:3:{i:0;s:5:"cdata";i:1;a:1:{i:0;s:153:"Dit zijn alle mogelijk combinaties van RequestName en de argumenten. De argumenten zoals ze hier worden getoond moeten dus elk in een <arg></arg> komen.
";}i:2;i:17;}i:5;a:3:{i:0;s:9:"linebreak";i:1;a:0:{}i:2;i:170;}i:6;a:3:{i:0;s:5:"cdata";i:1;a:1:{i:0;s:37:" Deze structuren worden gebruikt bij ";}i:2;i:172;}i:7;a:3:{i:0;s:12:"internallink";i:1;a:2:{i:0;s:18:"xmlschema#requests";i:1;N;}i:2;i:209;}i:8;a:3:{i:0;s:5:"cdata";i:1;a:1:{i:0;s:2:".
";}i:2;i:231;}i:9;a:3:{i:0;s:9:"linebreak";i:1;a:0:{}i:2;i:233;}i:10;a:3:{i:0;s:5:"cdata";i:1;a:1:{i:0;s:1:" ";}i:2;i:235;}i:11;a:3:{i:0;s:6:"smiley";i:1;a:1:{i:0;s:3:":!:";}i:2;i:236;}i:12;a:3:{i:0;s:5:"cdata";i:1;a:1:{i:0;s:16:" Opmerking: zie ";}i:2;i:239;}i:13;a:3:{i:0;s:12:"internallink";i:1;a:2:{i:0;s:19:"requestname#Vraagje";i:1;N;}i:2;i:255;}i:14;a:3:{i:0;s:7:"p_close";i:1;a:0:{}i:2;i:278;}i:15;a:3:{i:0;s:6:"p_open";i:1;a:0:{}i:2;i:278;}i:16;a:3:{i:0;s:5:"cdata";i:1;a:1:{i:0;s:14:"Bijvoorbeeld:
";}i:2;i:280;}i:17;a:3:{i:0;s:7:"p_close";i:1;a:0:{}i:2;i:299;}i:18;a:3:{i:0;s:4:"code";i:1;a:2:{i:0;s:195:"
<Request>
    <RequestName>addSensorToDB</RequestName>
    <arg>nodeid</arg>
    <arg>nodetype</arg>
    <arg>mapimg</arg>
    <arg>name</arg>
    <arg>posx</arg>
    <arg>posy</arg>
</Request>
";i:1;s:3:"xml";}i:2;i:299;}i:19;a:3:{i:0;s:6:"p_open";i:1;a:0:{}i:2;i:299;}i:20;a:3:{i:0;s:5:"cdata";i:1;a:1:{i:0;s:62:"
Betekent dus dat we in de Request het volgende hebben staan:
";}i:2;i:506;}i:21;a:3:{i:0;s:7:"p_close";i:1;a:0:{}i:2;i:573;}i:22;a:3:{i:0;s:4:"code";i:1;a:2:{i:0;s:537:"
<?xml version="1.0" standalone="yes"?>
<Requests xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns="http://tempuri.org/Requests.xsd">
  <Request>
    <RequestName>addSensorToDB</RequestName>
    <arg>1</arg> <!-- De unieke ID van de node waar we info over willen -->
    <arg>0</arg> <!-- Type van de node -->
    <arg>Woonkamer</arg> <!-- ID van de plattegrond -->
    <arg>TV</arg> <!-- Node die aan de TV is gekoppeld -->
    <arg>0</arg> <!-- In den hoek -->
    <arg>0</arg> <!-- in den hoek -->
  </Request>
</Requests>
";i:1;s:3:"xml";}i:2;i:573;}i:23;a:3:{i:0;s:13:"section_close";i:1;a:0:{}i:2;i:1124;}i:24;a:3:{i:0;s:6:"header";i:1;a:3:{i:0;s:29:"Lijst van Request/arg combo's";i:1;i:4;i:2;i:1124;}i:2;i:1124;}i:25;a:3:{i:0;s:12:"section_open";i:1;a:1:{i:0;i:4;}i:2;i:1124;}i:26;a:3:{i:0;s:13:"section_close";i:1;a:0:{}i:2;i:1160;}i:27;a:3:{i:0;s:12:"section_edit";i:1;a:4:{i:0;i:-1;i:1;i:1159;i:2;i:1;i:3;s:0:"";}i:2;i:1160;}i:28;a:3:{i:0;s:6:"header";i:1;a:3:{i:0;s:11:"Reply (int)";i:1;i:2;i:2;i:1160;}i:2;i:1160;}i:29;a:3:{i:0;s:12:"section_open";i:1;a:1:{i:0;i:2;}i:2;i:1160;}i:30;a:3:{i:0;s:4:"code";i:1;a:2:{i:0;s:1247:"
  <Request>
    <RequestName>addSensorToDB</RequestName>
    <arg>nodeid</arg>
    <arg>nodetype</arg>
    <arg>mapimg</arg>
    <arg>name</arg>
    <arg>posx</arg>
    <arg>posy</arg>
  </Request>

  <Request>
    <RequestName>addMapToDB</RequestName>
    <arg>filename</arg>
    <arg>map_name</arg>
  </Request>

  <Request>
    <RequestName>checkMap</RequestName>
    <arg>filename</arg>
  </Request>

  <Request>
    <RequestName>checkSensorID</RequestName>
    <arg>sensorid</arg>
    <arg>map</arg>
  </Request>

  <Request>
    <RequestName>changePassword</RequestName>
    <arg>username</arg>
    <arg>oldpassword</arg>
    <arg>newpassword</arg>
  </Request>

  <Request>
    <RequestName>checkUsername</RequestName>
    <arg>username</arg>
  </Request>

  <Request>
    <RequestName>adduser</RequestName>
    <arg>username</arg>
    <arg>password</arg>
    <arg>admin</arg>
  </Request>

  <Request>
    <RequestName>checkLogin</RequestName>
    <arg>username</arg>
    <arg>password</arg>
    <arg>admin</arg>
  </Request>

  <Request>
    <RequestName>setPostionSensor</RequestName>
    <arg>nodeid</arg>
    <arg>posx</arg>
    <arg>posy</arg>
  </Request>

  <Request>
    <RequestName>deleteSensorFromMap</RequesName>
  </Request>
";i:1;s:3:"xml";}i:2;i:1187;}i:31;a:3:{i:0;s:6:"p_open";i:1;a:0:{}i:2;i:1187;}i:32;a:3:{i:0;s:5:"cdata";i:1;a:1:{i:0;s:132:"
De bovenstaande functies zullen enkel een integer teruggeven (ofwel een ID ofwel het aantal rijen dat was aangepast/geselecteerd).
";}i:2;i:2446;}i:33;a:3:{i:0;s:9:"linebreak";i:1;a:0:{}i:2;i:2578;}i:34;a:3:{i:0;s:5:"cdata";i:1;a:1:{i:0;s:52:" Het antwoord op deze request is in de vorm van het ";}i:2;i:2580;}i:35;a:3:{i:0;s:18:"doublequoteopening";i:1;a:0:{}i:2;i:2632;}i:36;a:3:{i:0;s:5:"cdata";i:1;a:1:{i:0;s:11:"Reply (int)";}i:2;i:2633;}i:37;a:3:{i:0;s:18:"doublequoteclosing";i:1;a:0:{}i:2;i:2644;}i:38;a:3:{i:0;s:5:"cdata";i:1;a:1:{i:0;s:8:"-schema.";}i:2;i:2645;}i:39;a:3:{i:0;s:7:"p_close";i:1;a:0:{}i:2;i:2653;}i:40;a:3:{i:0;s:13:"section_close";i:1;a:0:{}i:2;i:2655;}i:41;a:3:{i:0;s:12:"section_edit";i:1;a:4:{i:0;i:1160;i:1;i:2654;i:2;i:2;i:3;s:11:"Reply (int)";}i:2;i:2655;}i:42;a:3:{i:0;s:6:"header";i:1;a:3:{i:0;s:7:"Vraagje";i:1;i:3;i:2;i:2655;}i:2;i:2655;}i:43;a:3:{i:0;s:12:"section_open";i:1;a:1:{i:0;i:3;}i:2;i:2655;}i:44;a:3:{i:0;s:6:"p_open";i:1;a:0:{}i:2;i:2670;}i:45;a:3:{i:0;s:5:"cdata";i:1;a:1:{i:0;s:164:"Is er een probleem moest er steeds een ID worden teruggestuurd? (ID zal niet 0 zijn indien aanwezig, dus omzetten van int naar bool zou 'juiste' resultaten geven)  ";}i:2;i:2671;}i:46;a:3:{i:0;s:6:"entity";i:1;a:1:{i:0;s:3:"---";}i:2;i:2835;}i:47;a:3:{i:0;s:5:"cdata";i:1;a:1:{i:0;s:1:" ";}i:2;i:2838;}i:48;a:3:{i:0;s:13:"emphasis_open";i:1;a:0:{}i:2;i:2839;}i:49;a:3:{i:0;s:9:"emaillink";i:1;a:2:{i:0;s:26:"olivier__coude@hotmail.com";i:1;s:7:"Olivier";}i:2;i:2841;}i:50;a:3:{i:0;s:5:"cdata";i:1;a:1:{i:0;s:17:" 2008/10/14 09:38";}i:2;i:2879;}i:51;a:3:{i:0;s:14:"emphasis_close";i:1;a:0:{}i:2;i:2896;}i:52;a:3:{i:0;s:7:"p_close";i:1;a:0:{}i:2;i:2898;}i:53;a:3:{i:0;s:13:"section_close";i:1;a:0:{}i:2;i:2900;}i:54;a:3:{i:0;s:12:"section_edit";i:1;a:4:{i:0;i:2655;i:1;i:2899;i:2;i:3;i:3;s:7:"Vraagje";}i:2;i:2900;}i:55;a:3:{i:0;s:6:"header";i:1;a:3:{i:0;s:18:"Sensors op een map";i:1;i:2;i:2;i:2900;}i:2;i:2900;}i:56;a:3:{i:0;s:12:"section_open";i:1;a:1:{i:0;i:2;}i:2;i:2900;}i:57;a:3:{i:0;s:4:"code";i:1;a:2:{i:0;s:161:"
  <Request>
    <RequestName>getSensors</RequestName>
    <arg>mapimg</arg>
  </Request>

  <Request>
    <RequestName>getAllSensors</RequestName>
  </Request>
";i:1;s:3:"xml";}i:2;i:2934;}i:58;a:3:{i:0;s:6:"p_open";i:1;a:0:{}i:2;i:2934;}i:59;a:3:{i:0;s:5:"cdata";i:1;a:1:{i:0;s:16:"
Antwoord is in ";}i:2;i:3107;}i:60;a:3:{i:0;s:18:"doublequoteopening";i:1;a:0:{}i:2;i:3123;}i:61;a:3:{i:0;s:5:"cdata";i:1;a:1:{i:0;s:18:"Sensors op een map";}i:2;i:3124;}i:62;a:3:{i:0;s:18:"doublequoteclosing";i:1;a:0:{}i:2;i:3142;}i:63;a:3:{i:0;s:5:"cdata";i:1;a:1:{i:0;s:8:"-schema.";}i:2;i:3143;}i:64;a:3:{i:0;s:7:"p_close";i:1;a:0:{}i:2;i:3151;}i:65;a:3:{i:0;s:13:"section_close";i:1;a:0:{}i:2;i:3153;}i:66;a:3:{i:0;s:12:"section_edit";i:1;a:4:{i:0;i:2900;i:1;i:3152;i:2;i:2;i:3;s:18:"Sensors op een map";}i:2;i:3153;}i:67;a:3:{i:0;s:6:"header";i:1;a:3:{i:0;s:19:"Reply (SensorTypes)";i:1;i:2;i:2;i:3153;}i:2;i:3153;}i:68;a:3:{i:0;s:12:"section_open";i:1;a:1:{i:0;i:2;}i:2;i:3153;}i:69;a:3:{i:0;s:4:"code";i:1;a:2:{i:0;s:78:"
  <Request>
    <RequestName>getSensortypesFromDB</RequestName>
  </Request>
";i:1;s:3:"xml";}i:2;i:3188;}i:70;a:3:{i:0;s:6:"p_open";i:1;a:0:{}i:2;i:3188;}i:71;a:3:{i:0;s:5:"cdata";i:1;a:1:{i:0;s:20:"
Antwoord is in het ";}i:2;i:3278;}i:72;a:3:{i:0;s:18:"doublequoteopening";i:1;a:0:{}i:2;i:3298;}i:73;a:3:{i:0;s:5:"cdata";i:1;a:1:{i:0;s:19:"Reply (SensorTypes)";}i:2;i:3299;}i:74;a:3:{i:0;s:18:"doublequoteclosing";i:1;a:0:{}i:2;i:3318;}i:75;a:3:{i:0;s:5:"cdata";i:1;a:1:{i:0;s:8:"-schema.";}i:2;i:3319;}i:76;a:3:{i:0;s:7:"p_close";i:1;a:0:{}i:2;i:3327;}i:77;a:3:{i:0;s:4:"code";i:1;a:2:{i:0;s:92:"
  <Request>
    <RequestName>getNodetype</RequestName>
    <arg>nodeid</arg>
  </Request> 
";i:1;s:3:"xml";}i:2;i:3334;}i:78;a:3:{i:0;s:6:"p_open";i:1;a:0:{}i:2;i:3334;}i:79;a:3:{i:0;s:5:"cdata";i:1;a:1:{i:0;s:21:"
Antwoord is volgens ";}i:2;i:3438;}i:80;a:3:{i:0;s:18:"doublequoteopening";i:1;a:0:{}i:2;i:3459;}i:81;a:3:{i:0;s:5:"cdata";i:1;a:1:{i:0;s:19:"Reply (SensorTypes)";}i:2;i:3460;}i:82;a:3:{i:0;s:18:"doublequoteclosing";i:1;a:0:{}i:2;i:3479;}i:83;a:3:{i:0;s:5:"cdata";i:1;a:1:{i:0;s:8:"-schema.";}i:2;i:3480;}i:84;a:3:{i:0;s:7:"p_close";i:1;a:0:{}i:2;i:3488;}i:85;a:3:{i:0;s:13:"section_close";i:1;a:0:{}i:2;i:3490;}i:86;a:3:{i:0;s:12:"section_edit";i:1;a:4:{i:0;i:3153;i:1;i:3489;i:2;i:2;i:3;s:19:"Reply (SensorTypes)";}i:2;i:3490;}i:87;a:3:{i:0;s:6:"header";i:1;a:3:{i:0;s:20:"Sensor Data (detail)";i:1;i:2;i:2;i:3490;}i:2;i:3490;}i:88;a:3:{i:0;s:12:"section_open";i:1;a:1:{i:0;i:2;}i:2;i:3490;}i:89;a:3:{i:0;s:4:"code";i:1;a:2:{i:0;s:91:"
  <Request>
    <RequestName>getNodeInfo</RequestName>
    <arg>nodeid</arg>
  </Request>
";i:1;s:3:"xml";}i:2;i:3526;}i:90;a:3:{i:0;s:6:"p_open";i:1;a:0:{}i:2;i:3526;}i:91;a:3:{i:0;s:5:"cdata";i:1;a:1:{i:0;s:21:"
Antwoord is volgens ";}i:2;i:3629;}i:92;a:3:{i:0;s:18:"doublequoteopening";i:1;a:0:{}i:2;i:3650;}i:93;a:3:{i:0;s:5:"cdata";i:1;a:1:{i:0;s:20:"Sensor Data (detail)";}i:2;i:3651;}i:94;a:3:{i:0;s:18:"doublequoteclosing";i:1;a:0:{}i:2;i:3671;}i:95;a:3:{i:0;s:5:"cdata";i:1;a:1:{i:0;s:8:"-schema.";}i:2;i:3672;}i:96;a:3:{i:0;s:7:"p_close";i:1;a:0:{}i:2;i:3680;}i:97;a:3:{i:0;s:13:"section_close";i:1;a:0:{}i:2;i:3682;}i:98;a:3:{i:0;s:12:"section_edit";i:1;a:4:{i:0;i:3490;i:1;i:3681;i:2;i:2;i:3;s:20:"Sensor Data (detail)";}i:2;i:3682;}i:99;a:3:{i:0;s:6:"header";i:1;a:3:{i:0;s:26:"Sensor Data (geschiedenis)";i:1;i:2;i:2;i:3682;}i:2;i:3682;}i:100;a:3:{i:0;s:12:"section_open";i:1;a:1:{i:0;i:2;}i:2;i:3682;}i:101;a:3:{i:0;s:4:"code";i:1;a:2:{i:0;s:540:"
  <Request>
    <RequestName>getHistorytime</RequestName>
    <arg>nodeid</arg>
    <arg>MeasurementType</arg> <!-- zie schema van de meetwaarden om te zien wat die allemaal kunnen zijn -->
    <arg>Timestamp (start)</arg>
    <arg>Timestamp (end)</arg> <!-- Optioneel, weglaten = tot nu -->
  </Request> 

  <Request>
    <RequestName>getHistorylast</RequestName>
    <arg>nodeid</arg>
    <arg>MeasurementType</arg> <!-- zie schema van de meetwaarden om te zien wat die allemaal kunnen zijn -->
    <arg>Last X values<arg>
  </Request> 
";i:1;s:3:"xml";}i:2;i:3724;}i:102;a:3:{i:0;s:6:"p_open";i:1;a:0:{}i:2;i:3724;}i:103;a:3:{i:0;s:5:"cdata";i:1;a:1:{i:0;s:21:"
Antwoord is volgens ";}i:2;i:4276;}i:104;a:3:{i:0;s:18:"doublequoteopening";i:1;a:0:{}i:2;i:4297;}i:105;a:3:{i:0;s:5:"cdata";i:1;a:1:{i:0;s:26:"Sensor Data (geschiedenis)";}i:2;i:4298;}i:106;a:3:{i:0;s:18:"doublequoteclosing";i:1;a:0:{}i:2;i:4324;}i:107;a:3:{i:0;s:5:"cdata";i:1;a:1:{i:0;s:7:"-schema";}i:2;i:4325;}i:108;a:3:{i:0;s:7:"p_close";i:1;a:0:{}i:2;i:4332;}i:109;a:3:{i:0;s:13:"section_close";i:1;a:0:{}i:2;i:4334;}i:110;a:3:{i:0;s:12:"section_edit";i:1;a:4:{i:0;i:3682;i:1;i:4333;i:2;i:2;i:3;s:26:"Sensor Data (geschiedenis)";}i:2;i:4334;}i:111;a:3:{i:0;s:6:"header";i:1;a:3:{i:0;s:13:"Sensor acties";i:1;i:2;i:2;i:4334;}i:2;i:4334;}i:112;a:3:{i:0;s:12:"section_open";i:1;a:1:{i:0;i:2;}i:2;i:4334;}i:113;a:3:{i:0;s:4:"code";i:1;a:2:{i:0;s:95:"
  <Request>
    <RequestName>getNodeActions</RequestName>
    <arg>nodeid</arg>
  </Request> 
";i:1;s:3:"xml";}i:2;i:4363;}i:114;a:3:{i:0;s:6:"p_open";i:1;a:0:{}i:2;i:4363;}i:115;a:3:{i:0;s:5:"cdata";i:1;a:1:{i:0;s:21:"
Antwoord is volgens ";}i:2;i:4470;}i:116;a:3:{i:0;s:18:"doublequoteopening";i:1;a:0:{}i:2;i:4491;}i:117;a:3:{i:0;s:5:"cdata";i:1;a:1:{i:0;s:14:"Sensor Actions";}i:2;i:4492;}i:118;a:3:{i:0;s:18:"doublequoteclosing";i:1;a:0:{}i:2;i:4506;}i:119;a:3:{i:0;s:5:"cdata";i:1;a:1:{i:0;s:7:"-schema";}i:2;i:4507;}i:120;a:3:{i:0;s:7:"p_close";i:1;a:0:{}i:2;i:4514;}i:121;a:3:{i:0;s:13:"section_close";i:1;a:0:{}i:2;i:4516;}i:122;a:3:{i:0;s:12:"section_edit";i:1;a:4:{i:0;i:4334;i:1;i:4515;i:2;i:2;i:3;s:13:"Sensor acties";}i:2;i:4516;}i:123;a:3:{i:0;s:6:"header";i:1;a:3:{i:0;s:5:"Extra";i:1;i:2;i:2;i:4516;}i:2;i:4516;}i:124;a:3:{i:0;s:12:"section_open";i:1;a:1:{i:0;i:2;}i:2;i:4516;}i:125;a:3:{i:0;s:4:"code";i:1;a:2:{i:0;s:178:"
  <Request>
    <RequestName>getWSNID</RequestName>
    <arg>Nodeid</arg>
  </Request> 

  <Request>
    <RequestName>getNodeid</RequestName>
    <arg>WSNID</arg>
  </Request> 
";i:1;s:3:"xml";}i:2;i:4537;}i:126;a:3:{i:0;s:6:"p_open";i:1;a:0:{}i:2;i:4727;}i:127;a:3:{i:0;s:6:"entity";i:1;a:1:{i:0;s:3:"...";}i:2;i:4729;}i:128;a:3:{i:0;s:7:"p_close";i:1;a:0:{}i:2;i:4732;}i:129;a:3:{i:0;s:13:"section_close";i:1;a:0:{}i:2;i:4734;}i:130;a:3:{i:0;s:12:"section_edit";i:1;a:4:{i:0;i:4516;i:1;i:0;i:2;i:2;i:3;s:5:"Extra";}i:2;i:4734;}i:131;a:3:{i:0;s:12:"document_end";i:1;a:0:{}i:2;i:4734;}}