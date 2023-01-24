using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class StatisticsUI : MonoBehaviour
{
    [SerializeField] private TMP_Text playerName, numberPoints, levelText, victoriesNumber, lossesNumber, matchesNumber, victoryPercent, lossesPercent, tiesPercent, pointToNetxLevelNumber, tiesNumber;
    private string initialPlayerName = "";
    private string initialPoints = "0";
    private string initialLevel = "Beginner";
    private string initialVictory = "0";
    private string initialMatches = "0";
    private string initialTies = "0";
    private string initialLosses = "0";
    private string initialPercentLosses = "0";
    private string initialPercentVictories = "0";
    private string initialPercentTies = "0";

    private void Update()
    {
        //Invoke("SetPlayerStats", 0.3f);
       SetPlayerStats();
    }

    public void SetPlayerStats()
    {
        bool playerNameIsEmmpty = string.IsNullOrEmpty(StatisticsManager.Instance.stats.playerName);
        bool victoriesNumberIsEmmpty = string.IsNullOrEmpty(StatisticsManager.Instance.stats.victories.ToString());
        bool lossesNumberIsEmmpty = string.IsNullOrEmpty(StatisticsManager.Instance.stats.losses.ToString());
        bool tiesNumberIsEmmpty = string.IsNullOrEmpty(StatisticsManager.Instance.stats.ties.ToString());
        bool MatchesNumberIsEmmpty = string.IsNullOrEmpty(StatisticsManager.Instance.stats.matches.ToString());
        bool victoriesPercentIsEmmpty = string.IsNullOrEmpty(StatisticsManager.Instance.stats.victoriesPercent.ToString());
        bool lossesPercentIsEmmpty = string.IsNullOrEmpty(StatisticsManager.Instance.stats.lossesPercent.ToString());
        bool tiesPercentIsEmmpty = string.IsNullOrEmpty(StatisticsManager.Instance.stats.tiesPercent.ToString());
        bool pointsToNextLevelIsEmmpty = string.IsNullOrEmpty(StatisticsManager.Instance.stats.pointToNetxLevel.ToString());
        bool numberPointsIsEmmpty = string.IsNullOrEmpty(StatisticsManager.Instance.stats.points.ToString());

        if (!playerNameIsEmmpty) return;
        
        // playerName.text = StatisticsManager.Instance.stats.playerName;
        // else playerName.text = initialPlayerName;

        if (!MatchesNumberIsEmmpty)
        {
            matchesNumber.text = StatisticsManager.Instance.stats.matches.ToString();
        }
        else
        {
            matchesNumber.text = initialMatches;
            levelText.text = initialLevel;
            victoriesNumber.text = initialVictory;
            lossesNumber.text = initialLosses;
            tiesNumber.text = initialTies;
            numberPoints.text = initialPoints;
            levelText.text = StatisticsManager.Instance.ControllerLevel();
            lossesPercent.text = initialPercentLosses;
            victoryPercent.text = initialPercentVictories;
            tiesPercent.text = initialPercentTies;
            pointToNetxLevelNumber.text = 1440.ToString();
        }

        if (!numberPointsIsEmmpty || StatisticsManager.Instance.stats.points == 0)
        {
            numberPoints.text = StatisticsManager.Instance.stats.points.ToString();
            levelText.text = StatisticsManager.Instance.ControllerLevel();
        }
        else
        {
            numberPoints.text = initialPoints;
            levelText.text = StatisticsManager.Instance.ControllerLevel();
        }

        if (!victoriesNumberIsEmmpty)
        {
            victoriesNumber.text = StatisticsManager.Instance.stats.victories.ToString();
            victoryPercent.text = StatisticsManager.Instance.stats.victoriesPercent.ToString()   + "%";
        }
        else
        {
            victoriesNumber.text = initialVictory;
            victoryPercent.text = initialPercentVictories;
        }

        if (!lossesNumberIsEmmpty)
        {
            lossesNumber.text = StatisticsManager.Instance.stats.losses.ToString();
            lossesPercent.text = StatisticsManager.Instance.stats.lossesPercent.ToString() + "%";
        }
        else
        {
            lossesNumber.text = initialLosses;
            lossesPercent.text = initialPercentLosses;
        }
        
        if (!tiesNumberIsEmmpty)
        {
            tiesNumber.text = StatisticsManager.Instance.stats.ties.ToString();
            tiesPercent.text = StatisticsManager.Instance.stats.tiesPercent.ToString() + "%";
        }
        else
        {
            tiesNumber.text = initialTies;
            tiesPercent.text = initialPercentTies;
        }

        if (!pointsToNextLevelIsEmmpty)
        {
            pointToNetxLevelNumber.text = StatisticsManager.Instance.stats.pointToNetxLevel.ToString();
        }

    }
}





