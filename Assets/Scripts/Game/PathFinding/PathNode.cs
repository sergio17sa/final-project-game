using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    public TilePosition NodeTilePosition { get; private set; }
    public int GCost { get; set; }
    public int HCost { get; set; }
    public int FCost { get; private set; }

    public PathNode LastNode { get; set; }
    public PathNode(TilePosition tilePosition)
    {
        NodeTilePosition = tilePosition;
    }

    public override string ToString()
    {
        return NodeTilePosition.ToString();
    }

    public void CalculteFCost()
    {
        FCost = GCost + HCost;
    }

    public void ResetLastNode() => LastNode = null;

}
