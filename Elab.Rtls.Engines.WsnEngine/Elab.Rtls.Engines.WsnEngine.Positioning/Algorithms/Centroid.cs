using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using DatabaseConnection;


namespace Elab.Rtls.Engines.WsnEngine.Positioning
{
    /// <summary>
    /// Centroid Localization algorithm, the position is the centroid of the anchor nodes position
    /// </summary>
    public class CentroidLocalization : ConnectivityBasedPositioning
    {
        /// <summary>
        /// Calculates the position
        /// </summary>
        /// <param name="BlindNode">The BlindNode to be positioned</param>
        /// <returns></returns>
        public static Point CalculatePosition(Node BlindNode)
        {
            Point position = new Point();

            foreach (AnchorNode anchorNode in BlindNode.Anchors)
            {
                position.x += anchorNode.posx;
                position.y += anchorNode.posy;
            }

            position.x /= BlindNode.Anchors.Count;
            position.y /= BlindNode.Anchors.Count;

            return position;
        }
    }
}