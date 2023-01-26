using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManagerMultiplayerLocal : GameManager
{
    [SerializeField] private GameObject camera1, camera2;
    private void Start()
    {
        TurnSystemManager.Instance.OnTurnChanged += ToggleCamera;
        base.Start();
        TurnSystemManager.Instance.player1name = "MEDIEVAL TEAM";
        TurnSystemManager.Instance.player2name = "FUTURE TEAM";
    }

    protected override void StartGame()
    {
        gameMode = GameMode.multiplayerMode;
        Debug.Log(gameMode);
        isActive = true;
    }

    private void OnDisable()
    {
        TurnSystemManager.Instance.OnTurnChanged -= ToggleCamera;
    }

    private void ToggleCamera(object sender, EventArgs e)
    {
        if (TurnSystemManager.Instance.GetTeamTurn() == Team.Team1)
        {
            camera1.SetActive(true);
            camera2.SetActive(false);
        }
        else
        {
            camera1.SetActive(false);
            camera2.SetActive(true);
        }
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
            if (_spawnManager.SpawnedMedievalTeam.Count > 0)
            {
                message = "Medieval Team Wins";
            }

            if (_spawnManager.SpawnedFutureTeam.Count > 0)
            {
                message = "Future Team Wins";
            }
            
           StartCoroutine(EndMatch());
        }
    }
}


