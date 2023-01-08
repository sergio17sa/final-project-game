using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAction : BaseAction
{
    private float totalSpinAmount;

    private void Update()
    {
        if (!_isActive) return;

        float spinAddAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAddAmount, 0);

        totalSpinAmount += spinAddAmount;
        if (totalSpinAmount >= 360f)
        {
            ActionComplete(this);
        }

    }
    public override void TakeAction(TilePosition gridPosition, Action OnActionComplete)
    {
        ActionStart(OnActionComplete);
        totalSpinAmount = 0f;
    }

    public override string GetActionName()
    {
        return "Spin";
    }

    public override List<TilePosition> GetValidActionTiles()
    {
        TilePosition unitGridPosition = _character.CharacterTilePosition;
        return new List<TilePosition>
        {
            unitGridPosition,
        };
    }
}
