namespace Elab.Rtls.Engines.WsnEngine.Positioning
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Text;

    using DatabaseConnection;

    public class LSTrilateration : RangeBasedPositioning
    {
        #region Methods

        public static Point CalculatePosition(Node BlindNode, Node.FilterMethod filterMethod, Node.RangingMethod rangingMethod, bool multihop)
        {
            Point position = new Point();
            List<AnchorNode> AllAnchors = new List<AnchorNode>();
            double[][] y = new double[BlindNode.Anchors.Count-1][];
            double[][] x = new double[BlindNode.Anchors.Count-1][];

            foreach (AnchorNode an in BlindNode.Anchors)
            {
                an.fRSS = filterMethod(an.RSS);
                an.range = rangingMethod(an.fRSS);
            }

            if (!multihop)
            {
                if (BlindNode.Anchors.Count >= 3)
                {
                    for (int i = 1; i < BlindNode.Anchors.Count; i++)
                    {
                        y[i - 1] = new double[] { Math.Pow(BlindNode.Anchors[i].posx, 2) - Math.Pow(BlindNode.Anchors[0].posx, 2) + Math.Pow(BlindNode.Anchors[i].posy, 2) - Math.Pow(BlindNode.Anchors[0].posy, 2) - Math.Pow(BlindNode.Anchors[i].range, 2) + Math.Pow(BlindNode.Anchors[0].range, 2) };
                        x[i - 1] = new double[] { BlindNode.Anchors[i].posx - BlindNode.Anchors[0].posx, BlindNode.Anchors[i].posy - BlindNode.Anchors[0].posy };
                    }
                }
                else
                    position = null;

            }
            else
            {
                foreach (AnchorNode an in BlindNode.Anchors)
                    AllAnchors.Add(an);
                foreach (AnchorNode van in BlindNode.VirtualAnchors)
                    AllAnchors.Add(van);

                for (int i = 1; i < AllAnchors.Count; i++)
                {
                    if (AllAnchors[i].posx == AllAnchors[0].posx)
                        AllAnchors[i].posx += 0.1;
                    if (AllAnchors[i].posy == AllAnchors[0].posy)
                        AllAnchors[i].posy += 0.1;
                    y[i - 1] = new double[] { Math.Pow(AllAnchors[i].posx, 2) - Math.Pow(AllAnchors[0].posx, 2) + Math.Pow(AllAnchors[i].posy, 2) - Math.Pow(AllAnchors[0].posy, 2) - Math.Pow(AllAnchors[i].range, 2) + Math.Pow(AllAnchors[0].range, 2) };
                    x[i - 1] = new double[] { AllAnchors[i].posx - AllAnchors[0].posx, AllAnchors[i].posy - AllAnchors[0].posy };
                }
            }
            GeneralMatrix Y = new GeneralMatrix(y);
            GeneralMatrix X = new GeneralMatrix(x);
            GeneralMatrix XT = X.Transpose();
            GeneralMatrix haakjes = XT.Multiply(X);
            GeneralMatrix inverted = haakjes.Inverse(); // 2 * 2
            GeneralMatrix XTY = XT.Multiply(Y);         // 2 * 1

            GeneralMatrix sol = inverted.Multiply(XTY);
            GeneralMatrix SOL2 = sol.Multiply(0.5);
            position.x = SOL2.Array[0][0];
            position.y = SOL2.Array[1][0];
            return position;
        }

        #endregion Methods
    }
}