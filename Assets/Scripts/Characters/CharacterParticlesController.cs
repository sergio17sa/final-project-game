using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterParticlesController : MonoBehaviour
{
    [SerializeField]
    List <ParticleSystem> characterEffects = new List<ParticleSystem>();
    int lastParticle;

    public void CallStartParticle(int index, bool isLooping)
    {
        lastParticle = index;

        if (isLooping)
        {
            StartParticleLooping();
        }
        else
        {
            StartParticle();
        }
    }

    public void StopParticleLooping(int index)
    {
        lastParticle = index;
        characterEffects[lastParticle].Stop();
        characterEffects[lastParticle].gameObject.SetActive(false);
    }

    void StartParticleLooping()
    {
        characterEffects [lastParticle].gameObject.SetActive(true);
        characterEffects[lastParticle].Play();
    }

    void StartParticle()
    {
        characterEffects[lastParticle].gameObject.SetActive(true);
        characterEffects[lastParticle].Play();
    }

}
