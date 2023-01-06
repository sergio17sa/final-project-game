using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class StatisticsUI : MonoBehaviour
{
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text numberPoints;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text victoriesNumber;
    private string temporalPlayerName = "Welcome";


    private void Start()
    {
        SetPlayerName();
    }

    public void SetPlayerName()
    {
        if (!string.IsNullOrEmpty(StatisticsManager.Instance.stats.playerName))
        {

            playerName.text = StatisticsManager.Instance.stats.playerName;
        }
        else
        {
            playerName.text = temporalPlayerName;
        }
    }

    
}
