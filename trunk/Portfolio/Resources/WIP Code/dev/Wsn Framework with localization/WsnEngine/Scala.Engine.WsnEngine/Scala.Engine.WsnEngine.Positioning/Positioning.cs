using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using DatabaseConnection;

using SocketConnection;

namespace Elab.Rtls.Engines.WsnEngine.Positioning
{
        /// <summary>
        /// Abstract base class for all positioning algorithms used for type safety
        /// </summary>
        public abstract class Positioning
        {
        }

        /// <summary>
        /// Abstract base class for all range based positioning algorithms used for type safety
        /// 2 static methods:
        /// FilterRSS: filters the RSS
        /// Ranging: convert RSS in dBm to centimeters
        /// </summary>
        public abstract class RangeBasedPositioning : Positioning
        {
            /// <summary>
            /// Simple FIR filter, with weight 90%
            /// </summary>
            /// <param name="RSS"></param>
            /// <returns></returns>
            public static double FilterRSS(double RSS)
            {
                double fRSS = 0;

                if (fRSS == 0)
                    fRSS = RSS;
                else
                    fRSS = 0.9 * fRSS + 0.1 * RSS;

                return fRSS;
            }

            /// <summary>
            /// Translates RSS readings into distances expressed in meters
            /// 
            /// Formula from the paper "Relative location in Wireless Networks
            /// baseRSS was already measured in our BAP, however this data has not been validated
            /// lossFactor has the free space value, in indoor environments this can be 1,6 to 1,8
            /// </summary>
            /// <param name="fRSS">The filtered RSS</param>
            /// <returns>The distance expressed in centimeters</returns>
            public static double Ranging(double fRSS)
            {
                double distance;
                const double baseRSS = -51, lossFactor = 2;

                distance = 100 * Math.Pow(10, ((baseRSS - fRSS) / (10 * lossFactor)));

                return distance;
            }
        }

        /// <summary>
        /// Abstract base class for all connectivity-based positioning algorithms used for type safety
        /// </summary>
        public abstract class ConnectivityBasedPositioning : Positioning
        {
        }

        /// <summary>
        /// Centroid Localization algorithm, the position is the centroid of the anchor nodes position
        /// </summary>
        public class CentroidLocalization : ConnectivityBasedPositioning
        {
            public static Point CalculatePosition(Node BlindNode)
            {
                Point position = new Point();
                int tempx = 0, tempy = 0, count = 0;

                foreach (AnchorNode AN in BlindNode.Anchors)
                {
                    tempx += AN.posx;
                    tempy += AN.posy;
                    count++;
                }

                position.x = tempx / count;
                position.y = tempy / count;

                return position;
            }
        }

        /// <summary>
        /// Min-Max Localization algorithm with simple radio propagation model
        /// Model parameters:
        /// </summary>
        public class MinMaxSimpleModel : RangeBasedPositioning
        {
            public static Point CalculatePosition(Node BlindNode)
            {
                BoundingBox BnBox = new BoundingBox(0);
                Point position = new Point();
                double distance;

                if (BlindNode.Anchors.Count >= 3)
                {
                    foreach (AnchorNode AN in BlindNode.Anchors)
                    {
                        //perform the ranging
                        distance = Ranging(AN.RSS);

                        Point center = new Point(AN.posx, AN.posy);
                        BoundingBox AnBox = new BoundingBox(center, distance);

                        BnBox.Xmin = Math.Max(BnBox.Xmin, AnBox.Xmin);
                        BnBox.Xmax = Math.Min(BnBox.Xmax, AnBox.Xmax);
                        BnBox.Ymin = Math.Max(BnBox.Ymin, AnBox.Ymin);
                        BnBox.Ymax = Math.Min(BnBox.Ymax, AnBox.Ymax);
                    }
                }
                else
                    throw new ApplicationException("Less than three anchor nodes available");

                position.x = (int)(BnBox.Xmin + BnBox.Xmax) / 2;
                position.y = (int)(BnBox.Ymin + BnBox.Ymax) / 2;

                return position;
            }
        }

        /// <summary>
        /// CHANGE: struct for the boundingbox in min max
        /// </summary>
        public struct BoundingBox
        {
            public double Xmin, Xmax, Ymin, Ymax;

            public BoundingBox(double number)
            {
                this.Xmin = number;
                this.Xmax = number;
                this.Ymin = number;
                this.Ymax = number;
            }

            public BoundingBox(Point center, double deviation)
            {
                this.Xmin = center.x - deviation;
                this.Xmax = center.x + deviation;
                this.Ymin = center.y - deviation;
                this.Ymax = center.y + deviation;
            }
        }

        public class Node : Elab.Rtls.Engines.WsnEngine.Positioning.INode
        {
            private MySQLClass MyDb;

            private string WsnId, AnchorWsnId;

            private List<AnchorNode> Anchorlist = new List<AnchorNode>();

            public List<AnchorNode> Anchors
            {
                get { return Anchorlist; }
            }

            public string WsnIdProperty
            {
                get { return WsnId; }
            }

            public Node(string WsnId, MySQLClass MyDb)
            {
                this.MyDb = MyDb;
                this.WsnId = WsnId;
            }

            public Node(string WsnId, MySQLClass MyDb, string AnchorWsnId, double RSS)
            {
                this.MyDb = MyDb;
                this.WsnId = WsnId;
                AddAnchor(AnchorWsnId, RSS);
            }

            public void AddAnchor(string AnchorWsnId, double RSS)
            {
                this.AnchorWsnId = AnchorWsnId;
                Point ANpos = new Point();
                AnchorNode TempNode;
                double fRSS;

                ANpos = GetANPosition(AnchorWsnId);

                if (!Anchorlist.Exists(HasNodeID))
                {
                    Anchorlist.Add(new AnchorNode(AnchorWsnId, ANpos.x, ANpos.y, RSS));
                }
                else
                {
                    //RSS bestaande Anchor zoeken
                    //bestaande Anchor Zoeken
                    TempNode = Anchorlist.Find(HasNodeID);
                    fRSS = RangeBasedPositioning.FilterRSS(TempNode.RSS);
                    //RSS filteren via RangeBasedPositioning.Filter
                    Anchorlist.RemoveAll(HasNodeID);
                    Anchorlist.Add(new AnchorNode(AnchorWsnId, ANpos.x, ANpos.y, fRSS));
                }
            }

            /// <summary>
            /// Retrieves the position of the specified node from the DB
            /// </summary>
            /// <param name="AN">The WSNid of the Anchor Node</param>
            /// <returns>The position of the Anchor Node</returns>
            private Point GetANPosition(string AnchorWsnId)
            {
                DataSet returnSet;
                string cmd = "call getStatusWSNID('" + AnchorWsnId + "');";
                Point pos = new Point();

                try
                {
                    {
                        returnSet = MyDb.Query(cmd);

                        foreach (DataRow row in returnSet.Tables[0].Rows)
                        {
                            pos.x = Convert.ToInt32(row["X"]);
                            pos.y = Convert.ToInt32(row["Y"]);
                        }
                    }
                }
                catch (Exception e_mysql)
                {
                    Console.WriteLine(e_mysql.Message);
                    Console.WriteLine(e_mysql.StackTrace);
                    Console.WriteLine("Some field is probably missing");
                }

                return pos;
            }

            /// <summary>
            /// Delegate predicate for list.Exists
            /// </summary>
            /// <param name="AN"></param>
            /// <returns></returns>
            private bool HasNodeID(AnchorNode AN)
            {
                if (AN.nodeid == this.AnchorWsnId)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Struct that holds information about the anchors, including RSS!
        /// </summary>
        public struct AnchorNode
        {
            public string nodeid;
            public int posx;
            public int posy;
            public double RSS;

            //NOT USED FOR NOW
            public DateTime time;

            public AnchorNode(string wsnid, int posx, int posy, double RSS)
            {
                this.nodeid = wsnid;
                this.posx = posx;
                this.posy = posy;
                this.RSS = RSS;
                this.time = DateTime.UtcNow;
            }
        }

        /// <summary>
        /// Simple struct to hold a 2D point
        /// </summary>
        public struct Point
        {
            /// <summary>
            /// X & Y coordinates
            /// </summary>
            public int x, y;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            public Point(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }
}
