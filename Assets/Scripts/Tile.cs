using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Tile : MonoBehaviour
{
    public Unit _unit;

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

    public bool Walkable => _isWalkable && _unit == null;

    public void SetUnit(Unit unit)
    {
        if(_unit != null) return;
        unit.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        _unit = unit;
    }

    //Move later
    private void OnMouseDown()
    {
        ActionManager.Instance.MouseDown(this);
    }

    
}
