namespace LocalizationAlgorithmsRunner
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Text;

    using DatabaseConnection;

    using Elab.Rtls.Engines.WsnEngine.Positioning;

    class AlgorithmsCalculator
    {
        #region Fields

        private List<Node> AnchorNodes = new List<Node>();

        /// <summary>
        /// List with all the node that should be positioned
        /// </summary>
        private List<Node> BlindNodes = new List<Node>();
        private MySQLClass MyDb;

        /// <summary>
        /// BN's WsnId
        /// </summary>
        private string currentID;

        #endregion Fields

        #region Constructors

        public AlgorithmsCalculator()
        {
            MyDb = new MySQLClass("DRIVER={MySQL ODBC 3.51 Driver};SERVER=localhost;DATABASE=senseless;UID=root;PASSWORD=admin;OPTION=3;");

            string response;

            do
            {
                UpdateAnhors();
                UpdateBlind();

                Console.WriteLine("Using a new batch of data!");
                DataSet TempSet = FetchData();
                ExecuteAlgorithms(TempSet);

                Console.Write("Process another batch of data? (Y/N) ");
                response = Console.ReadLine();
            } while (response == "Yes" || response == "Y");

            Console.WriteLine("Done! Press ENTER to exit");
            Console.ReadLine();
        }

        private void UpdateBlind()
        {
            Console.WriteLine("Enter the position of the blind node in doubles!");   
            Point position = new Point();
            Console.Write("X: ");
            position.x = Convert.ToDouble(Console.ReadLine());
            Console.Write("Y: ");
            position.y = Convert.ToDouble(Console.ReadLine());

            Node CurrentNode = BlindNodes.Find(BN => BN.WsnIdProperty == currentID);
            CurrentNode.Position.x = position.x;
            CurrentNode.Position.y = position.y;
        }

        #endregion Constructors

        #region Methods

        private void AddPosition(string wsnID, Point position)
        {
            string AddPosition = "call addPosition(" + wsnID + ", '"
                                 + DateTime.Now.ToString("u").Remove(19) + "', " + 1 + ", ";

            if (position != null)
                AddPosition += position.x.ToString().Replace(',', '.') + ", " + position.y.ToString().Replace(',', '.') + ")";
            else
                AddPosition += "null, null )";

            MyDb.Query(AddPosition);
        }

        private void ExecuteAlgorithms(DataSet Set)
        {
            Console.WriteLine("Executing each algorithm on the data");

            //write the data to the stream ...
            StreamWriter logger = new StreamWriter("Log.csv", true);

            foreach (DataRow row in Set.Tables[0].Rows)
            {
                currentID = row["node"].ToString();
                Node CurrentNode;

                //Positioning.Point pos = new Positioning.Point(0, 0);
                Point pos = new Point(0, 0);

                if (currentID == "11")
                {
                    if (!BlindNodes.Exists(BN => BN.WsnIdProperty == currentID))
                    {
                        BlindNodes.Add(new Node(row["node"].ToString(), MyDb));
                        Console.WriteLine("Added new BN to be positioned\n\n\n");
                    }
                        CurrentNode = BlindNodes.Find(BN => BN.WsnIdProperty == currentID);
                        CurrentNode.UpdateAnchors(row["ANode"].ToString(), Convert.ToDouble(row["RSSI"].ToString()), 1, DateTime.Now);
                        ////TODO: check if automatically updated
                        //CurrentNode = BlindNodes.Find(BN => BN.WsnIdProperty == currentID);

                        logger.Write(row["idlocalization"].ToString() + ",");
                        logger.Write(row["time"].ToString() + ",");
                        logger.Write(currentID + ",");
                        logger.Write(CurrentNode.Anchors.Count + ",");

                        //centroid localization
                        logger.Write("CL,");

                        pos = CentroidLocalization.CalculatePosition(CurrentNode);

                        if (pos != null)
                        {
                            logger.Write(pos.x + ",");
                            logger.Write(pos.y + ",");
                            logger.Write(DistanceBetweenTwoPoints(pos, CurrentNode.Position));
                        }
                        else
                        {
                            logger.Write("null,");
                            logger.Write("null");
                        }

                        //min-max
                        logger.Write("Min-Max,");

                        if (pos != null)
                        {
                            pos = MinMaxExtended.CalculatePosition(CurrentNode, RangeBasedPositioning.AverageFilter, RangeBasedPositioning.DefaultRanging, false);
                            logger.Write(pos.x + ",");
                            logger.Write(pos.y + ",");
                        }
                        else
                        {
                            logger.Write("null,");
                            logger.Write("null");
                        }

                        logger.WriteLine("");
                        logger.Flush();
                }
                else
                {
                    if (!AnchorNodes.Exists(AN => AN.WsnIdProperty == currentID))
                    {
                        AnchorNodes.Add(new Node(row["node"].ToString(), MyDb));
                    }

                    CurrentNode = AnchorNodes.Find(AN => AN.WsnIdProperty == currentID);
                    CurrentNode.UpdateAnchors(row["ANode"].ToString(), Convert.ToDouble(row["RSSI"].ToString()), 1, DateTime.Now);
                    //CurrentNode = AnchorNodes.Find(AN => AN.WsnIdProperty == currentID);

                    RangeBasedPositioning.CalibratePathloss(AnchorNodes, RangeBasedPositioning.AverageFilter);
                }
            }
            logger.Close();
        }

        private DataSet FetchData()
        {
            string LowerBound, UpperBound, MyQuery;
            DataSet Set;

            Console.WriteLine("Enter the starting value of idLocalization: (dd hh:mm:ss)");
            LowerBound = Console.ReadLine();
            Console.WriteLine("Enter the ending value of idLocalization: (dd hh:mm:ss)");
            UpperBound = Console.ReadLine();

            MyQuery =  "SELECT * FROM localization l where time between '2009-06-" + LowerBound +  "' and '2009-06 -" + UpperBound + "';";
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

        private void UpdateAnhors()
        {
            Console.Write("Do you wish to update the position of the anchors? (Y/N) ");

            string response = Console.ReadLine();

            if (response == "Y" || response == "Yes")
            {
                Console.WriteLine("Enter the new anchors positions");

                do
                {
                    Console.Write("WsnId: ");
                    string wsnID = Console.ReadLine();
                    Point position = new Point();
                    Console.Write("X: ");
                    position.x = Convert.ToDouble(Console.ReadLine());
                    Console.Write("Y: ");
                    position.y = Convert.ToDouble(Console.ReadLine());

                    AddPosition(wsnID, position);

                    Console.Write("Enter another position? (Y/N) ");
                    response = Console.ReadLine();

                } while (response == "Y" || response == "Yes");

                Console.WriteLine("Thank you for updating the database!");
            }
        }

        private static double DistanceBetweenTwoPoints(Point point1, Point point2)
        {
            return Math.Pow((Math.Pow((point1.x - point2.x), 2) + Math.Pow((point1.y - point2.y), 2)), 0.5);
        }

        private double AverageDistanceToAnchors()
        {
            double distance = 0.0;
            Node CurrentNode = BlindNodes.Find(BN => BN.WsnIdProperty == "11");
            
            foreach (Node anchorNode in AnchorNodes)
            {
                distance += DistanceBetweenTwoPoints(anchorNode.Position, CurrentNode.Position);
            }

            distance /= AnchorNodes.Count;
            return distance;
        }

        #endregion Methods
    }
}