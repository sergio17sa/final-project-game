using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public abstract class BaseAction : NetworkBehaviour
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

    public virtual int GetActionPoinsCost()
    {
        return 1;
    }

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
}
