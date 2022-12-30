using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerStats stats;
    public List<Character> characters;

    // Start is called before the first frame update
    void Start()
    {
        
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


    // Update is called once per frame
    void Update()
    {
        
    }
}
