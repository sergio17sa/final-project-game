using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerMultiplayerLocal : GameManager
{


    protected override void Update()
    {
        base.Update();
    }

    public override void EndMatch()
    {

    }

    protected override IEnumerator StartGame()
    {
        SpawnManager.OnVictory += victory;
        SpawnManager.OnKill += kill;
        SpawnManager.OnLoss += Loss;
        yield return null;
    }

}
