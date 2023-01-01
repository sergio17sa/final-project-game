using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterActionManager : Singleton<CharacterActionManager>
{
     public event EventHandler OnSelectedCharacter;
    [SerializeField] private Character _selectedCharacter;
    [SerializeField] private LayerMask _characterLayerMask;

    private void Update()
    {
        TryUnitSelection();
    }

    private bool TryUnitSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, _characterLayerMask))
            {
                if(raycastHit.transform.TryGetComponent<Character>(out Character character))
                {
                    SetSelectedCharacter(character);
                    return true;
                }   
            }

            if(_selectedCharacter)
            {
                TilePosition mouseTilePosition = GridManager.Instance.GetTilePosition(MousePosition.GetPosition());

                if(_selectedCharacter.CharacterMoveAction.IsValidAction(mouseTilePosition))
                {
                    _selectedCharacter.CharacterMoveAction.SetTargetPosition(mouseTilePosition);    
                }
                
            }
            
        }
        return false;
    }

    public void SetSelectedCharacter(Character character)
    {
        _selectedCharacter = character;
        OnSelectedCharacter?.Invoke(this, EventArgs.Empty);
    }

    public Character GetSelectedCharacter()
    {
        return _selectedCharacter;
    }
}
