using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elab.Rtls.Engines.WsnEngine.Positioning
{
    static class GeometryHelper
    {
        public static List<Point> Intersect(double x1, double y1, double radius1, double x2, double y2, double radius2)
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


            //Calculating:
            // y = lx + b
            l = (2 * c - 2 * a) / (2 * b - 2 * d);
            k = (r2 - r1 - (c * c) - (d * d) + (a * a) + (b * b)) / (2 * b - 2 * d);

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
                throw new ApplicationException("No crossing");

        }

        public static ClosestPointPair ShortestDistanceBetweenCutPoints(List<Point> intersectionPoints)
        {
            List<ClosestPointPair> Distances = new List<ClosestPointPair>();
            ClosestPointPair shortestClosest = new ClosestPointPair();
            ClosestPointPair closest = new ClosestPointPair();

            shortestClosest.distance = 100000;

            for (int i = 0; i < intersectionPoints.Count - 1; i++)
            {
                for (int j = i + 1; j < intersectionPoints.Count; j++)
                {
                    closest.point1 = intersectionPoints[i];
                    closest.point2 = intersectionPoints[j];
                    closest.distance = Math.Pow((Math.Pow(2, (intersectionPoints[0].x - intersectionPoints[1].x)) + Math.Pow(2, (intersectionPoints[0].y - intersectionPoints[1].y))),0.5);
                    Distances.Add(closest);
                }
            }

            foreach (ClosestPointPair dist in Distances)
            {
                if (dist.distance < shortestClosest.distance)
                {
                    shortestClosest.distance = dist.distance;
                    shortestClosest.point1 = dist.point1;
                    shortestClosest.point2 = dist.point2;
                }
            }

            return shortestClosest;
        }

        public static Point ShortestDistanceBetweenCutPoints(List<Point> intersectionPoints, Point centroid)
        {
            List<ClosestToCentroid> DistancesToCentroid = new List<ClosestToCentroid>();
            ClosestToCentroid intersectionPoint = new ClosestToCentroid();
            ClosestToCentroid closest = new ClosestToCentroid();
            closest.distance = 100000;
            //double distance = 100000;

            for (int i = 0; i < intersectionPoints.Count - 1; i++)
            {
                intersectionPoint.intersectionPoint = intersectionPoints[i];
                intersectionPoint.distance = Math.Pow((Math.Pow(2, (intersectionPoints[i].x - centroid.x)) + Math.Pow(2, (intersectionPoints[0].y - centroid.y))),0.5);
                DistancesToCentroid.Add(intersectionPoint);
            }

            foreach (ClosestToCentroid dist in DistancesToCentroid)
            {
                if (dist.distance < closest.distance)
                {
                    closest.distance = dist.distance;
                    closest.intersectionPoint = dist.intersectionPoint;
                }
            }

            return closest.intersectionPoint;
        }

        //public static Dictionary<double, double> AnchorNodeDistance
    }

    public struct ClosestPointPair
    {
        public Point point1;
        public Point point2;
        public double distance;
    }

    public struct ClosestToCentroid
    {
        public Point intersectionPoint;
        public double distance;
    }
}
