using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elab.Rtls.Engines.WsnEngine.Positioning
{
    public class WCL : ConnectivityBasedPositioning
    {
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
                weight = 1 / Math.Pow(anchorNode.fRSS, 2);
                position.x += anchorNode.posx * weight;
                position.y += anchorNode.posy * weight;

                totalWeight += weight;
            }

            position.x /= totalWeight;
            position.y /= totalWeight;

            return position;
        }
    }
}
