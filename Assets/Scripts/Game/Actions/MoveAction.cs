using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    [Header("Setup Move and Rotation")]
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _rotateSpeed = 30f;

    [SerializeField] private int _maxMoveDistance = 1;

    private List<Vector3> positionList;
    private int currentPositionIndex;

    public static event EventHandler OnStartMoving;
    public static event EventHandler OnStopMoving;

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
        OnStartMoving?.Invoke(this, EventArgs.Empty);
    }

    private void Move()
    {
        if (!_isActive)
        {
            return;
        }

        Vector3 targetPosition = positionList[currentPositionIndex];
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * _rotateSpeed);

        float stoppingDistance = .1f;
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            transform.position += moveDirection * _moveSpeed * Time.deltaTime;
            _character.GetMovement(1);
        }
        else
        {
            currentPositionIndex++;
            if (currentPositionIndex >= positionList.Count)
            {
                OnStopMoving?.Invoke(this, EventArgs.Empty);

                ActionComplete(this);

                _character.GetMovement(0);
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

                if (PathFinding.Instance.GetPathLength(charactetTilePosition, testTilePosition) > _maxMoveDistance) continue;



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

    public override EnemyAIAction GetEnemyAIAction(TilePosition tilePosition)
    {

        int targetCount = _character.GetAction<SwordAction>() ?
            _character.GetAction<SwordAction>().GetTargetsAtPosition(tilePosition) :
            _character.GetAction<RangeAttackAction>().GetTargetsAtPosition(tilePosition);

        if(targetCount == 0)
        {
            TilePosition randomTileposition = GetValidActionTiles()[UnityEngine.Random.Range(0, GetValidActionTiles().Count)];
            return new EnemyAIAction
            {
                tilePosition = randomTileposition,
                actionValue = targetCount * 10
            };
        }

        return new EnemyAIAction
        {
            tilePosition = tilePosition,
            actionValue = targetCount * 10
        };
    }
}
