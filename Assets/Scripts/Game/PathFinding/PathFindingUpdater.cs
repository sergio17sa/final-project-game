using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PathFindingUpdater : MonoBehaviour
{
    private void Start()
    {
        MoveAction.OnStartMoving += MoveAction_OnStartMoving;
        MoveAction.OnStopMoving += MoveAction_OnStopMoving;
        Character.OnDead += Character_OnDead;
    }

    private void OnDisable()
    {
        MoveAction.OnStartMoving -= MoveAction_OnStartMoving;
        MoveAction.OnStopMoving -= MoveAction_OnStopMoving;
        Character.OnDead -= Character_OnDead;
    }


    private void MoveAction_OnStartMoving(object sender, EventArgs e)
    {
        MoveAction moveAction = (MoveAction)sender;
        Character character = moveAction.GetComponent<Character>();

        PathFinding.Instance.SetIsWalkableTile(character.CharacterTilePosition, true);
    }

    private void MoveAction_OnStopMoving(object sender, EventArgs e)
    {
        MoveAction moveAction = (MoveAction)sender;
        Character character = moveAction.GetComponent<Character>();

        PathFinding.Instance.SetIsWalkableTile(character.CharacterTilePosition, false);
    }

    private void Character_OnDead(object sender, EventArgs e)
    {
        Character character= (Character)sender;

        PathFinding.Instance.SetIsWalkableTile(character.CharacterTilePosition, true);
    }
}
