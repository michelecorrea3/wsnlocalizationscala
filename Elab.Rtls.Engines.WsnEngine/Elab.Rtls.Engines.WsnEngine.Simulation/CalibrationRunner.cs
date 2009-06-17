using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DatabaseConnection;
using Elab.Rtls.Engines.WsnEngine.Positioning;

namespace LocalizationAlgorithmsRunner
{
    class CalibrationRunner
    {
        /// <summary>
        /// Connectionstring to use for the connection to the database (MySQL!)
        /// </summary>
        private MySQLClass MyDb;

        /// <summary>
        /// BN's WsnId
        /// </summary>
        private string currentID;

        /// <summary>
        /// Contains all the anchor nodes that should be used for calibration
        /// </summary>
        private List<Node> AnchorNodes = new List<Node>();

        /// <summary>
        /// Entry point for the application
        /// </summary>
        internal static void Main()
        {
            CalibrationRunner calibrationRunner = new CalibrationRunner();
        }

        /// <summary>
        /// Constructor
        /// Initializes the log file
        /// Contains the loop which calls all the required functions for simulation
        /// </summary>
        CalibrationRunner()
        {
            MyDb = new MySQLClass("DRIVER={MySQL ODBC 3.51 Driver};SERVER=localhost;DATABASE=senseless;UID=root;PASSWORD=admin;OPTION=3;");
            string response;

            do
            {
                Console.WriteLine("Using a new batch of data!");
                DataSet TempSet = FetchData();
                ExecuteCalibrationBlindNode(TempSet);
                Console.Write("Process another batch of data? (Y/N) ");
                response = Console.ReadLine();
            } while (response == "Yes" || response == "Y");

            Console.WriteLine("Press ENTER to exit");
            Console.ReadLine();
        }

        /// <summary>
        /// Calculates the calibration algorithms
        /// </summary>
        /// <param name="dataSet">The dataset to be processed</param>
        private void ExecuteCalibration(DataSet dataSet)
        {
            double baseLoss = 0, pathLossExponent = 0;
            int counter = 0;

            Console.WriteLine("Executing the two calibration methods");

            Console.WriteLine("Normal Calibration");
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                currentID = row["node"].ToString();
                Node CurrentNode;

                if (currentID != "11")
                {
                    if (!AnchorNodes.Exists(AN => AN.WsnIdProperty == currentID))
                    {
                        AnchorNodes.Add(new Node(row["node"].ToString(), MyDb));
                    }

                    CurrentNode = AnchorNodes.Find(AN => AN.WsnIdProperty == currentID);
                    CurrentNode.UpdateAnchors(row["ANode"].ToString(), Convert.ToDouble(row["RSSI"].ToString()), 1, DateTime.Now);

                    RangeBasedPositioning.CalibratePathloss(AnchorNodes, RangeBasedPositioning.NoFilter);
                    pathLossExponent += RangeBasedPositioning.pathLossExponent;
                    counter++;
                }
            }

            pathLossExponent /= counter;
            Console.WriteLine("baseLoss = " + RangeBasedPositioning.baseLoss.ToString());
            Console.WriteLine("pathLossExponent = " + pathLossExponent.ToString());

            baseLoss = 0;
            pathLossExponent = 0;
            counter = 0;
            AnchorNodes.Clear();

            Console.WriteLine("LS Calibration");
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                currentID = row["node"].ToString();
                Node CurrentNode;

                if (currentID != "11")
                {
                    if (!AnchorNodes.Exists(AN => AN.WsnIdProperty == currentID))
                    {
                        AnchorNodes.Add(new Node(row["node"].ToString(), MyDb));
                    }

                    CurrentNode = AnchorNodes.Find(AN => AN.WsnIdProperty == currentID);
                    CurrentNode.UpdateAnchors(row["ANode"].ToString(), Convert.ToDouble(row["RSSI"].ToString()), 1, DateTime.Now);

                    RangeBasedPositioning.CalibratePathlossLS(AnchorNodes, RangeBasedPositioning.NoFilter);
                    baseLoss += RangeBasedPositioning.baseLoss;
                    pathLossExponent += RangeBasedPositioning.pathLossExponent;
                    counter++;
                }
            }

            baseLoss /= counter;
            pathLossExponent /= counter;
            Console.WriteLine("baseLoss = " + baseLoss.ToString());
            Console.WriteLine("pathLossExponent = " + pathLossExponent.ToString());
        }

        /// <summary>
        /// Calculates the calibration algorithms where the node to be calibrated is the blind node
        /// </summary>
        /// <param name="dataSet">The dataset to be processed</param>
        private void ExecuteCalibrationBlindNode(DataSet dataSet)
        {
            Console.WriteLine("Executing the two calibration methods");

            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                currentID = row["node"].ToString();
                Node CurrentNode;

                if (currentID == "11")
                {
                    if (!AnchorNodes.Exists(AN => AN.WsnIdProperty == currentID))
                    {
                        AnchorNodes.Add(new Node(row["node"].ToString(), MyDb));

                        CurrentNode = AnchorNodes.Find(AN => AN.WsnIdProperty == currentID);

                        Console.WriteLine("Enter the position of the blind node in doubles!");
                        Point position = new Point();
                        Console.Write("X: ");
                        position.x = Convert.ToDouble(Console.ReadLine());
                        Console.Write("Y: ");
                        position.y = Convert.ToDouble(Console.ReadLine());

                        CurrentNode.position = new Point(position.x, position.y);
                    }

                    CurrentNode = AnchorNodes.Find(AN => AN.WsnIdProperty == currentID);

                    CurrentNode.UpdateAnchors(row["ANode"].ToString(), Convert.ToDouble(row["RSSI"].ToString()), 1, DateTime.Now);
                }
            }

            RangeBasedPositioning.CalibratePathloss(AnchorNodes, RangeBasedPositioning.AverageFilter);
            Console.WriteLine("Normal Calibration");
            Console.WriteLine(RangeBasedPositioning.pathLossExponent);
            Console.WriteLine(RangeBasedPositioning.baseLoss);
            RangeBasedPositioning.CalibratePathlossLS(AnchorNodes, RangeBasedPositioning.AverageFilter);
            Console.WriteLine("LS Calibration");
            Console.WriteLine(RangeBasedPositioning.pathLossExponent);
            Console.WriteLine(RangeBasedPositioning.baseLoss);

            AnchorNodes.Clear();
        }

        /// <summary>
        /// Fetches the RSS data from the database
        /// </summary>
        /// <returns>The DataSet</returns>
        private DataSet FetchData()
        {
            string LowerBound, UpperBound, MyQuery;
            DataSet Set;

            Console.WriteLine("Enter the starting value of time: (dd hh:mm:ss)");
            LowerBound = Console.ReadLine();
            Console.WriteLine("Enter the ending value of time: (dd hh:mm:ss)");
            UpperBound = Console.ReadLine();

            MyQuery = "SELECT * FROM localization l where time between '2009-06-" + LowerBound + "' and '2009-06-" + UpperBound + "';";
            Set = MyDb.Query(MyQuery);

            //translate NodeIDs into WsnIDs
            foreach (DataRow row in Set.Tables[0].Rows)
            {
                string CheckIfNodeInDB = "call getWSNID('" + row["node"].ToString() + "');";
                DataSet tempSet = MyDb.Query(CheckIfNodeInDB);
                row["node"] = tempSet.Tables[0].Rows[0]["sensor"];

                CheckIfNodeInDB = "call getWSNID('" + row["anode"].ToString() + "');";
                tempSet = MyDb.Query(CheckIfNodeInDB);
                row["anode"] = tempSet.Tables[0].Rows[0]["sensor"];
            }

            Console.WriteLine("Retreived the data from the database");

            return Set;
        }
    }
}
