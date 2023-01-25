using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType { Shoot, Spell }

public class RangeAttackAction : BaseAction
{
    [SerializeField] private int _maxAttackRange = 4;
    [SerializeField] private LayerMask obstaclesLayerMask;
    [SerializeField] private Transform _projectilePrefab;
    [SerializeField] private Transform _spawnProjectilePosition;
    [SerializeField] private GameObject projectileHitEffect;

    private Character _targetCharacter;
    private Transform _projectile;

    [SerializeField] private AttackType _attackType;

    private enum State
    {
        Aiming,
        Shooting,
        Load,
        Rest
    }

    //States duration
    private float _aimDuration = 0.5f;
    [SerializeField] private float _loadDuration = 2f;
    private float _shootDuration = 0.5f;
    private float _restDuration = 0.5f;

    //State machine varibles
    private State state;
    private float _stateTimer;
    private bool _canShoot;
    private bool _canLoad;

    protected override void Awake()
    {
        base.Awake();

        _projectile = Instantiate(_projectilePrefab, _spawnProjectilePosition.position, Quaternion.identity);
        _projectile.SetParent(_spawnProjectilePosition);

        _projectile.gameObject.SetActive(false);
    }

    private void Start()
    {
        _projectile.GetComponent<ProjectileBehaviour>().OnReachTarget += ProjectileBehaviour_OnReachTarget;
    }

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
                if (_stateTimer <= 0) TransitionToLoadState();
                break;
            case State.Load:
                if (_canLoad) LoadAttack();
                if (_stateTimer <= 0) TransitionToShootingState();
                break;
            case State.Shooting:
                if (_canShoot) LaunchAttack();
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

    private void LoadAttack()
    {
        //Trigger animation
        _character.GetAttack();

        _projectile.gameObject.SetActive(true);

        _canLoad = false;
    }

    private void LaunchAttack()
    {
        ProjectileBehaviour projectileBehaviour = _projectile.GetComponent<ProjectileBehaviour>();

        Vector3 targetPositionOffset = _targetCharacter.transform.position;
        targetPositionOffset.y = _spawnProjectilePosition.position.y;

        projectileBehaviour.SetShoot(targetPositionOffset, _canShoot);

        _canShoot = false;
    }

    private void TransitionToLoadState()
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

    public override string GetActionName()
    {
        return "Attack";
    }

    public override List<TilePosition> GetValidActionTiles()
    {
        TilePosition characterTilePosition = _character.CharacterTilePosition;
        return GetValidAttackTiles(characterTilePosition);
    }

    private List<TilePosition> GetValidAttackTiles(TilePosition characterTilePosition)
    {
        List<TilePosition> validTilePositions = new List<TilePosition>();

        for (int x = -_maxAttackRange; x <= _maxAttackRange; x++)
        {
            for (int z = -_maxAttackRange; z <= _maxAttackRange; z++)
            {
                // Get the tiles around the player
                TilePosition offsetTilePosition = new TilePosition(x, z);
                TilePosition testTilePosition = characterTilePosition + offsetTilePosition;

                //Ignore tiles out of bounds 
                if (!GridManager.Instance.IsValidTilePosition(testTilePosition)) continue;

                //Ignore tiles that does not have charactes on it
                if (!GridManager.Instance.HasCharacterOnTilePosition(testTilePosition)) continue;

                //Ignore targets that are on the same team
                Character targetUnit = GridManager.Instance.GetCharacterAtTilePosition(testTilePosition);
                if (targetUnit.GetCharacterTeam() == _character.GetCharacterTeam()) continue;

                //Choose type of scope according to the type of the attack
                if (_attackType == AttackType.Shoot)
                {
                    int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                    if (testDistance > _maxAttackRange) continue;

                    if(IsTargetBlocked(targetUnit, characterTilePosition, validTilePositions)) continue;

                    validTilePositions.Add(testTilePosition);
                }

                if (_attackType == AttackType.Spell)
                {
                    if (testTilePosition.x == characterTilePosition.x || testTilePosition.z == characterTilePosition.z)
                    {
                        if (IsTargetBlocked(targetUnit, characterTilePosition, validTilePositions)) continue;

                        validTilePositions.Add(testTilePosition);
                    }
                }
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

    private void ProjectileBehaviour_OnReachTarget(object sender, EventArgs e)
    {
        _targetCharacter.GetDamage(_character.characterStats.powerAttack);
        _projectile.SetParent(_spawnProjectilePosition);
        _projectile.localPosition = new Vector3(0, 0, 0);
        _projectile.gameObject.SetActive(false);

        Vector3 spawnPosition = new Vector3(_targetCharacter.gameObject.transform.position.x, _targetCharacter.gameObject.transform.position.y + 2.5f, _targetCharacter.gameObject.transform.position.z);
        Instantiate(projectileHitEffect, spawnPosition, Quaternion.identity);
    }

    public int GetAttackRange() => _maxAttackRange;
    public AttackType GetAttackType() => _attackType;

    public int GetTargetsAtPosition(TilePosition tilePosition)
    {
        return GetValidAttackTiles(tilePosition).Count;
    }

    private bool IsTargetBlocked(Character targetUnit, TilePosition characterTilePosition, List<TilePosition> validTilePositions)
    {
        Vector3 characterWorldPosition = GridManager.Instance.GetWorldPosition(characterTilePosition);
        Vector3 shootDir = (targetUnit.transform.position - characterWorldPosition).normalized;

        float CharacterShoulderHeight = 1.7f;

        if (Physics.Raycast(
            characterWorldPosition + Vector3.up * CharacterShoulderHeight,
            shootDir,
            out RaycastHit hit,
            Vector3.Distance(characterWorldPosition, targetUnit.transform.position),
            obstaclesLayerMask))
        {

            int characterLayer = LayerMask.NameToLayer("Character");
            GameObject hitGameObject = hit.collider.gameObject;

            if (hitGameObject.layer == characterLayer && 
                hitGameObject.GetComponent<Character>().GetCharacterTeam() != _character.GetCharacterTeam())
            {
                validTilePositions.Add(hit.collider.gameObject.GetComponent<Character>().CharacterTilePosition);  
            }

            return true;
        }

        return false;
    }

    public override EnemyAIAction GetEnemyAIAction(TilePosition tilePosition)
    {
        return new EnemyAIAction
        {
            tilePosition = tilePosition,
            actionValue = 100,
        };
    }
}
