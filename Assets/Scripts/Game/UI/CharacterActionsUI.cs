using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacterActionsUI : MonoBehaviour
{
    [SerializeField] private Transform _actionButtonPrefab;
    [SerializeField] private Transform _actionButtonContainer;
    private List<ActionButtonUI> _actionButtons;

    private void Awake() 
    {
        _actionButtons = new List<ActionButtonUI>();
    }

    private void Start() 
    {
        CharacterActionManager.Instance.OnSelectedCharacter += CharacterActionManager_OnSelectedCharacter;
        CharacterActionManager.Instance.OnSelectedActionChanged += CharacterActionManager_OnSelectedActionChanged;
        TurnSystemManager.Instance.OnTurnChanged += TurnSystemManager_OnTurnChanged;
    }
    private void CreateActionButtons()
    {
        Character selectedCharacter = CharacterActionManager.Instance.GetSelectedCharacter();

        /*foreach(Transform button in _actionButtonContainer)
        {
            Destroy(button.gameObject);
        }
        
        _actionButtons.Clear();*/

        ClearButtons();

        foreach(BaseAction baseAction in selectedCharacter.BaseActions)
        {
            Transform actionButtonTransform = Instantiate(_actionButtonPrefab, _actionButtonContainer);
            ActionButtonUI actionButton = actionButtonTransform.GetComponent<ActionButtonUI>();
            actionButton.SetButtonAction(baseAction);

            _actionButtons.Add(actionButton);
        }
    }

    private void CharacterActionManager_OnSelectedCharacter(object sender, EventArgs e)
    {
        CreateActionButtons();
        UpdateSelectedVisual();
    }

    private void UpdateSelectedVisual()
    {
        foreach(ActionButtonUI actionButtonUI in _actionButtons)
        {
            actionButtonUI.UpdateSelectedVisual();
        }
    }

    private void ClearButtons()
    {
        foreach (Transform button in _actionButtonContainer)
        {
            Destroy(button.gameObject);
        }

        _actionButtons.Clear();
    }

    private void CharacterActionManager_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateSelectedVisual();
    }

    private void TurnSystemManager_OnTurnChanged(object sender, EventArgs e)
    {
        ClearButtons();
    }
}
