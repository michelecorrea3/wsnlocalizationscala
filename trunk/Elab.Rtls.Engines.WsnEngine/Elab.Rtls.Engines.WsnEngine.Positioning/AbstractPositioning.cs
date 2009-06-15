namespace Elab.Rtls.Engines.WsnEngine.Positioning
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;

    using DatabaseConnection;

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
        #region Fields

        /// <summary>
        /// X & Y coordinates
        /// </summary>
        public double x, y;

        #endregion Fields

        #region Constructors

        public Point()
        {
        }

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

        #endregion Constructors
    }

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
        #region Fields

        public static double baseLoss = 51.00;
        public static double pathLossExponent = 2.00;

        #endregion Fields

        #region Methods

        public static double AverageFilter(Queue<double> RSS)
        {
            double total = 0;

            foreach (double d in RSS)
            {
                total += d;
            }

            return (total / RSS.Count);
        }

        public static void CalibratePathloss(List<Node> AnchorNodes, Node.FilterMethod filterMethod)
        {
            double pathlossExponent = 0;

                //if (AnchorNodes.Count < 3)
                //    return;

                //determine Pd0 & d0
                //use default values for now...

                //foreach anchornode:
                foreach (Node node in AnchorNodes)
                {
                    double tempPathLossExponent = 0.00;

                    //node.SetOwnPosition();

                    foreach (AnchorNode anchorNode in node.Anchors)
                    {
                        anchorNode.fRSS = filterMethod(anchorNode.RSS);
                        double distance = Math.Pow((Math.Pow((node.Position.x - anchorNode.posx), 2) + Math.Pow((node.Position.y - anchorNode.posy), 2)), 0.5);

                        //formula does not work when the distance is 1m, then np can not be calculated
                        if (distance != 1)
                            tempPathLossExponent += (anchorNode.fRSS + 51.00) / (-10 * Math.Log10(distance));
                        else
                            tempPathLossExponent += 2.00;
                    }
                    tempPathLossExponent /= node.Anchors.Count;
                    pathlossExponent += tempPathLossExponent;
                }
                pathlossExponent /= AnchorNodes.Count;
                RangeBasedPositioning.pathLossExponent = pathlossExponent;
        }

        public static void CalibratePathlossLS(List<Node> CalibrationNodes, Node.FilterMethod filterMethod)
        {
            double pathlossExponent = 0;
            List<Node> AllAnchors = new List<Node>();
            TwoAnchors twoAnchors1 = new TwoAnchors();
            TwoAnchors twoAnchors2 = new TwoAnchors();
            List<TwoAnchors> AllCalAnchors = new List<TwoAnchors>();
            AllAnchors = CalibrationNodes;

            for (int j = 0; j < CalibrationNodes.Count; j++)
            {
                twoAnchors1.a1 = CalibrationNodes[j].WsnId;
                twoAnchors2.a2 = CalibrationNodes[j].WsnId;
                //CalibrationNodes[j].SetOwnPosition();

                for (int i = 0; i < CalibrationNodes[j].Anchors.Count; i++)
                {
                    twoAnchors1.a2 = CalibrationNodes[j].Anchors[i].nodeid;
                    twoAnchors2.a1 = CalibrationNodes[j].Anchors[i].nodeid;
                    if (!AllCalAnchors.Contains(twoAnchors1) && !AllCalAnchors.Contains(twoAnchors2))
                    {
                        AllCalAnchors.Add(twoAnchors1);
                        AllCalAnchors.Add(twoAnchors2);
                    }
                    else
                    {
                        foreach (Node mote in AllAnchors)
                        {
                            if (mote.WsnId == CalibrationNodes[j].Anchors[i].nodeid)
                                foreach (AnchorNode an in mote.Anchors)
                                    if (an.nodeid == CalibrationNodes[j].WsnId)
                                    {
                                        foreach (double d in CalibrationNodes[j].Anchors[i].RSS)
                                        {
                                            an.RSS.Enqueue(d);
                                        }
                          //              mote.Anchors.Remove(CalibrationNodes[j].Anchors[i]);
                                    }
                        }
                        foreach (Node mote in AllAnchors)
                            if (mote.WsnId == CalibrationNodes[j].WsnId)
                                mote.Anchors.Remove(CalibrationNodes[j].Anchors[i]);
                    }

                }

            }
            int totalcountt = 0;
            foreach (Node nod in AllAnchors)
                totalcountt += nod.Anchors.Count;
            if (totalcountt >= 3)
            {
                int totalcount = 0;
                int count = 0;
                foreach (Node node in AllAnchors)
                    totalcount += node.Anchors.Count;

                double[][] y = new double[totalcount][];
                double[][] x = new double[totalcount][];

                foreach (Node cal in AllAnchors)
                {

                    for (int i = 0; i < cal.Anchors.Count; i++)
                    {
                            cal.Anchors[i].fRSS = filterMethod(cal.Anchors[i].RSS);
                            double distance = Math.Pow((Math.Pow((cal.Position.x - cal.Anchors[i].posx), 2) + Math.Pow((cal.Position.y - cal.Anchors[i].posy), 2)), 0.5);
                            if (distance == 0)
                                distance = 0.1;
                            y[count] = new double[1] { cal.Anchors[i].fRSS };
                            x[count] = new double[2] { 1, -10 * Math.Log10(distance) };
                            count++;
                    }

                }
                GeneralMatrix Y = new GeneralMatrix(y);
                GeneralMatrix X = new GeneralMatrix(x);
                GeneralMatrix XT = X.Transpose();
                GeneralMatrix haakjes = XT.Multiply(X);
                GeneralMatrix inverted = haakjes.Inverse();
                GeneralMatrix XTY = XT.Multiply(Y);

                GeneralMatrix sol = inverted.Multiply(XTY);

                RangeBasedPositioning.baseLoss = -sol.Array[0][0];
                RangeBasedPositioning.pathLossExponent = sol.Array[1][0];
            }
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

            return Math.Pow(10, ((fRSS + baseRSS) / ( -10 * pathLossExponent)));
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

        public static double NoFilter(Queue<double> RSS)
        {
            List<double> RSSList = RSS.ToList();

            return RSSList[RSS.Count - 1];
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
            //if (RangeBasedPositioning.pathLossExponent < 0.01 && RangeBasedPositioning.pathLossExponent > 0.0 || RangeBasedPositioning.pathLossExponent == 0.00)
            //    RangeBasedPositioning.pathLossExponent = 0.01;
            //if (RangeBasedPositioning.pathLossExponent > -0.01 && RangeBasedPositioning.pathLossExponent < 0.0)
            //    RangeBasedPositioning.pathLossExponent = -0.01;
            return Math.Pow(10, ((fRSS + 48.155) / (-10 * 1.335)));
        }

        #endregion Methods

        #region Nested Types

        public struct TwoAnchors
        {
            #region Fields

            public string a1;
            public string a2;

            #endregion Fields
        }

        #endregion Nested Types
    }
}