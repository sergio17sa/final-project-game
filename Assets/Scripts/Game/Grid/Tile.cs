using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    private GridSystem _gridSystem;
    private TilePosition _tilePosition;

    public Tile(GridSystem gridSystem, TilePosition tilePosition)
    {
        _gridSystem = gridSystem;
        _tilePosition = tilePosition;
    }

    public override string ToString()
    {
        return _tilePosition.ToString();
    }
}
