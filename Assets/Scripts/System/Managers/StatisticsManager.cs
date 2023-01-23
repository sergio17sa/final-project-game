using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using System;

[DefaultExecutionOrder(2)]
public class StatisticsManager : Singleton<StatisticsManager>
{
    public PlayerStats stats;
    [SerializeField] private List<Character> characters;
    [SerializeField] private int pointsPerVictory = 624, pointsPerTie = 214, pointsPerKill = 124;
    private int victoryPerMatch = 1, lossPerMatch = 1, tiesPerMatch = 1, match;
    [SerializeField] private bool victory, loss, tie, kill;
    private string levelToPrint = "";
    [SerializeField] private string level1 = "Beginner", level2 = "Advanced", level3 = "Expert", level4 = "Master";
    [SerializeField] private float[] beginnerLevel = new float[2] { 0, 1439 }, advancedLevel = new float[2] { 1440, 3839 }, expertLevel = new float[2] { 3840, 7199 }, MasterLevel = new float[2] { 7200, int.MaxValue };

    private void Start()
    {
        victory = false;
        loss = false;
        tie = false;
        kill = false;
        SetNullStats();
        levelRange();
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


    public void StatsStorage()
    {
        if (victory)
        {
            stats.points += pointsPerVictory;
            stats.victories += victoryPerMatch;
            stats.matches += victoryPerMatch;
            stats.PointsPerMatch += pointsPerVictory;
            PercentMatches();
            SaveLastTenMatches(stats.PointsPerMatch);
            stats.PointsPerMatch = 0;
            victory = false;
        }

        if (loss)
        {
            stats.losses += lossPerMatch;
            stats.matches = lossPerMatch;
            PercentMatches();
            SaveLastTenMatches(stats.PointsPerMatch);
            stats.PointsPerMatch = 0;
            loss = false;
        }

        if (tie)
        {
            stats.ties += tiesPerMatch;
            stats.points += pointsPerTie;
            stats.matches = tiesPerMatch;
            stats.PointsPerMatch += pointsPerTie;
            PercentMatches();
            SaveLastTenMatches(stats.PointsPerMatch);
            stats.PointsPerMatch = 0;
            tie = false;
        }

        if (kill)
        {
            stats.PointsPerMatch += pointsPerKill;
            stats.points += pointsPerKill;
            kill = false;
        }

        SavePlayerData();
        LoadPlayerData();
    }



    public string ControllerLevel()
    {

        foreach (string level in stats.levels)
        {

            if ((stats.points >= stats.levelsRange[level1][0] && stats.points <= stats.levelsRange[level1][1]))
            {
                if (level == level1)
                {
                    stats.pointToNetxLevel = stats.levelsRange[level2][0] - stats.points;
                    levelToPrint = level;
                }
            }
            if (stats.points >= stats.levelsRange[level2][0] && stats.points <= stats.levelsRange[level2][1])
            {
                if (level == level2)
                {
                    stats.pointToNetxLevel = stats.levelsRange[level3][0] - stats.points;
                    levelToPrint = level;
                }
            }
            if (stats.points >= stats.levelsRange[level3][0] && stats.points <= stats.levelsRange[level3][1])
            {
                if (level == level3)
                {
                    levelToPrint = level;
                    stats.pointToNetxLevel = stats.levelsRange[level4][0] - stats.points;
                }
            }
            if (stats.points >= stats.levelsRange[level4][0])
            {
                if (level == level4)
                {
                    levelToPrint = level;
                    stats.pointToNetxLevel = 0;
                }
            }
        }

        return levelToPrint;
    }

    public void PercentMatches()
    {
        stats.matches = stats.victories + stats.losses + stats.ties;
        stats.victoriesPercent = (stats.victories / stats.matches);
        stats.lossesPercent = (stats.losses / stats.matches);
        stats.tiesPercent = (stats.ties / stats.matches);
    }

    public void levelRange()
    {
        if (stats.levelsRange.Count == 0)
        {
            stats.levelsRange.Add(stats.levels[0], beginnerLevel);
            stats.levelsRange.Add(stats.levels[1], advancedLevel);
            stats.levelsRange.Add(stats.levels[2], expertLevel);
            stats.levelsRange.Add(stats.levels[3], MasterLevel);
        }
    }


    public void SaveLastTenMatches(double pointsPerMatch  )
    {

        for (int i = 0; i < stats.lastTenMatches.Length; i++)
        {
            if (i != stats.lastTenMatches.Length - 1)
            {
                stats.lastTenMatches[i] = stats.lastTenMatches[i + 1];
            }
            else
            {
                stats.lastTenMatches[stats.lastTenMatches.Length - 1] = pointsPerMatch;
            }
        }
    }


    [ContextMenu("ToggleWin")]
    public void ToggleVictory()
    {
        victory = !victory;
        StatsStorage();
    }


    [ContextMenu("ToggleLoss")]
    public void ToggleLoss()
    {
        loss = !loss;
        StatsStorage();
    }

    [ContextMenu("ToggleTie")]
    public void ToggleTie()
    {
        tie = !tie;
        StatsStorage();
    }

    [ContextMenu("ToggleKill")]
    public void ToggleKill()
    {
        if (!victory || !loss || !tie)
        {
            kill = !kill;
            StatsStorage();
        }
    }





    [ContextMenu("SetNulPlayer")]
    public void SetNullStats()
    {
        if (string.IsNullOrEmpty(stats.playerName))
        {
            stats.points = 0;
            stats.victories = 0;
            stats.losses = 0;
            stats.ties = 0;
            stats.matches = 0;
            stats.tiesPercent = 0;
            stats.lossesPercent = 0;
            stats.victoriesPercent = 0;
            stats.levels.Clear();
            stats.levels.Add(level1);
            stats.levels.Add(level2);
            stats.levels.Add(level3);
            stats.levels.Add(level4);
            stats.lastTenMatches = new double[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            SavePlayerData();
        }
    }

    [ContextMenu("SetNullPlayerName")]
    public void SetNullPlayerName()
    {
        stats.playerName = null;
        SavePlayerData();
    }

}
