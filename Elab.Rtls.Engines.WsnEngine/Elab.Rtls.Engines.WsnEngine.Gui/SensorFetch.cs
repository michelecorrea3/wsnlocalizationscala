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
    partial class Form1
    {
        #region SensorFetch

        /// <summary>
        /// Event for the discovery protocol
        /// Discovery protocol sends a WSN action Request to the controller, which forwards it to the WSN parser.
        /// The WSN parser disseminates a status request to each node, the nodes that reply are given as the active nodes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void timerSensorFetch_Tick(object sender, EventArgs e)
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
            catch (Exception ex)
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
    }
}