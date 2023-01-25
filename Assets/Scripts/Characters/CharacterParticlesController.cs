using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterParticlesController : MonoBehaviour
{
    [SerializeField]
    List <ParticleSystem> characterEffects = new List<ParticleSystem>();
    int lastParticle;

    public void StopParticle(int index)
    {
        lastParticle = index;
        characterEffects[lastParticle].Stop();
        characterEffects[lastParticle].gameObject.SetActive(false);
    }
   
    public  void StartParticle(int index)
    {
        lastParticle = index;
        characterEffects[lastParticle].gameObject.SetActive(true);
        characterEffects[lastParticle].Play();
    }

}
