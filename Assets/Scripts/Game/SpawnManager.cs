using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;


public class SpawnManager : NetworkSingleton<SpawnManager>
{
    [SerializeField] private List<GameObject> _medievalTeam;
    [SerializeField] private List<GameObject> _futureTeam;
    [SerializeField] private List<GameObject> _obstacles;
    [SerializeField] private List<GameObject> _props;


    private List<GameObject> _spawnedMedievalTeam;
    private List<GameObject> _spawnedFutureTeam;

    [SerializeField] LayerMask obstaclesLayerMask;

    public event EventHandler OnGameFinished;
    public event EventHandler OnSpawnsFinished;

    private int numberOfObstacles = 2;
    private int numberOfProps = 80;

    private void Awake()
    {
        _spawnedMedievalTeam = new List<GameObject>();
        _spawnedFutureTeam = new List<GameObject>();
    }

    private bool canSpawn = true;

    private void Start()
    {
        //Character.OnDead += Character_OnDead;

        //SpawnObstacles();

        //SpawnProps();

        //TeamsSpawn();

        //OnSpawnsFinished?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.G) && canSpawn)
        {
            SpawnObstacles();
        }
    }

    public void SpawnObjects()
    {
        if (!IsServer) return;

        for (int i = 0; i < 2; i++)
        {
            GameObject go = Instantiate(_obstacles[0], 
                new Vector3(UnityEngine.Random.Range(-10, 10), 10.0f, UnityEngine.Random.Range(-10, 10)), Quaternion.identity);

            go.GetComponent<NetworkObject>().Spawn();
        }
    }


    //Refactor later
    public List<Vector3> ValidTiles(int minZ, int maxZ)
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


    public void SpawnObstacles()
    {

        if (!IsServer) return;

        List<Vector3> validList = ValidTiles(0, 10);

        for (int i = 0; i <= numberOfObstacles; i++)
        {
            Vector3 tilePosition = validList[UnityEngine.Random.Range(0, validList.Count - 1)];

            int randomIndex = UnityEngine.Random.Range(0, _obstacles.Count);

            GameObject g = Instantiate(_obstacles[randomIndex], tilePosition, Quaternion.identity);
            g.GetComponent<NetworkObject>().Spawn();

            validList.Remove(tilePosition);
        }

        canSpawn = false;
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

            _spawnedMedievalTeam.Add(newCharacter);
            validMedievalTiles.Remove(tilePosition);
        }

        foreach (GameObject futureCharacter in _futureTeam)
        {
            Vector3 tilePosition = validFutureTiles[UnityEngine.Random.Range(0, validFutureTiles.Count - 1)];
            GameObject newCharacter = Instantiate(futureCharacter, tilePosition, futureCharacter.transform.rotation);

            _spawnedFutureTeam.Add(newCharacter);
            validFutureTiles.Remove(tilePosition);
        }
    }

    private void Character_OnDead(object sender, EventArgs e)
    {
        Character character= (Character)sender;

        GridManager.Instance.ClearCharacterAtTilePosition(character.CharacterTilePosition);

        if(character.GetCharacterTeam() == Team.Team1)
        {
            _spawnedMedievalTeam.Remove(character.gameObject);
        } else
        {
            _spawnedFutureTeam.Remove(character.gameObject);
        }

        if(_spawnedMedievalTeam.Count == 0 || _spawnedFutureTeam.Count == 0)
        {
            Debug.Log("Game Finished");
            OnGameFinished?.Invoke(this, EventArgs.Empty);
        }
    }

}
