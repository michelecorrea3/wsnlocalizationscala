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
    /// CHANGE: struct for the boundingbox in min max
    /// </summary>
    public struct BoundingBox
    {
        #region Fields

        public double Xmin, Xmax, Ymin, Ymax;

        #endregion Fields

        #region Constructors

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

        #endregion Constructors
    }

    /// <summary>
    /// Min-Max Localization algorithm with simple radio propagation model
    /// Model parameters:
    /// </summary>
    public class MinMax : RangeBasedPositioning
    {
        #region Methods

        public static Point CalculatePosition(Node BlindNode, Node.FilterMethod filterMethod, bool multiHop)
        {
            //Point position = new Point();
            //double distance;
            Point center = new Point();
            List<AnchorNode> Anchors = new List<AnchorNode>();
            List<AnchorNode> AllAnchors = new List<AnchorNode>();
            int count = 0;

            StreamWriter Log = new StreamWriter("MinMax.csv", false);

            for (int i = 0; i < BlindNode.Anchors.Count; i++)
            {
                count = 0;
                BlindNode.Anchors[i].fRSS = filterMethod(BlindNode.Anchors[i].RSS);
                for (int j = 0; j < BlindNode.Anchors.Count; j++)
                {
                    BlindNode.Anchors[j].fRSS = filterMethod(BlindNode.Anchors[j].RSS);
                    if (BelongsToAllBoxes(BlindNode.Anchors[i].posx, BlindNode.Anchors[i].posy, Ranging(BlindNode.Anchors[i].fRSS), BlindNode.Anchors[j].posx, BlindNode.Anchors[j].posy, Ranging(BlindNode.Anchors[j].fRSS)))
                        //if (BelongsToAllBoxes(BlindNode.Anchors[i].posx, BlindNode.Anchors[i].posy, 10, BlindNode.Anchors[j].posx, BlindNode.Anchors[j].posy, 10) )
                        count++;
                }
                if (count >= 3)
                    Anchors.Add(BlindNode.Anchors[i]);
            }

            if (!multiHop)
            {
                if (Anchors.Count >= 3)
                {
                    center = MinMaxCalc(Anchors, filterMethod);
                    //logger.Write(currentID + ",");
                }
                else
                {
                    center.x = 0;
                    center.y = 0;
                }
                //Log.Write("
                Log.Write(BlindNode.Anchors.Count.ToString());
                Log.Write(Anchors.Count.ToString());

            }
            else
            {

                if (Anchors.Count >= 3)
                    center = MinMaxCalc(Anchors, filterMethod);
                else if (Anchors.Count < 3)
                {
                    Anchors.Clear();
                    foreach (AnchorNode an in BlindNode.Anchors)
                        AllAnchors.Add(an);
                    foreach (AnchorNode van in BlindNode.VirtualAnchors)
                        AllAnchors.Add(van);

                    for (int i = 0; i < AllAnchors.Count; i++)
                    {
                        count = 0;
                        for (int j = 0; j < AllAnchors.Count; j++)
                        {
                            //if (BelongsToAllBoxes(AllAnchors[i].posx, AllAnchors[i].posy, Ranging(AllAnchors[i].fRSS), AllAnchors[j].posx, AllAnchors[j].posy, Ranging(Anchors[j].fRSS)))
                            if (BelongsToAllBoxes(AllAnchors[i].posx, AllAnchors[i].posy, 10, AllAnchors[j].posx, AllAnchors[j].posy, 10))
                                count++;
                        }
                        if (count >= 3)
                            Anchors.Add(AllAnchors[i]);
                    }
                    if (Anchors.Count < 3)
                    {
                        center.x = 0;
                        center.y = 0;
                        //return center;
                    }
                    else
                        center = MinMaxCalc(Anchors, filterMethod);

                }
                else
                {
                    center.x = 0;
                    center.y = 0;
                    //return center;
                }

                Log.Write(BlindNode.Anchors.Count.ToString());
                Log.Write(BlindNode.VirtualAnchors.Count.ToString());
                Log.Write(AllAnchors.Count.ToString());
            }

            //center = MinMaxCalc(Anchors, filterMethod);

            return center;
        }

        /*
        public static Point CalculatePosition(Node BlindNode, Node.FilterMethod filterMethod, bool multiHop)
        =======
        public static Point CalculatePosition(Node BlindNode, Node.FilterMethod filterMethod, Node.RangingMethod rangingMethod, bool multiHop)
        >>>>>>> .r66
        {
            BoundingBox BnBox, AnBox;
            Point position = new Point();
            double distance;
            Point center;

            //if (multiHop && (BlindNode.Anchors.Count + BlindNode.VirtualAnchors.Count) >= 3)
            //{
            //    if (BlindNode.Anchors.Count >= 1)
            //    {
            //        //comment for the test
            //        BlindNode.Anchors[0].fRSS = filterMethod(BlindNode.Anchors[0].RSS);
            //        distance = rangingMethod(BlindNode.Anchors[0].fRSS);

            //        center = new Point(BlindNode.Anchors[0].posx, BlindNode.Anchors[0].posy);

            //        //TEST: replace distance with constance
            //        AnBox = new BoundingBox(center, distance);
            //        BnBox = AnBox;
            //    }

            //    if (BlindNode.Anchors.Count >= 2)
            //    {
            //        for ( int i = 1; i < BlindNode.Anchors.Count; i++ )
            //        {
            //            //disabled for testing
            //            BlindNode.Anchors[i].fRSS = filterMethod(BlindNode.Anchors[i].RSS);
            //            distance = rangingMethod(BlindNode.Anchors[i].fRSS);

            //            center = new Point(BlindNode.Anchors[i].posx, BlindNode.Anchors[i].posy);

            //            AnBox = new BoundingBox(center, distance);

            //            //TEST
            //            //AnBox = new BoundingBox(center, 1);

            //            BnBox.Xmin = Math.Max(BnBox.Xmin, AnBox.Xmin);
            //            BnBox.Xmax = Math.Min(BnBox.Xmax, AnBox.Xmax);
            //            BnBox.Ymin = Math.Max(BnBox.Ymin, AnBox.Ymin);
            //            BnBox.Ymax = Math.Min(BnBox.Ymax, AnBox.Ymax);
            //        }
            //    }

            //    if ( BlindNode.Anchors.Count < 3 )
            //    {
            //        if (BlindNode.Anchors.Count == 0)
            //        {
            //            BlindNode.Anchors[0].fRSS = filterMethod(BlindNode.VirtualAnchors[0].RSS);
            //            distance = rangingMethod(BlindNode.VirtualAnchors[0].fRSS);

            //            center = new Point(BlindNode.VirtualAnchors[0].posx, BlindNode.VirtualAnchors[0].posy);

            //            //TEST: replace distance with constance
            //            AnBox = new BoundingBox(center, distance);
            //            BnBox = AnBox;

            //            for (int i = 1; i < BlindNode.VirtualAnchors.Count; i++)
            //            {
            //                //disabled for testing
            //                BlindNode.Anchors[i].fRSS = filterMethod(BlindNode.VirtualAnchors[i].RSS);
            //                distance = rangingMethod(BlindNode.VirtualAnchors[i].fRSS);

            //                center = new Point(BlindNode.VirtualAnchors[i].posx, BlindNode.VirtualAnchors[i].posy);

            //                AnBox = new BoundingBox(center, distance);

            //                //TEST
            //                //AnBox = new BoundingBox(center, 1);

            //                BnBox.Xmin = Math.Max(BnBox.Xmin, AnBox.Xmin);
            //                BnBox.Xmax = Math.Min(BnBox.Xmax, AnBox.Xmax);
            //                BnBox.Ymin = Math.Max(BnBox.Ymin, AnBox.Ymin);
            //                BnBox.Ymax = Math.Min(BnBox.Ymax, AnBox.Ymax);
            //            }
            //        }
            //        else
            //        {
            //            for (int i = 0; i < BlindNode.VirtualAnchors.Count; i++)
            //            {
            //                //disabled for testing
            //                BlindNode.Anchors[i].fRSS = filterMethod(BlindNode.VirtualAnchors[i].RSS);
            //                distance = rangingMethod(BlindNode.VirtualAnchors[i].fRSS);

            //                center = new Point(BlindNode.VirtualAnchors[i].posx, BlindNode.VirtualAnchors[i].posy);

            //                AnBox = new BoundingBox(center, distance);

            //                //TEST
            //                //AnBox = new BoundingBox(center, 1);

            //                BnBox.Xmin = Math.Max(BnBox.Xmin, AnBox.Xmin);
            //                BnBox.Xmax = Math.Min(BnBox.Xmax, AnBox.Xmax);
            //                BnBox.Ymin = Math.Max(BnBox.Ymin, AnBox.Ymin);
            //                BnBox.Ymax = Math.Min(BnBox.Ymax, AnBox.Ymax);
            //            }
            //        }
            //    }
            //}
            //else
            if ( !multiHop & BlindNode.Anchors.Count >= 3)
            {
                //comment for the test
                BlindNode.Anchors[0].fRSS = filterMethod(BlindNode.Anchors[0].RSS);
                distance = rangingMethod(BlindNode.Anchors[0].fRSS);

                center = new Point(BlindNode.Anchors[0].posx, BlindNode.Anchors[0].posy);

                //TEST: replace distance with constance
                AnBox = new BoundingBox(center, distance);
                BnBox = AnBox;

                for (int i = 1; i < BlindNode.Anchors.Count; i++)
                {
                    //disabled for testing
                    BlindNode.Anchors[i].fRSS = filterMethod(BlindNode.Anchors[i].RSS);
                    distance = rangingMethod(BlindNode.Anchors[i].fRSS);

                    center = new Point(BlindNode.Anchors[i].posx, BlindNode.Anchors[i].posy);

                    AnBox = new BoundingBox(center, distance);

                    //TEST
                    //AnBox = new BoundingBox(center, 1);

                    BnBox.Xmin = Math.Max(BnBox.Xmin, AnBox.Xmin);
                    BnBox.Xmax = Math.Min(BnBox.Xmax, AnBox.Xmax);
                    BnBox.Ymin = Math.Max(BnBox.Ymin, AnBox.Ymin);
                    BnBox.Ymax = Math.Min(BnBox.Ymax, AnBox.Ymax);
                }
            }
            else
                throw new ApplicationException("Less than three anchor nodes available");

            position.x = (BnBox.Xmin + BnBox.Xmax) / 2;
            position.y = (BnBox.Ymin + BnBox.Ymax) / 2;

            return position;
        }
        }
         */
        public static Point MinMaxCalc(List<AnchorNode> Anchors, Node.FilterMethod filterMethod)
        {
            BoundingBox BnBox, AnBox;
            //comment for the test
            Anchors[0].fRSS = filterMethod(Anchors[0].RSS);
            double distance = Ranging(Anchors[0].fRSS);
            Point center;

            center = new Point(Anchors[0].posx, Anchors[0].posy);

            //TEST: replace distance with constance
            //AnBox = new BoundingBox(center, 10);
            AnBox = new BoundingBox(center, Ranging(Anchors[0].fRSS));
            BnBox = AnBox;

            for (int i = 1; i < Anchors.Count; i++)
            {
                //disabled for testing
                Anchors[i].fRSS = filterMethod(Anchors[i].RSS);
                distance = Ranging(Anchors[i].fRSS);
                //TEST
                //distance = 10;

                center = new Point(Anchors[i].posx, Anchors[i].posy);

                AnBox = new BoundingBox(center, distance);

                //TEST
                //AnBox = new BoundingBox(center, 1);
                if (((BnBox.Xmin <= AnBox.Xmax) && (AnBox.Xmax <= BnBox.Xmax)) || ((BnBox.Xmin <= AnBox.Xmin) && (AnBox.Xmin <= BnBox.Xmax)))
                    if (((BnBox.Ymin <= AnBox.Ymax) && (AnBox.Ymax <= BnBox.Ymax)) || ((BnBox.Ymin <= AnBox.Ymin) && (AnBox.Ymin <= BnBox.Ymax)))
                    {
                        BnBox.Xmin = Math.Max(BnBox.Xmin, AnBox.Xmin);
                        BnBox.Xmax = Math.Min(BnBox.Xmax, AnBox.Xmax);
                        BnBox.Ymin = Math.Max(BnBox.Ymin, AnBox.Ymin);
                        BnBox.Ymax = Math.Min(BnBox.Ymax, AnBox.Ymax);
                    }
            }
            center.x = (BnBox.Xmin + BnBox.Xmax) / 2;
            center.y = (BnBox.Ymin + BnBox.Ymax) / 2;

            return center;
        }

        private static bool BelongsToAllBoxes(double x1, double y1, double r1, double x2, double y2, double r2)
        {
            Point a1, a2;
            a1 = new Point(x1, y1);
            a2 = new Point(x2, y2);
            BoundingBox bn1, bn2;
            bn1 = new BoundingBox(a1, r1);
            bn2 = new BoundingBox(a2, r2);

            if ((((bn1.Xmin <= bn2.Xmax) && (bn2.Xmax <= bn1.Xmax)) || ((bn1.Xmin <= bn2.Xmin) && (bn2.Xmin <= bn1.Xmax))) && (((bn1.Ymin <= bn2.Ymax) && (bn2.Ymax <= bn1.Ymax)) || ((bn1.Ymin <= bn2.Ymin) && (bn2.Ymin <= bn1.Ymax))))
                return true;
            else
                return false;
        }

        #endregion Methods
    }
}