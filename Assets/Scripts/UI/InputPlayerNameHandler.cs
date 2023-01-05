using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class InputPlayerNameHandler : MonoBehaviour
{
    [SerializeField] private PlayerStatsSave playerStatsSave;
    [SerializeField] private TMP_InputField inputPlayerName;
    [SerializeField] private GameObject panelInputPlayerName;
    [SerializeField] private GameObject paneljoinBtn;

    private void Awake()
    {
        HideInputPlayerName();
    }

    public void HideInputPlayerName()
    {
        if (String.IsNullOrEmpty(playerStatsSave.stats.playerName))
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
        playerStatsSave.stats.playerName = inputPlayerName.text;
        playerStatsSave.SavePlayerData();
        HideInputPlayerName();
    }
    
    [ContextMenu("SetNulPlayerName")]
    public void SetNullPlayerName()
    {
        playerStatsSave.stats.playerName = null;
    }
}
