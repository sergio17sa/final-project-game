using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;
    [SerializeField] List<Tile> _tiles;

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        for(int x = 0; x < _width; x++)
        {
            for(int z = 0; z < _height; z++)
            {
                int random = GetRandomDistributedIndex(15);
                Tile newTile = Instantiate(_tiles[random], new Vector3(x, 0  , z), Quaternion.identity);
                newTile.transform.parent = transform;
                newTile.name = $"Tile {x} {z}";
            }
        }
    }

    private int GetRandomDistributedIndex(int maxNumber)
    {
        int randomIndex = Random.Range(0, maxNumber);
        if (randomIndex == maxNumber - 1) return 1;
        else if (randomIndex == maxNumber - 2) return 2;
        else return 0;
    }
}
