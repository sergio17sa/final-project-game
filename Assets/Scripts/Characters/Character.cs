using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterAnimationController characterAnim;
    public CharacterParticlesController characterParticles;
    public CharacterStadistics characterstats;
    public float currentLife;

    public TilePosition CharacterTilePosition { get; private set;}

    public BaseAction[] BaseActions { get; private set; }
    public List<BaseAction> ActionsTaken { get; private set; }

    [SerializeField] private Team _team;

    public event EventHandler OnGetDamaged;

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
        //GetMoviment();

        /*if (Input.GetKeyDown(KeyCode.A))
        {
            GetAttack();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            GetDamage(20);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            GetHealing(characterstats.healing);
        }*/

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
        characterAnim.SetDamage(currentLife);
        OnGetDamaged?.Invoke(this, EventArgs.Empty);
    }

    //Funcion para Curarse
    public void GetHealing(float healing)
    {
        characterParticles.CallStartParticle(3, false); 
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
        characterParticles.CallStartParticle(2, false);
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
