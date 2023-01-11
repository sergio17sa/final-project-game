using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterAnimationController characterAnim;
    public CharacterStadistics characterstats;
    public float currentLife;

    public TilePosition CharacterTilePosition { get; private set;}

    public BaseAction[] BaseActions { get; private set; }
    public List<BaseAction> ActionsTaken { get; private set; }

    [SerializeField] private Team _team;

    public event EventHandler OnGetDamaged;
    public static event EventHandler OnDead;

    private void Awake() 
    {
        BaseActions = GetComponents<BaseAction>();
        ActionsTaken = new List<BaseAction>();

        currentLife = characterstats.initialLife;
    }
    
    private void Start()
    {
        CharacterTilePosition = GridManager.Instance.GetTilePosition(transform.position);
        GridManager.Instance.SetCharacterOnTile(CharacterTilePosition, this);

        TurnSystemManager.Instance.OnTurnChanged += TurnSystemManager_OnTurnChanged;
    }

    void Update()
    {
        TilePosition newTilePosition = GridManager.Instance.GetTilePosition(transform.position);

        if(newTilePosition != CharacterTilePosition)
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
        characterAnim.SetHealing();

        if (currentLife > characterstats.initialLife)
        {
            currentLife = characterstats.initialLife;
        }
    }
    
    //Funcion para Atacar
    public void GetAttack()
    {
        characterAnim.SetAttack();
    }
    
    //Funcion para moverse
    public void GetMovement(float move)
    {
        characterAnim.SetMove(move);
    }

    public T GetAction<T>() where T : BaseAction
    {
        foreach(BaseAction baseAction in BaseActions)
        {
            if(baseAction is T)
            {
                return (T)baseAction;
            }
        }
        return null;
    }

    public void AddActionTaken(BaseAction action) => ActionsTaken.Add(action);

    public Team GetCharacterTeam() => _team;

    private void TurnSystemManager_OnTurnChanged(object sender, EventArgs e)
    {
        ActionsTaken.Clear();
    }

    public float GetNormalizeHealth() => currentLife/ (float)characterstats.initialLife;

}
