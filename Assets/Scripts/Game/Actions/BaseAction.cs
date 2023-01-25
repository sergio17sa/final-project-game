using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public abstract class BaseAction : MonoBehaviour
{
    protected Character _character;
    protected bool _isActive;
    protected Action _onActionComplete;

    public static event EventHandler OnActionPerformed;

    protected virtual void Awake()
    {
        _character = GetComponent<Character>();
    }

    public abstract string GetActionName();

    public abstract void TakeAction(TilePosition tilePosition, Action onActionComplete);

    public virtual bool IsValidActionTile(TilePosition tilePosition)
    {
        List<TilePosition> validTilePositionList = GetValidActionTiles();
    
        return validTilePositionList.Contains(tilePosition);
    }

    public abstract List<TilePosition> GetValidActionTiles();

    protected void ActionStart(Action onActionComplete)
    {
        _isActive = true;
        _onActionComplete = onActionComplete;
    }

    protected void ActionComplete(BaseAction action)
    {
        _isActive = false;
        _onActionComplete();
        _character.RemainingActions.Remove(action);
        _character.AddActionTaken(action);

        OnActionPerformed?.Invoke(this, EventArgs.Empty);
    }

    public EnemyAIAction GetBestAIAction()
    {
        List<EnemyAIAction> enemyAIActions= new List<EnemyAIAction>();

        List<TilePosition> validActionTiles = GetValidActionTiles();

        foreach (TilePosition tilePosition in validActionTiles)
        {
            EnemyAIAction enemyAIAction = GetEnemyAIAction(tilePosition);

            enemyAIActions.Add(enemyAIAction);
        }

        if(enemyAIActions.Count > 0)
        {
            enemyAIActions.Sort((EnemyAIAction a, EnemyAIAction b) => b.actionValue - a.actionValue);

            return enemyAIActions[0];
        } 
        else
        {
            return null;
        }
        
    }

    public abstract EnemyAIAction GetEnemyAIAction(TilePosition tilePosition); 
}
