using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TileDebug : MonoBehaviour
{
    private object _debugTile;
    [SerializeField] private TextMeshPro _debugTileText;
    public virtual void SetDebugTile(object debugTile)
    {
        _debugTile = debugTile;
    }

    protected virtual void Update()
    {
        _debugTileText.text = _debugTile.ToString();
    }
}
