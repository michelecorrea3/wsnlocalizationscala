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
    public class ExtendedTrilateration : RangeBasedPositioning
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
                    // This lus will continue with expanding and narrowing the range of the anchors
                    // until all circles cut in one or two points
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
                            else if ( (StatusCircles.Contains("In") || StatusCircles.Contains("Separated") ) && !StatusCircles.Contains("Overlap"))
                                BlindNode.Anchors[i].range *= 1.1;
                             StatusCircles.Clear();
                        }

                        // Check if all nodes intersect
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

                    // Determine crossings
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

                    // Calculate BN position with clustering
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
            return position;
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

            // first pass.
            ClosestPointPair closestPointPair = new ClosestPointPair();

            //get the two closest intersection point
            closestPointPair = GeometryHelper.ShortestDistanceBetweenCutPoints(crossings); // get struct back

            // add these points to the cluster
            cluster.Add(new Point(closestPointPair.point1.x, closestPointPair.point1.y));
            cluster.Add(new Point(closestPointPair.point2.x, closestPointPair.point2.y));
            // remove them from the intersection
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