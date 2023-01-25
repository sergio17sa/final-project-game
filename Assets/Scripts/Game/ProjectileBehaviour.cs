using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    private Vector3 _targetPosition;
    private bool _canShoot = false;

    public event EventHandler OnReachTarget;

    private void Update()
    {
        if (!_canShoot && _targetPosition != null) return;



        Vector3 moveDirection = (_targetPosition - transform.position).normalized;

        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * 50);

        float distanceBeforeMoving = Vector3.Distance(transform.position, _targetPosition);

        float moveSpeed = 20f;
        
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(90, 0, 0);

        float distanceAfterMoving = Vector3.Distance(transform.position, _targetPosition);

        if (distanceBeforeMoving < distanceAfterMoving)
        {
            OnReachTarget?.Invoke(this, EventArgs.Empty);

            _canShoot = false;
        }
    }

    public void SetShoot(Vector3 targetPosition, bool canShoot)
    {
        _targetPosition = targetPosition;
        _canShoot = canShoot;
        transform.parent = null;
    }
}
