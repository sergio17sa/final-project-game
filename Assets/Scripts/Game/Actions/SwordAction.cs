using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAction : BaseAction
{

    [SerializeField] private int _maxSwordDistance = 1;
    private Character _targetCharacter;

    private void Update()
    {
        if (!_isActive) return;
    }
    public override string GetActionName()
    {
        return "Slash";
    }

    public override List<TilePosition> GetValidActionTiles()
    {
        List<TilePosition> validGridPositionList = new List<TilePosition>();

        TilePosition unitGridPosition = _character.CharacterTilePosition;

        for (int x = -_maxSwordDistance; x <= _maxSwordDistance; x++)
        {
            for (int z = -_maxSwordDistance; z <= _maxSwordDistance; z++)
            {
                TilePosition offsetGridPosition = new TilePosition(x, z);
                TilePosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!GridManager.Instance.IsValidTilePosition(testGridPosition)) continue;

                if (!GridManager.Instance.HasCharacterOnTilePosition(testGridPosition)) continue;

                Character targetUnit = GridManager.Instance.GetCharacterAtTilePosition(testGridPosition);

                if (targetUnit.GetCharacterTeam() == _character.GetCharacterTeam()) continue;

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(TilePosition tilePosition, Action onActionComplete)
    {
        _targetCharacter = GridManager.Instance.GetCharacterAtTilePosition(tilePosition);

        /*Vector3 aimDir = (_targetCharacter.transform.position - _character.transform.position).normalized;

        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);*/

        ActionStart(onActionComplete);
        
        _character.GetAttack();
        _targetCharacter.GetDamage(50);

        ActionComplete(this);
    }

    public int GetSwordRange() => _maxSwordDistance;
}
