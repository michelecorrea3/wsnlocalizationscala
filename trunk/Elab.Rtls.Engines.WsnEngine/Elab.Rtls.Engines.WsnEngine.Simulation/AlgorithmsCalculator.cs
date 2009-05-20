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
            DataSet TempSet;
            MyDb = new MySQLClass("DRIVER={MySQL ODBC 3.51 Driver};SERVER=localhost;DATABASE=senseless;UID=root;PASSWORD=admin;OPTION=3;");

            TempSet = FetchData();

            ExecuteAlgorithms(TempSet);

            Console.WriteLine("Done! Press ENTER to exit");
            Console.ReadLine();
        }

        #endregion Constructors

        #region Methods

        private void ExecuteAlgorithms(DataSet Set)
        {
            Console.WriteLine("Executing each algorithm on the data");

            //foreach algorithms that exists ...
            //foreach (ob

            //write the data to the stream ...
            StreamWriter logger = new StreamWriter("Log.csv", false);

            foreach (DataRow row in Set.Tables[0].Rows)
            {
                currentID = row["node"].ToString();
                Node CurrentBlindNode;

                //Positioning.Point pos = new Positioning.Point(0, 0);
                Point pos = new Point(0, 0);

                if (!BlindNodes.Exists(ExistsNode))
                {
                    BlindNodes.Add(new Node(row["node"].ToString(), MyDb, row["ANode"].ToString(), Convert.ToDouble(row["RSSI"].ToString())));
                    //Console.WriteLine("Added new BN to be positioned\n\n\n");
                    CurrentBlindNode = BlindNodes.Find(ExistsNode);
                }
                else
                {
                    CurrentBlindNode = BlindNodes.Find(ExistsNode);
                    CurrentBlindNode.AddAnchor(row["ANode"].ToString(), Convert.ToDouble(row["RSSI"].ToString()));
                }

                logger.Write(row["idlocalization"].ToString() + ",");
                logger.Write(row["time"].ToString() + ",");
                logger.Write(currentID + ",");
                logger.Write(CurrentBlindNode.Anchors.Count + ",");

                //centroid localization
                logger.Write("CL,");

                try
                {
                    pos = CentroidLocalization.CalculatePosition(CurrentBlindNode);
                    logger.Write(pos.x + ",");
                    logger.Write(pos.y + ",");
                }
                catch (ApplicationException)
                {
                    logger.Write("null,");
                    logger.Write("null");
                }

                //min-max
                logger.Write("Min-Max,");

                try
                {
                    pos = MinMaxSimpleModel.CalculatePosition(CurrentBlindNode);
                    logger.Write(pos.x + ",");
                    logger.Write(pos.y + ",");
                }
                catch (ApplicationException)
                {
                    logger.Write("null,");
                    logger.Write("null");
                }

                logger.WriteLine("");
                logger.Flush();
            }
            logger.Close();
        }

        /// <summary>
        /// Predicate, checks if the node is in the list
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

        private DataSet FetchData()
        {
            string LowerBound, UpperBound, MyQuery;
            DataSet Set;

            Console.WriteLine("Enter the starting value of idLocalization: ");
            LowerBound = Console.ReadLine();
            Console.WriteLine("Enter the ending value of idLocalization: ");
            UpperBound = Console.ReadLine();

            MyQuery =  "SELECT * FROM localization l where idLocalization between " + LowerBound +  " and " + UpperBound + ";";
            Set = MyDb.Query(MyQuery);
            Console.WriteLine("Retreived the data from the database");

            return Set;
        }

        #endregion Methods
    }
}