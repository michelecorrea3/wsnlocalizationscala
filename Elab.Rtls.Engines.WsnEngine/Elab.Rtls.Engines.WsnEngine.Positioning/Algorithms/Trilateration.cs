namespace Elab.Rtls.Engines.WsnEngine.Positioning
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Text;

    using DatabaseConnection;
    /// <summary>
    /// Trilateration algorithm without Least Square, the position is the cutting point of all the circles of the anchor nodes
    /// </summary>
    public class ClusterTrilateration : RangeBasedPositioning
    {
        #region Methods
        /// <summary>
        /// Calculates the position
        /// </summary>
        /// <param name="BlindNode">The BlindNode to be positioned</param>
        /// <param name="filterMethod">The filter to use on the RSS values</param>
        /// <param name="RangingMethod">The ranging method</param>
        /// <param name="multihop">use multihop or not</param>
        /// <returns>The position of the blind node</returns>
        public static Point CalculatePosition(Node BlindNode, Node.FilterMethod filterMethod, Node.RangingMethod rangingMethod, bool multihop)
        {
            List<Point> intersectionPoints = new List<Point>();
            List<AnchorNode> AllAnchors = new List<AnchorNode>();
            List<IntersectedAnchors> anchors = new List<IntersectedAnchors>();
            IntersectedAnchors anchor = new IntersectedAnchors();
            List<int> CountOfCircles = new List<int>();

            Point center = new Point();
            int numberOfCircles = 0;

            foreach (AnchorNode AN in BlindNode.Anchors)
            {
                AN.fRSS = filterMethod(AN.RSS);
                AN.range = rangingMethod(AN.fRSS);
            }
            foreach (AnchorNode VAN in BlindNode.VirtualAnchors)
            {
                VAN.fRSS = filterMethod(VAN.RSS);
                VAN.range = rangingMethod(VAN.fRSS);
            }

                if (!multihop)
                {
                    if (BlindNode.Anchors.Count >= 3)
                    {

                        for (int i = 0; i < BlindNode.Anchors.Count - 1; i++)
                        {
                            for (int j = i + 1; j < BlindNode.Anchors.Count; j++)
                            {
                                List<Point> crossingPoints = GeometryHelper.Intersect(BlindNode.Anchors[i].posx, BlindNode.Anchors[i].posy, BlindNode.Anchors[i].range, BlindNode.Anchors[j].posx, BlindNode.Anchors[j].posy, BlindNode.Anchors[j].range);
                                if (crossingPoints != null)
                                {
                                    foreach (Point crossing in crossingPoints)
                                        intersectionPoints.Add(crossing);

                                }
                            }
                        }
                        if (intersectionPoints.Count >= 3)
                            center = Cluster(intersectionPoints, BlindNode.Anchors.Count);
                        else
                        {
                            center = null;
                        }
                    }
                    else
                        center = null;

                }
                else
                {
                    for (int i = 0; i < BlindNode.Anchors.Count - 1; i++)
                    {
                        for (int j = i + 1; j < BlindNode.Anchors.Count; j++)
                        {
                            List<Point> crossingPoints = GeometryHelper.Intersect(BlindNode.Anchors[i].posx, BlindNode.Anchors[i].posy, BlindNode.Anchors[i].range, BlindNode.Anchors[j].posx, BlindNode.Anchors[j].posy, BlindNode.Anchors[j].range);
                            if (crossingPoints != null)
                            {
                                foreach (Point crossing in crossingPoints)
                                    intersectionPoints.Add(crossing);

                            }
                        }
                    }
                    if (intersectionPoints.Count < 3)
                    {
                        intersectionPoints.Clear();

                        foreach (AnchorNode an in BlindNode.Anchors)
                            AllAnchors.Add(an);
                        foreach (AnchorNode van in BlindNode.VirtualAnchors)
                            AllAnchors.Add(van);

                        for (int i = 0; i < AllAnchors.Count - 1; i++)
                        {
                            for (int j = i + 1; j < AllAnchors.Count; j++)
                            {
                                List<Point> crossingPoints = GeometryHelper.Intersect(AllAnchors[i].posx, AllAnchors[i].posy, AllAnchors[i].range, AllAnchors[j].posx, AllAnchors[j].posy, AllAnchors[j].range);
                                if (crossingPoints != null)
                                {
                                    foreach (Point crossing in crossingPoints)
                                        intersectionPoints.Add(crossing);

                                }
                            }
                        }
                        for (int i = 0; i < AllAnchors.Count; i++)
                        {
                            numberOfCircles = 0;
                            for (int j = 0; j < AllAnchors.Count; j++)
                            {
                                
                                if (GeometryHelper.BelongTo(AllAnchors[i].posx, AllAnchors[i].posy, AllAnchors[i].range, AllAnchors[j].posx, AllAnchors[j].posy, AllAnchors[j].range))
                                    numberOfCircles++;
                            }
                            CountOfCircles.Add(numberOfCircles);
                        }
                        List<int> anchorss = CountOfCircles.FindAll(number => number == 1);

                        if (intersectionPoints.Count >= 3)
                            center = Cluster(intersectionPoints, (AllAnchors.Count - anchorss.Count) );
                        else
                        {
                            center = null;
                        }

                    }
                    else
                        center = Cluster(intersectionPoints, BlindNode.Anchors.Count);

                }
                return center;
        }
        /// <summary>
        /// Check the condition of the circles of the anchors
        /// </summary>
        /// <param name="anchors">List of the anchors</param>
        /// <returns>Return if the anchors are to far apart or in each other</returns>
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
        /// <summary>
        /// Calculates the centroid of the cluster
        /// </summary>
        /// <param name="crossings">The obtained cutting points</param>
        /// <returns></returns>
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