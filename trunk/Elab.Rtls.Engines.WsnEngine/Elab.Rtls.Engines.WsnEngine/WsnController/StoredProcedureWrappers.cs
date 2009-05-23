namespace Elab.Rtls.Engines.WsnEngine
{
    using System;
    using System.Globalization;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.IO;
    using System.Net.Sockets;
    using System.Xml;

    using DatabaseConnection;

    using Elab.Rtls.Engines.WsnEngine.Positioning;

    using Scala.Core;

    using SocketConnection;

    public partial class Controller
    {
        #region Methods

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

        private void AddPosition(DataRow row, Point pos, int anchor)
        {
            string AddPosition = "call addPosition(" + row["ID"].ToString() + ", '"
                                 + row["Time"].ToString() + "', " + anchor + ", ";

            if (pos != null)
            {                string tempString = pos.x.ToString() + ", " + pos.y.ToString() + ")";                tempString.Replace(',', '.');                AddPosition += tempString;            }                            else
                AddPosition += "null, null )";

            MySQLConn.Query(AddPosition);
        }

        private void AddPosition(DataRow row)
        {
            string AddPosition = "call addPosition(" + row["ID"].ToString() + ", '"
                                 + row["Time"].ToString() + "', " + row["AN"].ToString() + ", " + row["X"].ToString() + ", " + row["Y"].ToString() + ")";

            MySQLConn.Query(AddPosition);
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

        #endregion Methods
    }
}