using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManager : Singleton<GridManager>
{
    [SerializeField] private int _width, _height;
    [SerializeField] List<Tile> _tileTypes;
    public Dictionary<Vector2, Tile> tiles;

    public bool CanSpawnUnits { get; set; }

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        tiles = new Dictionary<Vector2, Tile>();

        for(int x = 0; x < _width; x++)
        {
            for(int z = 0; z < _height; z++)
            {
                int random = GetRandomDistributedIndex(20);
                Tile newTile = Instantiate(_tileTypes[random], new Vector3(x, 0, z), Quaternion.identity);
                newTile.transform.parent = transform;
                newTile.name = $"Tile {x} {z}";

                tiles.Add(new Vector2(x, z), newTile);
            }
        }
        CanSpawnUnits = true;

        /*foreach(KeyValuePair<Vector2, Tile> i in _tiles)
        {
            Debug.Log($"key: {i.Key}, Value: {i.Value}");
        }*/
    }

    //Move to helpers!
    private int GetRandomDistributedIndex(int maxNumber)
    {
        int randomIndex = Random.Range(0, maxNumber);
        if (randomIndex == maxNumber - 1) return 1;
        else if (randomIndex == maxNumber - 2) return 2;
        else return 0;
    }

    public Tile RandomLeftMapTile()
    {
       return tiles.Where(tile => tile.Key.y > _height / 2 && tile.Value.Walkable).OrderBy(tile => Random.value).First().Value;
    }

    public Tile RandomRightMapTile()
    {
        return tiles.Where(tile => tile.Key.y < _height / 4 && tile.Value.Walkable).OrderBy(tile => Random.value).First().Value;
    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }
}
