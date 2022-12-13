using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField] private List<Unit> _team1Units;
    [SerializeField] private List<Unit> _team2Units;
    [SerializeField] private Color _team1Color;
    [SerializeField] private Color _team2Color;


    private void Update()
    {
        if (GridManager.Instance.CanSpawnUnits)
        {
            SpawnUnits();
            GridManager.Instance.CanSpawnUnits = false;
        }
    }
    public void SpawnUnits()
    {
        foreach(Unit unit in _team1Units)
        {
            Unit spawnedUnit = Instantiate(unit);
            spawnedUnit.tag = "Team1";
            spawnedUnit.GetComponentInChildren<MeshRenderer>().material.color = _team1Color;
            Tile randomTile = GridManager.Instance.RandomLeftMapTile();
            randomTile.SetUnit(spawnedUnit);
        }

        foreach (Unit unit in _team2Units)
        {
            Unit spawnedUnit = Instantiate(unit);
            spawnedUnit.tag = "Team2";
            spawnedUnit.GetComponentInChildren<MeshRenderer>().material.color = _team2Color;
            Tile randomTile = GridManager.Instance.RandomRightMapTile();
            randomTile.SetUnit(spawnedUnit);
        }
    }
}
