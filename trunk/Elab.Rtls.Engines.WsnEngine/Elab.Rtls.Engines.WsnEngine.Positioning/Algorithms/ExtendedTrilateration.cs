namespace Elab.Rtls.Engines.WsnEngine.Positioning
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Text;

    using DatabaseConnection;

    public class ExtendedTrilateration : RangeBasedPositioning
    {
        #region Methods(

        public static Point CalculatePosition(Node BlindNode, Node.FilterMethod filterMethod, Node.RangingMethod rangingMethod, bool multihop)
        {
            
            List<Point> intersectionPoints = new List<Point>();

            //multihop?
            //List<AnchorNode> AllAnchors = new List<AnchorNode>();
            //List<IntersectedAnchors> anchors = new List<IntersectedAnchors>();

            Point position = new Point();
            List<string> StatusCircles = new List<string>();

            List<int> ListOfCounts = new List<int>();
            int count;
            bool AllCirclesIntersected = false;

            foreach (AnchorNode an in BlindNode.Anchors)
            {
                an.fRSS = filterMethod(an.RSS);
                an.range = rangingMethod(an.fRSS);
            }

            if (!multihop)
            {
                if(BlindNode.Anchors.Count >= 3)
                {
                    while (!AllCirclesIntersected)
                    {
                        for (int i = 0; i < BlindNode.Anchors.Count; i++)
                        {
                            count = 0;

                            for (int j = 0; j < BlindNode.Anchors.Count; j++)
                            {
                                StatusCircles.Add(GeometryHelper.InOrOut(BlindNode.Anchors[i].posx, BlindNode.Anchors[i].posy, BlindNode.Anchors[i].range, BlindNode.Anchors[j].posx, BlindNode.Anchors[j].posy, BlindNode.Anchors[j].range));
                            }
                             if(StatusCircles.Contains("Overlap") && !( StatusCircles.Contains("In") || StatusCircles.Contains("Separated")) )
                                BlindNode.Anchors[i].range *= 0.9;
                            else if ( (StatusCircles.Contains("In") || StatusCircles.Contains("Separated") ) && !StatusCircles.Contains("Out"))
                                BlindNode.Anchors[i].range *= 1.1;

                             StatusCircles.Clear();
                        }

                        //Check if all nodes intersect
                        ListOfCounts.Clear();
                        for (int i = 0; i < BlindNode.Anchors.Count; i++)
                        {
                            count = 0;
                            for (int j = 0; j < BlindNode.Anchors.Count; j++)
                            {
                                if (GeometryHelper.BelongTo(BlindNode.Anchors[i].posx, BlindNode.Anchors[i].posy, BlindNode.Anchors[i].range, BlindNode.Anchors[j].posx, BlindNode.Anchors[j].posy, BlindNode.Anchors[j].range))
                                    count++;
                            }
                            ListOfCounts.Add(count);
                        }
                        if (ListOfCounts.Average() == BlindNode.Anchors.Count)
                        {
                            AllCirclesIntersected = true;
                        }
                    }

                    //Determine crossings
                    for (int i = 0; i < BlindNode.Anchors.Count - 1; i++)
                    {
                        for (int j = i + 1; j < BlindNode.Anchors.Count; j++)
                        {
                            //returns 0, 1 or 2 Pointss
                            List<Point> crossingPoints = GeometryHelper.Intersect(BlindNode.Anchors[i].posx, BlindNode.Anchors[i].posy, BlindNode.Anchors[i].range, BlindNode.Anchors[j].posx, BlindNode.Anchors[j].posy, BlindNode.Anchors[j].range);
                            if (crossingPoints != null)
                            {
                                foreach (Point crossing in crossingPoints)
                                    intersectionPoints.Add(crossing);
                            }

                        }
                    }

                    //Calculate BN position with clustering
                    if (intersectionPoints.Count >= 3)
                        position = Cluster(intersectionPoints, BlindNode.Anchors.Count);
                    else
                    {
                        position = null;
                    }
                }
                else
                    position = null;

            }
            /*
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
            */
            return position;
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

        #endregion Methods
    }
}