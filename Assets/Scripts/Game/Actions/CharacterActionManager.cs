using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterActionManager : Singleton<CharacterActionManager>
{
    
    [SerializeField] private Character _selectedCharacter;
    [SerializeField] private LayerMask _characterLayerMask;

    public bool isBusy;
    private BaseAction _selectedAction;

    //Events
    public event EventHandler OnSelectedCharacter;
    public event EventHandler OnSelectedActionChanged;
    public event EventHandler<bool> OnBusyChanged;
    public event EventHandler OnActionStarted;

    private void Update()
    {
        if(isBusy) return;

        if(TryUnitSelection()) return;
  
        HandleSelectedAction();
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
        }
        return false;
    }

    private void HandleSelectedAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TilePosition mouseTilePosition = GridManager.Instance.GetTilePosition(MousePosition.GetPosition());

            if (!_selectedAction.IsValidActionTile(mouseTilePosition)) return;

            //if (!_selectedCharacter.TrySpendActionPointsToTakeAction(selectedAction)) return;

            SetBusy();
            _selectedAction.TakeAction(mouseTilePosition, ClearBusy);

            OnActionStarted?.Invoke(this, EventArgs.Empty);
        }
    }

    //Getters

    public Character GetSelectedCharacter() =>  _selectedCharacter;

    public BaseAction GetSelectedAction() => _selectedAction;

    //Setters

    private void SetBusy()
    {
        isBusy = true;

        OnBusyChanged?.Invoke(this, isBusy);
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        _selectedAction = baseAction;

        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }

    private void SetSelectedCharacter(Character character)
    {
        _selectedCharacter = character;

        SetSelectedAction(character.CharacterMoveAction);

        OnSelectedCharacter?.Invoke(this, EventArgs.Empty);
    }

    private void ClearBusy()
    {
        isBusy = false;

        OnBusyChanged?.Invoke(this, isBusy);
    }
}
