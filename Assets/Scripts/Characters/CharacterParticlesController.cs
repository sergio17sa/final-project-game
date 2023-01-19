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
            StartCoroutine(StartParticle());
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

    IEnumerator StartParticle()
    {
        characterEffects[lastParticle].gameObject.SetActive(true);
        characterEffects[lastParticle].Play();
        yield return new WaitUntil(() => !characterEffects[lastParticle].isPlaying);
        characterEffects[lastParticle].gameObject.SetActive(false);
    }

}
