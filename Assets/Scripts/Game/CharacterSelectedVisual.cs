using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectedVisual : MonoBehaviour
{
    [SerializeField] private Character _character;
    private MeshRenderer _meshRenderer;

    private void Awake() 
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

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
            _meshRenderer.enabled = true;
        }
        else
        {
            _meshRenderer.enabled = false;
        }
    }

    private void TurnSystemManager_OnTurnChanged(object sender, EventArgs eventArgs)
    {
        _meshRenderer.enabled = false;
    }

    private  void OnDisable()
    {
        CharacterActionManager.Instance.OnSelectedCharacter -= CharacterActionManager_OnSelectedCharacter;
    }

    
}
