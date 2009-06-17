namespace Elab.Rtls.Engines.WsnEngine.Positioning
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    /// <summary>
    /// Weighted Centroid Localization algorithm, the position is the centroid of the weighted anchor nodes position
    /// </summary>
    public class WCL : ConnectivityBasedPositioning
    {
        #region Methods
        /// <summary>
        /// Calculates the position
        /// </summary>
        /// <param name="BlindNode">The BlindNode to be positioned</param>
        /// <returns>The position of the blind node</returns>
        public static Point CalculatePosition(Node BlindNode, Node.FilterMethod filterMethod)
        {
            Point position = new Point();
            double weight;
            double totalWeight = 0;

            foreach (AnchorNode an in BlindNode.Anchors)
            {
                an.fRSS = filterMethod(an.RSS);
            }

            foreach (AnchorNode anchorNode in BlindNode.Anchors)
            {
                weight = 1 / Math.Pow(anchorNode.fRSS, 3);
                position.x += anchorNode.posx * weight;
                position.y += anchorNode.posy * weight;

                totalWeight += weight;
            }

            position.x /= totalWeight;
            position.y /= totalWeight;

            return position;
        }

        #endregion Methods
    }
}