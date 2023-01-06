using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticsManager : MonoBehaviour
{
    public static StatisticsManager Instance;
    //[SerializeField] private PlayerStatsSave playerStatsSave;
    public PlayerStats stats;
    public List<Character> characters;
    private string temporalPlayerName = "no ha ingreso el nombre";


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

    public void PointsStorage(int points){

    }

}
