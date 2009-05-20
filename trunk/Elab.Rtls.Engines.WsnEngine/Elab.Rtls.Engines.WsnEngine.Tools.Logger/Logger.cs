namespace Elab.Rtls.Engines.WsnEngine
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Odbc;
    using System.IO;
    using System.Linq;
    using System.Text;

    public static class Logger
    {
        #region Methods

        /// <summary>
        /// Global function that can be used to log errors (mostly from within a catch statement) to a log-file
        /// </summary>
        /// <param name="e">The Exception that has to be logged</param>
        /// <param name="LogFilePath">The location of the logfile (including filename).</param>
        public static void LogException(Exception e)
        {
            Console.WriteLine("An exception occurred in: " + e.TargetSite.ToString());
            StreamWriter logger = new StreamWriter("Exceptions.txt", true); //Open the file and get ready to append the error-info to the file.
            logger.WriteLine(e); //Write the error-data
            logger.WriteLine(DateTime.Now); //Put a timestamp so we know when the error happened
            logger.WriteLine("----------++++++++++----------"); //Make it clear this is the end of the error.
            logger.Flush();
            logger.Close();
        }

        public static void LogOdbcException(OdbcException odbcException, string query)
        {
            StreamWriter logger = new StreamWriter("DBFaults.txt", true); //Open the file and get ready to append the error-info to the file.
            logger.WriteLine(odbcException.Message);
            logger.WriteLine(odbcException.Errors[0].Message);
            logger.WriteLine(odbcException.Errors[0].NativeError);
            logger.WriteLine(odbcException.Errors[0].SQLState);
            logger.WriteLine(query);
            logger.WriteLine(DateTime.Now); //Put a timestamp so we know when the error happened
            logger.WriteLine("----------++++++++++----------"); //Make it clear this is the end of the error.
            logger.Flush();
            logger.Close();
        }

        #endregion Methods
    }
}