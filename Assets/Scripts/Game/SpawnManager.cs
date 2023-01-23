using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField] private List<GameObject> _medievalTeam;
    [SerializeField] private List<GameObject> _futureTeam;
    [SerializeField] private List<GameObject> _obstacles;
    [SerializeField] private List<GameObject> _props;

    [SerializeField] private bool _isAIGame = false;

    public List<GameObject> SpawnedMedievalTeam { get; private set; }
    public List<GameObject> SpawnedFutureTeam { get; private set; }

    [SerializeField] LayerMask obstaclesLayerMask;

    public event EventHandler OnSpawnsFinished;
    public static event EventHandler OnVictory;
    public static event EventHandler OnKill;
    public static event EventHandler OnLoss;

    private int numberOfObstacles = 15;
    private int numberOfProps = 80;

    protected override void Awake()
    {
        base.Awake();
        SpawnedMedievalTeam = new List<GameObject>();
        SpawnedFutureTeam = new List<GameObject>();
    }


    private void Start()
    {
        Character.OnDead += Character_OnDead;

        SpawnObstacles();

        SpawnProps();

        TeamsSpawn();

        OnSpawnsFinished?.Invoke(this, EventArgs.Empty);
    }


    //Refactor later
    private List<Vector3> ValidTiles(int minZ, int maxZ)
    {
        List<Vector3> validTilePositions = new List<Vector3>();

        for (int x = 0; x <= 10; x++)
        {
            for (int z = minZ; z <= maxZ; z++)
            {
                TilePosition tilePosition = new TilePosition(x, z);
                Vector3 worldPosition = GridManager.Instance.GetWorldPosition(tilePosition);

                //Ignore tiles out of bounds 
                if (!GridManager.Instance.IsValidTilePosition(tilePosition)) continue;

                //Ignore tiles that has a character on it
                if (GridManager.Instance.HasCharacterOnTilePosition(tilePosition)) continue;


                float raycastOffsetDistance = 5f;
                if (Physics.Raycast(
                    worldPosition + Vector3.down * raycastOffsetDistance,
                    Vector3.up,
                    raycastOffsetDistance * 2,
                    obstaclesLayerMask))
                {
                    continue;
                }

                validTilePositions.Add(worldPosition);
            }
        }

        return validTilePositions;
    }


    private void SpawnObstacles()
    {
        List<Vector3> validList = ValidTiles(0, 10);

        for (int i = 0; i <= numberOfObstacles; i++)
        {
            Vector3 tilePosition = validList[UnityEngine.Random.Range(0, validList.Count - 1)];

            int randomIndex = UnityEngine.Random.Range(0, _obstacles.Count);

            Instantiate(_obstacles[randomIndex], tilePosition, Quaternion.identity);

            validList.Remove(tilePosition);
        }
    }

    private void SpawnProps()
    {
        List<Vector3> validList = ValidTiles(0, 10);

        for (int i = 0; i <= numberOfProps; i++)
        {
            Vector3 tilePosition = validList[UnityEngine.Random.Range(0, validList.Count - 1)];

            int randomIndex = UnityEngine.Random.Range(0, _props.Count);

            Instantiate(_props[randomIndex], tilePosition, Quaternion.identity);
        }
    }

    private void TeamsSpawn()
    {
        List<Vector3> validMedievalTiles = ValidTiles(0, 2);
        List<Vector3> validFutureTiles = ValidTiles(8, 10);

        foreach (GameObject medievalCharacter in _medievalTeam)
        {
            Vector3 tilePosition = validMedievalTiles[UnityEngine.Random.Range(0, validMedievalTiles.Count - 1)];

            GameObject newCharacter = Instantiate(medievalCharacter, tilePosition, Quaternion.identity);

            SpawnedMedievalTeam.Add(newCharacter);
            validMedievalTiles.Remove(tilePosition);
        }

        foreach (GameObject futureCharacter in _futureTeam)
        {
            Vector3 tilePosition = validFutureTiles[UnityEngine.Random.Range(0, validFutureTiles.Count - 1)];
            GameObject newCharacter = Instantiate(futureCharacter, tilePosition, futureCharacter.transform.rotation);

            if (_isAIGame)
            {
                newCharacter.tag = "EnemyAI";
                newCharacter.AddComponent<EnemyAI>();
            }


            SpawnedFutureTeam.Add(newCharacter);
            validFutureTiles.Remove(tilePosition);
        }
    }

    private void Character_OnDead(object sender, EventArgs e)
    {
        Character character = (Character)sender;

        GridManager.Instance.ClearCharacterAtTilePosition(character.CharacterTilePosition);

        if (character.GetCharacterTeam() == Team.Team1)
        {
            SpawnedMedievalTeam.Remove(character.gameObject);
        }
        else
        {
            SpawnedFutureTeam.Remove(character.gameObject);
            OnKill?.Invoke(this, EventArgs.Empty);
        }

        if (SpawnedMedievalTeam.Count == 0 || SpawnedFutureTeam.Count == 0)
        {
            GameManager.Instance.EndMatch();
            
            if (SpawnedMedievalTeam.Count > 0) OnVictory?.Invoke(this, EventArgs.Empty);

            if (SpawnedFutureTeam.Count > 0) OnLoss?.Invoke(this, EventArgs.Empty);

        }
    }
}
