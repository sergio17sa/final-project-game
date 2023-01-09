using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StatisticsManager : MonoBehaviour
{
    public static StatisticsManager Instance;
    //[SerializeField] private PlayerStatsSave playerStatsSave;
    public PlayerStats stats;
    [SerializeField] private List<Character> characters;
    private int pointsPerMatch = 480;
    private int victoryMatch = 1;
    public bool victory;
    public string levelToPrint = "";

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        victory = false;
        LoadPlayerData();
        SetNullPlayerName();
    }

    [ContextMenu("SavePlayerData")]
    public void SavePlayerData()
    {
        BaseDataManager.Instance.Save("/PlayerData.json", stats);
    }


    [ContextMenu("LoadPlayerData")]
    public void LoadPlayerData()
    {
        BaseDataManager.Instance.Load("/PlayerData.json", stats);
    }


    public void VictoryPointStorage()
    {
        if (victory)
        {
            stats.points += pointsPerMatch;
            stats.victories += victoryMatch;
            victory = false;
            SavePlayerData();
            LoadPlayerData();
        }
    }


    public string ControllerLevel()
    {

        foreach (string level in stats.levels)
        {

            if (stats.points >= 0 && stats.points <= 1439)
            {
                if (level == "Beginner")
                {
                    levelToPrint = level;
                }
            }
            if (stats.points >= 1440 && stats.points <= 3839)
            {
                if (level == "Advanced")
                {
                    return levelToPrint = level;
                }
            }
            if (stats.points >= 3840 && stats.points <= 7199)
            {
                if (level == "Expert")
                {
                    return levelToPrint = level;
                }
            }
            if (stats.points >= 7200)
            {
                if (level == "Master")
                {
                    return levelToPrint = level;
                }
            }
        }
        return levelToPrint;
    }

    [ContextMenu("ToggleWin")]
    public void ToggleWinVictory()
    {
        victory = !victory;
        VictoryPointStorage();
    }

    [ContextMenu("SetNulPlayerName")]
    public void SetNullPlayerName()
    {
        LoadPlayerData();

        if (String.IsNullOrEmpty(stats.playerName))
        {

            stats.points = 0;
            stats.levels.Add("Beginner");
            stats.levels.Add("Advanced");
            stats.levels.Add("Expert");
            stats.levels.Add("Master");
            stats.victories = 0;
            SavePlayerData();
        }
        else
        {
            Debug.Log("ya existen datos del jugador");
        }

    }

    [ContextMenu("SetNulPlayer")]
    public void SetNullPlayer()
    {
        stats.playerName = null;
        SavePlayerData();
    }


}
