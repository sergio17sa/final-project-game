using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAction : BaseAction
{

    [SerializeField] GameObject projectileHitEffect;
    [SerializeField] private int _maxSwordDistance = 1;
    private Character _targetCharacter;

    private enum State
    {
        Aiming,
        Slash,
        Rest
    }

    //States duration
    private float _aimDuration = 0.5f;
    [SerializeField] private float _slashDuration = 0.5f;
    private float _restDuration = 0.1f;

    //State machine varibles
    private State state;
    private float _stateTimer;
    private bool _canAtack;

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
                if (_stateTimer <= 0) TransitionToSlashState();
                break;
            case State.Slash:
                if(_canAtack) Slash();
                if (_stateTimer <= 0) TransitionToRestState();
                break;
            case State.Rest:
                _targetCharacter.GetDamage(GetComponent<Character>().characterstats.powerAttack);
                ActionComplete(this);
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

    private void Slash()
    {
        //Trigger animation
        _character.GetAttack();
        Instantiate(projectileHitEffect, _targetCharacter.gameObject.transform.position, Quaternion.identity);
        _canAtack = false;
    }

    private void TransitionToSlashState()
    {
        state = State.Slash;
        _stateTimer = _slashDuration;
    }

    private void TransitionToRestState()
    {
        state = State.Rest;
        _stateTimer = _restDuration;
    }
    public override string GetActionName()
    {
        return "Attack";
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

        state = State.Aiming;

        _stateTimer = _aimDuration;
        _canAtack = true;

        ActionStart(onActionComplete);
         
    }

    public int GetSwordRange() => _maxSwordDistance;
}
