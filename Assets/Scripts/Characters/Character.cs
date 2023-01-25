using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterAnimationController characterAnim;
    public CharacterParticlesController characterParticles;
    public CharacterStadistics characterStats;
    public CharacterUI characterUI;
    public float currentLife;

    public TilePosition CharacterTilePosition { get; private set; }

    public BaseAction[] BaseActions { get; private set; }
    public List<BaseAction> ActionsTaken { get; private set; }
    public List<BaseAction> RemainingActions { get; private set; }

    [SerializeField] private Team _team;

    public event EventHandler OnGetDamaged;
    public event EventHandler OnHeal;
    public static event EventHandler OnDead;

    public bool IsHealing { get; set; }
    private int _turnsHealing = 0;

    public int ActionsCounter { get; set; }

    private void Awake()
    {
        BaseActions = GetComponents<BaseAction>();

        RemainingActions = new List<BaseAction>();

        AddRemainingActions();

        ActionsTaken = new List<BaseAction>();

        ActionsCounter = BaseActions.Length;

        currentLife = characterStats.initialLife;
    }

    private void Start()
    {
        CharacterTilePosition = GridManager.Instance.GetTilePosition(transform.position);
        GridManager.Instance.SetCharacterOnTile(CharacterTilePosition, this);
        TurnSystemManager.Instance.OnTurnChanged += TurnSystemManager_OnTurnChanged;
    }
    private void OnDisable()
    {
        TurnSystemManager.Instance.OnTurnChanged -= TurnSystemManager_OnTurnChanged;
    }

    void Update()
    {
        TilePosition newTilePosition = GridManager.Instance.GetTilePosition(transform.position);

        if (newTilePosition != CharacterTilePosition)
        {
            TilePosition lastTilePosition = CharacterTilePosition;
            CharacterTilePosition = newTilePosition;
            GridManager.Instance.CharacterMoveTile(this, lastTilePosition, newTilePosition);
        }
    }

    //Funcion para RecibirDa�o
    public void GetDamage(float enemyDamage)
    {
        currentLife -= enemyDamage;

        //Send event to update UI
        OnGetDamaged?.Invoke(this, EventArgs.Empty);

        if (currentLife <= 0)
        {
            OnDead?.Invoke(this, EventArgs.Empty);
        }

        characterAnim.SetDamage(currentLife);
    }

    //Funcion para Curarse
    public void GetHealing(float healing)
    {
        currentLife += healing;
        characterParticles.StartParticle(3);
        characterAnim.SetHealing();
        OnHeal?.Invoke(this, EventArgs.Empty);

        if (currentLife > characterStats.initialLife)
        {
            currentLife = characterStats.initialLife;
        }
    }

    //Funcion para Atacar
    public void GetAttack()
    {
        characterParticles.StartParticle(2);
        characterAnim.SetAttack();
    }

    //Funcion para moverse
    public void GetMovement(float move)
    {
        characterAnim.SetMove(move);
    }

    public T GetAction<T>() where T : BaseAction
    {
        foreach (BaseAction baseAction in BaseActions)
        {
            if (baseAction is T)
            {
                return (T)baseAction;
            }
        }
        return null;
    }

    private void HealCharacter()
    {
        if (_turnsHealing <= 2)
        {
            HealAction healAction = GetAction<HealAction>();
            ActionsTaken.Add(healAction);
            _turnsHealing++;
        }
        else
        {
            _turnsHealing = 0;
            IsHealing = false;
        }
    }

    public void AddActionTaken(BaseAction action)
    {
        ActionsCounter--;
        ActionsTaken.Add(action);
    }

    private void AddRemainingActions()
    {
        for (int i = 0; i < BaseActions.Length; i++)
        {
            RemainingActions.Add(BaseActions[i]);
        }
    }

    public Team GetCharacterTeam() => _team;

    private void TurnSystemManager_OnTurnChanged(object sender, EventArgs e)
    {
        ActionsTaken.Clear();
        RemainingActions.Clear();

        ActionsCounter = BaseActions.Length;

        if (IsHealing) HealCharacter();

        AddRemainingActions();
    }

    public float GetNormalizeHealth() => currentLife / (float)characterStats.initialLife;

}
