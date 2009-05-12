using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using DatabaseConnection;
using SocketConnection;

namespace Elab.Rtls.Engines.WsnEngine.Positioning
{
    public class ClusterTrilateration : RangeBasedPositioning
    {
        public static Point CalculatePosition(Node BlindNode, Node.FilterMethod filterMethod)
        {
            List<Point> intersectionPoints = new List<Point>();

            if (BlindNode.Anchors.Count >= 3)
            {
                foreach (AnchorNode AN in BlindNode.Anchors)
                {
                    AN.fRSS = filterMethod(AN.RSS);
                }

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
                        foreach (Point crossing in Intersect(BlindNode.Anchors[i].posx, BlindNode.Anchors[i].posy, 1.41, BlindNode.Anchors[j].posx, BlindNode.Anchors[j].posy, 1.41))
                        {
                            intersectionPoints.Add(crossing);
                        }
                    }
                }
                return Cluster(intersectionPoints, BlindNode.Anchors.Count);

            }
            else
                throw new ApplicationException("Less than three anchor nodes available");

        }

        private static Point Cluster(List<Point> crossings, int anchors)
        {
            List<Point> cluster = new List<Point>();
            Point centroid = new Point();

            //first pass.
            ClosestPointPair closestPointPair = new ClosestPointPair();

            //get the two closest intersection point
            closestPointPair = ShortestDistanceBetweenCutPoints(crossings); // get struct back
            
            //add these points to the cluster
            cluster.Add(new Point(closestPointPair.point1.x, closestPointPair.point1.y));
            cluster.Add(new Point(closestPointPair.point2.x, closestPointPair.point2.y));
            //remove them from the intersection
            crossings.Remove(closestPointPair.point1);
            crossings.Remove(closestPointPair.point2);

            centroid.x = (cluster[0].x + cluster[1].x) / 2;
            centroid.y = (cluster[0].y + cluster[1].y) / 2;

            while (cluster.Count < anchors && crossings.Count > 0 )
            {
                Point closestPoint = ShortestDistanceBetweenCutPoints(crossings, centroid);

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
    }
}