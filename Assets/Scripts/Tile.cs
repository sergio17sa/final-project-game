using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Tile : MonoBehaviour
{
    public Unit unitOnTile;

    [SerializeField] private bool _isWalkable;
    [SerializeField] private GameObject _hightlight;
    [field: SerializeField] public GameObject RangeHighlight { get; private set; }
    public bool IsInRange { get; set; } = false;
    public bool IsCheck { get; set; } = false;
    public int Visited { get; set; } = -1;
    public Tile Parent { get; set; } = null;

    //change later


    private void OnMouseEnter()
    {
        _hightlight.SetActive(true);

    }

    private void OnMouseExit()
    {
        _hightlight.SetActive(false);
    }

    public bool Walkable => _isWalkable && unitOnTile == null;

    public void SetUnit(Unit unit)
    {
        if(unitOnTile != null) return;
        unit.transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        unitOnTile = unit;
        unit.UnitTile = this;
    }

    public void CleanUnit(Unit unit)
    {
        unit.UnitTile = null;
        unitOnTile = null;
    }

    //Move later
    private void OnMouseDown()
    {
        ActionManager.Instance.MouseDown(this);
    }

    
}
