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

    }

    protected override IEnumerator StartGame()
    {
        TurnSystemManager.Instance.OnTurnChanged += ToggleCamera;
        yield return null;
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

}


