using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerMultiplayerLocal : GameManager
{
    [SerializeField] private GameObject camera1, camera2;
    protected override void Update()
    {
        base.Update();
    }

    public override void EndMatch()
    {
        Debug.Log("Game Finished");
    }

    protected override IEnumerator StartGame()
    {
        TurnSystemManager.Instance.OnTurnChanged += ToggleCamera;
        isActive = true;
        yield return null;
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
            EndMatch();

            if (_spawnManager.SpawnedMedievalTeam.Count > 0)
            {
                Debug.Log("Medieval team wins");
            }

            if (_spawnManager.SpawnedFutureTeam.Count > 0)
            {
                Loss();
                Debug.Log("Future team wins");
            }
        }
    }

}


