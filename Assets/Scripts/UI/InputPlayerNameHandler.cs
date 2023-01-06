using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class InputPlayerNameHandler : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputPlayerName;
    [SerializeField] private GameObject panelInputPlayerName;
    [SerializeField] private GameObject paneljoinBtn;
    [SerializeField] private StatisticsUI statisticsUI;

    private void Start()
    {
        HideInputPlayerName();
    }

    public void HideInputPlayerName()
    {
        if (String.IsNullOrEmpty(StatisticsManager.Instance.stats.playerName))
        {
            panelInputPlayerName.SetActive(true);
            paneljoinBtn.SetActive(false);
        }
        else
        {
            panelInputPlayerName.SetActive(false);
            paneljoinBtn.SetActive(true);
        }
    }

    public void SubmitPlayerName()
    {
        StatisticsManager.Instance.stats.playerName = inputPlayerName.text;
        StatisticsManager.Instance.SavePlayerData();
        HideInputPlayerName();
        statisticsUI.SetPlayerName();
    }
    
    [ContextMenu("SetNulPlayerName")]
    public void SetNullPlayerName()
    {
        StatisticsManager.Instance.stats.playerName = null;
    }
}
