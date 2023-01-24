using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NodeDebug : TileDebug
{
    private PathNode _pathNode;
    [SerializeField] private TextMeshPro _gCostText;
    [SerializeField] private TextMeshPro _hCostText;
    [SerializeField] private TextMeshPro _fCostText;
    public override void SetDebugTile(object debugTile)
    {
        base.SetDebugTile(debugTile);
        _pathNode = (PathNode)debugTile;
    }

    protected override void Update()
    {
        base.Update();
        _gCostText.text = _pathNode.GCost.ToString();
        _fCostText.text = _pathNode.FCost.ToString();
        _hCostText.text = _pathNode.HCost.ToString();
    }
}
