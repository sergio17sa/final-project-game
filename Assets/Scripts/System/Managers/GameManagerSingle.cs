using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerSingle : GameManager
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
        yield return null;
    }

}
