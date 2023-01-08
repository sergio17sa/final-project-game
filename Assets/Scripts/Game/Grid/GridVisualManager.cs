using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridVisualManager : Singleton<GridVisualManager>
{
    [SerializeField] private Transform gridTileVisual;
    private TileVisual[,] _tileVisualArray;
    
    private void Start()
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

    private void Update()
    {
        UpdateTileVisual();
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

    public void ShowTilePositionList(List<TilePosition> tilePositionList) 
    {
        if (tilePositionList == null) return;

        foreach (TilePosition tilePosition in tilePositionList) 
        {
            _tileVisualArray[tilePosition.x, tilePosition.z].Show();
        }
    }

    private void UpdateTileVisual() 
    {
        HideAllTilePositions();

        Character selectedCharacter = CharacterActionManager.Instance.GetSelectedCharacter();
        BaseAction selectedAction = CharacterActionManager.Instance.GetSelectedAction();

        if(selectedCharacter != null)
        {
            ShowTilePositionList(selectedAction.GetValidActionTiles());
        } else
        {
            ShowTilePositionList(null);
        }
    }
}
