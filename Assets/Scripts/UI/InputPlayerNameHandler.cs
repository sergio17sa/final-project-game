using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InputPlayerNameHandler : MonoBehaviour
{
    [SerializeField] private PlayerStatsSave playerStatsSave;
    [SerializeField] private TMP_InputField inputPlayerName;
    [SerializeField] private GameObject panelInputPlayerName;
    [SerializeField] private GameObject paneljoinBtn;

    private void Awake()
    {
       // playerStatsSave.stats.playerName = null;
        Debug.Log(playerStatsSave.stats.playerName != null);
        Debug.Log(playerStatsSave.stats.playerName);
        HideInputPlayerName();
    }

    public void SubmitPlayerName()
    {
        playerStatsSave.stats.playerName = inputPlayerName.text;
        playerStatsSave.SavePlayerData();
        HideInputPlayerName();
    }

    public void HideInputPlayerName()
    {
        if (playerStatsSave.stats.playerName != null )
        {
            panelInputPlayerName.SetActive(false);
            paneljoinBtn.SetActive(true);
        }
        else 
        {
            panelInputPlayerName.SetActive(true);
            paneljoinBtn.SetActive(false);
        }
    }
}
