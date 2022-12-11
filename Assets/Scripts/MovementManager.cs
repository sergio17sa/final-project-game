using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class MovementManager : Singleton<MovementManager>
{
    public void SetInRangeTiles(Vector2 pos, int movement/*, Color color*/)
    {
        Dictionary<Vector2, Tile> tiles = GridManager.Instance.tiles;

        //select tiles in range
        List<Tile> area = tiles
            .Where(t => (Mathf.Abs(t.Key.x - pos.x) < movement && Mathf.Abs(t.Key.y - pos.y) < movement))
            .ToDictionary(t => t.Key, t => t.Value)
            .Values
            .ToList();

        //makes tiles inRange true
        foreach (Tile tile in area)
        {
            tile.IsInRange = true;
            if(tile.RangeHighlight) tile.RangeHighlight.SetActive(true);
            //tile.RangeHightlight.GetComponent<SpriteRenderer>().color = color;
        }
    }

    public void CleanRangeHighlitedTiles()
    {
        Dictionary<Vector2, Tile> tiles = GridManager.Instance.tiles;

        //makes tiles inRange false
        foreach (Tile tile in tiles.Values)
        {
            tile.IsInRange = false;
            if (tile.RangeHighlight) tile.RangeHighlight.SetActive(false);
        }
    }

    public void SetMovementTiles(Vector2 pos, int movement/*, Color color*/)
    {

        int moveCount = 0;
        //select tiles in range
        List<Tile> area = new List<Tile>
        {
            GridManager.Instance.GetTileAtPosition(pos)
        };

        while (moveCount < movement)
        {
            foreach (Tile tile in area.ToList())
            {
                //movement BFS
                Vector2 tilePos = new Vector2(tile.transform.position.x, tile.transform.position.z);

                if (tile.Walkable == true || tilePos == pos && tile.IsCheck == false)
                {

                    // add for directions
                    if (GridManager.Instance.GetTileAtPosition(new Vector2(tilePos.x + 1, tilePos.y)) != null && GridManager.Instance.GetTileAtPosition(new Vector2(tilePos.x + 1, tilePos.y)).IsCheck == false)
                    {
                        var nextTile = GridManager.Instance.GetTileAtPosition(new Vector2(tilePos.x + 1, tilePos.y));
                        area.Add(nextTile);
                        nextTile.Parent = tile;
                        if (nextTile.Visited == -1) nextTile.Visited = moveCount + 1;
                    }

                    if (GridManager.Instance.GetTileAtPosition(new Vector2(tilePos.x - 1, tilePos.y)) != null && GridManager.Instance.GetTileAtPosition(new Vector2(tilePos.x - 1, tilePos.y)).IsCheck == false)
                    {
                        var nextTile = GridManager.Instance.GetTileAtPosition(new Vector2(tilePos.x - 1, tilePos.y));
                        area.Add(nextTile);
                        nextTile.Parent = tile;
                        if (nextTile.Visited == -1) nextTile.Visited = moveCount + 1;
                    }

                    if (GridManager.Instance.GetTileAtPosition(new Vector2(tilePos.x, tilePos.y + 1)) != null && GridManager.Instance.GetTileAtPosition(new Vector2(tilePos.x, tilePos.y + 1)).IsCheck == false)
                    {
                        var nextTile = GridManager.Instance.GetTileAtPosition(new Vector2(tilePos.x, tilePos.y + 1));
                        area.Add(nextTile);
                        nextTile.Parent = tile;
                        if (nextTile.Visited == -1) nextTile.Visited = moveCount + 1;
                    }

                    if (GridManager.Instance.GetTileAtPosition(new Vector2(tilePos.x + 1, tilePos.y - 1)) != null && GridManager.Instance.GetTileAtPosition(new Vector2(tilePos.x, tilePos.y - 1)).IsCheck == false)
                    {
                        var nextTile = GridManager.Instance.GetTileAtPosition(new Vector2(tilePos.x, tilePos.y - 1));
                        area.Add(nextTile);
                        nextTile.Parent = tile;
                        if (nextTile.Visited == -1) nextTile.Visited = moveCount + 1;
                    }


                    tile.IsCheck = true;
                }


            }
            moveCount++;
        }
        //makes tiles inRange true
        foreach (Tile tile in area.ToList())
        {
            if (tile.Walkable == true)
            {
                tile.IsInRange = true;
                tile.RangeHighlight.SetActive(true);
                //tile.RangeHighlight.GetComponent<SpriteRenderer>().color = color;


                tile.IsCheck = true;
            }
        }
    }

    public void CleanMovementTiles()
    {
        Dictionary<Vector2, Tile> tiles = GridManager.Instance.tiles;

        //makes tiles inRange false
        foreach (Tile tile in tiles.Values)
        {
            tile.IsInRange = false;
             
            if(tile.RangeHighlight) tile.RangeHighlight.SetActive(false);

            tile.IsCheck = false;
            tile.Parent = tile;
            tile.Visited = -1;
        }

    }

    /*public bool CheckRangeTiles()
    {
        Dictionary<Vector2, Tile> tiles = GridManager.Instance._tiles;
        foreach (Tile tile in tiles.Values)
        {
            if (tile.inRange == true)
            {
                if (tile.OccupiedUnit != null && tile.OccupiedUnit.Faction == Faction.Enemy)
                {
                    return true;
                }
            }

        }
        return false;
    }*/
}
