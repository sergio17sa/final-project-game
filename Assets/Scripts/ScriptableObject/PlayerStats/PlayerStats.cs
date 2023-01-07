using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerStat", menuName = "ScriptableObjects/PlayerStats", order = 3)]
public class PlayerStats : ScriptableObject
{
    public string playerName;
    public int points, victories;
    public List<string> levels = new List<string>();
}
