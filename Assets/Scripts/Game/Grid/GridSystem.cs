using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem
{
    private int _width;
    private int _height;
    private float _cellSize;
    private Tile [,] _tileArray;

    public GridSystem(int width, int height, float cellSize)
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;

        _tileArray = new Tile[width, height];

        for(int x = 0; x < width; x++)
        {
            for(int z = 0; z < height; z++)
            {
                TilePosition tilePosition = new TilePosition(x, z); 
                               
                _tileArray[x,z] = new Tile(this, tilePosition);
            }
        }
    }

    /// <summary>
    /// Returns the world coordinates based on the tile position
    /// </summary>
    public Vector3 GetWorldPosition(TilePosition tileposition)
    {
        
        return new Vector3(tileposition.x, 0, tileposition.z) * _cellSize;
    }

    /// <summary>
    /// Returns the tile position based on the world coordinates
    /// </summary>
    public TilePosition GetTilePosition(Vector3 worldPosition)
    {
        return new TilePosition(
            Mathf.RoundToInt(worldPosition.x / _cellSize), 
            Mathf.RoundToInt(worldPosition.z / _cellSize)
        );
    }

    public void CreateDebugTiles(Transform debugPrefab)
    {
        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                TilePosition tilePosition = new TilePosition(x, z);

                Transform debugTileTransform = GameObject.Instantiate(debugPrefab, GetWorldPosition(tilePosition), Quaternion.identity);
                TileDebug tileDebugObject = debugTileTransform.GetComponent<TileDebug>();
                tileDebugObject.SetDebugTile(GetTile(tilePosition));
            }
        }
    }

    public Tile GetTile(TilePosition tilePosition)
    {
        return _tileArray[tilePosition.x, tilePosition.z];
    }

    public bool IsValidTilePosition(TilePosition tilePosition)
    {
        return tilePosition.x >= 0 && 
               tilePosition.z >= 0 && 
               tilePosition.x < _width && 
               tilePosition.z < _height;
    }
    
}
