using System;
namespace Elab.Rtls.Engines.WsnEngine.Positioning
{
    interface INode
    {
        //Adds an Anchor Node to this Node
        void AddAnchor(string AnchorWsnId, double RSS);

        //Used by the positioningalgorithms to get list with the anchor nodes
        System.Collections.Generic.List<AnchorNode> Anchors { get; }

        //Used to check which 
        string WsnIdProperty { get; }
    }
}
