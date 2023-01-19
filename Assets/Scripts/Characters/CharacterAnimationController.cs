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
        anim.SetFloat("Move", move);
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
        switch (brain.characterstats.characterType)
        {
            case CharacterType.MEDIEVAL:

                SoundManager.Instance?.PlayNewSound(soundsCharacter[2].name);
                anim.SetTrigger("Healing");
                break;

            case CharacterType.FUTURE:
                break;

        }
    }

    int RandomIndex(int max)
    {
        int randomNum  = Random.Range(0, max);
        return randomNum;
    }
}
