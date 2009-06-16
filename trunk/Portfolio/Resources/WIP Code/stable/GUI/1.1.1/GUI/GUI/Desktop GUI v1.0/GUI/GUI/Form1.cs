using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml.Linq;
using ZedGraph;
using System.IO;
using SocketConnection;

namespace GUI
{
    public partial class Form1 : Form
    {
        #region private variables

        //Port of the controller
        private int Port;

        //list with all active sensors
        //3 methods to get these sensors:
        //  getAllSensors
        //  Discovery
        //  TimeOutMethod (not implemented yet)
        private List<SensorNames> Sensorlijst = new List<SensorNames>();

        //temp struct for the control changes
        private Changes oldChanges;

        #endregion

        /// <summary>
        /// Constructor for the form
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }
      
        /// <summary>
        /// Initializes variables, called when the form loads
        /// Conenction parameters, parameter for the plot function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            SetSize();

            //default values
            controllerIP.Text = "localhost";
            controllerPort.Text = "1900";

            textLocRefresh.Text = "1000";
            textSensRefresh.Text = "5000";

            buttonDisconnect.Enabled = false;

            //
            // Add the graph parameters
            //
            List<Metingen>ComboMet = new List<Metingen>();
            ComboMet.Add(new Metingen());
            ComboMet.ForEach(delegate(Metingen M)
            {
                comboBox1.Items.Add(M.temperature);
                comboBox1.Items.Add(M.light);
                comboBox1.Items.Add(M.humidity);
                comboBox1.Items.Add(M.power);
                comboBox1.Items.Add(M.RSSI);
                comboBox1.Items.Add(M.position);
            });

        }

        #region helper methods
        ///<summary>
        ///Helper method for retreiving the nodeid of the framework
        /// </summary>
        /// <param name="selected">
        /// node id, unique identifier within Senseless
        /// </param>
        /// <returns>
        /// The MAC or TOSid of the mote
        /// </returns>
        private string getNodeID(string selected)
        {
            string nodeid;
            foreach (SensorNames telosb in Sensorlijst)
            {
                if (telosb.Sensorname == selected)
                {
                    nodeid = telosb.id;
                    return nodeid;
                }
            }
            return "N/A";

        }

        /// <summary>
        /// Helper method for retreiving the WSNid of the framework
        /// </summary>
        /// <param name="selected">
        /// The MAC or TOSid of the mote
        /// </param>
        /// <returns>
        /// node id, unique identifier within Senseless
        /// </returns>
        private string getTelosbId(string selected)
        {
            string sensorname;
            foreach (SensorNames telosb in Sensorlijst)
            {
                if (telosb.id == selected)
                {
                    sensorname = telosb.Sensorname;
                    return sensorname;
                }
            }
            return selected + " nodeid";
        }
        #endregion

        #region plot graph

        /// <summary>
        /// Resizes the graph area when the user resizes the form
        /// </summary>
        private void SetSize()
        {
            zg1.Location = new Point(250, 50);
            // Leave a small margin around the outside of the control
            zg1.Size = new Size(this.ClientRectangle.Width - 280, this.ClientRectangle.Height - 90);
        }

        /// <summary>
        /// Draws the graph 
        /// </summary>
        /// <param name="zgc">
        /// The control for the graph element
        /// </param>
        /// <param name="measurements">
        /// Points of the graph
        /// </param>
        private void CreateGraph(ZedGraphControl zgc, string measurements)
        {
            GraphPane myPane = zgc.GraphPane;
            myPane.CurveList.Clear();
            PointPairList list = new PointPairList();

            myPane.Title.Text = "TelosB Graph";
            //put the received data in the graph
            XmlDocument tempdoc = new XmlDocument();
            tempdoc.LoadXml(measurements);

            XmlNodeList bookList = tempdoc.GetElementsByTagName("MeasurementValue");

            double x = 1;
            foreach (XmlNode node in bookList)
            {
                string yyyy = node.InnerText.ToString();
                //listView2.Items.Add(id_Measurement2);
                double y = Convert.ToDouble(yyyy);
                x = x + 1;
                list.Add(x, y);
            }

            LineItem myCurve = myPane.AddCurve("My Curve", list, Color.Blue, SymbolType.Circle);
            //myPane.Chart.Fill = new Fill(Color.White, Color.LightGoldenrodYellow, 45F);
            //myPane.Fill = new Fill(Color.White, Color.FromArgb(220, 220, 255), 45F);
            zgc.AxisChange();
            zgc.Invalidate();
        }
        
        /// <summary>
        /// Event-handler for the plot button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click_1(object sender, EventArgs e)
        {
            //static part of the message
            string xml_send = @"<?xml version=""1.0"" standalone=""yes""?><Requests xmlns:xsi=""='http://www.w3.org/2001/XMLSchema-instance'""  xmlns=""xmlns='http://tempuri.org/Requests.xsd\'""><Request><RequestName>getHistoryLast</RequestName>";

            //return the nodeid with sensor as input
            try
            {
                string selectedNode = comboBox2.SelectedItem.ToString();

                if (selectedNode != null)
                {
                    string nodeid = getNodeID(selectedNode); ;

                    xml_send += "<arg>" + nodeid + "</arg>";

                    string selectedReading = comboBox1.SelectedItem.ToString();

                    if (selectedReading != null)
                    {
                        xml_send += "<arg>" + selectedReading + "</arg><arg>10</arg></Request></Requests>";


                    }
                }

                SocketClient socket_client = new SocketClient(Port, controllerIP.Text);
                string xml_receive = socket_client.Connect(xml_send, true);


                //put the received data in the graph
                XmlDocument tempdoc = new XmlDocument();
                tempdoc.LoadXml(xml_receive);

                listView2.Items.Clear();
                XmlNodeList bookList = tempdoc.GetElementsByTagName("MeasurementValue");

                foreach (XmlNode node in bookList)
                {
                    string id_Measurement = node.InnerText.ToString();
                    listView2.Items.Add(id_Measurement);
                }

                CreateGraph(zg1, xml_receive);
                SetSize();
            }
            catch { }
        }
        #endregion

        #region environment tab
        
        /// <summary>
        /// Event-handler for the timer which controls the polling for the sensordata
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            GetSensorData();
        }

        /// <summary>
        /// Retreives the sensordata from the controller
        /// </summary>
        public void GetSensorData()
        {
            //static part of the message
            string xml_send = @"<?xml version=""1.0"" standalone=""yes""?><Requests xmlns:xsi=""='http://www.w3.org/2001/XMLSchema-instance'""  xmlns=""xmlns='http://tempuri.org/Requests.xsd\'""><Request><RequestName>getnodeInfo</RequestName>";

            //return the nodeid with sensor as input
            try
            {
                string selected = listBoxLoc.SelectedItem.ToString();
                if (selected != null)
                {
                    string nodeid;

                    nodeid = getNodeID(selected);

                    xml_send += "<arg>" + nodeid + "</arg></Request></Requests>";

                    SocketClient socket_client = new SocketClient(Port, controllerIP.Text);
                    // Receiving data
                    string xml_receive = socket_client.Connect(xml_send, true);


                    XmlDocument tempdoc = new XmlDocument();
                    tempdoc.LoadXml(xml_receive);

                    XmlNodeList bookList = tempdoc.GetElementsByTagName("SensorMeasurements");
                    foreach (XmlNode node in bookList)
                    {
                        XmlElement ID = (XmlElement)(node);
                        try
                        {
                            textBox1.Text = ID.GetElementsByTagName("Node")[0].InnerText;
                        }
                        catch
                        {
                            Console.WriteLine("No sensors avaible!");
                        }
                        try
                        {
                            textBox2.Text = ID.GetElementsByTagName("Sensortype")[0].InnerText;
                        }
                        catch
                        {
                            Console.WriteLine("No sensortype info avaible!");
                        }
                        try
                        {
                            textBox3.Text = ID.GetElementsByTagName("Temperature")[0].InnerText;
                        }
                        catch
                        {
                            Console.WriteLine("No temperature data avaible!");
                        }
                        try
                        {
                            textBox4.Text = ID.GetElementsByTagName("Light")[0].InnerText;
                        }
                        catch
                        {
                            Console.WriteLine("No light data avaible!");
                        }
                        try
                        {
                            textBox5.Text = ID.GetElementsByTagName("Humidity")[0].InnerText;
                        }
                        catch
                        {
                            Console.WriteLine("No humidity data avaible!");
                        }
                        try
                        {
                            if (ID.GetElementsByTagName("Sensortype")[0].InnerText == "2")
                            {
                                textBox6.Text = ID.GetElementsByTagName("Power")[0].InnerText;
                            }
                            else
                            {
                                textBox6.Text = "N/A";
                            }
                        }
                        catch
                        {
                            Console.WriteLine("No power data avaible!");
                        }
                        try
                        {
                            int index = ID.GetElementsByTagName("Time")[0].InnerText.IndexOf(' ');
                            textBoxSensUpdate.Text = ID.GetElementsByTagName("Time")[0].InnerText.Substring(index);
                        }
                        catch
                        {
                            Console.WriteLine("No time available");
                        }
                    }

                }
            }
            catch
            {
                Console.WriteLine("Exception in GetSensorData");
                return;
            }
            #region readfromdummyfile
            //
            // Get data from Dummy XML file
            //
            //XmlDocument doc3 = new XmlDocument();
            //doc3.Load(new XmlTextReader("C:\\Documents and Settings\\Gerrit\\Bureaublad\\GUI v0.50\\GUI\\GUI\\Schemes\\Antwoord1.xml"));
            //doc3.Load(new XmlTextReader("D:\\School\\4EA\\ICT - databases & WSN\\C#\\GUI v0.50\\GUI\\GUI\\Schemes\\Antwoord1.xml"));
            /*XmlNodeList bookList3 = doc3.GetElementsByTagName("SensorMeasurements");
            foreach (XmlNode node in bookList3)
            {
                XmlElement ID = (XmlElement)(node);
                string id_ID = ID.GetElementsByTagName("ID")[0].InnerText;
                string id_Type = ID.GetElementsByTagName("Type")[0].InnerText;
                string id_Temp = ID.GetElementsByTagName("Temp")[0].InnerText;
                string id_Light = ID.GetElementsByTagName("Light")[0].InnerText;
                string id_Hum = ID.GetElementsByTagName("Humidity")[0].InnerText;
                string id_Power = ID.GetElementsByTagName("Power")[0].InnerText;
                textBox1.Text = (id_ID);
                textBox2.Text = (id_Type);
                textBox3.Text = (id_Temp);
                textBox4.Text = (id_Light);
                textBox5.Text = (id_Hum);
                textBox6.Text = (id_Power);
            }*/
            #endregion
        }
        #endregion

        #region localization tab
        private void timerLoc_Tick(object sender, EventArgs e)
        {
            GetLocData();
        }

        private void GetLocData()
        {
            //static part of the message
            string xml_send = @"<?xml version=""1.0"" standalone=""yes""?><Requests xmlns:xsi=""='http://www.w3.org/2001/XMLSchema-instance'""  xmlns=""xmlns='http://tempuri.org/Requests.xsd\'""><Request><RequestName>getnodeLocInfo</RequestName>";

            //return the nodeid with sensor as input
            try
            {
                string selected = listBoxLoc.SelectedItem.ToString();
                if (selected != null)
                {
                    string nodeid;
                    nodeid = getNodeID(selected);

                    xml_send += "<arg>" + nodeid + "</arg></Request></Requests>";

                    SocketClient socket_client = new SocketClient(Port, controllerIP.Text);
                    // Receiving data
                    string xml_receive = socket_client.Connect(xml_send, true);


                    XmlDocument tempdoc = new XmlDocument();
                    tempdoc.LoadXml(xml_receive);

                    XmlNodeList bookList = tempdoc.GetElementsByTagName("Position");
                    foreach (XmlNode node in bookList)
                    {
                        XmlElement ID = (XmlElement)(node);
                        try
                        {
                            textBoxNodeID.Text = ID.GetElementsByTagName("node")[0].InnerText;
                            textBoxANodeID.Text = getTelosbId(ID.GetElementsByTagName("ANode")[0].InnerText);
                            textBoxRSSI.Text = ID.GetElementsByTagName("RSSI")[0].InnerText;
                            textBoxX.Text = ID.GetElementsByTagName("X")[0].InnerText;
                            textBoxY.Text = ID.GetElementsByTagName("Y")[0].InnerText;

                            try
                            {
                                int index = ID.GetElementsByTagName("Time")[0].InnerText.IndexOf(' ');
                                textBoxLocUpdate.Text = ID.GetElementsByTagName("Time")[0].InnerText.Substring(index);
                            }
                            catch
                            {
                                Console.WriteLine("No time available for loc info");
                            }

                            try
                            {
                                textBoxZ.Text = ID.GetElementsByTagName("Z")[0].InnerText;
                            }
                            catch
                            {
                                textBoxZ.Text = "N/A";
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Some field is not available");
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("Error in getlocData");
            }
        }
        #endregion

        #region Control tab

        private void listBoxControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetStatData();
        }

        private void GetStatData()
        {
            //static part of the message
            string xml_send = @"<?xml version=""1.0"" standalone=""yes""?><Requests xmlns:xsi=""='http://www.w3.org/2001/XMLSchema-instance'""  xmlns=""xmlns='http://tempuri.org/Requests.xsd\'""><Request><RequestName>getStatus</RequestName>";

            //return the nodeid with sensor as input
            try
            {
                string selected = listBoxControl.SelectedItem.ToString();
                string nodeid = getNodeID(selected);

                xml_send += "<arg>" + nodeid + "</arg></Request></Requests>";

                SocketClient socket_client = new SocketClient(Port, controllerIP.Text);
                // Receiving data
                string xml_receive = socket_client.Connect(xml_send, true);

                XmlDocument tempdoc = new XmlDocument();
                tempdoc.LoadXml(xml_receive);

                XmlNodeList bookList = tempdoc.GetElementsByTagName("Status");
                foreach (XmlNode node in bookList)
                {
                    XmlElement ID = (XmlElement)(node);
                    try
                    {
                        //assign the values of the textboxes


                        ActiveProperty = ID.GetElementsByTagName("active")[0].InnerText;
                        AnchorProperty = ID.GetElementsByTagName("AN")[0].InnerText;

                        textBoxControlX.Text = ID.GetElementsByTagName("X")[0].InnerText;
                        textBoxControlY.Text = ID.GetElementsByTagName("Y")[0].InnerText;
                        textBoxSampleRate.Text = ID.GetElementsByTagName("Samplerate")[0].InnerText;
                        textBoxLocRate.Text = ID.GetElementsByTagName("LocRate")[0].InnerText;

                        //set the led bitmask
                        LedsProperty = ID.GetElementsByTagName("Leds")[0].InnerText;


                        PowerProperty = ID.GetElementsByTagName("Power")[0].InnerText;
                        FrequencyProperty = ID.GetElementsByTagName("Frequency")[0].InnerText;

                        textBoxConn.Text = ID.GetElementsByTagName("Conn")[0].InnerText;


                    }
                    catch
                    {
                        Console.WriteLine("Some field is not available");
                    }

                    //backup these values in the changes struct
                    oldChanges.Active = ActiveProperty;
                    oldChanges.AN = AnchorProperty;

                    oldChanges.X = textBoxControlX.Text;
                    oldChanges.Y = textBoxControlY.Text;
                    oldChanges.samplerate = textBoxSampleRate.Text;
                    oldChanges.locrate = textBoxLocRate.Text;

                    oldChanges.power = PowerProperty;
                    oldChanges.frequency = FrequencyProperty;
                    oldChanges.conn = textBoxConn.Text;

                    oldChanges.leds = LedsProperty;
                }
            }
            catch
            {
                Console.WriteLine("Error in getStatData");
            }

        }


        //apply changes
        private void buttonWSNControl_Click(object sender, EventArgs e)
        {
            //TODO: needs validation;
            panelControl.Enabled = false;
            buttonWSNControl.Enabled = false;
            
            //we send the WSNactionRequest here
            //first
            //static part of the message
            string xml_send = @"<?xml version=""1.0"" standalone=""yes""?><WSNReq xmlns:xsi=""='http://www.w3.org/2001/XMLSchema-instance'""  xmlns=""xmlns='http://tempuri.org/Requests.xsd\'""><WSNReq><RequestAction>";

            //return the nodeid with sensor as input
            try
            {
                string selected = listBoxControl.SelectedItem.ToString();
                string nodeid;
                nodeid = getNodeID(selected);
                xml_send += "<nodeid>" + nodeid + "</nodeid>";

                //secondly we must check which fields have changed
                //we can then add them ass nodes in the XML

                //specific to our localization scheme
                if (oldChanges.Active != ActiveProperty)
                    xml_send += "<active>" + ActiveProperty + "</active>";
                //only process these parameters if the node is active in the localization algorithm
                if (ActiveProperty == "1")
                {
                    if (oldChanges.AN != AnchorProperty)
                        xml_send += "<AN>" + AnchorProperty + "</AN>";
                    //only process these parameters if the node is an Anchor Node
                    if (AnchorProperty == "0")
                    {
                        if (oldChanges.X != textBoxControlX.Text)
                            xml_send += "<X>" + textBoxControlX.Text + "</X>";
                        if (oldChanges.Y != textBoxControlY.Text)
                            xml_send += "<Y>" + textBoxControlY.Text + "</y>";
                        if (oldChanges.locrate != textBoxLocRate.Text)
                            xml_send += "<locrate>" + textBoxLocRate.Text + "</locrate>";
                    }
                }

                //telosb general parameters
                //all independent
                if (oldChanges.samplerate != textBoxSampleRate.Text)
                    xml_send += "<samplerate>" + textBoxSampleRate.Text + "</samplerate>";
                if (oldChanges.leds != LedsProperty)
                    xml_send += "<leds>" + LedsProperty + "</leds>";
                if (oldChanges.power != PowerProperty)
                    xml_send += "<power>" + PowerProperty + "</power>";
                if (oldChanges.frequency != FrequencyProperty)
                    xml_send += "<frequency>" + FrequencyProperty+ "</frequency>";

                xml_send += "</RequestAction></WSNReq></WSNReq>";

                //we can now send the request, we will receive a status message as a response
                SocketClient socket_client = new SocketClient(Port, controllerIP.Text);
                // Sends: ActionRequest
                // Receives: Status
                string xml_receive = socket_client.Connect(xml_send, true);

                XmlDocument tempdoc = new XmlDocument();
                tempdoc.LoadXml(xml_receive);

                XmlNodeList bookList = tempdoc.GetElementsByTagName("Status");
                foreach (XmlNode node in bookList)
                {
                    XmlElement ID = (XmlElement)(node);
                    try
                    {
                        if (
                        ActiveProperty == ID.GetElementsByTagName("active")[0].InnerText &&
                        AnchorProperty == ID.GetElementsByTagName("AN")[0].InnerText &&
                        textBoxControlX.Text == ID.GetElementsByTagName("X")[0].InnerText &&
                        textBoxControlY.Text == ID.GetElementsByTagName("Y")[0].InnerText &&
                        textBoxSampleRate.Text == ID.GetElementsByTagName("Samplerate")[0].InnerText &&
                        textBoxLocRate.Text == ID.GetElementsByTagName("LocRate")[0].InnerText &&
                        LedsProperty == ID.GetElementsByTagName("Leds")[0].InnerText &&
                        PowerProperty == ID.GetElementsByTagName("Power")[0].InnerText &&
                        FrequencyProperty == ID.GetElementsByTagName("Frequency")[0].InnerText)
                        {
                            //WSN Succesfully replied to our request
                            //changes struct
                            oldChanges.Active = ActiveProperty;
                            oldChanges.AN = AnchorProperty;
                            oldChanges.X = textBoxControlX.Text;
                            oldChanges.Y = textBoxControlY.Text;
                            oldChanges.samplerate = textBoxSampleRate.Text;
                            oldChanges.locrate = textBoxLocRate.Text;
                            oldChanges.power = PowerProperty;
                            oldChanges.frequency = FrequencyProperty;
                            oldChanges.conn = textBoxConn.Text;
                            oldChanges.leds = LedsProperty;

                            Console.WriteLine("WSN succesfully replied");
                            MessageBox.Show("WSN succesfully replied");
                        }
                        else
                        {
                            //WSN did NOT! Succesfully replied to our request
                            //roll back to previous state;
                            ActiveProperty = oldChanges.Active;
                            AnchorProperty = oldChanges.AN;
                            textBoxControlX.Text = oldChanges.X;
                            textBoxControlY.Text = oldChanges.Y;
                            textBoxSampleRate.Text = oldChanges.samplerate;
                            textBoxLocRate.Text = oldChanges.locrate;
                            FrequencyProperty = oldChanges.power;
                            FrequencyProperty = oldChanges.frequency;
                            textBoxConn.Text = oldChanges.conn;
                            LedsProperty = oldChanges.leds;

                            Console.WriteLine("WSN did not succesfully reply");
                            MessageBox.Show("WSN did not succesfully reply");
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Some field is not available");
                    }

                }
            }
            catch { Console.WriteLine("Error in getStatData"); }

            panelControl.Enabled = true;
            buttonWSNControl.Enabled = true;
        }

        //discard changes
        private void buttonControlDiscard_Click(object sender, EventArgs e)
        {
            //roll back to previous state;

            ActiveProperty = oldChanges.Active;
            AnchorProperty = oldChanges.AN;

            textBoxControlX.Text = oldChanges.X;
            textBoxControlY.Text = oldChanges.Y;
            textBoxSampleRate.Text = oldChanges.samplerate;
            textBoxLocRate.Text = oldChanges.locrate;

            PowerProperty = oldChanges.power;
            FrequencyProperty = oldChanges.frequency;

            LedsProperty = oldChanges.leds;

            textBoxConn.Text = oldChanges.conn;
        }
        #endregion

        #region options tab
        //-----------------------------------------------------------------
        //
        //Options tab
        //

        //connect button
        //corrently the connection data is not checked, if it is incorrect the application will fail
        private void buttonConnect_Click(object sender, EventArgs e)
        {
            Port = Convert.ToInt32(controllerPort.Text);
            SocketClient sc = new SocketClient(Port, controllerIP.Text);

            if (sc.TryConnection())
            {
                timer1.Enabled = true;
                timerLoc.Enabled = true;
                //timerDiscovery.Enabled = true;


                //GUI tasks
                buttonConnect.Enabled = false;
                buttonDisconnect.Enabled = true;

                controllerIP.Enabled = false;
                controllerPort.Enabled = false;

                toolStripStatusLabel.Text = "Connected to controller at IP " + controllerIP.Text + ", port " + controllerPort.Text;

                //discovery
                GetSensors();
            }
            else
            {
                Console.WriteLine("Incorrect connection parameters");
                MessageBox.Show("Could not reach the controller!");
            }
            
        }

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {   
            timer1.Enabled = false;
            timerLoc.Enabled = false;
            timerDiscovery.Enabled = false;

            buttonConnect.Enabled = true;
            buttonDisconnect.Enabled = false;

            controllerIP.Enabled = true;
            controllerPort.Enabled = true;
        }

        private void textSensRefresh_TextChanged(object sender, EventArgs e)
        {
            timer1.Interval = Convert.ToInt32(textSensRefresh.Text);
        }

        private void textLocRefresh_TextChanged(object sender, EventArgs e)
        {
            timerLoc.Interval = Convert.ToInt32(textLocRefresh.Text);
        }
        #endregion

        #region node discovery
        //poll the WSN for all available nodes
        private void timerDiscovery_Tick(object sender, EventArgs e)
        {
            //we send the WSNactionRequest here
            //first
            //static part of the message
            string xml_send = @"<?xml version=""1.0"" standalone=""yes""?><WSNReq xmlns:xsi=""='http://www.w3.org/2001/XMLSchema-instance'""  xmlns=""xmlns='http://tempuri.org/Requests.xsd\'""><WSNReq><RequestAction><Discovery>1</Discovery>";

            SocketClient socket_client = new SocketClient(Port, controllerIP.Text);
            // Receiving data
            string xml_receive = socket_client.Connect(xml_send, true);


            XmlDocument tempdoc = new XmlDocument();
            tempdoc.LoadXml(xml_receive);

            XmlNodeList bookList = tempdoc.GetElementsByTagName("Sensor");
            foreach (XmlNode node in bookList)
            {
                XmlElement ID = (XmlElement)(node);
                try
                {
                    string sensor = ID.GetElementsByTagName("sensor")[0].InnerText;
                    string idnode = ID.GetElementsByTagName("idnode")[0].InnerText;
                    Sensorlijst.Add(new SensorNames(sensor, idnode));
                }
                catch
                {
                    Console.WriteLine("Could not contact the WSN!");
                }
            }

            Sensorlijst.ForEach(delegate(SensorNames SN)
            {
                comboBox2.Items.Add(SN.Sensorname);
                listBoxLoc.Items.Add(SN.Sensorname);
                listBoxLoc.Items.Add(SN.Sensorname);
                listBoxControl.Items.Add(SN.Sensorname);
            });
        }

        private void GetSensors()
        {
            string xml_send = @"<?xml version=""1.0"" standalone=""yes""?><Requests xmlns:xsi=""='http://www.w3.org/2001/XMLSchema-instance'""  xmlns=""xmlns='http://tempuri.org/Requests.xsd\'""><Request><RequestName>getAllTelosb</RequestName></Request></Requests>";

            SocketClient socket_client = new SocketClient(Port, controllerIP.Text);
            // Receiving data
            string xml_receive = socket_client.Connect(xml_send, true);

            #region alt. method: dataset
            ////Set up a memory-stream to store the xml
            //MemoryStream MemStream = new MemoryStream();
            ////Write the msg to the memory stream
            //StreamWriter SWriter = new StreamWriter(MemStream);
            //SWriter.WriteLine(xml_receive);
            //SWriter.Flush();
            //MemStream.Position = 0; //Reset the position so we start reading at the start
            ////store the xml data in a dataset
            //tempset.ReadXml(MemStream);

            //List<SensorNames> Sensorlijst = new List<SensorNames>();
            ////process the dataset
            //if (tempset.Tables.Count >= 1)
            //{   //Only send a reply if we actually got a correct Msg to send
            //    //(in other words, when the query actually succeeded)
            //    if ((tempset.Tables[0].Rows.Count <= 0))
            //    {   //We got a result back, but just nothing in it...
            //        //OutMsg = CreateReplyInt(0);
            //        ;
            //    }
            //    else
            //    {
            //        foreach (DataRow row in tempset.Tables[0].Rows) //Run through every sensor in the xml-message
            //        {
            //            Sensorlijst.Add(new SensorNames(row["nodeid"].ToString()));
            //        }
            //    }
            //}        
            #endregion


            XmlDocument tempdoc = new XmlDocument();
            tempdoc.LoadXml(xml_receive);

            XmlNodeList bookList = tempdoc.GetElementsByTagName("Sensor");
            foreach (XmlNode node in bookList)
            {
                XmlElement ID = (XmlElement)(node);
                try
                {
                    string sensor = ID.GetElementsByTagName("sensor")[0].InnerText;
                    string idnode = ID.GetElementsByTagName("idnode")[0].InnerText;
                    Sensorlijst.Add(new SensorNames(sensor, idnode));
                }
                catch
                {
                    Console.WriteLine("No sensors avaible!");
                }
            }

            Sensorlijst.ForEach(delegate(SensorNames SN)
            {
                comboBox2.Items.Add(SN.Sensorname);
                listBoxLoc.Items.Add(SN.Sensorname);
                listBoxControl.Items.Add(SN.Sensorname);
            });
        }
        #endregion

        private void Form1_Resize_1(object sender, EventArgs e)
        {
            SetSize();
        }

        #region properties

        /// <summary>
        /// Property of checkboxActive
        /// input: 0 or 1 string
        /// </summary>
        public string ActiveProperty
        {
            get{
                if (checkBoxActive.Checked)
                    return "1";
                else
                    return "0";
            }
            set {
                if (value == "1")
                    checkBoxActive.Checked = true;
                else
                    checkBoxActive.Checked = false;
            }
        }

        /// <summary>
        /// Property of checkBoxAnchorNode
        /// input: 0 or 1 string
        /// </summary>
        public string AnchorProperty
        {
            get {
                if (checkBoxAnchorNode.Checked)
                    return "1";
                else
                    return "0";
            }
            set {
                if (value == "1")
                    checkBoxAnchorNode.Checked = true;
                else
                    checkBoxAnchorNode.Checked = false;
            }
        }

        /// <summary>
        /// Property of the three leds 
        /// Input & Output: Bitmask of the leds
        /// </summary>
        public string LedsProperty
        {
            get{
                int ledsmask = 0;
                if (checkBoxLedRed.Checked)
                    ledsmask += 4;
                if (checkBoxLedGreen.Checked)
                    ledsmask += 2;
                if (checkBoxLedBlue.Checked)
                    ledsmask += 1;
                return ledsmask.ToString();
            }
            set {
                int mask = Convert.ToInt16(value);
                int remainder;

                remainder = mask;
                if (remainder >= 4)
                {
                    checkBoxLedRed.Checked = true;
                    remainder = remainder % 4;
                    if (remainder >= 2)
                    {
                        checkBoxLedGreen.Checked = true;
                        remainder %= 2;
                        if (remainder == 1)
                        {
                            checkBoxLedBlue.Checked = true;
                        }
                        else
                            checkBoxLedBlue.Checked = false;
                    }
                    else
                    {
                        checkBoxLedGreen.Checked = false;
                        remainder %= 2;
                        if (remainder == 1)
                        {
                            checkBoxLedBlue.Checked = true;
                        }
                        else
                            checkBoxLedBlue.Checked = false;
                    }
                }
                else
                {
                    checkBoxLedRed.Checked = false;
                    remainder = remainder % 4;
                    if (remainder >= 2)
                    {
                        checkBoxLedGreen.Checked = true;
                        remainder %= 2;
                        if (remainder == 1)
                        {
                            checkBoxLedBlue.Checked = true;
                        }
                        else
                            checkBoxLedBlue.Checked = false;
                    }
                    else
                    {
                        checkBoxLedGreen.Checked = false;
                        remainder %= 2;
                        if (remainder == 1)
                        {
                            checkBoxLedBlue.Checked = true;
                        }
                        else
                            checkBoxLedBlue.Checked = false;
                    }

                }
            }
        }

        /// <summary>
        /// Set or Gets the Power value
        /// </summary>
        public string PowerProperty
        {
            get { return numericUpDownPower.Value.ToString();  }
            set { numericUpDownPower.Value = Convert.ToDecimal(value);  }
        }

        /// <summary>
        /// Set or Gets the Frequency value
        /// </summary>
        public string FrequencyProperty
        {
            get { return numericUpDownFrequency.Value.ToString();  }
            set { numericUpDownFrequency.Value = Convert.ToDecimal(value);  }
        }


        #endregion
    }
}
