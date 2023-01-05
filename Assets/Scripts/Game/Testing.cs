using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private Character _character;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            TilePosition mouseTilePosition = GridManager.Instance.GetTilePosition(MousePosition.GetPosition());
            TilePosition startTilePosition = new TilePosition(0, 0);

            List<TilePosition> tilePositions = PathFinding.Instance.FindPath(startTilePosition, mouseTilePosition, out int pathLength);

            for(int i = 0; i < tilePositions.Count - 1; i++)
            {
                Debug.DrawLine(
                    GridManager.Instance.GetWorldPosition(tilePositions[i]),
                    GridManager.Instance.GetWorldPosition(tilePositions[i + 1]),
                    Color.blue,
                    10f
                    );
            }
        }
    }
}
