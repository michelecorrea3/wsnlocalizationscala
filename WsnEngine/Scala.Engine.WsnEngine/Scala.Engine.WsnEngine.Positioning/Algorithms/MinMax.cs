using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using DatabaseConnection;
using SocketConnection;

namespace Elab.Rtls.Engines.WsnEngine.Positioning
{
    /// <summary>
    /// Min-Max Localization algorithm with simple radio propagation model
    /// Model parameters:
    /// </summary>
    public class MinMax : RangeBasedPositioning
    {
        public static Point CalculatePosition(Node BlindNode, Node.FilterMethod filterMethod, bool multiHop)
        {
            BoundingBox BnBox, AnBox;
            Point position = new Point();
            double distance;
            Point center;

            if ((BlindNode.Anchors.Count + BlindNode.VirtualAnchors.Count) >= 3)
            {
                //comment for the test
                BlindNode.Anchors[0].fRSS = filterMethod(BlindNode.Anchors[0].RSS);
                distance = Ranging(BlindNode.Anchors[0].fRSS);

                center = new Point(BlindNode.Anchors[0].posx, BlindNode.Anchors[0].posy);

                //TEST: replace distance with constance
                AnBox = new BoundingBox(center, distance);
                BnBox = AnBox;

                for ( int i = 1; i < BlindNode.Anchors.Count; i++ )
                {   
                    //disabled for testing
                    BlindNode.Anchors[i].fRSS = filterMethod(BlindNode.Anchors[i].RSS);
                    distance = Ranging(BlindNode.Anchors[i].fRSS);

                    center = new Point(BlindNode.Anchors[i].posx, BlindNode.Anchors[i].posy);

                    AnBox = new BoundingBox(center, distance);

                    //TEST
                    //AnBox = new BoundingBox(center, 1);

                    BnBox.Xmin = Math.Max(BnBox.Xmin, AnBox.Xmin);
                    BnBox.Xmax = Math.Min(BnBox.Xmax, AnBox.Xmax);
                    BnBox.Ymin = Math.Max(BnBox.Ymin, AnBox.Ymin);
                    BnBox.Ymax = Math.Min(BnBox.Ymax, AnBox.Ymax);
                }

                if ( BlindNode.Anchors.Count < 3)
                {
                    if (BlindNode.Anchors.Count == 0)
                    {
                        BlindNode.Anchors[0].fRSS = filterMethod(BlindNode.VirtualAnchors[0].RSS);
                        distance = Ranging(BlindNode.VirtualAnchors[0].fRSS);
                        
                        center = new Point(BlindNode.VirtualAnchors[0].posx, BlindNode.VirtualAnchors[0].posy);

                        //TEST: replace distance with constance
                        AnBox = new BoundingBox(center, distance);
                        BnBox = AnBox;

                        for (int i = 1; i < BlindNode.VirtualAnchors.Count; i++)
                        {
                            //disabled for testing
                            BlindNode.Anchors[i].fRSS = filterMethod(BlindNode.VirtualAnchors[i].RSS);
                            distance = Ranging(BlindNode.VirtualAnchors[i].fRSS);

                            center = new Point(BlindNode.VirtualAnchors[i].posx, BlindNode.VirtualAnchors[i].posy);

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
                    {
                        for (int i = 0; i < BlindNode.VirtualAnchors.Count; i++)
                        {
                            //disabled for testing
                            BlindNode.Anchors[i].fRSS = filterMethod(BlindNode.VirtualAnchors[i].RSS);
                            distance = Ranging(BlindNode.VirtualAnchors[i].fRSS);

                            center = new Point(BlindNode.VirtualAnchors[i].posx, BlindNode.VirtualAnchors[i].posy);

                            AnBox = new BoundingBox(center, distance);

                            //TEST
                            //AnBox = new BoundingBox(center, 1);

                            BnBox.Xmin = Math.Max(BnBox.Xmin, AnBox.Xmin);
                            BnBox.Xmax = Math.Min(BnBox.Xmax, AnBox.Xmax);
                            BnBox.Ymin = Math.Max(BnBox.Ymin, AnBox.Ymin);
                            BnBox.Ymax = Math.Min(BnBox.Ymax, AnBox.Ymax);
                        }
                    }
                }
            }
            else
                throw new ApplicationException("Less than three anchor nodes available");

            position.x = (BnBox.Xmin + BnBox.Xmax) / 2;
            position.y = (BnBox.Ymin + BnBox.Ymax) / 2;

            return position;
        }
    }

    /// <summary>
    /// CHANGE: struct for the boundingbox in min max
    /// </summary>
    public struct BoundingBox
    {
        public double Xmin, Xmax, Ymin, Ymax;

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
    }

}