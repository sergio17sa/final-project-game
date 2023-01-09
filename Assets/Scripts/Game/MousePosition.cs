using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosition : Singleton<MousePosition>
{
    [SerializeField] private LayerMask _groundLayerMask;

    public static Vector3 GetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, Instance._groundLayerMask);
        return raycastHit.point;
    }
}
