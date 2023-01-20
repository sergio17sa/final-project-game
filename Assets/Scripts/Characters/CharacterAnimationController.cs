using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CharacterAnimationController : MonoBehaviour
{
    public Character brain;
    public Animator anim;

    [SerializeField]
    int index;
    
    public List<Sound> soundsCharacter = new List<Sound>();

    public void SetMove(float move)
    {
        if (move != 0)
        {
            anim.SetFloat("Move", move, 0.1f, Time.deltaTime);
        }
        else
        {
            anim.SetFloat("Move", move);
        }
    }

    public void SetDamage(float life)
    {
        if (life <= 0)
        {
            SoundManager.Instance?.PlayNewSound(soundsCharacter[3].name);
            brain.characterParticles.CallStartParticle(5, false);
            anim.SetBool("Die", true);

        }
        else
        {
            SoundManager.Instance?.PlayNewSound(soundsCharacter[0].name);
            brain.characterParticles.CallStartParticle(4, false);
            index = RandomIndex(brain.characterstats.maxDamageIndex);
            anim.SetInteger("Index", index);
            Debug.Log(index + " " + gameObject.name);
            anim.SetTrigger("Damage");
        }
    }

    public void SetAttack()
    {
        SoundManager.Instance?.PlayNewSound(soundsCharacter[1].name); 
        index = RandomIndex(brain.characterstats.maxAttackIndex);
        anim.SetInteger("Index", index);
        anim.SetTrigger("Attack");
    }
   
    public void SetHealing()
    {
        SoundManager.Instance?.PlayNewSound(soundsCharacter[2].name);
        
        if (brain.characterstats.characterType == CharacterType.MEDIEVAL)
        {
            anim.SetTrigger("Healing");
        }
    }

    int RandomIndex(int max)
    {
        int randomNum  = Random.Range(0, max);
        return randomNum;
    }
}
