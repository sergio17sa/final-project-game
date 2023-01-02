using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    protected Character _character;
    protected bool _isActive;
    protected Action _onActionComplete;

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

    protected void ActionComplete()
    {
        _isActive = false;
        _onActionComplete();
    }
}
