using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    [Header("Setup Move and Rotation")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] float rotateSpeed = 30f;

    [SerializeField] private int _maxMoveDistance = 1;

    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;

    private List<Vector3> positionList;
    private int currentPositionIndex;

    private void Update()
    {
        Move();
    }

    public override void TakeAction(TilePosition tilePosition, Action onActionComplete)
    {
        List<TilePosition> pathTilePositionList =
            PathFinding.Instance.FindPath(_character.CharacterTilePosition, tilePosition, out int pathLength);

        currentPositionIndex = 0;
        positionList = new List<Vector3>();

        foreach (TilePosition pathTilePosition in pathTilePositionList)
        {
            positionList.Add(GridManager.Instance.GetWorldPosition(pathTilePosition));
        }

        ActionStart(onActionComplete);

        //OnStartMoving?.Invoke(this, EventArgs.Empty);
    }

    private void Move()
    {
        if (!_isActive) return;


        Vector3 targetPosition = positionList[currentPositionIndex];
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);

        float stoppingDistance = .1f;
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            _character.GetMovement(1);
        }
        else
        {
            currentPositionIndex++;
            if (currentPositionIndex >= positionList.Count)
            {
                _character.GetMovement(0);
                //OnStopMoving?.Invoke(this, EventArgs.Empty);
                ActionComplete(this);
            }
        }
    }

    public override List<TilePosition> GetValidActionTiles()
    {
        List<TilePosition> validTilePositionList = new List<TilePosition>();

        TilePosition charactetTilePosition = _character.CharacterTilePosition;

        for (int x = -_maxMoveDistance; x <= _maxMoveDistance; x++)
        {
            for (int z = -_maxMoveDistance; z <= _maxMoveDistance; z++)
            {
                TilePosition offsetTilePosition = new TilePosition(x, z);
                TilePosition testTilePosition = charactetTilePosition + offsetTilePosition;

                if (!GridManager.Instance.IsValidTilePosition(testTilePosition)) continue;

                if (charactetTilePosition == testTilePosition) continue;

                if (GridManager.Instance.HasCharacterOnTilePosition(testTilePosition)) continue;

                if (!PathFinding.Instance.IsWalkableTile(testTilePosition)) continue;


                if (!PathFinding.Instance.HasPath(charactetTilePosition, testTilePosition)) continue;


                /*int pathfindingDistanceMultiplier = 10;
                if (PathFinding.Instance.GetPathLength(charactetTilePosition, testTilePosition) > _maxMoveDistance * pathfindingDistanceMultiplier)
                {
                    // Path length is too long
                    continue;
                }*/


                validTilePositionList.Add(testTilePosition);
            }
        }

        return validTilePositionList;
    }
    public bool IsValidAction(TilePosition tilePosition)
    {
        List<TilePosition> validTilesList = GetValidActionTiles();
        return validTilesList.Contains(tilePosition);
    }

    public override string GetActionName()
    {
        return "Move";
    }
}
