namespace Elab.Rtls.Engines.WsnEngine.Positioning
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public struct ClosestPointPair
    {
        #region Fields

        public double distance;
        public Point point1;
        public Point point2;

        #endregion Fields
    }

    public struct ClosestToCentroid
    {
        #region Fields

        public double distance;
        public Point intersectionPoint;

        #endregion Fields
    }

    public struct Error
    {
        #region Fields

        public int NumberSmall;
        public int NumberWide;

        #endregion Fields
    }

    public struct IntersectedAnchors
    {
        #region Fields

        public double r1;
        public double r2;
        public double x1;
        public double x2;
        public double y1;
        public double y2;

        #endregion Fields
    }

    static class GeometryHelper
    {
        #region Methods

        //public static Dictionary<double, double> AnchorNodeDistance
        public static bool BelongTo(double x1, double y1, double radius1, double x2, double y2, double radius2)
        {
            double distance = Math.Pow((Math.Pow((x1 - x2), 2) + Math.Pow((y1 - y2), 2)), 0.5);

            if ( (distance < Math.Abs(radius1-radius2)) || (distance > (radius1 + radius2)) )
                return false;
            else
                return true;
        }

        public static string InOrOut(double x1, double y1, double radius1, double x2, double y2, double radius2)
        {
            double distance = Math.Pow((Math.Pow((x1 - x2), 2) + Math.Pow((y1 - y2), 2)), 0.5);

            if (x1 == x2 && y1 == y2)
                return "Same";
            else if (radius1 < (distance - radius2))   // r1 + r2 < distance
                return "Separated";
            else if (radius1 > (distance + radius2))    //r1 - r2 > distance
                return "Overlap";
            else if (radius1 < radius2 - distance)      // r2 - r1 > distance
                return "In";
            else
                return "Cut";
        }

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

            double xdis = Math.Pow((x1 - x2), 2);
            double ydis = Math.Pow((y1 - y2), 2);

            //            double distance = Math.Pow(     (    Math.Pow(     (x1 - x2),2    ) + Math.Pow(   (y1 - y2),2     )   )     ,0.5);
            double distance = Math.Pow((xdis + ydis), 0.5);

            if ((distance > (radius1 + radius2)) || (distance < Math.Abs(radius1 - radius2)))
            {
                return points = null;
                //throw new ApplicationException("Distance between circles to big/small to cut");
            }
            else
            {
                if (x1 == x2)
                {
                    position1.y = ((b * b) - (d * d) - r1 + r2) / (2 * b - 2 * d);
                    position2.y = ((b * b) - (d * d) - r1 + r2) / (2 * b - 2 * d);

                    double e = 1;
                    double f = -2 * a;
                    double g = (a * a) + Math.Pow((position1.y - b), 2) - r1;

                    if (((f * f) - 4 * e * g) == 0)
                    {
                        position1.x = (-f) / (2 * e);
                        points.Add(position1);

                        return points;
                    }

                    else if (((f * f) - 4 * e * g) > 0)
                    {

                        position1.x = (-f + Math.Pow(((f * f) - 4 * e * g), 0.5) / (2 * e));
                        position2.x = (-f + Math.Pow(((f * f) - 4 * e * g), 0.5) / (2 * e));

                        points.Add(position1);
                        points.Add(position2);

                        return points;
                    }
                    else
                        return points = null;
                }
                else if (y1 == y2)
                {
                    position1.x = ((a * a) - (c * c) - r1 + r2) / (2 * a - 2 * c);
                    position2.x = ((a * a) - (c * c) - r1 + r2) / (2 * a - 2 * c);

                    double e = 1;
                    double f = -2 * b;
                    //double g = (b * b) + (Math.Pow(2.00, (position1.x - a)) - r1);
                    double g = (b * b) + (position1.x - a) * (position1.x - a) - r1;

                    if (((f * f) - 4 * e * g) == 0)
                    {
                        position1.y = (-f) / (2 * e);
                        points.Add(position1);

                        return points;
                    }

                    else if (((f * f) - 4 * e * g) > 0)
                    {

                        position1.y = (-f + Math.Pow(((f * f) - 4 * e * g), 0.5)) / (2 * e);
                        position2.y = (-f - Math.Pow(((f * f) - 4 * e * g), 0.5)) / (2 * e);

                        points.Add(position1);
                        points.Add(position2);

                        return points;
                    }
                    else
                        return points = null;
                        //throw new ApplicationException("No cutting points");

                }
                else
                {

                    //Calculatiings:
                    // y = lx + b
                    l = (2 * c - 2 * a) / (2 * b - 2 * d);
                    k = (r2 - r1 - (c * c) - (d * d) + (a * a) + (b * b)) / (2 * b - 2 * d);

                    //discriminant: y = ex^2 + fx + g
                    // D = -f +- V(f^2 -4eg)  /  2e                             // a*x^2 + b*x + c
                    double f = -2 * a + 2 * l * k - 2 * b * l;                  // b
                    double e = 1 + (l * l);                                     // a
                    double g = (a * a) + (k * k) - 2 * b * k + (b * b) - r1;    // c

                    D = (f * f) - 4 * e * g;

                    if (D == 0)
                    {
                        position1.x = (-f) / (2 * e);
                        position1.y = Math.Pow((r1 - ((position1.x - a) * (position1.x - a))), 0.5) + b;
                        points.Add(position1);

                        return points;

                    }
                    else if (D > 0)
                    {

                        position1.x = (-f + Math.Pow(((f * f) - 4 * e * g), 0.5)) / (2 * e);
                        position1.y = Math.Pow((r1 - ((position1.x - a) * (position1.x - a))), 0.5) + b;

                        position2.x = (-f - Math.Pow(((f * f) - 4 * e * g), 0.5)) / (2 * e);
                        position2.y = Math.Pow((r1 - ((position2.x - a) * (position2.x - a))), 0.5) + b;

                        points.Add(position1);
                        points.Add(position2);

                        return points;
                    }
                    else
                        return points = null;
                        //throw new ApplicationException("No cutting points");
                }

            }
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
                    closest.distance = Math.Pow((Math.Pow((intersectionPoints[i].x - intersectionPoints[j].x), 2) + Math.Pow((intersectionPoints[i].y - intersectionPoints[j].y), 2)), 0.5);
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
                intersectionPoint.distance = Math.Pow((Math.Pow((intersectionPoints[i].x - centroid.x), 2) + Math.Pow((intersectionPoints[i].y - centroid.y), 2)), 0.5);
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

        #endregion Methods
    }
}