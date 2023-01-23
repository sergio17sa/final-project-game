using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridVisualManager : Singleton<GridVisualManager>
{
    [SerializeField] private Transform gridTileVisual;
    private TileVisual[,] _tileVisualArray;

    [Serializable]
    public struct TileTypeMaterial
    {
        public TileColor tileColor;
        public Material material;
    }
    public enum TileColor
    {
        White,
        Red,
        SoftRed,
        Blue,
    }

    [SerializeField] private List<TileTypeMaterial> tileTypeMaterials;

    private void Start()
    {
        CreateVisualTiles();

        CharacterActionManager.Instance.OnSelectedActionChanged += CharacterActionManager_OnSelectedActionChanged;
        TurnSystemManager.Instance.OnTurnChanged += TurnSystemManager_OnTurnChanged;
        GridManager.Instance.OnCharacterMove += GridManager_OnCharacterMove;

        UpdateTileVisual();
    }

    private void OnDisable()
    {
        CharacterActionManager.Instance.OnSelectedActionChanged -= CharacterActionManager_OnSelectedActionChanged;
        TurnSystemManager.Instance.OnTurnChanged -= TurnSystemManager_OnTurnChanged;
        GridManager.Instance.OnCharacterMove -= GridManager_OnCharacterMove;
    }


    private void CreateVisualTiles()
    {
        _tileVisualArray = new TileVisual[
            GridManager.Instance.GetWidth(),
            GridManager.Instance.GetHeight()
            ];

        for (int x = 0; x < GridManager.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < GridManager.Instance.GetHeight(); z++)
            {
                TilePosition tilePosition = new TilePosition(x, z);
                Transform gridSystenVisualSingleTransform = Instantiate(
                    gridTileVisual,
                    GridManager.Instance.GetWorldPosition(tilePosition),
                    Quaternion.identity
                    );
                _tileVisualArray[x, z] = gridSystenVisualSingleTransform.GetComponent<TileVisual>();
            }
        }
    }
    public void HideAllTilePositions()
    {
        for (int x = 0; x < _tileVisualArray.GetLength(0); x++)
        {
            for (int z = 0; z < _tileVisualArray.GetLength(1); z++)
            {

                _tileVisualArray[x, z].Hide();
            }
        }
    }

    public void ShowTilePositionList(List<TilePosition> tilePositionList, TileColor tileColor)
    {
        if (tilePositionList == null) return;

        Material newMaterial = GetTileColor(tileColor);

        foreach (TilePosition tilePosition in tilePositionList)
        {
            _tileVisualArray[tilePosition.x, tilePosition.z].Show(newMaterial);
        }
    }

    private void ShowTilePositionRange(TilePosition tilePosition, int range, TileColor tileColor, AttackType attackType)
    {
        List<TilePosition> tilePositions = new List<TilePosition>();

        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {
                TilePosition testTilePosition = tilePosition + new TilePosition(x, z);

                if (!GridManager.Instance.IsValidTilePosition(testTilePosition)) continue;

                if (attackType == AttackType.Shoot)
                {
                    int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                    if (testDistance > range) continue;

                    tilePositions.Add(testTilePosition);
                }

                if (attackType == AttackType.Spell)
                {
                    if (testTilePosition.x == tilePosition.x || testTilePosition.z == tilePosition.z)
                    {
                        tilePositions.Add(testTilePosition);
                    }
                }
            }
        }

        ShowTilePositionList(tilePositions, tileColor);
    }


    private void ShowActionRangeSquare(TilePosition tilePosition, int range, TileColor tileColor)
    {
        List<TilePosition> tilePositions = new List<TilePosition>();

        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {
                TilePosition testTilePosition = tilePosition + new TilePosition(x, z);

                if (!GridManager.Instance.IsValidTilePosition(testTilePosition)) continue;

                tilePositions.Add(testTilePosition);
            }
        }

        ShowTilePositionList(tilePositions, tileColor);
    }


    private void UpdateTileVisual()
    {
        HideAllTilePositions();

        Character selectedCharacter = CharacterActionManager.Instance.GetSelectedCharacter();
        BaseAction selectedAction = CharacterActionManager.Instance.GetSelectedAction();

        if (selectedCharacter != null)
        {
            TileColor tileColor;

            switch (selectedAction)
            {
                default:
                case MoveAction moveAction:
                    tileColor = TileColor.White;
                    break;
                case HealAction heal:
                    tileColor = TileColor.Blue;
                    break;
                case SwordAction swordAction:
                    tileColor = TileColor.Red;

                    ShowActionRangeSquare(
                        selectedCharacter.CharacterTilePosition, 
                        swordAction.GetSwordRange(),
                        TileColor.SoftRed
                        );
                    break;
                case RangeAttackAction rangeAttackAction:
                    tileColor = TileColor.Red;

                    ShowTilePositionRange(
                        selectedCharacter.CharacterTilePosition,
                        rangeAttackAction.GetAttackRange(),
                        TileColor.SoftRed,
                        rangeAttackAction.GetAttackType()
                        );
                    break;
            }

            ShowTilePositionList(selectedAction.GetValidActionTiles(), tileColor);
        }
    }

    private Material GetTileColor(TileColor tileColor)
    {
        foreach (TileTypeMaterial tileTypeMaterial in tileTypeMaterials)
        {
            if (tileTypeMaterial.tileColor == tileColor)
            {
                return tileTypeMaterial.material;
            }
        }

        return null;
    }

    private void CharacterActionManager_OnSelectedActionChanged(object sender, EventArgs e)
    {
        BaseAction selectedAction = CharacterActionManager.Instance.GetSelectedAction();

        if(selectedAction == null)
        {
            HideAllTilePositions();
        }
        else
        {
            UpdateTileVisual();
        }
    }

    private void GridManager_OnCharacterMove(object sender, EventArgs e)
    {
        UpdateTileVisual();
    }

    private void TurnSystemManager_OnTurnChanged(object sender, EventArgs e)
    {
        HideAllTilePositions();
    }
}
