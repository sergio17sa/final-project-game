using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
    [SerializeField] Character _character;
    private Vector3 _targetPosition;

    [Header("Setup Move and Rotation")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] float rotateSpeed = 30f;

    private void Awake() 
    {
        _targetPosition = transform.position;
    }

    private void Update()
    {
        Move();
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
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
}
