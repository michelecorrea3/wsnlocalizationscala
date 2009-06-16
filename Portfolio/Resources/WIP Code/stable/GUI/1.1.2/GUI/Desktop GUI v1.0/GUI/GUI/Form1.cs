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

        #region Form methods
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
            textBoxGraphUpdateInterval.Text = "10000";

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
            });

        }

        /// <summary>
        /// Event that fires when the user resizes the form
        /// Resized the graph area
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Resize_1(object sender, EventArgs e)
        {
            SetSize();
        }
        #endregion

        #region helper methods
        ///<summary>
        ///Helper method for retreiving the nodeid of the framework
        /// </summary>
        /// <param name="selected">
        /// The MAC or TOSid of the mote
        /// </param>
        /// <returns>
        /// node id, unique identifier within Senseless
        /// </returns>
        private string getNodeID(string selected)
        {
            //first search local for the nodeid
            string nodeid;
            foreach (SensorNames telosb in Sensorlijst)
            {
                if (telosb.Sensorname == selected)
                {
                    nodeid = telosb.id;
                    return nodeid;
                }
            }
            
            //generate string
            string xml_send = @"<?xml version=""1.0"" standalone=""yes""?><Requests xmlns:xsi=""='http://www.w3.org/2001/XMLSchema-instance'""  xmlns=""xmlns='http://tempuri.org/Requests.xsd\'""><Request><RequestName>getNodeid</RequestName><arg>" + selected + "</arg></Request></Requests>";
            
            SocketClient socket_client = new SocketClient(Port, controllerIP.Text);
            // Receiving data
            string xml_receive = socket_client.Connect(xml_send, true);


            //process reply
            XmlDocument tempdoc = new XmlDocument();
            tempdoc.LoadXml(xml_receive);

            XmlNodeList bookList = tempdoc.GetElementsByTagName("NodeIDs");
            foreach (XmlNode node in bookList)
            {
                XmlElement ID = (XmlElement)(node);
                try {
                    string idnode = ID.GetElementsByTagName("idnode")[0].InnerText;
                    Sensorlijst.Add(new SensorNames(selected, idnode));
                    return idnode;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Node id not found in DB");
                }
            }
            return "N/A";
        }

        /// <summary>
        /// Helper method for retreiving the WSNid of the framework
        /// </summary>
        /// <param name="selected">
        /// node id, unique identifier within Senseless
        /// </param>
        /// <returns>
        /// The MAC or TOSid of the mote
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
            string xml_send = @"<?xml version=""1.0"" standalone=""yes""?><Requests xmlns:xsi=""='http://www.w3.org/2001/XMLSchema-instance'""  xmlns=""xmlns='http://tempuri.org/Requests.xsd\'""><Request><RequestName>getWSNID</RequestName><arg>" + selected + "</arg></Request></Requests>";

            SocketClient socket_client = new SocketClient(Port, controllerIP.Text);
            // Receiving data
            string xml_receive = socket_client.Connect(xml_send, true);

            XmlDocument tempdoc = new XmlDocument();
            tempdoc.LoadXml(xml_receive);

            XmlNodeList bookList = tempdoc.GetElementsByTagName("WSNID");
            foreach (XmlNode node in bookList)
            {
                XmlElement ID = (XmlElement)(node);
                try
                {
                    string sensor = ID.GetElementsByTagName("sensor")[0].InnerText;
                    Sensorlijst.Add(new SensorNames(sensor, selected));
                    return sensor;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Node id not found in DB");
                }
            }
            return "N/A";
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
            GetGraphData();
        }

        private void GetGraphData()
        {
                //request
                //static part
                string xml_send = @"<?xml version=""1.0"" standalone=""yes""?><Requests xmlns:xsi=""='http://www.w3.org/2001/XMLSchema-instance'""  xmlns=""xmlns='http://tempuri.org/Requests.xsd\'""><Request><RequestName>";

                //dynamic part
                if (comboBox1.SelectedItem.ToString() == "RSSI" || comboBox1.SelectedItem.ToString() == "Position")
                    xml_send += "getLocHistoryLast</RequestName>";
                else
                    xml_send += "getHistoryLast</RequestName>";

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
                        else
                            return;
                    }
                    else
                        return;

                    SocketClient socket_client = new SocketClient(Port, controllerIP.Text);
                    string xml_receive = socket_client.Connect(xml_send, true);


                    //proces the reply
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
                catch  (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.HelpLink);
                }
        }

        /// <summary>
        /// Occurs when timerGraph fires
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerGraph_Tick(object sender, EventArgs e)
        {
            GetGraphData();
        }

        /// <summary>
        /// Occurs when the user changes the check on checkboxPlotUpdate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxPlotUpdate_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxPlotUpdate.Checked == true)
                timerGraph.Enabled = true;
            else
                timerGraph.Enabled = false;
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
                if (listBoxLoc.SelectedItem.ToString() != null)
                {
                    string nodeid = getNodeID(listBoxLoc.SelectedItem.ToString());

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
                            textBox2.Text = ID.GetElementsByTagName("Sensortype")[0].InnerText;
                            textBox3.Text = ID.GetElementsByTagName("Temperature")[0].InnerText;
                            textBox4.Text = ID.GetElementsByTagName("Light")[0].InnerText;
                            textBox5.Text = ID.GetElementsByTagName("Humidity")[0].InnerText;
                            if (ID.GetElementsByTagName("Sensortype")[0].InnerText == "2")
                            {
                                textBox6.Text = ID.GetElementsByTagName("Power")[0].InnerText;
                            }
                            else
                            {
                                textBox6.Text = "N/A";
                            }
                            int index = ID.GetElementsByTagName("Time")[0].InnerText.IndexOf(' ');
                            textBoxSensUpdate.Text = ID.GetElementsByTagName("Time")[0].InnerText.Substring(index);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else
                    return;
            }
            catch
            {
                Console.WriteLine("Exception in GetSensorData");
                return;
            }
        }
        #endregion

        #region localization tab

        /// <summary>
        /// Occurs when timerLoc fires
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerLoc_Tick(object sender, EventArgs e)
        {
            GetLocData();
        }

        /// <summary>
        /// Retreives the localization data of the selected node from the controller
        /// </summary>
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.TargetSite);
            }
        }
        #endregion

        #region Control tab

        /// <summary>
        /// Occurs when the active sensor is changed in listBoxControl, calls GetStatData
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetStatData();
        }

        /// <summary>
        /// Gets the statusdata off the selected node from the controller
        /// </summary>
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


        /// <summary>
        /// Occurs when the apply changes button is click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                xml_send += "<NodeID>" + nodeid + "</NodeID>";

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
                            xml_send += "<LocRate>" + textBoxLocRate.Text + "</LocRate>";
                    }
                }

                //telosb general parameters
                //all independent
                if (oldChanges.samplerate != textBoxSampleRate.Text)
                    xml_send += "<Samplerate>" + textBoxSampleRate.Text + "</Samplerate>";
                if (oldChanges.leds != LedsProperty)
                    xml_send += "<Leds>" + LedsProperty + "</Leds>";
                if (oldChanges.power != PowerProperty)
                    xml_send += "<Power>" + PowerProperty + "</Power>";
                if (oldChanges.frequency != FrequencyProperty)
                    xml_send += "<Frequency>" + FrequencyProperty+ "</Frequency>";

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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.TargetSite);
            }

            panelControl.Enabled = true;
            buttonWSNControl.Enabled = true;
        }

        /// <summary>
        /// Occurs when the discard changes button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        #region Options tab
        
        /// <summary>
        /// Event for the connect button in the options tab
        /// Allows the user to connect & disconnect to the controller and set various parameters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonConnect_Click(object sender, EventArgs e)
        {
            Port = Convert.ToInt32(controllerPort.Text);
            SocketClient sc = new SocketClient(Port, controllerIP.Text);

            if (sc.TryConnection())
            {
                timer1.Enabled = true;
                timerLoc.Enabled = true;

                //GUI tasks
                buttonConnect.Enabled = false;
                buttonDisconnect.Enabled = true;

                //Disable the connection fields
                controllerIP.Enabled = false;
                controllerPort.Enabled = false;

                toolStripStatusLabel.Text = "Connected to controller at IP " + controllerIP.Text + ", port " + controllerPort.Text;

                //TODO: give the user the choice
                //discovery
                if (radioButtonGetSensors.Checked == true)
                    GetSensors();
                else if (radioButtonDiscovery.Checked == true)
                    timerDiscovery.Enabled = true;
                else
                    //not implemented yet
                    SensorsTimeOut();
            }
            else
            {
                Console.WriteLine("Incorrect connection parameters");
                MessageBox.Show("Could not reach the controller!\n Are the connection parameters correct?");
            }
            
        }

        /// <summary>
        /// Event for the disconnect button in the options tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDisconnect_Click(object sender, EventArgs e)
        {   
            timer1.Enabled = false;
            timerLoc.Enabled = false;
            timerDiscovery.Enabled = false;

            buttonConnect.Enabled = true;
            buttonDisconnect.Enabled = false;

            controllerIP.Enabled = true;
            controllerPort.Enabled = true;

            Sensorlijst.Clear();
            listBoxControl.Items.Clear();
            listBoxLoc.Items.Clear();
            comboBox2.Items.Clear();

            toolStripStatusLabel.Text = "No connection";
        }

        /// <summary>
        /// Occurs when the refreshrate of the Sensordata is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textSensRefresh_TextChanged(object sender, EventArgs e)
        {
            timer1.Interval = Convert.ToInt32(textSensRefresh.Text);
        }

        /// <summary>
        /// Occurs when the refreshrate of the LocData is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textLocRefresh_TextChanged(object sender, EventArgs e)
        {
            timerLoc.Interval = Convert.ToInt32(textLocRefresh.Text);
        }

        /// <summary>
        /// Occurs when the refreshrate of the graph is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxGraphUpdateInterval_TextChanged(object sender, EventArgs e)
        {
            timerGraph.Interval = Convert.ToInt32(textBoxGraphUpdateInterval.Text);
        }

        #endregion

        #region SensorFetch
        
        /// <summary>
        /// Event for the discovery protocol
        /// Discovery protocol sends a WSN action Request to the controller, which forwards it to the WSN parser.
        /// The WSN parser disseminates a status request to each node, the nodes that reply are given as the active nodes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Fetches all the motes in the databases
        /// Has NO offline detection mechanism
        /// </summary>
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
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.TargetSite);
                }
            }

            Sensorlijst.ForEach(delegate(SensorNames SN)
            {
                comboBox2.Items.Add(SN.Sensorname);
                listBoxLoc.Items.Add(SN.Sensorname);
                listBoxControl.Items.Add(SN.Sensorname);
            });
        }

        /// <summary>
        /// Active nodes are selected when they have sent a packet within a given time limit
        /// </summary>
        private void SensorsTimeOut()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Properties

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
