using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InputPlayerNameHandler : MonoBehaviour
{
    [SerializeField] private PlayerStatsSave playerStatsSave;
    [SerializeField] private TMP_InputField inputPlayerName;
    [SerializeField] private GameObject PanelInputPlayerName;


    private void Awake()
    {
        Debug.Log(playerStatsSave.stats.playerName );
        //playerStatsSave.stats.playerName = null;
        HideInputPlayerName();
    }

    public void SubmitPlayerName()
    {
        playerStatsSave.stats.playerName = inputPlayerName.text;
        playerStatsSave.SavePlayerData();
    }

    public void HideInputPlayerName()
    {
        if (playerStatsSave.stats.playerName != null)
        {
            PanelInputPlayerName.SetActive(false);
        }
    }
}
