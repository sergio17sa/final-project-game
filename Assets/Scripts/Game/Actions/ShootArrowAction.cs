using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ShootArrowAction : BaseAction
{
    [SerializeField] private int _maxArrowRange = 4;
    private Character _targetCharacter;

    [SerializeField] private LayerMask obstaclesLayerMask;

    private void Update()
    {
        if (!_isActive) return;
    }
    public override string GetActionName()
    {
        return "Shoot";
    }

    public override List<TilePosition> GetValidActionTiles()
    {
        TilePosition characterTilePosition = _character.CharacterTilePosition;
        return GetValidShootActionTiles(characterTilePosition);
    }

    public List<TilePosition> GetValidShootActionTiles(TilePosition characterTilePosition)
    {
        List<TilePosition> validTilePositions = new List<TilePosition>();

        for (int x = -_maxArrowRange; x <= _maxArrowRange; x++)
        {
            for (int z = -_maxArrowRange; z <= _maxArrowRange; z++)
            {
                TilePosition offsetTilePosition = new TilePosition(x, z);
                TilePosition testTilePosition = characterTilePosition + offsetTilePosition;

                if (!GridManager.Instance.IsValidTilePosition(testTilePosition)) continue;


                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > _maxArrowRange) continue;


                if (!GridManager.Instance.HasCharacterOnTilePosition(testTilePosition)) continue;

                Character targetUnit = GridManager.Instance.GetCharacterAtTilePosition(testTilePosition);

                if (targetUnit.GetCharacterTeam() == _character.GetCharacterTeam()) continue;

                Vector3 characterWorldPosition = GridManager.Instance.GetWorldPosition(characterTilePosition);
                Vector3 shootDir = (targetUnit.transform.position - characterWorldPosition).normalized;

                float CharacterShoulderHeight = 1.7f;
                if (Physics.Raycast(
                        characterWorldPosition + Vector3.up * CharacterShoulderHeight,
                        shootDir,
                        Vector3.Distance(characterWorldPosition, targetUnit.transform.position),
                        obstaclesLayerMask))
                {
                    // Blocked by an Obstacle
                    continue;
                }

                validTilePositions.Add(testTilePosition);
            }
        }

        return validTilePositions;
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

    public int GetArrowRange() => _maxArrowRange;

}
