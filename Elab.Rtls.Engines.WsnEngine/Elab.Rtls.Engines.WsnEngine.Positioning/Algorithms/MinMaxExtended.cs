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
    /// Min-Max Localization algorithm with simple radio propagation model
    /// Model parameters:
    /// </summary>
    public class MinMaxExtended : RangeBasedPositioning
    {
        #region Methods

        public static Point CalculatePosition(Node BlindNode, Node.FilterMethod filterMethod, Node.RangingMethod rangingMethod, bool multiHop)
        {
            //Point position = new Point();
            //double distance;
            Point center = new Point();
            List<AnchorNode> Anchors = new List<AnchorNode>();
            List<AnchorNode> AllAnchors = new List<AnchorNode>();
            List<int> ListOfCounts = new List<int>();
            int count = 0;
            bool AllBoxesIntersected = false;

            //StreamWriter Log = new StreamWriter("ExtendedMinMax.csv", false);

            foreach (AnchorNode an in BlindNode.Anchors)
            {
                an.fRSS = filterMethod(an.RSS);
                double frss = rangingMethod(an.fRSS);
                //an.range = rangingMethod(an.fRSS);
                an.range = frss;
                //TEST
                //an.range = 10;
            }

            if (!multiHop)
            {
                while (!AllBoxesIntersected)
                {

                    for (int i = 0; i < BlindNode.Anchors.Count; i++)
                    {
                        count = 0;
                        //BlindNode.Anchors[i].fRSS = filterMethod(BlindNode.Anchors[i].RSS);
                        //BlindNode.Anchors[i].range = Ranging(BlindNode.Anchors[i].fRSS);
                        //BlindNode.Anchors[i].range = 10;
                        for (int j = 0; j < BlindNode.Anchors.Count; j++)
                        {
                            //BlindNode.Anchors[j].fRSS = filterMethod(BlindNode.Anchors[j].RSS);
                            //BlindNode.Anchors[j].range = Ranging(BlindNode.Anchors[j].fRSS);
                            if (BelongsToAllBoxes(BlindNode.Anchors[i].posx, BlindNode.Anchors[i].posy, BlindNode.Anchors[i].range, BlindNode.Anchors[j].posx, BlindNode.Anchors[j].posy, BlindNode.Anchors[j].range))
                                //if (BelongsToAllBoxes(BlindNode.Anchors[i].posx, BlindNode.Anchors[i].posy, 10, BlindNode.Anchors[j].posx, BlindNode.Anchors[j].posy, 10) )
                                count++;
                        }

                        ListOfCounts.Add(count);
                    }
                    if (ListOfCounts.Average() != BlindNode.Anchors.Count)
                    {
                        ListOfCounts.Clear();
                        foreach (AnchorNode an in BlindNode.Anchors)
                            an.range *= 1.1;
                    }
                    else
                        AllBoxesIntersected = true;
                }
                if (BlindNode.Anchors.Count >= 3)
                {
                    center = MinMaxCalc(BlindNode.Anchors, filterMethod, rangingMethod);
                    //logger.Write(currentID + ",");
                }
                else
                {
                    center = null;
                }
                //Log.Write("
                //Log.Write(BlindNode.Anchors.Count.ToString());
                //Log.Write(Anchors.Count.ToString());

            }
                /*
            else
            {

                if (Anchors.Count >= 3)
                    center = MinMaxCalc(Anchors, filterMethod, rangingMethod);
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
                            if (BelongsToAllBoxes(AllAnchors[i].posx, AllAnchors[i].posy, AllAnchors[i].range, AllAnchors[j].posx, AllAnchors[j].posy, Anchors[j].range))
                                //                          if (BelongsToAllBoxes(AllAnchors[i].posx, AllAnchors[i].posy, 10, AllAnchors[j].posx, AllAnchors[j].posy, 10))
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
                        center = MinMaxCalc(Anchors, filterMethod, rangingMethod);

                }
                else
                {
                    center.x = 0;
                    center.y = 0;
                    //return center;
                }

                //Log.Write(BlindNode.Anchors.Count.ToString());
                //Log.Write(BlindNode.VirtualAnchors.Count.ToString());
                //Log.Write(AllAnchors.Count.ToString());
            }
            */
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
        public static Point MinMaxCalc(List<AnchorNode> Anchors, Node.FilterMethod filterMethod, Node.RangingMethod rangingMethod)
        {
            BoundingBox BnBox, AnBox;
            //comment for the test
            //Anchors[0].fRSS = filterMethod(Anchors[0].RSS);
            //double distance = (Anchors[0].fRSS);
            double distance = Anchors[0].range;
            //TEST
            //double distance = 10;
            Point center;

            center = new Point(Anchors[0].posx, Anchors[0].posy);

            //TEST: replace distance with constance
            AnBox = new BoundingBox(center, distance);
            //AnBox = new BoundingBox(center, Ranging(Anchors[0].fRSS));
            BnBox = AnBox;

            for (int i = 1; i < Anchors.Count; i++)
            {
                //disabled for testing
                //Anchors[i].fRSS = filterMethod(Anchors[i].RSS);
                //distance = rangingMethod(Anchors[i].fRSS);
                distance = Anchors[i].range;
                //TEST
                //distance = 10;

                center = new Point(Anchors[i].posx, Anchors[i].posy);

                AnBox = new BoundingBox(center, distance);

                //TEST
                //AnBox = new BoundingBox(center, 1);
                BnBox.Xmin = Math.Max(BnBox.Xmin, AnBox.Xmin);
                BnBox.Xmax = Math.Min(BnBox.Xmax, AnBox.Xmax);
                BnBox.Ymin = Math.Max(BnBox.Ymin, AnBox.Ymin);
                BnBox.Ymax = Math.Min(BnBox.Ymax, AnBox.Ymax);
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

            //if ((((bn1.Xmin <= bn2.Xmax) && (bn2.Xmax <= bn1.Xmax)) || ((bn1.Xmin <= bn2.Xmin) && (bn2.Xmin <= bn1.Xmax))) && (((bn1.Ymin <= bn2.Ymax) && (bn2.Ymax <= bn1.Ymax)) || ((bn1.Ymin <= bn2.Ymin) && (bn2.Ymin <= bn1.Ymax))))
            if(bn1.Xmax < bn2.Xmin || bn1.Xmin > bn2.Xmax || bn1.Ymax < bn2.Ymin || bn1.Ymin > bn2.Ymax)
                return false;
            else
                return true;
        }

        #endregion Methods
    }
}