using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Data;
using System.Xml;

namespace SocketConnection
{
    /// <summary>
    /// Baseclass that can be used for other socket-level classes.
    /// </summary>
    public abstract class SocketClass
    {
        /// <summary>
        /// Attribute to save the port for the socket to.
        /// </summary>
        protected int _Port;

        /// <summary>
        /// Constructor for this class, it saves the given port to the private/protected member.
        /// </summary>
        /// <param name="Port">The port we bind the socket to.</param>
        public SocketClass(int Port)
        {
            _Port = Port;
        }

        /// <summary>
        /// Global function that can be used to log errors (mostly from within a catch statement) to a log-file
        /// </summary>
        /// <param name="e">The Exception that has to be logged</param>
        /// <param name="LogFilePath">The location of the logfile (including filename).</param>
        public static void LogError(Exception e, string LogFilePath)
        {
            StreamWriter logger = new StreamWriter(LogFilePath, true); //Open the file and get ready to append the error-info to the file.
            logger.WriteLine(e); //Write the error-data
            logger.WriteLine(DateTime.Now); //Put a timestamp so we know when the error happened
            logger.WriteLine("----------++++++++++----------"); //Make it clear this is the end of the error.
            logger.Flush();
            logger.Close();
        }
    }

    /// <summary>
    /// Class that can be used to set up a socket-client (sender/activator).
    /// </summary>
    public class SocketClient: SocketClass
    {
        /// <summary>
        /// Hostname or ip of the target computer/system.
        /// </summary>
        private string _RemoteLocation;

        /// <summary>
        /// Constructor for the SocketClient-class.
        /// </summary>
        /// <param name="Port">The port to which we want to open a connection.</param>
        /// <param name="RemoteLocation">The hostname or ip-address of the target of our connection.</param>
        public SocketClient(int Port, string RemoteLocation):base(Port)
        {
            _RemoteLocation = RemoteLocation;
        }

        /// <summary>
        /// This function will open a connection to the target (given in the constructor) and will send the string of xml over the connection.
        /// It can be used to either send and ignore any replies or it can be asked to wait for a reply from the target.
        /// </summary>
        /// <param name="xmlstream">XML-string to send over the connection to the target.</param>
        /// <param name="WaitForResponse">If this is 'true' we will wait and read the reply from the target. If 'false' it will send the info and finish.</param>
        /// <returns>The reply of the target, if nothing is received or WaitForResponse argument was set to false, this will return be 'null'</returns>
        public string Connect(string xmlstream, bool WaitForResponse)
        {
            string ReceivedData = null;
            try
            {
                TcpClient tc = new TcpClient(_RemoteLocation, _Port);// Create a client that will send messages to the given _RemoteLocation (hostname) and _Port.
                tc.SendTimeout = 5000;
                NetworkStream ns = tc.GetStream(); //Get the stream so we can use it to communicate
                StreamWriter sw = new StreamWriter(ns);
                sw.WriteLine(xmlstream); //Write the info to the stream (a.k.a. send the data over the connection
                sw.Flush();
                if (WaitForResponse)
                {
                    StreamReader sr = new StreamReader(ns);
                    ReceivedData = sr.ReadToEnd(); //(Get ready to) read the reply of the server.
                    //ns.Close();
                }
            }
            catch (Exception e)
            {
                //If an error happens; write it to the logfile.
                LogError(e, "LogClient.txt");
            }
            
            return ReceivedData; //Return the reply of the server
        }

        public bool TryConnection()
        {
            TcpClient testclient = new TcpClient();

            try
            {
                testclient.Connect(_RemoteLocation, _Port);
                testclient.SendTimeout = 1000;
                Console.WriteLine("Connection open, host active");
                testclient.Close();
                return true;
            }
            catch (SocketException ex)
            {
                Console.WriteLine("Connection could not be established due to: \n" + ex.Message);
                return false;
            }

        }
    }

    /// <summary>
    /// Class that can be used to set up a socket-server (listener/react).
    /// </summary>
    public class SocketServer: SocketClass
    {
        /// <summary>
        /// Maximum number of connections that are waiting to be processed.
        /// </summary>
        private int _BackLogQueue;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Port">Port on which we want the server to listen</param>
        public SocketServer(int Port):base(Port)
        {
            _BackLogQueue = 3;
        }

        /// <summary>
        /// Constructor (which allows to set the number of pending connections)
        /// </summary>
        /// <param name="Port">Port on which we want the server to listen</param>
        /// <param name="BackLogQueue">Number of pending connections that can be in the queue (this is capped by your OS too)</param>
        public SocketServer(int Port, int BackLogQueue)
            : base(Port)
        {
            _BackLogQueue = BackLogQueue;
        }

        /// <summary>
        /// This function starts up and keeps running indefinatly, constantly listening and putting incoming connections in the 'to process' threadpool. (do not run this in your mainthread, as this is a BLOCKING function, indefinatly)
        /// </summary>
        /// <param name="CallBack">The callback of the function that we want to use to process incoming connections.</param>
        public void Listen(System.Threading.WaitCallback CallBack)
        {
            try
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); //Prepare the info for the socket that we want to listen on
                socket.Bind(new IPEndPoint(IPAddress.Any, _Port)); //Bind the socket to a port and a possible range of IPAdresses (will only react to IP's from this range)
                socket.Listen(_BackLogQueue); //Start the listening

                while (true)    //Continually listen and dispatch incomming messages.
                {
                    Socket client = socket.Accept();    //Accept the connection if we detect a request
                    System.Threading.ThreadPool.QueueUserWorkItem(CallBack, client);   //Dispatch the connection - see HandleRequest function for how we process a connection.
                }

            }
            catch (Exception e)
            {
                //If an error happens; write it to the logfile.
                LogError(e, "LogServer.txt");
            }
        }
    }

}
