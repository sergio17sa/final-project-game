using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Quaternion offsetRotation;

    void Awake()
    {
       // offsetRotation = transform.rotation;
    }

    void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
    }
}
