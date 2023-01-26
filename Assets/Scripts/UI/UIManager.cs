using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;
using System;

public class UIManager : Singleton<UIManager>
{
    [Header("Main Menu Panel")]
    public GameObject panelMainButtons;
    public GameObject panelSubs;
    public GameObject endPanel;
    public GameObject panelRuler;
    public GameObject pausePanel;

    [Header("Estats Panel Player")]
    [SerializeField] TMP_Text matches, level, points;
    private float barLength = 1.4f, percentMultiplier = 100f;
    private string levelStr;
    private List<double> valuesPie = new List<double>();
    private List<GameObject> provitionalGraphicsObjects = new List<GameObject>();
    [SerializeField] private Color[] colorsPie;
    [SerializeField] private Image widge, wonGames, lostGames, tiesGames, levelImg;
    [SerializeField] private TMP_Text wonGamesText, lostGamesText, tiesGamesText, pointsNextLevel, PlayerNameStats;
    [SerializeField] private GameObject pieGraph, barGraph, panelInputPlayerName;
    [SerializeField] private Sprite BeginnerImg, advancedImg, expertImg, masterImg;
    [SerializeField] private BarScript barPrefab;
    [SerializeField] private TMP_InputField inputPlayerName;

    private void Start()
    {
        HideInputPlayerName();
    }

    public void HideInputPlayerName()
    {
        if (String.IsNullOrEmpty(StatisticsManager.Instance.stats.playerName))
        {
            panelInputPlayerName.SetActive(true);
            panelMainButtons.SetActive(false);
        }
        else
        {
            panelInputPlayerName.SetActive(false);
            panelMainButtons.SetActive(true);
        }
    }

    public void SubmitPlayerName()
    {
        StatisticsManager.Instance.stats.playerName = inputPlayerName.text;
        StatisticsManager.Instance.SavePlayerData();
        HideInputPlayerName();
    }

    #region Statas
    public void LoadStadistics()
    {
        SetPercentBar();
        PieGraphMaker();
        StatisticsManager.Instance.levelRange();
        SetTextStatistics();
        ChangeSpriteLevel();
        BarsGraphMaker(StatisticsManager.Instance.stats.lastTenMatches);
    }

    public void SetPercentBar()
    {
        wonGames.fillAmount = (float)(StatisticsManager.Instance.stats.victoriesPercent * barLength);
        lostGames.fillAmount = (float)(StatisticsManager.Instance.stats.lossesPercent * barLength);
        tiesGames.fillAmount = (float)(StatisticsManager.Instance.stats.tiesPercent * barLength);
        wonGamesText.text = $"{Mathf.Round((float)(StatisticsManager.Instance.stats.victoriesPercent * percentMultiplier))}% ";
        lostGamesText.text = $"{Mathf.Round((float)(StatisticsManager.Instance.stats.lossesPercent * percentMultiplier))}% ";
        tiesGamesText.text = $"{Mathf.Round((float)(StatisticsManager.Instance.stats.tiesPercent * percentMultiplier))}% ";
    }

    public void PieGraphMaker()
    {
        valuesPie.Add(StatisticsManager.Instance.stats.victoriesPercent);
        valuesPie.Add(StatisticsManager.Instance.stats.lossesPercent);
        valuesPie.Add(StatisticsManager.Instance.stats.tiesPercent);

        double total = 0;
        float zRotation = 0;
        for (int i = 0; i < valuesPie.Count; i++)
        {
            total += valuesPie[i];
        }

        for (int i = 0; i < valuesPie.Count; i++)
        {
            Image newWedge = Instantiate(widge) as Image;
            newWedge.transform.SetParent(pieGraph.transform, false);
            newWedge.color = colorsPie[i];

            if (total == 0)
            {
                total = 1;
            }

            newWedge.fillAmount = (float)(valuesPie[i] / total);
            newWedge.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, zRotation));
            zRotation -= newWedge.fillAmount * 360;
            provitionalGraphicsObjects.Add(newWedge.gameObject);
        }
    }

    public void SetTextStatistics()
    {
        levelStr = StatisticsManager.Instance.ControllerLevel().ToUpper();
        matches.text = $"MATCHES: {StatisticsManager.Instance.stats.matches.ToString()}";
        level.text = $"LEVEL: {levelStr}";
        points.text = $"POINTS: {StatisticsManager.Instance.stats.points.ToString()}";
        pointsNextLevel.text = StatisticsManager.Instance.stats.pointToNetxLevel.ToString();
        PlayerNameStats.text = StatisticsManager.Instance.stats.playerName.ToUpper();
    }

    public void ResetGraphics()
    {
        for (int i = 0; i < provitionalGraphicsObjects.Count; i++)
        {
            Destroy(provitionalGraphicsObjects[i]);
        }

        valuesPie.Clear();
        provitionalGraphicsObjects.Clear();
    }

    public void ChangeSpriteLevel()
    {
        switch (levelStr)
        {
            case "BEGINNER":
                {
                    levelImg.sprite = BeginnerImg;
                    break;
                }
            case "ADVANCED":
                {
                    levelImg.sprite = advancedImg;
                    break;
                }
            case "EXPERT":
                {
                    levelImg.sprite = expertImg;
                    break;
                }
            case "MASTER":
                {
                    levelImg.sprite = masterImg;
                    break;
                }
        }
    }

    public void BarsGraphMaker(double[] lastMatches)
    {
        float total = 0;
        for (int i = 0; i < lastMatches.Length; i++)
        {
            BarScript newBar = Instantiate(barPrefab) as BarScript;
            newBar.transform.SetParent(barGraph.transform);
            total += (float)lastMatches[i];
            newBar.bar.fillAmount = ((float)lastMatches[i] / total);
            newBar.points.text = $"{lastMatches[i].ToString()}-";
            provitionalGraphicsObjects.Add(newBar.gameObject);
        }
    }
    #endregion
}
