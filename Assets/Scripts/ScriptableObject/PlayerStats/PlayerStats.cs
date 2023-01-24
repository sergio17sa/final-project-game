using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerStat", menuName = "ScriptableObjects/PlayerStats", order = 3)]
public class PlayerStats : ScriptableObject
{
    public string playerName;
    public double points, victories, losses, ties, matches, victoriesPercent, lossesPercent, tiesPercent, pointToNetxLevel, PointsPerMatch;
    public List<string> levels = new List<string>();

    public double[] lastTenMatches = new double[10];

    public Dictionary<string, float[]> levelsRange = new Dictionary<string, float[]>();
}