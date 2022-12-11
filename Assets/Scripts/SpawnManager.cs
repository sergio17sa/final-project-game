using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField] private List<Unit> _team1Units;
    [SerializeField] private List<Unit> _team2Units;

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
            Tile randomTile = GridManager.Instance.RandomLeftMapTile();
            randomTile.SetUnit(spawnedUnit);
        }

        foreach (Unit unit in _team2Units)
        {
            Unit spawnedUnit = Instantiate(unit);
            Tile randomTile = GridManager.Instance.RandomRightMapTile();
            randomTile.SetUnit(spawnedUnit);
        }
    }
}
