using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectedVisual : MonoBehaviour
{
    [SerializeField] private Character _character;

     private void Start()
    {
        CharacterActionManager.Instance.OnSelectedCharacter += CharacterActionManager_OnSelectedCharacter;
        TurnSystemManager.Instance.OnTurnChanged += TurnSystemManager_OnTurnChanged;
        UpdateVisual();
    }

    private void CharacterActionManager_OnSelectedCharacter(object sender, EventArgs empty)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (CharacterActionManager.Instance.GetSelectedCharacter() == _character)
        {
            _character.characterParticles.StartParticle(1);
        }
        else
        {
            _character.characterParticles.StopParticle(1);
        }
    }

    private void TurnSystemManager_OnTurnChanged(object sender, EventArgs eventArgs)
    {
        _character.characterParticles.StopParticle(1);
    }

    public void OnDisable()
    {
        Debug.Log("Finish");
        CharacterActionManager.Instance.OnSelectedCharacter -= CharacterActionManager_OnSelectedCharacter;
        TurnSystemManager.Instance.OnTurnChanged -= TurnSystemManager_OnTurnChanged;
    }
}
