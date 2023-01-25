using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacterActionsUI : MonoBehaviour
{
    [SerializeField] private Transform _actionButtonPrefab;
    [SerializeField] private Transform _actionButtonContainer;
    private List<ActionButtonUI> _actionButtons;

    [SerializeField] private int numberOfActionButtons = 3;

    private void Awake()
    {
        _actionButtons = new List<ActionButtonUI>();
    }

    private void Start()
    {
        CharacterActionManager.Instance.OnSelectedCharacter += CharacterActionManager_OnSelectedCharacter;
        CharacterActionManager.Instance.OnSelectedActionChanged += CharacterActionManager_OnSelectedActionChanged;
        TurnSystemManager.Instance.OnTurnChanged += TurnSystemManager_OnTurnChanged;
        BaseAction.OnActionPerformed += BaseAction_OnActionPerformed;

        CreateActionButtons();
    }

    private void OnDisable()
    {
        CharacterActionManager.Instance.OnSelectedCharacter -= CharacterActionManager_OnSelectedCharacter;
        CharacterActionManager.Instance.OnSelectedActionChanged -= CharacterActionManager_OnSelectedActionChanged;
        TurnSystemManager.Instance.OnTurnChanged -= TurnSystemManager_OnTurnChanged;
        BaseAction.OnActionPerformed -= BaseAction_OnActionPerformed;
    }

    private void CreateActionButtons()
    {
        for (int i = 0; i < numberOfActionButtons; i++)
        {
            Transform actionButtonTransform = Instantiate(_actionButtonPrefab, _actionButtonContainer);
            ActionButtonUI actionButton = actionButtonTransform.GetComponent<ActionButtonUI>();
            actionButton.gameObject.SetActive(false);
            _actionButtons.Add(actionButton);
        }
    }

    private void SetupActionButtons()
    {
        Character selectedCharacter = CharacterActionManager.Instance.GetSelectedCharacter();

        if (selectedCharacter.BaseActions.Length > numberOfActionButtons)
        {
            Debug.LogError("The number of actions exceed the number of buttons, please create more buttons");
            return;
        }

        for (int i = 0; i < selectedCharacter.BaseActions.Length; i++)
        {
            _actionButtons[i].SetButtonAction(selectedCharacter.BaseActions[i]);
            _actionButtons[i].gameObject.SetActive(true);
        }
    }

    private void CharacterActionManager_OnSelectedCharacter(object sender, EventArgs e)
    {
        SetupActionButtons();
        UpdateSelectedVisual();
    }

    private void UpdateSelectedVisual()
    {
        foreach (ActionButtonUI actionButtonUI in _actionButtons)
        {
            actionButtonUI.UpdateSelectedVisual();
        }
    }

    private void ClearButtons()
    {
        foreach (ActionButtonUI actionButton in _actionButtons)
        {
            actionButton.gameObject.SetActive(false);
        }
    }

    private void CharacterActionManager_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateSelectedVisual();
    }

    private void TurnSystemManager_OnTurnChanged(object sender, EventArgs e)
    {
        ClearButtons();
    }

    private void BaseAction_OnActionPerformed(object sender, EventArgs e)
    {
        UpdateSelectedVisual();
    }
}
