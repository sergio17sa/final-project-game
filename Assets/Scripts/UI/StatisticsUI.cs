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
    private string initialPlayerName = "";
    private string initialPoints = "0";
    private string initialLevel = "Beginner";
    private string initialVictory = "0";

    private void Update()
    {
        Invoke("SetPlayerStats", 0.3f);
        SetPlayerStats();
    }

    public void SetPlayerStats()
    {
        bool playerNameIsEmmpty = string.IsNullOrEmpty(StatisticsManager.Instance.stats.playerName);
        bool numberPointsIsEmmpty = string.IsNullOrEmpty(StatisticsManager.Instance.stats.points.ToString());
        // bool levelTextIsEmmpty = string.IsNullOrEmpty(StatisticsManager.Instance.stats.levels[0]);
        bool victoriesNumberIsEmmpty = string.IsNullOrEmpty(StatisticsManager.Instance.stats.victories.ToString());

        if (!playerNameIsEmmpty) playerName.text = StatisticsManager.Instance.stats.playerName;
        else playerName.text = initialPlayerName;

        if (!numberPointsIsEmmpty || StatisticsManager.Instance.stats.points == 0)
        {
            numberPoints.text = StatisticsManager.Instance.stats.points.ToString();
            levelText.text = StatisticsManager.Instance.ControllerLevel();
        }
        else
        {
            numberPoints.text = initialPoints;
            levelText.text = initialLevel;
        }


        if (!victoriesNumberIsEmmpty) victoriesNumber.text = StatisticsManager.Instance.stats.victories.ToString();
        else victoriesNumber.text = initialVictory;
    }


}





