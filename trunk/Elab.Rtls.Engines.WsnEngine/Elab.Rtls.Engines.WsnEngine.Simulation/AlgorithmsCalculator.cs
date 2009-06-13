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
        private StreamWriter logger;

        /// <summary>
        /// BN's WsnId
        /// </summary>
        private string currentID;

        #endregion Fields

        #region Constructors

        internal static void Main()
        {
            AlgorithmsCalculator calc = new AlgorithmsCalculator();
        }

        public AlgorithmsCalculator()
        {
            MyDb = new MySQLClass("DRIVER={MySQL ODBC 3.51 Driver};SERVER=localhost;DATABASE=senseless;UID=root;PASSWORD=admin;OPTION=3;");
            string response, fileName;

            BlindNodes.Add(new Node("11", MyDb));

            Console.Write("Enter the file which you wish to write to: ");
            fileName = Console.ReadLine();
            logger = new StreamWriter(fileName, true);
            //writing the csv header
            logger.WriteLine("idLocalization, time, WsnID, #anchors, CL X, CL X, CL AbsErr, CL RelErr," +
            "WCL X, WCL X, WCL AbsErr, WCL RelErr, MinMax R X, MinMax R Y, MinMax R AbsErr, MinMax R RelErr," +
            "MinMax DR X, MinMax DR Y, MinMax DR AbsErr, MinMax DR RelErr," +
            "TriLat R X, TriLat R Y, TriLat R AbsErr, TriLat R RelErr," +
            "TriLat DR X, TriLat DR Y, TriLat DR AbsErr, TriLat DR RelErr," +
            "LSTriLat R X, LSTriLat R Y, LSTriLat R AbsErr, LSTriLat R RelErr," +
            "LSTriLat DR X, LSTriLat DR Y, LSTriLat DR AbsErr, LSTriLat DR RelErr\n\n");

            do
            {
                UpdateAnhors();
                UpdateBlind();

                Console.WriteLine("Using a new batch of data!");
                DataSet TempSet = FetchData();
                Console.Write("Enter the number of anchor nodes that should be in this dataset: ");
                int numberAnchors = Convert.ToInt32(Console.ReadLine());
                ExecuteAlgorithms(TempSet, numberAnchors);

                Console.Write("Process another batch of data? (Y/N) ");
                response = Console.ReadLine();
            } while (response == "Yes" || response == "Y");

            logger.Close();
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


            Node CurrentNode = BlindNodes.Find(BN => BN.WsnIdProperty == "11");
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

        private void ExecuteAlgorithms(DataSet Set, int numberAnchors)
        {
            Console.WriteLine("Executing each algorithm on the data");            

            foreach (DataRow row in Set.Tables[0].Rows)
            {
                currentID = row["node"].ToString();
                Node CurrentNode;

                //Positioning.Point pos = new Positioning.Point(0, 0);
                Point pos = new Point(0, 0);

                if (currentID == "11")
                {
                        CurrentNode = BlindNodes.Find(BN => BN.WsnIdProperty == currentID);
                        CurrentNode.UpdateAnchors(row["ANode"].ToString(), Convert.ToDouble(row["RSSI"].ToString()), 1, DateTime.Now);
                        ////TODO: check if automatically updated
                        //CurrentNode = BlindNodes.Find(BN => BN.WsnIdProperty == currentID);

                    if (CurrentNode.Anchors.Count == numberAnchors)
                    {
                        logger.Write(row["idlocalization"].ToString() + ",");
                        logger.Write(row["time"].ToString() + ",");
                        logger.Write(currentID + ",");
                        logger.Write(CurrentNode.Anchors.Count + ",");

                        //centroid localization
                        //logger.Write("CL,");
                        pos = CentroidLocalization.CalculatePosition(CurrentNode);
                        LogPosition(pos, CurrentNode, logger);

                        //wcl
                        //logger.Write("WCL - NoFilter,");
                        pos = WCL.CalculatePosition(CurrentNode, RangeBasedPositioning.NoFilter);
                        LogPosition(pos, CurrentNode, logger);

                        //min-max
                        //logger.Write("Min-Max - NoFilter - Ranging,");
                        pos = MinMaxExtended.CalculatePosition(CurrentNode, RangeBasedPositioning.NoFilter, RangeBasedPositioning.Ranging, false);
                        LogPosition(pos, CurrentNode, logger);

                        //logger.Write("Min-Max - NoFilter - DefaultRanging,");
                        pos = MinMaxExtended.CalculatePosition(CurrentNode, RangeBasedPositioning.NoFilter, RangeBasedPositioning.DefaultRanging, false);
                        LogPosition(pos, CurrentNode, logger);

                        //trilateration
                        //logger.Write("TriLat - NoFilter - Ranging,");
                        pos = ExtendedTrilateration.CalculatePosition(CurrentNode, RangeBasedPositioning.NoFilter,
                                                                      RangeBasedPositioning.Ranging, false);
                        LogPosition(pos, CurrentNode, logger);

                        //logger.Write("TriLat - NoFilter - DefaultRanging,");
                        pos = ExtendedTrilateration.CalculatePosition(CurrentNode, RangeBasedPositioning.NoFilter,
                                                                      RangeBasedPositioning.DefaultRanging, false);
                        LogPosition(pos, CurrentNode, logger);

                        //lstrilateration
                        //logger.Write("LSTriLat - NoFilter - Ranging,");
                        pos = LSTrilateration.CalculatePosition(CurrentNode, RangeBasedPositioning.NoFilter,
                                                                      RangeBasedPositioning.Ranging, false);
                        LogPosition(pos, CurrentNode, logger);

                        //logger.Write("LSTriLat - NoFilter - DefaultRanging,");
                        pos = LSTrilateration.CalculatePosition(CurrentNode, RangeBasedPositioning.NoFilter,
                                                                      RangeBasedPositioning.DefaultRanging, false);
                        LogPosition(pos, CurrentNode, logger);
                    }
                    else if (CurrentNode.Anchors.Count > numberAnchors)
                    {
                        logger.WriteLine("Too many nodes for idlocalization:" + row["idlocalization"].ToString());
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

                    RangeBasedPositioning.CalibratePathloss(AnchorNodes, RangeBasedPositioning.NoFilter);
                    
                }
            }
        }

        private void LogPosition(Point pos, Node CurrentNode, StreamWriter logger)
        {
            if (pos != null)
            {
                logger.Write(pos.x + ",");
                logger.Write(pos.y + ",");
                logger.Write(DistanceBetweenTwoPoints(pos, CurrentNode.Position) + ",");
                logger.Write((DistanceBetweenTwoPoints(pos, CurrentNode.Position) / AverageDistanceToAnchors()) + ",");
            }
            else
            {
                logger.Write("null,");
                logger.Write("null,");
                logger.Write("null,");
                logger.Write("null,");
            }
        }

        private DataSet FetchData()
        {
            string LowerBound, UpperBound, MyQuery;
            DataSet Set;

            Console.WriteLine("Enter the starting value of time: (dd hh:mm:ss)");
            LowerBound = Console.ReadLine();
            Console.WriteLine("Enter the ending value of time: (dd hh:mm:ss)");
            UpperBound = Console.ReadLine();

            MyQuery =  "SELECT * FROM localization l where time between '2009-06-" + LowerBound +  "' and '2009-06-" + UpperBound + "';";
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
            
            foreach (AnchorNode anchorNode in CurrentNode.Anchors)
            {
                distance += DistanceBetweenTwoPoints(new Point(anchorNode.posx, anchorNode.posy), CurrentNode.Position);
            }

            distance /= CurrentNode.Anchors.Count;
            return distance;
        }

        #endregion Methods
    }
}