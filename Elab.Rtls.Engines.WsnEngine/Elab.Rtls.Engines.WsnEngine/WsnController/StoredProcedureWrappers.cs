namespace Elab.Rtls.Engines.WsnEngine
{
    using System;
    using System.Data;
    using System.IO;
    using Elab.Rtls.Engines.WsnEngine.Positioning;

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

        /// <summary>
        /// Stored procedure wrapper
        /// Adds a new position for a single node to the database
        /// </summary>
        /// <param name="wsnID">The WSNID of the node</param>
        /// <param name="position">The position in a point type</param>
        /// <param name="anchor">Anchor node or Blind Node</param>
        private void AddPosition(DataRow row, Point pos, int anchor)
        {
            string AddPosition = "call addPosition(" + row["ID"].ToString() + ", '"
                                 + row["Time"].ToString() + "', " + anchor + ", ";

            if (pos != null)
                AddPosition += pos.x.ToString().Replace(',', '.') + ", " + pos.y.ToString().Replace(',', '.') + ")";
            else
                AddPosition += "null, null )";

            MySQLConn.Query(AddPosition);
        }

        /// <summary>
        /// Stored procedure wrapper
        /// Adds a new position for a single node to the database
        /// </summary>
        /// <param name="row">The data to be added containing the position</param>
        private void AddPosition(DataRow row)
        {
            string AddPosition = "call addPosition(" + row["ID"].ToString() + ", '"
                                 + row["Time"].ToString() + "', " + row["AN"].ToString() + ", " + row["X"].ToString() + ", " + row["Y"].ToString() + ")";

            MySQLConn.Query(AddPosition);
        }

        /// <summary>
        /// Converts an XML-string to a DataSet
        /// </summary>
        /// <param name="incxml">XML-string to be converted</param>
        /// <returns>DataSet containing the data from the XML</returns>
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