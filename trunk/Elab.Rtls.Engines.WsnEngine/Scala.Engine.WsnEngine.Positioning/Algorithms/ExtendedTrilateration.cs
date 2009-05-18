using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using DatabaseConnection;
using SocketConnection;

using System.IO;

namespace Elab.Rtls.Engines.WsnEngine.Positioning
{
    public class ExtendedTrilateration : RangeBasedPositioning
    {
        public static Point CalculatePosition(Node BlindNode, Node.FilterMethod filterMethod, bool multihop)
        {
            List<Point> intersectionPoints = new List<Point>();
            List<AnchorNode> AllAnchors = new List<AnchorNode>();
            List<IntersectedAnchors> anchors = new List<IntersectedAnchors>();
            IntersectedAnchors anchor = new IntersectedAnchors();
            Point center = new Point();
            List<int> ListOfCounts = new List<int>();

            bool AllCirclesIntersected = false;

            StreamWriter Log = new StreamWriter("Trilateration.csv", false);

            if (BlindNode.Anchors.Count >= 3)
            {
                foreach (AnchorNode AN in BlindNode.Anchors)
                {
                    AN.fRSS = filterMethod(AN.RSS);
                }


                if (!multihop)
                {
                    while (!AllCirclesIntersected)
                    {
                        for (int i = 0; i < BlindNode.Anchors.Count; i++)
                        {
                            count = 0;
                            BlindNode.Anchors[i].fRSS = filterMethod(BlindNode.Anchors[i].RSS);
                            BlindNode.Anchors[i].range = Ranging(BlindNode.Anchors[i].fRSS);
                            for (int j = 0; j < BlindNode.Anchors.Count; j++)
                            {
                                BlindNode.Anchors[j].fRSS = filterMethod(BlindNode.Anchors[j].RSS);
                                BlindNode.Anchors[j].range = Ranging(BlindNode.Anchors[j].fRSS);
                                if (BelongsToAllCircles(BlindNode.Anchors[i].posx, BlindNode.Anchors[i].posy, Ranging(BlindNode.Anchors[i].fRSS), BlindNode.Anchors[j].posx, BlindNode.Anchors[j].posy, Ranging(BlindNode.Anchors[j].fRSS)))
                                    //                    if (BelongsToAllBoxes(BlindNode.Anchors[i].posx, BlindNode.Anchors[i].posy, 10, BlindNode.Anchors[j].posx, BlindNode.Anchors[j].posy, 10) )
                                    count++;
                            }
                            ListOfCounts.Add(count);
                       }
                        if (ListOfCounts.Average() != BlindNode.Anchors.Count)
                        {
                            ListOfCounts.Clear();
                            foreach (AnchorNode an in BlindNode.Anchors)
                                an.range = an.range * 1.1;
                        }
                        else
                            AllCirclesIntersected = true;
                    }

                    for (int i = 0; i < BlindNode.Anchors.Count - 1; i++)
                    {
                        for (int j = i + 1; j < BlindNode.Anchors.Count; j++)
                        {
                            //returns 0, 1 or 2 Pointss
                            foreach (Point crossing in Intersect(BlindNode.Anchors[i].posx, BlindNode.Anchors[i].posy, BlindNode.Anchors[i].range, BlindNode.Anchors[j].posx, BlindNode.Anchors[j].posy, BlindNode.Anchors[j].range))
                            {
                                intersectionPoints.Add(crossing);
                            }


                        }
                    }
                    if (intersectionPoints.Count >= 3)
                        center = Cluster(intersectionPoints, BlindNode.Anchors.Count);
                    else
                    {
                        center.x = 0;
                        center.y = 0;
                    }

                }
                else
                {
                    for (int i = 0; i < BlindNode.Anchors.Count - 1; i++)
                    {
                        for (int j = i + 1; j < BlindNode.Anchors.Count; j++)
                        {
                            //returns 0, 1 or 2 Pointss
                            //foreach (Point crossing in Intersect(BlindNode.Anchors[i].posx, BlindNode.Anchors[i].posy, Ranging(BlindNode.Anchors[i].fRSS), BlindNode.Anchors[j].posx, BlindNode.Anchors[j].posy, Ranging(BlindNode.Anchors[j].fRSS)))
                            //{
                            //    intersectionPoints.Add(crossing);
                            //}
                            //TEST
                            foreach (Point crossing in GeometryHelper.Intersect(BlindNode.Anchors[i].posx, BlindNode.Anchors[i].posy, 1.41, BlindNode.Anchors[j].posx, BlindNode.Anchors[j].posy, 1.41))
                            {
                                intersectionPoints.Add(crossing);
                            }
                        }
                    }
                    if (intersectionPoints.Count < 3)
                    {
                        foreach (AnchorNode an in BlindNode.Anchors)
                            AllAnchors.Add(an);
                        foreach (AnchorNode van in BlindNode.VirtualAnchors)
                            AllAnchors.Add(van);

                        for (int i = 0; i < AllAnchors.Count - 1; i++)
                        {
                            for (int j = i + 1; j < AllAnchors.Count; j++)
                            {
                                //returns 0, 1 or 2 Pointss
                                //foreach (Point crossing in Intersect(BlindNode.Anchors[i].posx, BlindNode.Anchors[i].posy, Ranging(BlindNode.Anchors[i].fRSS), BlindNode.Anchors[j].posx, BlindNode.Anchors[j].posy, Ranging(BlindNode.Anchors[j].fRSS)))
                                //{
                                //    intersectionPoints.Add(crossing);
                                //}
                                //TEST
                                foreach (Point crossing in GeometryHelper.Intersect(AllAnchors[i].posx, AllAnchors[i].posy, 1.41, AllAnchors[j].posx, AllAnchors[j].posy, 1.41))
                                {
                                    intersectionPoints.Add(crossing);
                                }
                            }
                        }

                    }
                    else
                        center = Cluster(intersectionPoints, BlindNode.Anchors.Count);

                }
                return center;

            }
            else
                throw new ApplicationException("Less than three anchor nodes available");

        }
        private static Error Anchorsintersection(List<IntersectedAnchors> anchors)
        {
            Error fault = new Error();

            double distance;
            int inter = 0;
            int exter = 0;

            foreach (IntersectedAnchors ANs in anchors)
            {
                distance = Math.Pow((Math.Pow((ANs.x2 - ANs.x1), 2) + Math.Pow((ANs.y2 - ANs.y1), 2)), 0.5);
                if (distance < Math.Abs(ANs.r1 - ANs.r2))
                    inter++;
                if (distance > (ANs.r1 + ANs.r2))
                    exter++;

            }
            fault.NumberSmall = inter;
            fault.NumberWide = exter;

            return fault;

        }

        private static Point Cluster(List<Point> crossings, int anchors)
        {
            List<Point> cluster = new List<Point>();
            Point centroid = new Point();

            //first pass.
            ClosestPointPair closestPointPair = new ClosestPointPair();

            //get the two closest intersection point
            closestPointPair = GeometryHelper.ShortestDistanceBetweenCutPoints(crossings); // get struct back

            //add these points to the cluster
            cluster.Add(new Point(closestPointPair.point1.x, closestPointPair.point1.y));
            cluster.Add(new Point(closestPointPair.point2.x, closestPointPair.point2.y));
            //remove them from the intersection
            crossings.Remove(closestPointPair.point1);
            crossings.Remove(closestPointPair.point2);

            centroid.x = (cluster[0].x + cluster[1].x) / 2;
            centroid.y = (cluster[0].y + cluster[1].y) / 2;

            crossings.Add(centroid);

            while (cluster.Count < anchors && crossings.Count > 0)
            {
                Point closestPoint = GeometryHelper.ShortestDistanceBetweenCutPoints(crossings, centroid);

                cluster.Add(closestPoint);
                crossings.Remove(closestPoint);

                Point tempCentroid = new Point();

                foreach (Point clusterPoint in cluster)
                {

                    tempCentroid.x += clusterPoint.x;
                    tempCentroid.y += clusterPoint.y;
                }

                tempCentroid.x /= cluster.Count;
                tempCentroid.y /= cluster.Count;

                centroid = tempCentroid;
            }

            return centroid;
        }

        private static bool BelongsToAllCircles(double x1, double y1, double r1, double x2, double y2, double r2)
        {
            double distance1 = Math.Pow((Math.Pow((x1 - x2), 2) + Math.Pow((y1 - y2), 2)), 0.5);

            if (distance1 <= (r1 + r2))
                return true;
            else
                return false;
        }

    }
}