using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerSingle : GameManager
{
    private void Start()
    {
        base.Start();
        TurnSystemManager.Instance.player1name = "PLAYER";
        TurnSystemManager.Instance.player2name = "ENEMY";
    }
    protected override void StartGame()
    {
        gameMode = GameMode.IAMode;
        Debug.Log(gameMode);
        isActive = true;
    }

    protected override void Character_OnDead(object sender, EventArgs e)
    {
        Character character = (Character)sender;

        GridManager.Instance.ClearCharacterAtTilePosition(character.CharacterTilePosition);

        if (character.GetCharacterTeam() == Team.Team1)
        {
            _spawnManager.SpawnedMedievalTeam.Remove(character.gameObject);
        }
        else
        {
            _spawnManager.SpawnedFutureTeam.Remove(character.gameObject);
        }

        if (_spawnManager.SpawnedMedievalTeam.Count == 0 || _spawnManager.SpawnedFutureTeam.Count == 0)
        {
            for (int i = 0; i < _spawnManager.SpawnedFutureTeam.Count; i++)
            {
                Kill();
            }

            if (_spawnManager.SpawnedMedievalTeam.Count > 0)
            {
                Victory();
                message = "You Are Winner";
            }

            if (_spawnManager.SpawnedFutureTeam.Count > 0)
            {
                Loss();
                message = "You Are loser";
            }

            StartCoroutine(EndMatch());
        }
    }
}
