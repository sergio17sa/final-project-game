using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterAnimationController characterAnim;
    public CharacterStadistics characterstats;
    public float currentLife;

    public TilePosition CharacterTilePosition { get; private set;}

    public BaseAction[] BaseActions { get; private set; }

    private void Awake() 
    {
        BaseActions = GetComponents<BaseAction>();
    }
    
    private void Start()
    {
        currentLife = characterstats.initialLife;

        CharacterTilePosition = GridManager.Instance.GetTilePosition(transform.position);
        GridManager.Instance.SetCharacterOnTile(CharacterTilePosition, this);

    }

    void Update()
    {
        //GetMoviment();

        if (Input.GetKeyDown(KeyCode.A))
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
        }

        TilePosition newTilePosition = GridManager.Instance.GetTilePosition(transform.position);

        if(newTilePosition != CharacterTilePosition)
        {
            GridManager.Instance.CharacterMoveTile(this, CharacterTilePosition, newTilePosition);
            CharacterTilePosition = newTilePosition;
        }
    }

    //Funcion para RecibirDa�o
    public void GetDamage(float enemyDamage)
    {
        currentLife -= enemyDamage;
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
}
