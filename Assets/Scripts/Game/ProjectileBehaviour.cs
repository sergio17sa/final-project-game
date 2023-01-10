using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProjectileBehaviour : MonoBehaviour
{
    private Vector3 _targetPosition = Vector3.zero;
    private bool _canShoot = false;


    private void Update()
    {
        if (_canShoot)
        {
            Vector3 moveDirection = (_targetPosition - transform.position).normalized;

            float distanceBeforeMoving = Vector3.Distance(transform.position, _targetPosition);

            float moveSpeed = 20f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            float distanceAfterMoving = Vector3.Distance(transform.position, _targetPosition);

            if (distanceBeforeMoving < distanceAfterMoving)
            {
                transform.position = _targetPosition;

                //trailRenderer.transform.parent = null;

                gameObject.SetActive(false);

                /*Instantiate(bulletHitVfxPrefab, targetPosition, Quaternion.identity);*/
            }
        }
        
    }

    public void SetShoot(Vector3 targetPosition, bool canShoot)
    {
        _targetPosition = targetPosition;
        _canShoot = canShoot;
        transform.parent = null;
    }
}
