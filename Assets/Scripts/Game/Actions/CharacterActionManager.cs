using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

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

    private void Start()
    {
        TurnSystemManager.Instance.OnTurnChanged += TurnSystemManager_OnTurnChanged;
        BaseAction.OnActionPerformed += BaseAction_OnActionPerformed;
    }

    private void OnDisable()
    {
        TurnSystemManager.Instance.OnTurnChanged -= TurnSystemManager_OnTurnChanged;
        BaseAction.OnActionPerformed -= BaseAction_OnActionPerformed;
        Debug.Log("Finish3");
    }

    private void Update()
    {
        if (isBusy) return;

        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (TryUnitSelection()) return;

        HandleSelectedAction();
    }

    private bool TryUnitSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, _characterLayerMask))
            {
                if (raycastHit.transform.TryGetComponent<Character>(out Character character))
                {

                    if (character.tag == "EnemyAI") return false;

                    if (character == _selectedCharacter) return false;

                    //if (character.IsHealing) return false;

                    if (character.GetCharacterTeam() != TurnSystemManager.Instance.GetTeamTurn()) return false;

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
            if (!_selectedCharacter) return;
            if (!_selectedAction) return;

            TilePosition mouseTilePosition = GridManager.Instance.GetTilePosition(MousePosition.GetPosition());

            if (!_selectedAction.IsValidActionTile(mouseTilePosition)) return;

            if (_selectedCharacter.ActionsTaken.Contains(_selectedAction)) return;

            //if (_selectedCharacter.IsHealing) return;

            /*if(_selectedCharacter.ActionsTaken.Contains(_selectedCharacter.GetAction<HealAction>())) return;

            if (_selectedAction.GetActionName() == "Heal" && _selectedCharacter.currentLife == 100) return;

            if (_selectedAction.GetActionName() == "Heal" && _selectedCharacter.ActionsTaken.Count > 0) return;*/


            SetBusy();
            _selectedAction.TakeAction(mouseTilePosition, ClearBusy);

            OnActionStarted?.Invoke(this, EventArgs.Empty);
        }
    }

    //Getters

    public Character GetSelectedCharacter() => _selectedCharacter;

    public BaseAction GetSelectedAction() => _selectedAction;

    //Setters

    private void SetBusy()
    {
        isBusy = true;

        OnBusyChanged?.Invoke(this, isBusy);
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        if (isBusy) return;

        _selectedAction = baseAction;

        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }

    private void SetSelectedCharacter(Character character)
    {
        if(character == null) return;

        _selectedCharacter = character;
        BaseAction action;

        if (_selectedCharacter.RemainingActions.Count <= 0)
        {
            action = null;
        }
        else
        {
            action =
            character.RemainingActions.Contains(
                character.GetAction<MoveAction>())
                    ? character.GetAction<MoveAction>()
                    : character.RemainingActions[0];
        }

        SetSelectedAction(action);

        OnSelectedCharacter?.Invoke(this, EventArgs.Empty);
    }

    private void ClearBusy()
    {
        isBusy = false;

        OnBusyChanged?.Invoke(this, isBusy);
    }

    private void TurnSystemManager_OnTurnChanged(object sender, EventArgs e)
    {
        _selectedCharacter = null;
        _selectedAction = null;
    }

    private void BaseAction_OnActionPerformed(object sender, EventArgs e)
    {
        SetSelectedCharacter(_selectedCharacter);
    }

}
