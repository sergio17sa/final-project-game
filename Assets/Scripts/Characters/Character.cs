using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterAnimationController characterAnim;
    public CharacterStadistics characterstats;
    public float currentLife;

    public MoveAction moveAction {get; private set; }

    private void Awake() 
    {
        moveAction = GetComponent<MoveAction>();
    }
    
    private void Start()
    {
        currentLife = characterstats.initialLife;
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
}
