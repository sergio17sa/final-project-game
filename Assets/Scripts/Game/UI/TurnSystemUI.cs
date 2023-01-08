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

        UpdateText();
    }

    private void UpdateText()
    {
        _turnCounterText.text = $"Turn number: {TurnSystemManager.Instance.TurnCounter}";
        _teamTurnText.text = $"{TurnSystemManager.Instance.GetTeamTurn()}";
    }

    private void TurnSystemManager_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateText();
    }
}
