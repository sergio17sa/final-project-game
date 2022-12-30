using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : Singleton<GridManager>
{
    [SerializeField] private Transform _tileDebug;
    private GridSystem _gridSystem;
    private void Start() 
    {
        
        _gridSystem = new GridSystem(10, 10, 2);
        
        _gridSystem.CreateDebugTiles(_tileDebug);
    }
}
