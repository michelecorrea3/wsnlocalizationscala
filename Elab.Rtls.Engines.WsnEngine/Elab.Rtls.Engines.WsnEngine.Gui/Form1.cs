using System;
using System.Net.Sockets;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

            //initialize fields
            controllerIP.Text = "localhost";
            controllerPort.Text = "1900";

            textLocRefresh.Text = "1000";
            textSensRefresh.Text = "5000";
            textBoxGraphUpdateInterval.Text = "10000";
            textBoxSensorFetchUpdateInterval.Text = "60000";

            //maskedTextBox1.ValidatingType = typeof(System.TimeSpan);
            maskedTextBox1.Text = "10 10:00:00";

            textBoxXmin.Text = "0";
            textBoxXmax.Text = "1000";
            textBoxYmin.Text = "0";
            textBoxYmax.Text = "1000";
            textBoxSensorRateMax.Text = "60000";
            textBoxSensorRateMin.Text = "1000";
            textBoxLocRateMin.Text = "100";
            textBoxLocRateMax.Text = "50000";

            buttonDisconnect.Enabled = false;

            // Add the graph parameters
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
            try
            {
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
                    try
                    {
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
            }
            catch (ArgumentNullException nullex)
            {
                Console.WriteLine(nullex.Message);
                Console.WriteLine(nullex.TargetSite);
                SocketClient sc = new SocketClient(Port, controllerIP.Text);
                if (!sc.TryConnection())
                    Disconnect();
                MessageBox.Show("Lost connection to the controller");
            }
            catch { }
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
            try
            {
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
            }
            catch (ArgumentNullException nullex)
            {
                Console.WriteLine(nullex.Message);
                Console.WriteLine(nullex.TargetSite);
                SocketClient sc = new SocketClient(Port, controllerIP.Text);
                if (!sc.TryConnection())
                    Disconnect();
                MessageBox.Show("Lost connection to the controller");
            }
            catch { }
            return "N/A";
        }
        #endregion

        #region Plot graph

        /// <summary>
        /// Resizes the graph area when the user resizes the form
        /// </summary>
        private void SetSize()
        {
            zg1.Location = new Point(250, 25);
            // Leave a small margin around the outside of the control
            zg1.Size = new Size(this.ClientRectangle.Width - 290, this.ClientRectangle.Height - 90);
            //scale the size of the parent control
            tabControl1.Size = new Size(this.ClientRectangle.Width - 20, this.ClientRectangle.Height - 20);
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
        private void CreateGraph(ZedGraphControl zgc, string measurements, string xaxis, string yaxis)
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
                double y = Convert.ToDouble(yyyy);
                x = x + 1;
                list.Add(x, y);
            }

            myPane.AddCurve("MyCurve", list, Color.Red, SymbolType.Square);
            myPane.XAxis.Title.Text = xaxis;
            myPane.YAxis.Title.Text = yaxis;

            zgc.AxisChange();
            zgc.Invalidate();
        }

        /// <summary>
        /// Retreives the graph data from the controller
        /// </summary>
        private void GetGraphData()
        {
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
                    if (comboBox2.SelectedItem != null)
                    {
                        string nodeid = getNodeID(comboBox2.SelectedItem.ToString()); ;

                        xml_send += "<arg>" + nodeid + "</arg>";

                        if (comboBox1.SelectedItem != null)
                        {
                            xml_send += "<arg>" + comboBox1.SelectedItem.ToString() + "</arg><arg>" + comboBoxGraphNumMeasurements.Text + "</arg></Request></Requests>";
                        }
                        else
                            return;
                    }
                    else
                        return;

                    //communication: send the command string to the controller and receive a response
                    SocketClient socket_client = new SocketClient(Port, controllerIP.Text);
                    string xml_receive = socket_client.Connect(xml_send, true);


                    //process the reply
                    XmlDocument tempdoc = new XmlDocument();
                    tempdoc.LoadXml(xml_receive);

                    //put the received values in the graph list
                    listViewGraphValues.Items.Clear();
                    XmlNodeList bookList = tempdoc.GetElementsByTagName("MeasurementValue");

                    foreach (XmlNode node in bookList)
                    {
                        string id_Measurement = node.InnerText.ToString();
                        listViewGraphValues.Items.Add(id_Measurement);
                    }

                    CreateGraph(zg1, xml_receive, "number of measurements", comboBox1.SelectedItem.ToString());
                    SetSize();
                }
                catch (ArgumentNullException nullex)
                {
                    Console.WriteLine(nullex.Message);
                    Console.WriteLine(nullex.TargetSite);
                    SocketClient sc = new SocketClient(Port, controllerIP.Text);
                    if (!sc.TryConnection())
                        Disconnect();
                    MessageBox.Show("Lost connection to the controller");
                }
        }

        /// <summary>
        /// Occurs when the user clicks the plot button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click_1(object sender, EventArgs e)
        {
            GetGraphData();
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

        #region Environment tab
        
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
                if (listBoxLoc.SelectedItem != null)
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
                            int index = ID.GetElementsByTagName("Time")[0].InnerText.IndexOf('T');
                            textBoxSensUpdate.Text = ID.GetElementsByTagName("Time")[0].InnerText.Substring(index);
                        }
                        //unable to parse some field
                        catch (Exception ex)
                        {
                            Console.WriteLine("Some field is not available");
                            Console.WriteLine(ex.Message);
                            Console.WriteLine(ex.TargetSite);
                        }
                    }
                }
                else
                    return;
            }
            catch (ArgumentNullException nullex)
            {
                Console.WriteLine(nullex.Message);
                Console.WriteLine(nullex.TargetSite);
                SocketClient sc = new SocketClient(Port, controllerIP.Text);
                if (!sc.TryConnection())
                    Disconnect();
                MessageBox.Show("Lost connection to the controller");
            }

        }
        #endregion

        #region Localization tab

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

                if (listBoxLoc.SelectedItem != null)
                {
                    string nodeid;
                    nodeid = getNodeID(listBoxLoc.SelectedItem.ToString());

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
                                int index = ID.GetElementsByTagName("Time")[0].InnerText.IndexOf('T');
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
                        catch (Exception ex)
                        {
                            Console.WriteLine("Some field is not available");
                            Console.WriteLine(ex.Message);
                            Console.WriteLine(ex.TargetSite);
                        }
                    }
                }
                else
                    return;
            }
            catch (ArgumentNullException nullex)
            {
                Console.WriteLine(nullex.Message);
                Console.WriteLine(nullex.TargetSite);
                SocketClient sc = new SocketClient(Port, controllerIP.Text);
                if (!sc.TryConnection())
                    Disconnect();
                MessageBox.Show("Lost connection to the controller");
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
            timerStatus.Enabled = true;
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
                //make the XML string to send
                string selected = listBoxControl.SelectedItem.ToString();
                string nodeid = getNodeID(selected);

                xml_send += "<arg>" + nodeid + "</arg></Request></Requests>";

                SocketClient socket_client = new SocketClient(Port, controllerIP.Text);
                // Receiving data
                string xml_receive = socket_client.Connect(xml_send, true);

                //set the textfields to the received XML nodes
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
                        LedsProperty = ID.GetElementsByTagName("Leds")[0].InnerText;
                        PowerProperty = ID.GetElementsByTagName("Power")[0].InnerText;
                        FrequencyProperty = ID.GetElementsByTagName("Frequency")[0].InnerText;
                        textBoxConn.Text = ID.GetElementsByTagName("Conn")[0].InnerText;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.StackTrace);
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
            //catch the exceptions
            catch (ArgumentNullException nullex)
            {
                Console.WriteLine(nullex.Message);
                Console.WriteLine(nullex.TargetSite);
                SocketClient sc = new SocketClient(Port, controllerIP.Text);
                if (!sc.TryConnection())
                {
                    Disconnect();
                    MessageBox.Show("Lost connection to the controller");
                    tabPage4.Select();
                }
            }
            catch (SocketException sockex)
            {
                Console.WriteLine(sockex.Message);
                Console.WriteLine(sockex.TargetSite);
                SocketClient sc = new SocketClient(Port, controllerIP.Text);
                if (!sc.TryConnection())
                {
                    buttonDisconnect_Click(this, EventArgs.Empty);
                    MessageBox.Show("Lost connection to the controller");
                    tabPage4.Select();
                }
                else
                    MessageBox.Show("The WSN took to long to respond");
            }
        }

        /// <summary>
        /// Sends any changes to the control tab to the controller.
        /// Fields are accessible according to business rules
        /// An acknowledgment is sent back, this is compared with the current values in the control tab 
        /// to determine if the changes were applied succesfully
        /// </summary>
        private void AcceptChanges()
        {
            DialogResult result = DialogResult.OK;

            //disable status timer to prevent race conditions
            timerStatus.Enabled = false;

            //TODO: needs validation;
            buttonWSNControl.Enabled = false;

            if (
                oldChanges.Active != ActiveProperty || oldChanges.AN != AnchorProperty || oldChanges.X != textBoxControlX.Text ||
                oldChanges.Y != textBoxControlY.Text || oldChanges.locrate != textBoxLocRate.Text || oldChanges.samplerate != textBoxSampleRate.Text ||
                oldChanges.leds != LedsProperty || oldChanges.power != PowerProperty || oldChanges.frequency != FrequencyProperty)
            {
                //we send the WSNactionRequest here
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
                        //if the AN status has changed send it
                        if (oldChanges.AN != AnchorProperty)
                            xml_send += "<AN>" + AnchorProperty + "</AN>";
                        //if the AN status has just changed to AN send all the parameters
                        if (AnchorProperty == "1" && oldChanges.AN != AnchorProperty)
                        {
                            xml_send += "<X>" + textBoxControlX.Text + "</X>";
                            xml_send += "<Y>" + textBoxControlY.Text + "</Y>";
                            xml_send += "<LocRate>" + textBoxLocRate.Text + "</LocRate>";

                            result = MessageBox.Show("Make this node an Anchor Node with these parameters?","Check parameters",MessageBoxButtons.OKCancel);
                        }
                        // if the node was an AN already we should still check if any of the parameters has changed
                        else if (AnchorProperty == "1")
                        {
                            if (oldChanges.X != textBoxControlX.Text)
                                xml_send += "<X>" + textBoxControlX.Text + "</X>";
                            if (oldChanges.Y != textBoxControlY.Text)
                                xml_send += "<Y>" + textBoxControlY.Text + "</Y>";
                            if (oldChanges.locrate != textBoxLocRate.Text)
                                xml_send += "<LocRate>" + textBoxLocRate.Text + "</LocRate>";
                        }
                    }

                    //these field are not unique to the localization algorithm and are thus independent
                    //sensor parameters
                    if (oldChanges.samplerate != textBoxSampleRate.Text)
                        xml_send += "<Samplerate>" + textBoxSampleRate.Text + "</Samplerate>";
                    //leds
                    if (oldChanges.leds != LedsProperty)
                        xml_send += "<Leds>" + LedsProperty + "</Leds>";
                    //radio parameters
                    if (oldChanges.power != PowerProperty)
                        xml_send += "<Power>" + PowerProperty + "</Power>";
                    if (oldChanges.frequency != FrequencyProperty)
                        xml_send += "<Frequency>" + FrequencyProperty + "</Frequency>";

                    xml_send += "</RequestAction></WSNReq></WSNReq>";

                    if (result == DialogResult.OK)
                    {
                    //we can now send the request, we will receive a status message as a response
                    SocketClient socket_client = new SocketClient(Port, controllerIP.Text);
                    // Sends: ActionRequest
                    // Receives: Status
                    string xml_receive = socket_client.Connect(xml_send, true);

                    XmlDocument tempdoc = new XmlDocument();
                    tempdoc.LoadXml(xml_receive);

                    if (tempdoc.DocumentElement.Name.ToString() == "Replies") 
                    {
                        DiscardChanges();
                        MessageBox.Show("The WSN did not reply in time");
                        buttonWSNControl.Enabled = true;
                        return;
                    }

                    XmlNodeList bookList = tempdoc.GetElementsByTagName("WSNReply");
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
                                DiscardChanges();

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
                }
                //good ol' catches
                catch (ArgumentNullException nullex)
                {
                    Console.WriteLine(nullex.Message);
                    Console.WriteLine(nullex.TargetSite);
                    SocketClient sc = new SocketClient(Port, controllerIP.Text);
                    if (!sc.TryConnection())
                        buttonDisconnect_Click(this, EventArgs.Empty);
                    MessageBox.Show("Lost connection to the controller");
                }
                catch (SocketException sockex)
                {
                    Console.WriteLine(sockex.Message);
                    Console.WriteLine(sockex.TargetSite);
                    SocketClient sc = new SocketClient(Port, controllerIP.Text);
                    if (!sc.TryConnection())
                        Disconnect();
                    MessageBox.Show("Lost connection to the controller");
                }
            }


            timerStatus.Enabled = true;
            buttonWSNControl.Enabled = true;
        }

        /// <summary>
        /// Occurs when the user clicks the Discard Changes Button
        /// The textfields are set to the values in oldchanges, the backup struct
        /// </summary>
        private void DiscardChanges()
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

        /// <summary>
        /// Gets the nodes status and compares it to the currently stored status.
        /// </summary>
        /// <returns>Bool indicating whether the statuses are different or not</returns>
        private bool CheckStatus()
        {
            //fetch status
            //static part of the message
            string xml_send = @"<?xml version=""1.0"" standalone=""yes""?><Requests xmlns:xsi=""='http://www.w3.org/2001/XMLSchema-instance'""  xmlns=""xmlns='http://tempuri.org/Requests.xsd\'""><Request><RequestName>getStatus</RequestName>";

            //return the nodeid with sensor as input
            try
            {
                //make the XML string to send
                string selected = listBoxControl.SelectedItem.ToString();
                string nodeid = getNodeID(selected);

                xml_send += "<arg>" + nodeid + "</arg></Request></Requests>";

                SocketClient socket_client = new SocketClient(Port, controllerIP.Text);
                // Receiving data
                string xml_receive = socket_client.Connect(xml_send, true);

                //set the textfields to the received XML nodes
                XmlDocument tempdoc = new XmlDocument();
                tempdoc.LoadXml(xml_receive);

                XmlNodeList bookList = tempdoc.GetElementsByTagName("Status");
                foreach (XmlNode node in bookList)
                {
                    XmlElement ID = (XmlElement)(node);
                    try
                    {
                        //compare status to changes
                        if (
                            oldChanges.Active == ID.GetElementsByTagName("active")[0].InnerText &&
                            oldChanges.AN == ID.GetElementsByTagName("AN")[0].InnerText &&
                            oldChanges.X == ID.GetElementsByTagName("X")[0].InnerText &&
                            oldChanges.Y == ID.GetElementsByTagName("Y")[0].InnerText &&
                            oldChanges.samplerate == ID.GetElementsByTagName("Samplerate")[0].InnerText &&
                            oldChanges.locrate == ID.GetElementsByTagName("LocRate")[0].InnerText &&
                            oldChanges.leds == ID.GetElementsByTagName("Leds")[0].InnerText &&
                            oldChanges.power == ID.GetElementsByTagName("Power")[0].InnerText &&
                            oldChanges.frequency == ID.GetElementsByTagName("Frequency")[0].InnerText)
                            return false;
                        else
                            return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.StackTrace);
                        Console.WriteLine("Some field is not available");
                    }
                }
            }
            //catch the exceptions
            catch (ArgumentNullException nullex)
            {
                Console.WriteLine(nullex.Message);
                Console.WriteLine(nullex.TargetSite);
                SocketClient sc = new SocketClient(Port, controllerIP.Text);
                if (!sc.TryConnection())
                    Disconnect();
                MessageBox.Show("Lost connection to the controller");
            }
            catch (SocketException sockex)
            {
                Console.WriteLine(sockex.Message);
                Console.WriteLine(sockex.TargetSite);
                SocketClient sc = new SocketClient(Port, controllerIP.Text);
                if (!sc.TryConnection())
                {
                    buttonDisconnect_Click(this, EventArgs.Empty);
                    MessageBox.Show("Lost connection to the controller");
                }
                else
                    MessageBox.Show("The WSN took to long to respond");
            }
            return false;
        }

        /// <summary>
        /// Occurs when the apply changes button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonWSNControl_Click(object sender, EventArgs e)
        {
            AcceptChanges();
        }

        /// <summary>
        /// Occurs when the discard changes button is clicked
        /// The fields in the Control tab are reset to their previous value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonControlDiscard_Click(object sender, EventArgs e)
        {
            DiscardChanges();
        }

        /// <summary>
        /// Occurs when the timer to get the status fires
        /// Calls CheckStatus() which gets the nodes status and compares it to the currently stored status.
        /// If these statuses are not the same, the current status is updated from the DB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerStatus_Tick(object sender, EventArgs e)
        {
            //GetStatData();
            //compare the status from a get to the changes struct
            bool statusChanged = CheckStatus();
            if (statusChanged)
            {
                MessageBox.Show("Status has changed\n Discarding any changes made and showing the newest status");
                GetStatData();
            }
        }

        /// <summary>
        /// Occurs when the Active checkbox is changes
        /// Determines the availability of other fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxActive_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxActive.Checked)
            {
                checkBoxAnchorNode.Enabled = true;
                if (checkBoxAnchorNode.Checked)
                {
                    textBoxLocRate.Enabled = true;
                    textBoxControlX.Enabled = true;
                    textBoxControlY.Enabled = true;
                }
            }
            else
            {
                checkBoxAnchorNode.Enabled = false;
                textBoxLocRate.Enabled = false;
                textBoxControlX.Enabled = false;
                textBoxControlY.Enabled = false;
            }
        }

        /// <summary>
        /// Occurs when the Anchor checkbox is changes
        /// Determines the availability of other fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxAnchorNode_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxActive.Checked)
            {
                checkBoxAnchorNode.Enabled = true;
                if (checkBoxAnchorNode.Checked)
                {
                    textBoxLocRate.Enabled = true;
                    textBoxControlX.Enabled = true;
                    textBoxControlY.Enabled = true;
                }
                else
                {
                    textBoxLocRate.Enabled = false;
                    textBoxControlX.Enabled = false;
                    textBoxControlY.Enabled = false;
                }
            }
            else
            {
                checkBoxAnchorNode.Enabled = false;
            }
        }

        #endregion

        #region Options tab
        
        /// <summary>
        /// Occurs when the user clicks the connect button on the options tab
        /// Allows the user to connect & disconnect to the controller and set various parameters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonConnect_Click(object sender, EventArgs e)
        {
            Connect(); 
        }

        private void Connect()
        {
            Port = Convert.ToInt32(controllerPort.Text);
            SocketClient sc = new SocketClient(Port, controllerIP.Text);

            if (sc.TryConnection())
            {
                timerSensor.Enabled = true;
                timerLoc.Enabled = true;

                //GUI tasks
                buttonConnect.Enabled = false;
                buttonDisconnect.Enabled = true;

                //Disable the connection fields
                controllerIP.Enabled = false;
                controllerPort.Enabled = false;

                toolStripStatusLabel.Text = "Connected to controller at IP " + controllerIP.Text + ", port " + controllerPort.Text;

                //Sensorfetch
                if (radioButtonGetSensors.Checked == true)
                    GetSensors();
                else if (radioButtonDiscovery.Checked == true)
                    timerSensorFetch.Enabled = true;
                else
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
            Disconnect();
        }

        private void Disconnect()
        {
            timerSensor.Enabled = false;
            timerLoc.Enabled = false;
            timerSensorFetch.Enabled = false;

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
            timerSensor.Interval = Convert.ToInt32(textSensRefresh.Text);
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
            if (radioButtonDiscovery.Checked)
                Discovery();
            else if (radioButtonSensorTimeOut.Checked)
                SensorsTimeOut();
        }

        private void Discovery()
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
            try
            {
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
            catch (ArgumentNullException nullex)
            {
                Console.WriteLine(nullex.Message);
                Console.WriteLine(nullex.TargetSite);
                SocketClient sc = new SocketClient(Port, controllerIP.Text);
                if (!sc.TryConnection())
                    buttonDisconnect_Click(this, EventArgs.Empty);
                MessageBox.Show("Lost connection to the controller");
            }
            catch { }
        }

        /// <summary>
        /// Active nodes are selected when they have sent a packet within a given time limit
        /// TODO: process empty string
        /// </summary>
        private void SensorsTimeOut()
        {
            //clear the existing sensors
            Sensorlijst.Clear();
            listBoxControl.Items.Clear();
            listBoxLoc.Items.Clear();
            comboBox2.Items.Clear();
            
            //Get the TimeOut timespan and parse it into the correct string format (d hh:mm:ss)
            //TimeSpan TimeOut = new TimeSpan(maskedTextBox1.Text;
            //string TimeOutString = TimeOut.Days.ToString() + " " + TimeOut.Hours.ToString() + ":" + TimeOut.Minutes.ToString() + ":" + TimeOut.Seconds.ToString();
            //string TimeOut = maskedTextBox1.Text;
            //TimeOu
            //TimeSpan TimeOut = new TimeSpan();

            //string xml_send = @"<?xml version=""1.0"" standalone=""yes""?><Requests xmlns:xsi=""='http://www.w3.org/2001/XMLSchema-instance'""  xmlns=""xmlns='http://tempuri.org/Requests.xsd\'""><Request><RequestName>getTimeOutSensors</RequestName><arg>" + TimeOutString + "</arg></Request></Requests>";
            string xml_send = @"<?xml version=""1.0"" standalone=""yes""?><Requests xmlns:xsi=""='http://www.w3.org/2001/XMLSchema-instance'""  xmlns=""xmlns='http://tempuri.org/Requests.xsd\'""><Request><RequestName>getTimeOutSensors</RequestName><arg>" + maskedTextBox1.Text + "</arg></Request></Requests>";
            try
            {
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
            catch (ArgumentNullException nullex)
            {
                Console.WriteLine(nullex.Message);
                Console.WriteLine(nullex.TargetSite);
                SocketClient sc = new SocketClient(Port, controllerIP.Text);
                if (!sc.TryConnection())
                    buttonDisconnect_Click(this, EventArgs.Empty);
                MessageBox.Show("Lost connection to the controller");
            }
            catch (Exception ex )
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.TargetSite);
            }
        }

        private void textBoxSensorFetchUpdateInterval_TextChanged(object sender, EventArgs e)
        {
            timerSensorFetch.Interval = Convert.ToInt32(textBoxSensorFetchUpdateInterval.Text);
        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {
            toolTipActive.ToolTipTitle = "Invalid input";
            toolTipActive.Show("Enter the timeout parameter as days hours:minutes:seconds (dd hh:mm:ss)", maskedTextBox1, maskedTextBox1.Location, 5000);
        }

        private void maskedTextBox1_TypeValidationCompleted(object sender, TypeValidationEventArgs e)
        {
            if (!e.IsValidInput)
            {
                toolTipActive.ToolTipTitle = "Invalid input";
                toolTipActive.Show("Enter the timeout parameter as days hours:minutes:seconds (dd hh:mm:ss)", maskedTextBox1, maskedTextBox1.Location, 5000);
                e.Cancel = true;
            }
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

        #region Control validation
        private void textBoxControlX_Validating(object sender, CancelEventArgs e)
        {
            string errorMsg;
            if (!ValidX(textBoxControlX.Text, out errorMsg))
            {
                // Cancel the event and select the text to be corrected by the user.
                e.Cancel = true;
                textBoxControlX.Select(0, textBoxControlX.Text.Length);

                // Set the ErrorProvider error with the text to display. 
                this.errorProvider1.SetError(textBoxControlX, errorMsg);
            }
        }

        private void textBoxControlX_Validated(object sender, EventArgs e)
        {
            // If all conditions have been met, clear the ErrorProvider of errors.
            errorProvider1.SetError(textBox1, "");
            errorProvider1.Clear();
        }

        private bool ValidX(string X, out string errorMessage)
        {   
            short result;

            if (!Int16.TryParse(X, out result))
            {
                errorMessage = "X is not numeric";
                return false;
            }
            
            if ((Convert.ToInt32(X)) < (Convert.ToInt32(textBoxXmin.Text)))
            {
                errorMessage = "X is too small";
                return false; 
            }
            else if ((Convert.ToInt32(X)) > (Convert.ToInt32(textBoxXmax.Text)))
            {
                    errorMessage = "X is too large";
                    return false;
            }

            errorMessage = "";
            return true;
        }

        private void textBoxControlY_Validating(object sender, CancelEventArgs e)
        {
            string errorMsg;
            if (!ValidY(textBoxControlY.Text, out errorMsg))
            {
                // Cancel the event and select the text to be corrected by the user.
                e.Cancel = true;
                textBoxControlY.Select(0, textBoxControlY.Text.Length);

                // Set the ErrorProvider error with the text to display. 
                this.errorProvider1.SetError(textBoxControlY, errorMsg);
            }
        }

        private void textBoxControlY_Validated(object sender, EventArgs e)
        {
            // If all conditions have been met, clear the ErrorProvider of errors.
            errorProvider1.SetError(textBox1, "");
            errorProvider1.Clear();
        }

        private bool ValidY(string Y, out string errorMessage)
        {
            short result;
            //if (Int16.TryParse(
            if (!Int16.TryParse(Y, out result))
            {
                errorMessage = "Y is not numeric";
                return false;
            }

            if ((Convert.ToInt32(Y)) < (Convert.ToInt32(textBoxYmin.Text)))
            {
                errorMessage = "Y is too small";
                return false;
            }
            else if ((Convert.ToInt32(Y)) > (Convert.ToInt32(textBoxYmax.Text)))
            {
                errorMessage = "Y is too large";
                return false;
            }

            errorMessage = "";
            return true;
        }

        private void textBoxLocRate_Validating(object sender, CancelEventArgs e)
        {
            string errorMsg;
            if (!ValidLocRate(textBoxLocRate.Text, out errorMsg))
            {
                // Cancel the event and select the text to be corrected by the user.
                e.Cancel = true;
                textBoxLocRate.Select(0, textBoxLocRate.Text.Length);

                // Set the ErrorProvider error with the text to display. 
                this.errorProvider1.SetError(textBoxLocRate, errorMsg);
            }
        }

        private bool ValidLocRate(string LocRate, out string errorMessage)
        {
            int result;
            //if (Int16.TryParse(
            if (!Int32.TryParse(LocRate, out result))
            {
                errorMessage = "LocRate is not numeric";
                return false;
            }

            if ((Convert.ToInt32(LocRate)) < (Convert.ToInt32(textBoxLocRateMin.Text)))
            {
                errorMessage = "LocRate is too small";
                return false;
            }
            else if ((Convert.ToInt32(LocRate)) > (Convert.ToInt32(textBoxLocRateMax.Text)))
            {
                errorMessage = "LocRate is too large";
                return false;
            }

            errorMessage = "";
            return true;
        }

        private void textBoxLocRate_Validated(object sender, EventArgs e)
        {
            // If all conditions have been met, clear the ErrorProvider of errors.
            errorProvider1.SetError(textBox1, "");
            errorProvider1.Clear();
        }

        private void textBoxSampleRate_Validating(object sender, CancelEventArgs e)
        {
            string errorMsg;
            if (!ValidSampleRate(textBoxSampleRate.Text, out errorMsg))
            {
                // Cancel the event and select the text to be corrected by the user.
                e.Cancel = true;
                textBoxLocRate.Select(0, textBoxSampleRate.Text.Length);

                // Set the ErrorProvider error with the text to display. 
                this.errorProvider1.SetError(textBoxSampleRate, errorMsg);
            }
        }

        private bool ValidSampleRate(string SampleRate, out string errorMessage)
        {
            int result;
            //if (Int16.TryParse(
            if (!Int32.TryParse(SampleRate, out result))
            {
                errorMessage = "SampleRate is not numeric";
                return false;
            }

            if ((Convert.ToInt32(SampleRate)) < (Convert.ToInt32(textBoxSampleRate.Text)))
            {
                errorMessage = "SampleRate is too small";
                return false;
            }
            else if ((Convert.ToInt32(SampleRate)) > (Convert.ToInt32(textBoxSensorRateMax.Text)))
            {
                errorMessage = "SampleRate is too large";
                return false;
            }

            errorMessage = "";
            return true;
        }

        private void textBoxSampleRate_Validated(object sender, EventArgs e)
        {
            // If all conditions have been met, clear the ErrorProvider of errors.
            errorProvider1.SetError(textBox1, "");
            errorProvider1.Clear();
        }
        #endregion
    }
}
