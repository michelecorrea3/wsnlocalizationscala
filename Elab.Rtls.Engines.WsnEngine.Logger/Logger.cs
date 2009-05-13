using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Elab.Rtls.Engines.WsnEngine
{
    public static class Logger
    {
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
}
