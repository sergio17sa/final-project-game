using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : Singleton<GridManager>
{
    [SerializeField] private Transform _tileDebug;

    [Header("Setup size of the Grid and Tiles")]
    [SerializeField] private int _cellSize;
    [SerializeField] private int _gridHeight;
    [SerializeField] private int _gridWidth;

    private GridSystem<Tile> _gridSystem;

    public event EventHandler OnCharacterMove;

    protected override void Awake()
    {
        base.Awake();

        _gridSystem = new GridSystem<Tile>(_gridWidth, _gridHeight, _cellSize,
                (GridSystem<Tile> tile, TilePosition tilePosition) => new Tile(tile, tilePosition)
            );

        //_gridSystem.CreateDebugTiles(_tileDebug);
    }

    private void Start()
    {
        SpawnManager.Instance.OnSpawnsFinished += SpawnManager_OnSpawnsFinished;
    }

    private void OnDisable()
    {
        SpawnManager.Instance.OnSpawnsFinished -= SpawnManager_OnSpawnsFinished;
    }


    public void SetCharacterOnTile(TilePosition tilePosition, Character character)
    {
        Tile tile = _gridSystem.GetTile(tilePosition);
        tile.CharacterOnTile = character;
    }

    public Character GetCharacterAtTilePosition(TilePosition tilePosition)
    {
        Tile tile = _gridSystem.GetTile(tilePosition);
        return tile.CharacterOnTile;
    }

    public TilePosition GetTilePosition(Vector3 worldPosition) => _gridSystem.GetTilePosition(worldPosition);

    public Vector3 GetWorldPosition(TilePosition tilePosition) => _gridSystem.GetWorldPosition(tilePosition);

    public int GetWidth() => _gridWidth;
    public int GetHeight() => _gridHeight;

    public void ClearCharacterAtTilePosition(TilePosition tilePosition)
    {
        Tile tile = _gridSystem.GetTile(tilePosition);
        tile.CharacterOnTile = null;
    }

    public bool IsValidTilePosition(TilePosition tilePosition) => _gridSystem.IsValidTilePosition(tilePosition);

    public bool HasCharacterOnTilePosition(TilePosition tilePosition)
    {
        Tile tile = _gridSystem.GetTile(tilePosition);
        return tile.HasCharacter();
    }

    public void CharacterMoveTile(Character character, TilePosition fromTilePosition, TilePosition toTilePosition)
    {
        ClearCharacterAtTilePosition(fromTilePosition);
        SetCharacterOnTile(toTilePosition, character);

        OnCharacterMove?.Invoke(this, EventArgs.Empty);
    }
    private void SpawnManager_OnSpawnsFinished(object sender, EventArgs e)
    {
        PathFinding.Instance.Setup(_gridWidth, _gridHeight, _cellSize);
    }
}
