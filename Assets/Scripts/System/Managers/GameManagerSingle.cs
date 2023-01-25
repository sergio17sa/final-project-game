using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerSingle : GameManager
{

    protected override void Start()
    {
        base.Start();
        gameMode = GameMode.IAMode;
        Debug.Log(gameMode);
    }

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
        isActive = true;
        yield return null;
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
            Kill();
        }

        if (_spawnManager.SpawnedMedievalTeam.Count == 0 || _spawnManager.SpawnedFutureTeam.Count == 0)
        {
            EndMatch();

            if (_spawnManager.SpawnedMedievalTeam.Count > 0)
            {
                Victory();
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
