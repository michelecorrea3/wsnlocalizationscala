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
            public static double AverageFilter(Queue<double> RSS)
            {
                double total = 0;

                foreach (var d in RSS)
                {
                    total += d;
                }

                return (total/RSS.Count);
            }

            public static double MedianFilter(Queue<double> RSS)
            {
                List<double> RSSList = RSS.ToList();
                RSSList.Sort();

                //uneven
                if (RSSList.Count % 2 != 0)
                {
                    return RSSList[(RSSList.Count - 1)/2];
                }
                //even
                else
                {
                    return ((RSSList[(RSSList.Count/2) - 1] + RSSList[(RSSList.Count/2)])/2);
                }
            }

            ///// <summary>
            ///// Simple FIR filter, with weight 90%
            ///// </summary>
            ///// <param name="RSS"></param>
            ///// <returns></returns>
            //public static double FilterRSS(double RSS)
            //{
            //    double fRSS = 0;

            //    if (fRSS == 0)
            //        fRSS = RSS;
            //    else
            //        fRSS = 0.9 * fRSS + 0.1 * RSS;

            //    return fRSS;
            //}

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
                const double baseRSS = 51, pathLossExponent = 2;

                distance = Math.Pow(10, ((-baseRSS - fRSS) / (10 * pathLossExponent)));

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
                double tempx = 0, tempy = 0;
                int count = 0;

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
        public class MinMax : RangeBasedPositioning
        {
            public static Point CalculatePosition(Node BlindNode, Node.FilterMethod filterMethod)
            {
                BoundingBox BnBox = new BoundingBox(0);
                Point position = new Point();
                double distance;

                if (BlindNode.Anchors.Count >= 3)
                {
                    foreach (AnchorNode AN in BlindNode.Anchors)
                    {
                        //perform the ranging
                        double fRSS = filterMethod(AN.RSS);
                        distance = Ranging(fRSS);

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

        public class ClusterTrilateration : RangeBasedPositioning
        {
            
//            Point coordinates = new Point();

            public static Point CalculatePosition(Node BlindNode, Node.FilterMethod filterMethod)
            {
                int anchors = 0;
                Point coordinates = new Point();
                List<Point> IntersectionPoints = new List<Point>();
                if (BlindNode.Anchors.Count >= 3)
                {
                    anchors = BlindNode.Anchors.Count;

                    for (int i = 0; i < anchors-1; i++)
                    {
                        for (int j = i + 1; j < anchors; j++)
                        {
                            foreach (Point cuttingpoint in CuttingPoint(BlindNode.Anchors[i].posx, BlindNode.Anchors[i].posy, Ranging(BlindNode.Anchors[i].fRSS), BlindNode.Anchors[j].posx, BlindNode.Anchors[j].posy, Ranging(BlindNode.Anchors[j].fRSS)))
                            {
                                IntersectionPoints.Add(cuttingpoint);
                            }
                        }
                    }
                    return Cluster(IntersectionPoints, anchors);
                    
                }
                else
                    throw new ApplicationException("Less than three anchor nodes available");
      
            }

            private struct DistanceBetweenCutpoints
            {
                public Point point1;
                public Point point2;
                public double distance;
            };

            private static List<Point> CuttingPoint(double x1, double y1, double radius1, double x2, double y2, double radius2)
            {

                Point position1 = new Point();
                Point position2 = new Point();
                List<Point> points = new List<Point>();
                double l = 0;
                double k = 0;

                double a = x1;
                double b = y1;

                double c = x2;
                double d = y2;

                double D;

                double r1 = radius1 * radius1;
                double r2 = radius2 * radius2;


                //Calculatiings:
                // y = lx + b
                l = (2*c - 2*a)/(2*b - 2*d);
                k = (r2 - r1 - (c * c) - (d*d) + (a*a) + (b*b))/(2*b - 2*d);

                //discriminant
                double f = -2 * a + 2 * l * k - 2 * b * l;
                double e = 1 + (l * l);
                double g = (a * a) + (k * k) - 2 * b * k + (b * b) - r1;

                D = (f * f) - 4 * e * g;

                if (D == 0)
                {
                    position1.x = (-f) / (2 * e);
                    position1.y = Math.Pow(0.5, (r1 - ((position1.x - a) * (position1.x - a)))) + b;
                    points.Add(position1);

                    return points;

                }
                else if (D > 0)
                {

                    position1.x = (-f + Math.Pow(0.5, ((f * f) - 4 * e * g)) / (2 * e));
                    position1.y = Math.Pow(0.5, (r1 - ((position1.x - a) * (position1.x - a)))) + b;

                    position2.x = (-f + Math.Pow(0.5, ((f * f) - 4 * e * g)) / (2 * e));
                    position2.y = Math.Pow(0.5, (r1 - ((position1.x - a) * (position1.x - a)))) + b;

                    points.Add(position1);
                    points.Add(position2);

                    return points;
                }
                else
                    throw new ApplicationException("No cutting points");

            }

            private static DistanceBetweenCutpoints ShortestDistanceBetweenCutPoints(List<Point> CuttingPoints)
            {
                List<DistanceBetweenCutpoints> Distances = new List<DistanceBetweenCutpoints>();
                DistanceBetweenCutpoints ShortestDistance = new DistanceBetweenCutpoints();
                DistanceBetweenCutpoints Distance = new DistanceBetweenCutpoints();

                ShortestDistance.distance = 100000;

                for (int i = 0; i < CuttingPoints.Count - 1; i++)
                {
                    for (int j = i + 1; j < CuttingPoints.Count; j++)
                    {
                        Distance.point1 = CuttingPoints[i];
                        Distance.point2 = CuttingPoints[j];
                        Distance.distance = Math.Pow(0.5, (Math.Pow(2, (CuttingPoints[0].x - CuttingPoints[1].x)) + Math.Pow(2, (CuttingPoints[0].y - CuttingPoints[1].y))));
                        Distances.Add(Distance);
                    }
                }

                foreach (DistanceBetweenCutpoints dist in Distances)
                {
                    if (dist.distance < ShortestDistance.distance)
                    {
                        ShortestDistance.distance = dist.distance;
                        ShortestDistance.point1 = dist.point1;
                        ShortestDistance.point2 = dist.point2;
                    }
                }

                return ShortestDistance;
            }

            private static Point Cluster(List<Point> CuttingPoints, int anchors)
            {
                //List<DistanceBetweenCutpoints> ListOfDistance = new List<DistanceBetweenCutpoints>();
                //DistanceBetweenCutpoints cuts = new DistanceBetweenCutpoints();
                Point centroid = new Point();

                DistanceBetweenCutpoints shortest = new DistanceBetweenCutpoints();
                //shortest.distance = 100000;

                while (CuttingPoints.Count != 0)
                {
                    shortest = ShortestDistanceBetweenCutPoints(CuttingPoints); // get struct back

                    centroid.x = (shortest.point1.x - shortest.point2.x) / 2;
                    centroid.y = (shortest.point1.y - shortest.point2.y) / 2;

                    CuttingPoints.Remove(shortest.point1);
                    CuttingPoints.Remove(shortest.point2);
                    CuttingPoints.Add(centroid);
                }
                
                return centroid;
            }


        }




        public class Node //: Elab.Rtls.Engines.WsnEngine.Positioning.INode
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

            public delegate double FilterMethod(Queue<double> RSS);

            public Node(string WsnId, MySQLClass MyDb)
            {
                this.MyDb = MyDb;
                this.WsnId = WsnId;
            }

            public Node(string WsnId, MySQLClass MyDb, string AnchorWsnId, double RSS)
            {
                this.MyDb = MyDb;
                this.WsnId = WsnId;
                NewAnchor(AnchorWsnId, RSS);
            }

            private void NewAnchor(string AnchorWsnId, double RSS)
            {
                this.AnchorWsnId = AnchorWsnId;
                Point ANpos = new Point();

                ANpos = GetANPosition(AnchorWsnId);

                Anchorlist.Add(new AnchorNode(AnchorWsnId, ANpos.x, ANpos.y, RSS));
            }

            public void AddAnchor(string AnchorWsnId, double RSS)
            {
                this.AnchorWsnId = AnchorWsnId;
                Point ANpos = new Point();
                AnchorNode TempNode;
                double fRSS;

                ANpos = GetANPosition(AnchorWsnId);

                AnchorNode TempANode = Anchorlist.Find(AN => AN.nodeid == AnchorWsnId);
                int index = Anchorlist.IndexOf(TempANode);

                Anchorlist[index].RSS.Enqueue(RSS);

                if (Anchorlist[index].RSS.Count > 20)
                {
                    do
                    {
                        Anchorlist[index].RSS.Dequeue();
                    } while (Anchorlist[index].RSS.Count > 20);
                }   
                    //RSS bestaande Anchor zoeken
                    //bestaande Anchor Zoeken
                    //TempNode = Anchorlist.Find(HasNodeID);
                    //fRSS = RangeBasedPositioning.FilterRSS(TempNode.RSS);
                    ////RSS filteren via RangeBasedPositioning.Filter
                    //Anchorlist.RemoveAll(HasNodeID);
                    //Anchorlist.Add(new AnchorNode(AnchorWsnId, ANpos.x, ANpos.y, fRSS));
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
                            pos.x = Convert.ToDouble(row["X"]);
                            pos.y = Convert.ToDouble(row["Y"]);
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
        public class AnchorNode
        {
            public string nodeid;
            public double posx;
            public double posy;
            public double fRSS;
            public Queue<double> RSS = new Queue<double>(20);

            //NOT USED FOR NOW
            public DateTime time;

            public AnchorNode(string wsnid, double posx, double posy, double RSS)
            {
                this.nodeid = wsnid;
                this.posx = posx;
                this.posy = posy;
                this.RSS.Enqueue(RSS);
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
            public double x, y;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            public Point(double x, double y)
            {
                this.x = x;
                this.y = y;
            }
        }
}
