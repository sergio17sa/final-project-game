using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootArrowAction : BaseAction
{
    [SerializeField] private int _maxArrowRange = 4;
    [SerializeField] private LayerMask obstaclesLayerMask;

    private Character _targetCharacter;

    private enum State
    {
        Aiming,
        Shooting,
        Load,
        Rest
    }

    //States duration
    private float _aimDuration = 0.5f;
    private float _loadDuration = 2.5f;
    private float _shootDuration = 0.5f;
    private float _restDuration = 0.5f;

    //State machine varibles
    private State state;
    private float _stateTimer;
    private bool _canShoot;
    private bool _canLoad;

    private void Update()
    {
        if (!_isActive) return;

        UpdateStateMachine();
    }

    private void UpdateStateMachine()
    {
        _stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.Aiming:
                Aim();
                if (_stateTimer <= 0) TransitionToReloadState();
                break;
            case State.Load:
                if(_canLoad) LoadBow();
                if (_stateTimer <= 0) TransitionToShootingState();
                break;
            case State.Shooting:
                if (_canShoot) Shoot();
                if (_stateTimer <= 0) TransitionToRestState();
                break;
            case State.Rest:
                if (_stateTimer <= 0) ActionComplete(this);
                break;
        }
    }

    private void Aim()
    {
        Vector3 aimDir = (_targetCharacter.transform.position - _character.transform.position).normalized;

        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);
    }

    private void LoadBow()
    {
        //Trigger animation
        _character.GetAttack();
        _canLoad = false;
    }

    private void TransitionToReloadState()
    {
        state = State.Load;
        _stateTimer = _loadDuration;
    }

    private void TransitionToShootingState()
    {
        state = State.Shooting;
        _stateTimer = _shootDuration;
    }

    private void TransitionToRestState()
    {
        state = State.Rest;
        _stateTimer = _restDuration;
    }

    private void Shoot()
    {
        _targetCharacter.GetDamage(50);

        _canShoot = false;
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

        state = State.Aiming;
        _stateTimer = _aimDuration;

        _canShoot = true;
        _canLoad = true;

        ActionStart(onActionComplete);
    }

    public int GetArrowRange() => _maxArrowRange;

}
