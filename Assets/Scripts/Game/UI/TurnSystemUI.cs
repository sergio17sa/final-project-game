using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private Button _endTurnButton;
    [SerializeField] private TextMeshProUGUI _turnCounterText;
    [SerializeField] private TextMeshProUGUI _teamTurnText;

    private void Start()
    {
        _endTurnButton.onClick.AddListener(() =>
        {
            TurnSystemManager.Instance.NextTurn();
        });

        TurnSystemManager.Instance.OnTurnChanged += TurnSystemManager_OnTurnChanged;
        CharacterActionManager.Instance.OnBusyChanged += CharacterActionManager_OnBusyChanged;

        UpdateText();
    }

    private void UpdateText()
    {
        _turnCounterText.text = $"End Turn {TurnSystemManager.Instance.TurnCounter}";
        _teamTurnText.text = $"{TurnSystemManager.Instance.GetTeamTurn()}";
    }

    private void TurnSystemManager_OnTurnChanged(object sender, EventArgs e)
    {
        if (GameManager.gameMode == GameManager.GameMode.IAMode)
        {
            if (TurnSystemManager.Instance.GetTeamTurn() != Team.Team1)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
            }
        }

        UpdateText();
    }

    private void CharacterActionManager_OnBusyChanged(object sender, bool isBusy)
    {
        if (isBusy) _endTurnButton.interactable = false;
        else _endTurnButton.interactable = true;
    }
}
