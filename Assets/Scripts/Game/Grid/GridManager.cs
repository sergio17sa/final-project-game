using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : Singleton<GridManager>
{
    [SerializeField] private Transform _tileDebug;
    [SerializeField] private int _cellSize;
    [SerializeField] private int _gridHeight;
    [SerializeField] private int _gridWidth;
    private GridSystem _gridSystem;
    private void Start() 
    {
        
        _gridSystem = new GridSystem(_gridWidth, _gridHeight, _cellSize);
        
        _gridSystem.CreateDebugTiles(_tileDebug);
    }
}
