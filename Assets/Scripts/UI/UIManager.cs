using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    public TMP_Text playerServer;
    public TMP_Text playerClient;
    [SerializeField] TMP_Text matches, level, points;
    public NetworkUIController networkUI;
    private float barLength = 1.4f, percentMultiplier = 100f;
    private string levelStr;
    private List<double> valuesPie = new List<double>();
    [SerializeField] private Color[] colorsPie;
    [SerializeField] private Image widge, wonGames, lostGames, tiesGames, levelImg;
    [SerializeField] private TMP_Text wonGamesText, lostGamesText, tiesGamesText, pointsNextLevel;
    [SerializeField] private GameObject pieGraph;
    [SerializeField] private Sprite BeginnerImg, advancedImg, expertImg, masterImg;

    private void Start()
    {
        SetPercentBar();
        PieGraphMaker();
        StatisticsManager.Instance.levelRange();
        SetTextStatistics();
        ChangeSpriteLevel();
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
            newWedge.fillAmount = (float)(valuesPie[i] / total);
            newWedge.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, zRotation));
            zRotation -= newWedge.fillAmount * 360;
        }
    }

    public void SetTextStatistics()
    {
        levelStr = StatisticsManager.Instance.ControllerLevel().ToUpper();
        matches.text = $"MATCHES: {StatisticsManager.Instance.stats.matches.ToString()}";
        level.text = $"LEVEL: {levelStr}";
        points.text = $"POINTS: {StatisticsManager.Instance.stats.points.ToString()}";
        pointsNextLevel.text = StatisticsManager.Instance.stats.pointToNetxLevel.ToString();
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
}
