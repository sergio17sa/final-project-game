using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TileDebug : MonoBehaviour
{
    private Tile _debugTile;
    [SerializeField] private TextMeshPro _debugTileText;
    public void SetDebugTile(Tile debugTile)
    {
        _debugTile = debugTile;
    }

    private void Update()
    {
        _debugTileText.text = _debugTile.ToString();
    }
}
