using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAction : BaseAction
{
    private void Update()
    {
        if (!_isActive) return;
    }
    public override void TakeAction(TilePosition gridPosition, Action OnActionComplete)
    {
        ActionStart(OnActionComplete);
        HealCharacter();
    }

    private void HealCharacter()
    {
        _character.GetHealing(_character.characterStats.healing);
        _character.IsHealing = true;
        ActionComplete(this);
    }

    public override string GetActionName()
    {
        return "Heal";
    }

    public override List<TilePosition> GetValidActionTiles()
    {
        TilePosition unitTilePosition = _character.CharacterTilePosition;
        return new List<TilePosition>
        {
            unitTilePosition,
        };
    }

    public override EnemyAIAction GetEnemyAIAction(TilePosition gridPosition)
    {
        return new EnemyAIAction
        {
            tilePosition = gridPosition,
            actionValue = 0,
        };
    }

}
