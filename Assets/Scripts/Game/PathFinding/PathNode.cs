using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    public TilePosition NodeTilePosition { get; private set; }
    public int GCost { get; set; }
    public int HCost { get; set; }
    public int FCost { get; private set; }
    public bool IsWalkable { get; set; } = true;

    public PathNode LastNode { get; set; }
    public PathNode(TilePosition tilePosition)
    {
        NodeTilePosition = tilePosition;
    }

    public override string ToString()
    {
        return NodeTilePosition.ToString();
    }

    public void CalculateFCost() => FCost = GCost + HCost;
   

    public void ResetLastNode() => LastNode = null;
}
