using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    private GridSystem<Tile> _gridSystem;
    private TilePosition _tilePosition;
    public Character CharacterOnTile { get; set; }

    public Tile(GridSystem<Tile> gridSystem, TilePosition tilePosition)
    {
        _gridSystem = gridSystem;
        _tilePosition = tilePosition;
    }

    public override string ToString()
    {
        return _tilePosition.ToString() + "\n" + CharacterOnTile;
    }

    public bool HasCharacter() => CharacterOnTile != null;
}
