using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private GameObject _hightlight;


    private void OnMouseEnter()
    {
        _hightlight?.SetActive(true);

    }

    private void OnMouseExit()
    {
        _hightlight?.SetActive(false);
    }
}
