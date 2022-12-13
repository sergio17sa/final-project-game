using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//change later
using System.Threading.Tasks;

public enum GameState { Player1Turn, Player2Turn }
public class ActionManager : Singleton<ActionManager>
{
    Unit selectedUnit = null;
    public GameState gameState;

    private void Start()
    {
        gameState = GameState.Player1Turn;
    }

    public async void MouseDown(Tile tile)
    {
        if (tile.unitOnTile == null && !selectedUnit) return;

        if (tile.unitOnTile == null && selectedUnit && tile.Walkable && tile.IsInRange)
        {
            await MoveInPath(selectedUnit, tile);
        }

        if (gameState == GameState.Player1Turn)
        {
            if (!tile.unitOnTile.CompareTag("Team1")) return;
            MovementAction(tile);
        }

        if (gameState == GameState.Player2Turn)
        {
            if (!tile.unitOnTile.CompareTag("Team2")) return;
            MovementAction(tile);
        }
    }

    public async Task MoveInPath(Unit unit, Tile targetTile)
    {
        int dist = targetTile.Visited;
        Stack<Vector3> positions = new Stack<Vector3>();

        Vector3 targetPos= targetTile.transform.position;

        positions.Push(new Vector3(targetPos.x, targetPos.y + 1, targetPos.z));

        Tile tile = targetTile;

        for (int i = 0; i < dist; i++)
        {
            tile = tile.Parent;
            positions.Push(new Vector3(tile.transform.position.x, tile.transform.position.y + 1, tile.transform.position.z));
        }

        Vector3 pos = new Vector3();
        int count = 0;
        while (count <= dist)
        {
            await Task.Delay(System.TimeSpan.FromSeconds(0.5f));
            pos = positions.Pop();

            unit.transform.position = pos;
            count++;

        }
        targetTile.SetUnit(unit);
        tile.CleanUnit(unit);
        MovementManager.Instance.CleanMovementTiles();
        
    }

    private void MovementAction(Tile tile)
    {
        if (tile.unitOnTile && !selectedUnit || tile.unitOnTile != selectedUnit && tile.unitOnTile != null  )
        {
            selectedUnit = tile.unitOnTile;
            MovementManager.Instance.CleanMovementTiles();

            MovementManager.Instance
                .SetMovementTiles(new Vector2(tile.transform.position.x, tile.transform.position.z), 3);
        }
    }
}
