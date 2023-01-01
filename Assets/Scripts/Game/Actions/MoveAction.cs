using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
    private Character _character;
    private Vector3 _targetPosition;

    [Header("Setup Move and Rotation")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] float rotateSpeed = 30f;

    [SerializeField] private int _maxMoveDistance = 1;

    private void Awake() 
    {
        _character = GetComponent<Character>();
        _targetPosition = transform.position;
    }

    private void Update()
    {
        Move();
    }

    public void SetTargetPosition(TilePosition tilePosition)
    {
        _targetPosition = GridManager.Instance.GetWorldPosition(tilePosition);
    }

    private void Move()
    {
        float stoppingDistance = 0.1f;
        Vector3 moveDirection = (_targetPosition - transform.position).normalized;

        if (Vector3.Distance(transform.position, _targetPosition) > stoppingDistance)
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            _character.GetMovement(1);
        } 
        else 
        {
            _character.GetMovement(0);
        }

        
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
    }

    public List<TilePosition> GetValidTilesPositionList()
    {
        List<TilePosition> validTilePositionList = new List<TilePosition>();

        TilePosition charactetTilePosition = _character.CharacterTilePosition;

        for(int x = -_maxMoveDistance; x <= _maxMoveDistance; x++)
        {
            for(int z = -_maxMoveDistance; z <= _maxMoveDistance; z++)
            {
                TilePosition offsetTilePosition = new TilePosition(x, z);
                TilePosition testTilePosition = charactetTilePosition + offsetTilePosition;

                if (!GridManager.Instance.IsValidTilePosition(testTilePosition)) continue;
  
                if(charactetTilePosition == testTilePosition) continue;

                if (GridManager.Instance.HasCharacterOnTilePosition(testTilePosition)) continue;

                validTilePositionList.Add(testTilePosition);
            }
        }

        return validTilePositionList;
    }
    public bool IsValidAction(TilePosition tilePosition)
    {
        List<TilePosition> validTilesList = GetValidTilesPositionList();
        return validTilesList.Contains(tilePosition);
    }
}
