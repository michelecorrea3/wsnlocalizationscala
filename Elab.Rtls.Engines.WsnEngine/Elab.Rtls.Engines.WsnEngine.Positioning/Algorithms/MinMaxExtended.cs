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
    /// </summary>
    public class MinMaxExtended : RangeBasedPositioning
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
        public static Point CalculatePosition(Node BlindNode, Node.FilterMethod filterMethod, Node.RangingMethod rangingMethod, bool multiHop)
        {
            Point center = new Point();
            List<AnchorNode> Anchors = new List<AnchorNode>();
            List<AnchorNode> AllAnchors = new List<AnchorNode>();
            List<int> ListOfCounts = new List<int>();
            int count = 0;
            bool AllBoxesIntersected = false;

            foreach (AnchorNode an in BlindNode.Anchors)
            {
                an.fRSS = filterMethod(an.RSS);
                double frss = rangingMethod(an.fRSS);
                an.range = frss;
            }

            if (!multiHop)
            {
                while (!AllBoxesIntersected)
                {

                    for (int i = 0; i < BlindNode.Anchors.Count; i++)
                    {
                        count = 0;
                        for (int j = 0; j < BlindNode.Anchors.Count; j++)
                        {

                            if (BelongsToAllBoxes(BlindNode.Anchors[i].posx, BlindNode.Anchors[i].posy, BlindNode.Anchors[i].range, BlindNode.Anchors[j].posx, BlindNode.Anchors[j].posy, BlindNode.Anchors[j].range))
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
                }
                else
                {
                    center = null;
                }

            }

            return center;
        }

        /// <summary>
        /// Calculates the intersection points between two circles
        /// </summary>
        /// <param name="Anchors">List of anchor nodes</param>
        /// <param name="filterMethod">The filter to use on the RSS values</param>
        /// <returns>The center of the box in common</returns>
        public static Point MinMaxCalc(List<AnchorNode> Anchors, Node.FilterMethod filterMethod, Node.RangingMethod rangingMethod)
        {
            BoundingBox BnBox, AnBox;
            double distance = Anchors[0].range;
            Point center;

            center = new Point(Anchors[0].posx, Anchors[0].posy);

            AnBox = new BoundingBox(center, distance);

            BnBox = AnBox;

            for (int i = 1; i < Anchors.Count; i++)
            {

                distance = Anchors[i].range;

                center = new Point(Anchors[i].posx, Anchors[i].posy);

                AnBox = new BoundingBox(center, distance);

                BnBox.Xmin = Math.Max(BnBox.Xmin, AnBox.Xmin);
                BnBox.Xmax = Math.Min(BnBox.Xmax, AnBox.Xmax);
                BnBox.Ymin = Math.Max(BnBox.Ymin, AnBox.Ymin);
                BnBox.Ymax = Math.Min(BnBox.Ymax, AnBox.Ymax);
            }
            center.x = (BnBox.Xmin + BnBox.Xmax) / 2;
            center.y = (BnBox.Ymin + BnBox.Ymax) / 2;

            return center;
        }
        /// <summary>
        /// Checks if the boxes are out of range
        /// </summary>
        /// <param name="x1">x coordinate of the first cicrcle</param>
        /// <param name="y1">y coordinate of the first cicrcle</param>
        /// <param name="r1">the radius of the first circle</param>
        /// <param name="x2">x coordinate of the second cicrcle</param>
        /// <param name="y2">y coordinate of the second cicrcle</param>
        /// <param name="r2">the radius of the second circle</param>
        /// <returns>Returns true if the boxes cut</returns>
        private static bool BelongsToAllBoxes(double x1, double y1, double r1, double x2, double y2, double r2)
        {
            Point a1, a2;
            a1 = new Point(x1, y1);
            a2 = new Point(x2, y2);
            BoundingBox bn1, bn2;
            bn1 = new BoundingBox(a1, r1);
            bn2 = new BoundingBox(a2, r2);

            if(bn1.Xmax < bn2.Xmin || bn1.Xmin > bn2.Xmax || bn1.Ymax < bn2.Ymin || bn1.Ymin > bn2.Ymax)
                return false;
            else
                return true;
        }

        #endregion Methods
    }
}