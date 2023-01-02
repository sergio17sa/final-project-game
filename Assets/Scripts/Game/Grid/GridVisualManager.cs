using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridVisualManager : Singleton<GridVisualManager>
{
    [SerializeField] private Transform gridTileVisual;
    private TileVisual[,] _tileCisualArray;
    
    private void Start()
    {
        _tileCisualArray = new TileVisual[
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
                _tileCisualArray[x, z] = gridSystenVisualSingleTransform.GetComponent<TileVisual>();
            }
        }
    }

    private void Update()
    {
        UpdateTileVisual();
    }

    public void HideAllTilePositions()
    {
        for (int x = 0; x < _tileCisualArray.GetLength(0); x++)
        {
            for (int z = 0; z < _tileCisualArray.GetLength(1); z++)
            {

                _tileCisualArray[x, z].Hide();
            }
        }
    }

    public void ShowTilePositionList(List<TilePosition> tilePositionList) 
    {  
        foreach(TilePosition tilePosition in tilePositionList) 
        {
            _tileCisualArray[tilePosition.x, tilePosition.z].Show();
        }
    }

    private void UpdateTileVisual() 
    {
        HideAllTilePositions();

        Character selectedCharacter = CharacterActionManager.Instance.GetSelectedCharacter();
        if(selectedCharacter)
        {
            ShowTilePositionList(selectedCharacter.CharacterMoveAction.GetValidActionTiles());
        }
    }
}
