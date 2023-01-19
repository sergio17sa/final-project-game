using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PieGraph : MonoBehaviour
{

    private List<double> valuesPie = new List<double>();
    [SerializeField] private Color[] colorsPie;
    [SerializeField] private Image widge, wonGames, lostGames, tiesGames;
    [SerializeField] private TMP_Text wonGamesText, lostGamesText, tiesGamesText;

    void Start()
    {
        PieGraphMaker();

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
            newWedge.transform.SetParent(transform, false);
            newWedge.color = colorsPie[i];
            newWedge.fillAmount = (float)(valuesPie[i] / total);
            newWedge.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, zRotation));
            zRotation -= newWedge.fillAmount * 360;
        }
    }
}
