using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Xml;
using System.IO;
using System.Net.Sockets;
using System.Data;
using System.ComponentModel;
using System.ServiceModel;
using System.Diagnostics;
using System.Xml.Linq;

using SocketConnection;
using DatabaseConnection;
using Elab.Rtls.Engines.WsnEngine.Positioning;

using Elab.Toolkit.Core.Xml;
using Scala.Core;

namespace Elab.Rtls.Engines.WsnEngine
{
    /// <summary>
    /// The Senseless Controller
    /// </summary>
    public class Controller
    {
        #region private variables
        /// <summary>
        /// Array that has all the backgroundworkers in it (=SocketServers)
        /// </summary>
        private Array BackGroundWorkers;

        /// <summary>
        /// The dataset with the information that is read from the config.txt file
        /// </summary>
        private DataSet Options = new DataSet();

        /// <summary>
        /// MySQL-linker
        /// </summary>
        private MySQLClass MySQLConn;

        /// <summary>
        /// BN's WsnId
        /// </summary>
        private string currentID;

        /// <summary>
        /// List with all the node that should be positioned
        /// </summary>
        private List<Node> BlindNodes = new List<Node>();

        private List<Node> AnchorNodes = new List<Node>();

        #endregion private variables

        #region Properties

        /// <summary>
        /// Property for the used algororithm
        /// </summary>
        public string SelectedAlgorithm
        {
            get; set;
        }

        /// <summary>
        /// Property for the used filter
        /// </summary>
        public string SelectedFilter
        {
            get; set;
        }

        public bool UseCalibration
        {
            get; set;
        }

        public bool UseMultihop
        {
            get; set;
        }

        #endregion

        #region Event variables

        public event EventHandler<EventMessage> LocationUpdated;
        public event EventHandler<EventMessage> TemperatureChanged;
        public event EventHandler<EventMessage> HumidityChanged;
        public event EventHandler<EventMessage> LightChanged;
        public event EventHandler<EventMessage> ButtonPressed;

        #endregion

        /// <summary>
        /// Constructor for the control panel (SocketServerPanel-class)
        /// </summary>
        public Controller()
        {
            Console.WriteLine("Preparing SocketServerApp for use...");
            Console.WriteLine("loading config...");
            LoadOptions();  //Load the config-file
            Console.WriteLine("config loaded");

            //if (File.Exists("DBFaults.txt"))
            //    File.Delete("DBFaults.txt");

            StartWsnEngine();
        }

        /// <summary>
        /// Read the config.txt in the base-directory of the executable and prepare the database-linkers.
        /// Peter: Loads the database info 
        /// </summary>
        private void LoadOptions()
        {
            try
            {
                Options.ReadXml("config.txt"); //Read the options
                //Try to set up the MySQL-database linker
                MySQLConn = new MySQLClass(Options.Tables["ConnectionString"].Select("ID = 'MySQL'")[0]["ConnString"].ToString());
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Could not connect to the db or load the config");
            }
        }

        /// <summary>
        /// Starts the threads for the socket listeners
        /// This will use the information stored in the config and read it at program-startup to decide on which ports the GUI and WSN servers have to listen.
        /// </summary>
        private void StartWsnEngine()
        {
            Console.WriteLine("Setting up SocketServers...");
            BackGroundWorkers = Array.CreateInstance(typeof(BackgroundWorker), Options.Tables["SocketServer"].Rows.Count);

            BackgroundWorker SocketServerListenerWSN = new BackgroundWorker();
            SocketServerListenerWSN.DoWork += new DoWorkEventHandler(SocketServerListenerWSN_DoWork);
            Console.WriteLine("\tSocketServer for WSN set up");

            BackgroundWorker SocketServerListenerGUI = new BackgroundWorker();
            SocketServerListenerGUI.DoWork += new DoWorkEventHandler(SocketServerListenerGUI_DoWork);
            Console.WriteLine("\tSocketServer for GUI set up");

            Console.WriteLine("Starting SocketServers...");

            Console.WriteLine("\tStarting up SocketServer for WSN...");
            SocketServerListenerWSN.RunWorkerAsync(int.Parse(Options.Tables["SocketServer"].Select("[Use] = 'WSN'")[0]["Port"].ToString()));
            Console.WriteLine("\tSocketServer for WSN started");

            Console.WriteLine("\tStarting up SocketServer for GUI...");
            SocketServerListenerGUI.RunWorkerAsync(int.Parse(Options.Tables["SocketServer"].Select("[Use] = 'GUI'")[0]["Port"].ToString()));
            Console.WriteLine("\tSocketServer for GUI started");
        }

        /// <summary>
        /// This function corresponds to the socketserver for the GUI-side/port.
        /// </summary>
        /// <param name="sender">The 'thing' doing the actually calling of this function, the backgroundworker.</param>
        /// <param name="e">Argument that was passed to this function when we called the function.</param>
        private void SocketServerListenerWSN_DoWork(object sender, DoWorkEventArgs e)
        {
            SocketServer SServer = new SocketServer(int.Parse(e.Argument.ToString()), 100); //Set up a SocketServer on the given port.
            Console.WriteLine("\t\tListening on port {0} for WSN", e.Argument.ToString());
            SServer.Listen(HandleRequestSocket);    //Start listening for incoming messages
        }

        /// <summary>
        /// This function corresponds to the socketserver for the GUI-side/port.
        /// </summary>
        /// <param name="sender">The 'thing' doing the actually calling of this function, the backgroundworker.</param>
        /// <param name="e">Argument that was passed to this function when we called the function.</param>
        private void SocketServerListenerGUI_DoWork(object sender, DoWorkEventArgs e)
        {
            SocketServer SServer = new SocketServer(int.Parse(e.Argument.ToString()), 100); //Set up a SocketServer on the given port.
            Console.WriteLine("\t\tListening on port {0} for GUI", e.Argument.ToString());
            SServer.Listen(HandleRequestSocket);    //Start listening for incoming messages
        }

        /// <summary>
        /// Function that allows to easily create a reply-message with the given int as content.
        /// </summary>
        /// <param name="data">The that has to put into the reply</param>
        /// <returns>The dataset with the correct sturcture for the reply</returns>
        private DataSet CreateReplyInt(int data)
        {
            //This function will create a dataset according to the reply (int)-schema.
            DataSet Set = new DataSet("Replies");
            Set.Tables.Add("Reply");
            Set.Tables[0].Columns.Add("INT");

            DataRow newRow = Set.Tables[0].NewRow();
            newRow[0] = data;
            Set.Tables[0].Rows.Add(newRow);
            return Set;
        }

        /// <summary>
        /// Function that can be used to convert from a UnixTimeStamp (=seconds since 1 jan 1970) to a string in the format "yyyy-MM-dd HH:mm:ss".
        /// </summary>
        /// <param name="UnixTimestamp">UnixTimeStamp (integer, seconds) to convert to local time.</param>
        /// <returns>A string with the UnixTimeStamp in the format "yyyy-MM-dd HH:mm:ss".</returns>
        private static string ConvertUnixToLocalTimeStamp(int UnixTimestamp)
        {
            // First make a System.DateTime equivalent to the UNIX Epoch.
            System.DateTime dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            // Add the number of seconds in UNIX timestamp to be converted.
            dateTime = dateTime.AddSeconds(UnixTimestamp);
            dateTime = dateTime.ToLocalTime();

            // The dateTime now contains the right date/time so to format the string,
            // use the standard formatting methods of the DateTime object.
            //dateTime.ToString(
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// Sub-function that takes a dataset which holds (a number of) sensormeasurements and saves these to the database.
        /// Note: the MAC-address (SunSpot) or the Unique ID (TelosB) has to be in the database or the measurement will be ignored and not saved tot he database.
        /// </summary>
        /// <param name="Set">DataSet with the table that contains the sensormeasurement.</param>
        /// <returns>The last UID of the inserted row (which means an int >= 1) anything else means the last insert failed.</returns>
        private DataSet AddSensorMeasurements(DataSet Set)
        {
            DataSet returnSet = new DataSet();
            DataSet tempSet = new DataSet();

            double temp;
            int tempint;
            string cmd = "";

            foreach (DataRow row in Set.Tables[0].Rows) //Run through every sensor in the xml-message
            {
                #region check if node exist, else add it
                string CheckIfNodeInDB = "call getNodeID('" + row["ID"].ToString() + "');";

                tempSet = MySQLConn.Query(CheckIfNodeInDB);

                if (tempSet.Tables[0].Rows.Count < 1)  //If the sensor is in the database, proceed with inserting the new measurements into the database.
                {
                    string addTelosb = "call addSensorToDBTelosb('" + row["ID"] + "'," + "2);";

                    returnSet = MySQLConn.Query(addTelosb);

                }
                #endregion


                if (Set.DataSetName == "SensorMeasurements")
                    cmd = ParseSensor(row);
                else if (Set.DataSetName == "LocationMessage")
                {
                    if (Convert.ToInt16(row["VANr"]) == 1)
                    {
                        ParseAnchor(row);
                        return null;
                    }
                    else
                        cmd = ParseBlind(row);
                }
                else if (Set.DataSetName == "StatusMessage")
                    cmd = ParseStatus(row);


                returnSet = MySQLConn.Query(cmd);
            }
            return returnSet;
        }

        private string ParseBlind(DataRow row)
        {
            string cmd;
            int tempint;
           
                int TimeSecs;
                if (int.TryParse(row["Time"].ToString(), out TimeSecs))
                    row["Time"] = ConvertUnixToLocalTimeStamp(TimeSecs);
                        //SunSpot sends the timestamp as unix-timestamp, convert it to normal timestamp.

                currentID = row["ID"].ToString();
                Positioning.Point pos = new Positioning.Point(0, 0);
                Node CurrentNode;

                    if (!BlindNodes.Exists(ExistsNode))
                    {
                        BlindNodes.Add(new Node(row["ID"].ToString(), MySQLConn, row["ANode"].ToString(),
                                                Convert.ToDouble(row["RSSI"]), Convert.ToInt32(row["VANs"])));
                        Console.WriteLine("Added new BN to be positioned\n\n\n");
                        CurrentNode = BlindNodes.Find(ExistsNode);
                    }
                    else
                    {
                        CurrentNode = BlindNodes.Find(ExistsNode);
                        CurrentNode.AddAnchor(row["ANode"].ToString(), Convert.ToDouble(row["RSSI"].ToString()), Convert.ToInt32(row["VANs"]));
                    }

                    //TODO: switch on the bulletlist or whatever you use to select the algorithm
                    Node.FilterMethod myFilter = new Node.FilterMethod(RangeBasedPositioning.MedianFilter);;
                    Node.RangingMethod myRanging;
        
                    if (UseCalibration)
                        myRanging = new Node.RangingMethod(RangeBasedPositioning.Ranging);
                    else
                        myRanging = new Node.RangingMethod(RangeBasedPositioning.DefaultRanging);
                    

                    switch (SelectedFilter)
                    {
                        case "Median":
                            myFilter = new Node.FilterMethod(RangeBasedPositioning.MedianFilter);
                            break;
                        case "Average":
                            myFilter = new Node.FilterMethod(RangeBasedPositioning.AverageFilter);
                            break;
                    }

                    switch (SelectedAlgorithm)
                    {
                        case "CentroidLocalization":
                            pos = CentroidLocalization.CalculatePosition(CurrentNode);
                            break;
                        //case "MinMax":
                        //    pos = MinMax.CalculatePosition(CurrentNode, myFilter, myRanging, UseMultihop);
                        //    break;
                        //case "Trilateration":
                        //    pos = ClusterTrilateration.CalculatePosition(CurrentNode, myFilter);
                    }

                    //Create the command that we send to the database to insert the new row.
                    cmd = "call addLocalizationData(" +
                  ((int.TryParse(row["RSSI"].ToString(), out tempint)) ? row["RSSI"] : "null") + ",";

                          if (pos != null)
                              cmd += pos.x.ToString() + ", " + pos.y.ToString() + ", ";
                          else
                              cmd += "null, null, ";


                          cmd += ((int.TryParse(row["Z"].ToString(), out tempint)) ? row["Z"] : "null") + "," +
                          row["ID"] + ",'" +
                          row["Time"] + "'," +
                          ((int.TryParse(row["ANode"].ToString(), out tempint)) ? row["ANode"] : "null") + ",'" +
                          "Centroid Localization" + "');";

                    //add the position to the position table
                     string AddPosition = "call addPosition(" + row["ID"].ToString() + ", '" 
                         + row["Time"].ToString() + "', " + "0, ";

                     if (pos != null)
                         AddPosition += pos.x.ToString() + ", " + pos.y.ToString() + ")";
                     else
                         AddPosition += "null, null, ";

                    MySQLConn.Query(AddPosition);

                    #region LocationUpdated

                    if (LocationUpdated != null && pos != null)
                    {
                        EventMessage EventData = new EventMessage();
                        //EventData.EventType = "LocationUpdated";

                        EventData.TagBlink["TagID"] = row["idnode"].ToString();
                        EventData.TagBlink["X"] = pos.x.ToString();
                        EventData.TagBlink["Y"] = pos.y.ToString();

                        LocationUpdated(this, EventData);
                    }

                    #endregion
                
                return cmd;
        }

        private void ParseAnchor(DataRow row)
        {
            Node CurrentNode;
            currentID = row["ID"].ToString();
            {
                if (!AnchorNodes.Exists(ExistsNode))
                {
                    AnchorNodes.Add(new Node(row["ID"].ToString(), MySQLConn, row["ANode"].ToString(),
                                            Convert.ToDouble(row["RSSI"]), Convert.ToInt32(row["VANs"])));
                    CurrentNode = BlindNodes.Find(ExistsNode);
                }
                else
                {
                    CurrentNode = AnchorNodes.Find(ExistsNode);
                    CurrentNode.AddAnchor(row["ANode"].ToString(), Convert.ToDouble(row["RSSI"].ToString()), Convert.ToInt32(row["VANs"]));
                }

                Node.FilterMethod myFilter;

                switch (SelectedFilter)
                {
                    case "Median":
                        myFilter = new Node.FilterMethod(RangeBasedPositioning.MedianFilter);
                        break;
                    case "Average":
                        myFilter = new Node.FilterMethod(RangeBasedPositioning.AverageFilter);
                        break;
                    default:
                        myFilter = new Node.FilterMethod(RangeBasedPositioning.MedianFilter);
                        break;
                }
            }
        }

        private string ParseSensor(DataRow row)
        {
            string cmd;
            double temp;
            int tempint;
            {
                int TimeSecs;
                if (int.TryParse(row["Time"].ToString(), out TimeSecs))
                    row["Time"] = ConvertUnixToLocalTimeStamp(TimeSecs);        //SunSpot sends the timestamp as unix-timestamp, convert it to normal timestamp.

                //Create the command that we send to the database to insert the new row.
                cmd = "call addSensorMeasurements('" +
                      row["ID"] + "','" +
                      row["Time"] + "','" +
                      row["Temp"] + "','" +
                      row["Light"] + "'," +
                      ((double.TryParse(row["Humidity"].ToString(), out temp)) ? row["Humidity"] : "null") + "," +
                      ((int.TryParse(row["Power"].ToString(), out tempint)) ? row["Power"] : "null") + "," +
                      ((double.TryParse(row["TiltX"].ToString(), out temp)) ? row["TiltX"] : "null") + "," +
                      ((double.TryParse(row["TiltY"].ToString(), out temp)) ? row["TiltY"] : "null") + "," +
                      ((double.TryParse(row["TiltZ"].ToString(), out temp)) ? row["TiltZ"] : "null") + "," +
                      ((double.TryParse(row["AccX"].ToString(), out temp)) ? row["AccX"] : "null") + "," +
                      ((double.TryParse(row["AccY"].ToString(), out temp)) ? row["AccY"] : "null") + "," +
                      ((double.TryParse(row["AccZ"].ToString(), out temp)) ? row["AccZ"] : "null") + ",'" +
                      ((int.TryParse(row["Button1"].ToString(), out tempint)) ? row["Button1"] : "0") + "'," +
                      ((int.TryParse(row["Button2"].ToString(), out tempint)) ? row["Button2"] : "null") + ",'" +
                      ((double.TryParse(row["LED1"].ToString(), out temp)) ? row["LED1"] : "null") + "','" +
                      ((double.TryParse(row["LED2"].ToString(), out temp)) ? row["LED2"] : "null") + "','" +
                      ((double.TryParse(row["LED3"].ToString(), out temp)) ? row["LED3"] : "null") + "','" +
                      ((double.TryParse(row["LED4"].ToString(), out temp)) ? row["LED4"] : "null") + "','" +
                      ((double.TryParse(row["LED5"].ToString(), out temp)) ? row["LED5"] : "null") + "','" +
                      ((double.TryParse(row["LED6"].ToString(), out temp)) ? row["LED6"] : "null") + "','" +
                      ((double.TryParse(row["LED7"].ToString(), out temp)) ? row["LED7"] : "null") + "','" +
                      ((double.TryParse(row["LED8"].ToString(), out temp)) ? row["LED8"] : "null") + "'," +
                      ((int.TryParse(row["Polling"].ToString(), out tempint)) ? row["Polling"] : "null") + ");";

                #region SensorDataEvents
                //RTLSBlinkTime is generated in the TryQuery to reduce the code size
                if (TemperatureChanged != null)
                {
                    EventMessage EventData = new EventMessage();
                    //EventData.EventType = "TemperatureChanged";

                    EventData.TagBlink["TagID"] = row["idnode"].ToString();
                    EventData.TagBlink["Temperature"] = row["Temp"].ToString();

                    TemperatureChanged(this, EventData);
                }
                if (LightChanged != null)
                {
                    EventMessage EventData = new EventMessage();
                    //EventData.EventType = "LightChanged";

                    EventData.TagBlink["TagID"] = row["idnode"].ToString();
                    EventData.TagBlink["Light"] = row["Light"].ToString();

                    LightChanged(this, EventData);
                }
                if (HumidityChanged != null)
                {
                    EventMessage EventData = new EventMessage();
                    //EventData.EventType = "HumidityChanged";

                    EventData.TagBlink["TagID"] = row["idnode"].ToString();
                    EventData.TagBlink["Humidity"] = row["Humidity"].ToString();

                    HumidityChanged(this, EventData);
                }
                if (ButtonPressed!= null)
                {
                    EventMessage EventData = new EventMessage();
                    //EventData.EventType = "ButtonPressed";

                    EventData.TagBlink["TagID"] = row["idnode"].ToString();
                    EventData.TagBlink["Button"] = row["temperature"].ToString();

                    ButtonPressed(this, EventData);
                }

                #endregion

            }
            return cmd;
        }

        private string ParseStatus(DataRow row)
        {
            string cmd;
            int TimeSecs, tempint;
            if (int.TryParse(row["Time"].ToString(), out TimeSecs))
                row["Time"] = ConvertUnixToLocalTimeStamp(TimeSecs);        //SunSpot sends the timestamp as unix-timestamp, convert it to normal timestamp.

            //Create the command that we send to the database to insert the new row.
            cmd = "call addStatus(" +
            row["ID"] + ",'" +
            row["Time"] + "'," +
            row["Active"] + "," +
            row["AN"] + "," +
            ((int.TryParse(row["X"].ToString(), out tempint)) ? row["X"] : "null") + "," +
            ((int.TryParse(row["Y"].ToString(), out tempint)) ? row["Y"] : "null") + "," +
            ((int.TryParse(row["SampleRate"].ToString(), out tempint)) ? row["SampleRate"] : "null") + "," +
            ((int.TryParse(row["LocRate"].ToString(), out tempint)) ? row["LocRate"] : "null") + "," +
            ((int.TryParse(row["Leds"].ToString(), out tempint)) ? row["Leds"] : "null") + "," +
            ((int.TryParse(row["Power"].ToString(), out tempint)) ? row["Power"] : "null") + "," +
            ((int.TryParse(row["Frequency"].ToString(), out tempint)) ? row["Frequency"] : "null") + "," +
            ((int.TryParse(row["Conn"].ToString(), out tempint)) ? row["Conn"] : "null") + ");";

             string AddPosition = "call addPosition(" + row["ID"].ToString() + ", '"
                 + row["Time"].ToString() + "', " + "1, " + row["X"].ToString() + ", " + row["Y"].ToString() + ")";

            MySQLConn.Query(AddPosition);

            return cmd;
        }

        /// <summary>
        /// Checks if the node is in the list
        /// </summary>
        /// <param name="alg"></param>
        /// <returns></returns>
        private bool ExistsNode(Node BlindNode)
        {
            if (BlindNode.WsnIdProperty == currentID)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Sub-function that is called when we detect that we have a request-xml-message.
        /// </summary>
        /// <param name="Set">The incoming message as a dataset (= tables)</param>
        /// <returns>The result of the request.</returns>
        private DataSet RequestsProcess(DataSet Set)
        {
            DataSet ReqSet = new DataSet();

            string ReqName = Set.Tables["Request"].Rows[0]["RequestName"].ToString().ToLower(); //Get the actual name of the request

            #region request
            //Create the message that we will send to the database.
            string cmd = "call " + ReqName + "(";
            if (Set.Tables.Count <= 1)
            {
                if (Set.Tables[0].Columns.Count >= 2)
                    cmd += "'" + Set.Tables["Request"].Rows[0]["arg"] + "'";
            }
            else
            {
                try
                {
                    foreach (DataRow rows in Set.Tables["arg"].Rows)
                    {
                        cmd += "'" + rows["arg_Text"] + "',";
                    }
                    cmd = cmd.Substring(0, cmd.LastIndexOf(","));
                }
                catch (Exception e_cmd)
                {
                    Logger.LogException(e_cmd);
                }
            }
            cmd += ");";


            if (ReqName == "gethistorytime")    //in case we have gethistorytime, we should check if we have an endtime-argument in the request.
            {   //check if the timestamp (end) is an actual value or just null
                try
                {
                    int index = cmd.LastIndexOf("''");
                    if (index >= 20)
                    {
                        cmd = cmd.Remove(cmd.LastIndexOf("''"));
                        cmd += "current_timstamp());";
                    }
                }
                catch (Exception e)
                {
                    Logger.LogException(e);
                }
            }


            //try
            {
                try
                {
                    ReqSet = MySQLConn.Query(cmd);
                }
                catch (Exception e_mysql)
                {
                    Logger.LogException(e_mysql);
                }
            #endregion

            #region reply
                /********************************************************************************
                 *                                                                              *
                 *                   PREPARE THE REPLY THAT WILL BE SEND BACK TO THE GUI        *
                 *                   if you want to add a new request, you have to add          *
                 *                   it here into the if-else if structure.                     *
                 *                                                                              *
                 *******************************************************************************/

                //Depending on the request we have to send the xml back in a specific format
                //The column and table/dataset-names we get back from the database are not always as they should be,
                //so sometimes they have to be renamed.
                if (ReqName == "getnodeinfo")
                {
                    //try
                    {
                        //Sensormeasurements as reply;
                        //Time can be a problem, so to be save:
                        ReqSet.DataSetName = "SensorMeasurements";
                        ReqSet.Tables[0].TableName = "Sensor";
                        return ReqSet;
                    }
                    //catch (IOException e_io)
                    //{
                    //    return CreateReplyInt(0);
                    //}
                }
                else if (ReqName == "getnodetype")
                {
                    //try
                    {
                        //SensorTypes as reply;
                        //DataSet has to be recreated to have the correct syntax
                        DataSet temp = new DataSet("SensorTypes");
                        temp.Tables.Add("SensorType");
                        temp.Tables[0].Columns.Add("typeid");
                        temp.Tables[0].Columns.Add("typename");
                        DataRow tempRow = temp.Tables[0].NewRow();
                        tempRow["typename"] = ReqSet.Tables[0].Rows[0][0];
                        temp.Tables[0].Rows.Add(tempRow);
                        return temp;
                    }
                    //catch (IOException)
                    //{
                    //    return CreateReplyInt(0);
                    //}
                }
                else if (ReqName == "getsensortypesfromdb")
                {
                    //SensorTypes as reply
                    ReqSet.DataSetName = "SensorTypes";
                    ReqSet.Tables[0].TableName = "SensorType";
                    ReqSet.Tables[0].Columns["idSensortype"].ColumnName = "typeid";
                    ReqSet.Tables[0].Columns["Typename"].ColumnName = "typename";
                    return ReqSet;
                }
                else if ((ReqName == "adduser") || (ReqName == "changepassword")
                    || (ReqName == "checklogin") || (ReqName == "checkusername")
                    || (ReqName == "checksensorid") || (ReqName == "setpositionsensor")
                    || (ReqName == "addmaptodb") || (ReqName == "addsensortodb")
                    || (ReqName == "checkmap") || (ReqName == "checksensorid2"))
                {
                    //Reply (int) as reply
                    ReqSet.DataSetName = "Replies";
                    ReqSet.Tables[0].TableName = "Reply";
                    ReqSet.Tables[0].Columns[0].ColumnName = "INT";
                    if (ReqSet.Tables[0].Rows.Count == 0)
                    {
                        DataRow newRow = ReqSet.Tables[0].NewRow();
                        newRow[0] = 0;
                        ReqSet.Tables[0].Rows.Add(newRow);
                    }
                    return ReqSet;
                }
                else if ((ReqName == "gethistorytime") || (ReqName == "gethistorylast") || (ReqName == "getLochistorylast"))
                {
                    ReqSet.DataSetName = "NodeHistory";
                    ReqSet.Tables[0].TableName = "Measurement";
                    return ReqSet;
                }
                else if ((ReqName == "getallsensors") || (ReqName == "getalltelosb") || (ReqName == "getsensors") || (ReqName == "gettimeoutsensors"))
                {
                    //MapSensors as reply
                    ReqSet.DataSetName = "AllSensors";
                    ReqSet.Tables[0].TableName = "Sensor";
                    return ReqSet;
                }
                else if (ReqName == "getnodelocinfo")
                {
                    ReqSet.DataSetName = "LocMeasurements";
                    ReqSet.Tables[0].TableName = "Position";
                    return ReqSet;
                }
                else if (ReqName == "getstatus")
                {
                    ReqSet.DataSetName = "StatusData";
                    ReqSet.Tables[0].TableName = "Status";
                    return ReqSet;
                }
                else if (ReqName == "getnodeactions")
                {
                    //NodeActions as reply
                    ReqSet.DataSetName = "NodeActions";
                    ReqSet.Tables[0].TableName = "SensorNode";
                    ReqSet.Tables[0].Columns[0].ColumnName = "AvailAction";
                    return ReqSet;
                }
                else if (ReqName == "getnodeid")
                {
                    ReqSet.DataSetName = "NodeIDs";
                    ReqSet.Tables[0].TableName = "NodeID";
                    return ReqSet;
                }
                else if (ReqName == "getwsnid")
                {
                    ReqSet.DataSetName = "WSNIDs";
                    ReqSet.Tables[0].TableName = "WSNID";
                    return ReqSet;
                }
            }
            //catch (Exception e)
            //{
            //    SocketServer.LogException(e, "LogServer.txt");

            //    //Create an error-xml-msg
            //    ReqSet = CreateReplyInt(0);
            //}

            /********************************************************************************
             *                                                                              *
             *        END OF THE DIFFERENT REQUESTS AND THEIR CORRESPONDING REPLIES        *
             *                                                                              *
             *******************************************************************************/
                #endregion

            return ReqSet;
        }

        /// <summary>
        /// Sub-function that is called when we detect an incoming message that ask for an action from a WSN (e.g. get LEDs to light up,...)
        /// </summary>
        /// <param name="WSNActionSet">The XML-message in a dataset</param>
        /// <returns>A reply of either 0 (failed) or 1 (succeeded) in xml (in the dataset)</returns>
        private DataSet WSNActionReqProcess(DataSet WSNActionSet)
        {
            //DataSet to store the result of the query (temporarily) and the final reply to the GUI
            DataSet WSNActionWSNSet = new DataSet();
            string cmd = "call getWSNid('" + WSNActionSet.Tables["RequestAction"].Rows[0]["NodeID"] + "');";
            SocketClient SendActionReq;
            int temp;


                WSNActionWSNSet = MySQLConn.Query(cmd);

                WSNActionSet.Tables["RequestAction"].Rows[0]["NodeID"] = WSNActionWSNSet.Tables[0].Rows[0][0];

                //check if telosb
                if ((int.TryParse(WSNActionSet.Tables["RequestAction"].Rows[0]["NodeID"].ToString(), out temp)) && (WSNActionSet.Tables["RequestAction"].Rows[0]["NodeID"].ToString().Length < 10))
                {   //Parsing the NodeID to an int works (and the value in string-format is smaller than 10 characters); so we have a TelosB

                    //fetch these from the config
                    SendActionReq = new SocketClient(
                        int.Parse(Options.Tables["SocketClient"].Select("Use = 'TelosB'")[0]["Port"].ToString()),
                        Options.Tables["SocketClient"].Select("Use = 'TelosB'")[0]["HostName"].ToString());

                    //Because TelosB expects a simple 1 or 0, but GUI might send a hexadecimal string
                    //Led on = 0 = white light = FFFFFF
                    foreach (DataColumn col in WSNActionSet.Tables[0].Columns)
                    {
                        if (col.ColumnName.IndexOf("LED", StringComparison.CurrentCultureIgnoreCase) != -1)
                        {
                            try
                            {
                                if (WSNActionSet.Tables[0].Rows[0][col].ToString().Length >= 2)
                                {
                                    int tempint;
                                    if (int.TryParse(WSNActionSet.Tables[0].Rows[0][col].ToString(), out tempint))
                                    {
                                        if (tempint == 0)
                                            WSNActionSet.Tables[0].Rows[0][col] = 0;
                                        else
                                            WSNActionSet.Tables[0].Rows[0][col] = 1;
                                    }
                                    else
                                        WSNActionSet.Tables[0].Rows[0][col] = 1;
                                }
                            }
                            catch (Exception)
                            { }
                        }
                    }
                }

                #region Sun SPOT!
                else
                {   //Parsing the NodeID to an int didn't work (or the value in string-format is longer than 10 characters); we have a SunSpot
                    SendActionReq = new SocketClient(
                        int.Parse(Options.Tables["SocketClient"].Select("Use = 'SunSpot'")[0]["Port"].ToString()),
                        Options.Tables["SocketClient"].Select("Use = 'SunSpot'")[0]["HostName"].ToString());

                    //Because SunSpot expects a hexadecimal 6-character string, but the GUI sends '1', we have to change this.
                    //Led on = white light = FFFFFF
                    foreach (DataColumn col in WSNActionSet.Tables[0].Columns)
                    {
                        if (col.ColumnName.IndexOf("LED", StringComparison.CurrentCultureIgnoreCase) != -1)
                        {
                            try
                            {
                                if (WSNActionSet.Tables[0].Rows[0][col].ToString().Length == 1)
                                {
                                    if (int.Parse(WSNActionSet.Tables[0].Rows[0][col].ToString()) == 1)
                                        WSNActionSet.Tables[0].Rows[0][col] = "FFFFFF";
                                    else if (int.Parse(WSNActionSet.Tables[0].Rows[0][col].ToString()) == 0)
                                        WSNActionSet.Tables[0].Rows[0][col] = "000000";
                                }
                            }
                            catch (Exception)
                            { }
                        }
                    }
                }
                #endregion



                MemoryStream OutMemStream = new MemoryStream();
                WSNActionSet.WriteXml(OutMemStream);
                OutMemStream.Position = 0;
                StreamReader OutMemStreamReader = new StreamReader(OutMemStream);

            try
            {
                string replyWSN = SendActionReq.Connect(OutMemStreamReader.ReadToEnd().Replace("\r\n", ""), true, 10000);
                if (replyWSN == null)
                    throw new ArgumentNullException("The WSN did not reply in time");

                MemoryStream ReplyStream = new MemoryStream();
                StreamWriter ReplyWriter = new StreamWriter(ReplyStream);
                ReplyWriter.Write(replyWSN);
                ReplyWriter.Flush();
                ReplyStream.Position = 0;

                WSNActionWSNSet = new DataSet();
                WSNActionWSNSet.ReadXml(ReplyStream);

                foreach (DataRow row in WSNActionWSNSet.Tables[0].Rows)
                {
                    string AddPosition = "call addPosition(" + row["ID"].ToString() + ", '"
                        + row["Time"].ToString() + "', " + "1, " + row["X"].ToString() + ", " + row["Y"].ToString() + ")";

                    MySQLConn.Query(AddPosition);
                }
            }
            catch (ArgumentNullException nullex)
            {
                Logger.LogException(nullex);
                WSNActionWSNSet = CreateReplyInt(0);
                return WSNActionWSNSet;
            }
            catch (XmlException xmlex)
            {
                Logger.LogException(xmlex);
            }
            catch (SocketException sockex)
            {
                Logger.LogException(sockex);
            }

            return WSNActionWSNSet;
        }

        /// <summary>
        /// The function that is called to process incoming connections.
        /// It will decide which type of message it is and act accordingly.
        /// </summary>
        /// <param name="state">The socket with the incoming message.</param>
        public void HandleRequestSocket(object state)
        {
            using (Socket client = (Socket)state)   //Get the socket-instance that we have to process
            using (NetworkStream stream = new NetworkStream(client)) //Create the network-stream so we can communicate
            using (StreamReader reader = new StreamReader(stream))  //Create a reader for the stream
            using (StreamWriter writer = new StreamWriter(stream))  //Create a writer for the stream
            {
                DataSet IncMsg = new DataSet(); //DataSet to store incoming msg
                DataSet OutMsg = new DataSet(); //DataSet to store outgoing msg
                DataSet TempSet = new DataSet();

                //Get ready to read the incoming xml-msg
                string incxml = "";
                try
                {
                    incxml = reader.ReadLine(); //Read the incoming data

                    IncMsg = StringToDataSet(incxml);

                    #region incoming
                    do
                    {

                        //Check which type of incoming msg we have (if it is not in the list; we discard it)
                        if (IncMsg.DataSetName == "SensorMeasurements" || IncMsg.DataSetName == "LocationMessage" || IncMsg.DataSetName == "StatusMessage")
                        {   //SensorMeasurement
                            Console.WriteLine("Received WSN message: " + IncMsg.DataSetName);
                            AddSensorMeasurements(IncMsg);
                            break;
                        }
                        else if (IncMsg.DataSetName == "Requests")
                        {   //Requests
                            Console.WriteLine("Received DB request");
                            OutMsg = RequestsProcess(IncMsg);                            
                            break;
                        }
                        else if (IncMsg.DataSetName == "WSNReq")
                        {   
                            Console.WriteLine("Received WSN REQUEST");
                            OutMsg = WSNActionReqProcess(IncMsg);
                            Console.WriteLine("Received WSN REPLY");
                            break;
                        }
                    }
                    while (false);
                    #endregion

                    #region outgoing
                    //Answer
                    if (IncMsg.DataSetName == "SensorMeasurements" || IncMsg.DataSetName == "LocationMessage" || IncMsg.DataSetName == "StatusMessage")
                    {
                        //No reply needed
                    }
                    else
                    {   //Reply needed
                        if (OutMsg.Tables.Count >= 1)
                        {   //Only send a reply if we actually got a correct Msg to send
                            //(in other words, when the query actually succeeded)
                            if ((OutMsg.Tables[0].Rows.Count <= 0))
                            {   //We got a result back, but just nothing in it...
                                OutMsg = CreateReplyInt(0);
                            }
                            MemoryStream OutMemStream = new MemoryStream();

                            if (OutMsg.DataSetName == "Replies")
                            {
                                int intreply;
                                if (int.TryParse(OutMsg.Tables["Reply"].Rows[0]["INT"].ToString(), out intreply))
                                {
                                    if (intreply == -404)
                                        writer.WriteLine("404");
                                    else
                                    {
                                        OutMsg.WriteXml(OutMemStream);
                                        OutMemStream.Position = 0;
                                        StreamReader OutMemStreamReader = new StreamReader(OutMemStream);
                                        writer.WriteLine(OutMemStreamReader.ReadToEnd());
                                        writer.Flush();
                                    }
                                }
                            }
                            else if (OutMsg.DataSetName == "DiscoveryReply")
                            {
                                //in: list with all the WSN id's
                                //proces: sets all other nodes to inactive
                                //out: list with all the WSN id's and 
                                OutMsg = Discovery(OutMsg);

                                OutMsg.WriteXml(OutMemStream);
                                OutMemStream.Position = 0;
                                StreamReader OutMemStreamReader = new StreamReader(OutMemStream);
                                writer.WriteLine(OutMemStreamReader.ReadToEnd());
                                writer.Flush();
                            }
                            else if (OutMsg.DataSetName == "WSNReply")
                            {
                                //needed so that AddSensorMeasurements parses the message correct
                                OutMsg.DataSetName = "StatusMessage";
                                //store the message in the db
                                TempSet = AddSensorMeasurements(OutMsg);

                                OutMsg.WriteXml(OutMemStream);
                                OutMemStream.Position = 0;
                                StreamReader OutMemStreamReader = new StreamReader(OutMemStream);
                                writer.WriteLine(OutMemStreamReader.ReadToEnd());
                                writer.Flush();
                            }
                            else
                            {
                                OutMsg.WriteXml(OutMemStream);
                                OutMemStream.Position = 0;
                                StreamReader OutMemStreamReader = new StreamReader(OutMemStream);
                                writer.WriteLine(OutMemStreamReader.ReadToEnd());
                                writer.Flush();
                            }
                        }
                        else
                        {   //Query didn't succeed - send back an 'error'
                            DataSet Error = CreateReplyInt(0);
                            MemoryStream OutMemStream = new MemoryStream();
                            Error.WriteXml(OutMemStream);
                            OutMemStream.Position = 0;
                            StreamReader OutMemStreamReader = new StreamReader(OutMemStream);
                            writer.WriteLine(OutMemStreamReader.ReadToEnd());
                            writer.Flush();
                        }
                    }

                }
                    #endregion

                catch (XmlException xmlex)
                {
                    Console.WriteLine(xmlex.Message);
                    Console.WriteLine(xmlex.StackTrace);
                    Console.WriteLine("Received an incorrect XML message\n");
                }
                catch (Exception e)
                {
                    Console.WriteLine("\n" + e.Message);
                    Console.WriteLine(e.StackTrace + "\n");

                    //This is stupid, delete?
                    //Fatal error - send back a message to indicate a problem
                    //DataSet Error = CreateReplyInt(0);
                    //MemoryStream OutMemStream = new MemoryStream();
                    //Error.WriteXml(OutMemStream);
                    //OutMemStream.Position = 0;
                    //StreamReader OutMemStreamReader = new StreamReader(OutMemStream);

                    //try
                    //{
                    //    writer.WriteLine(OutMemStreamReader.ReadToEnd());
                    //    writer.Flush();
                    //}
                    //catch (Exception)
                    //{
                    //    Console.WriteLine("Could not even send back an error message");
                    //}
                    
                }
            }
        }

        private DataSet StringToDataSet(string incxml)
        {
            DataSet IncMsg = new DataSet();
            //Set up a memory-stream to store the xml
            MemoryStream MemStream = new MemoryStream();
            //Write the msg to the memory stream
            StreamWriter SWriter = new StreamWriter(MemStream);
            SWriter.WriteLine(incxml);
            SWriter.Flush();
            MemStream.Position = 0; //Reset the position so we start reading at the start

            IncMsg.ReadXml(MemStream);  //Read the data into the dataset

            return IncMsg;
        }

        /// <summary>
        /// Processes the discovery reply
        /// TODO: add further info
        /// Possibly remove due to not tested OR test this of course..
        /// </summary>
        /// <param name="Set"></param>
        /// <returns></returns>
        private DataSet Discovery(DataSet Set)
        {
            DataSet TempSet = new DataSet();
            DataSet ReturnSet = new DataSet();


            //here we process the discovery reply
            //we set the received node id's to active and the rest to inactive in the DB

            //SetNodesInactive
            string setNodesInactive = "call setAllNodesState(0);";

            try
            {
                TempSet = MySQLConn.Query(setNodesInactive);
            }
            catch (Exception e_mysql)
            {
                Logger.LogException(e_mysql);
            }

            //foreach row.... SetState
            foreach (DataRow row in Set.Tables[0].Rows) //Run through every sensor in the xml-message
            {
                string setNodeActive = "call setNodeState(1,'" + row["Nodeid"] + "');";
                try
                {
                        TempSet = MySQLConn.Query(setNodeActive);
                }
                catch (Exception e_mysql)
                {
                    Logger.LogException(e_mysql);
                }
            }
            //we then send the wsn id and nodeid to the GUI
            //GetActiveSensors
            string getActiveTelosb = "call getActiveTelosb();";

            try
            {
                    ReturnSet = MySQLConn.Query(getActiveTelosb);
            }
            catch (Exception e_mysql)
            {
                Logger.LogException(e_mysql);
            }
            return ReturnSet;
        }
    }
}
