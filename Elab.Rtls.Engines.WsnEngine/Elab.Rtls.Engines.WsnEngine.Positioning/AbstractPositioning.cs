using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using DatabaseConnection;


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
            public static double pathLossExponent
            {
                get; set;
            }

            public static double baseLoss
            {
                get; set;
            }
            
            public static double AverageFilter(Queue<double> RSS)
            {
                double total = 0;

                foreach (double d in RSS)
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

            public static void CalibratePathloss(List<Node> AnchorNodes, Node.FilterMethod filterMethod)
            {
                double pathlossExponent = 0;
                
                
                if (AnchorNodes.Count < 3)
                    return;

                //determine Pd0 & d0
                //use default values for now...

                //foreach anchornode:
                foreach (Node node in AnchorNodes)
                {
                    double tempPathLossExponent = 0.00;

                    node.SetOwnPosition();
                    
                    foreach (AnchorNode anchorNode in node.Anchors)
                    {
                        anchorNode.fRSS = filterMethod(anchorNode.RSS);
                        double distance = Math.Pow((Math.Pow((node.Position.x - anchorNode.posx), 2) + Math.Pow((node.Position.y - anchorNode.posy), 2)), 0.5);  

                        tempPathLossExponent += (anchorNode.fRSS - 51.00)/(10*Math.Log10(distance));
                    }
                    tempPathLossExponent /= node.Anchors.Count;
                    pathlossExponent += tempPathLossExponent;
                }
                pathlossExponent /= AnchorNodes.Count;
                RangeBasedPositioning.pathLossExponent = pathlossExponent;
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
            public static double DefaultRanging(double fRSS)
            {                
                const double baseRSS = 51, pathLossExponent = 2;

                return Math.Pow(10, ((-baseRSS - fRSS) / (10 * pathLossExponent)));
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
                return Math.Pow(10, ((-RangeBasedPositioning.baseLoss - fRSS) / (10 * RangeBasedPositioning.pathLossExponent)));
            }
        }

        /// <summary>
        /// Abstract base class for all connectivity-based positioning algorithms used for type safety
        /// </summary>
        public abstract class ConnectivityBasedPositioning : Positioning
        {
        }

        /// <summary>
        /// Simple struct to hold a 2D point
        /// </summary>
        public class Point
        {
            /// <summary>
            /// X & Y coordinates
            /// </summary>
            public double x, y;

            public Point()
            {}
            
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
