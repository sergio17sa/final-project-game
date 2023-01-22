using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum Team
{
    Team1,
    Team2
}
public class TurnSystemManager : Singleton<TurnSystemManager>
{
    public event EventHandler OnTurnChanged;
    private Team _teamTurn;
    public int TurnCounter { get; private set; } = 0;
    public GameObject camera1, camera2;

    public bool camera;

    protected override void Awake()
    {
        base.Awake();
        NextTurn();
    }

    public void NextTurn()
    {
        TurnCounter++;
        SetTeamTurn();
        Debug.Log(TurnCounter);
        OnTurnChanged += ToggleCamera;
        OnTurnChanged?.Invoke(this, EventArgs.Empty);
    }

    private void SetTeamTurn()
    {
        if (TurnCounter % 2 == 0) _teamTurn = Team.Team2;
        else _teamTurn = Team.Team1;
    }

    public Team GetTeamTurn() => _teamTurn;

    private void ToggleCamera(object sender, EventArgs e)
    {
        if(TurnCounter % 2 != 0){
            camera1.SetActive(true);
            camera2.SetActive(false);
        } else {
            camera1.SetActive(false);
            camera2.SetActive(true);
        }
    }
}
