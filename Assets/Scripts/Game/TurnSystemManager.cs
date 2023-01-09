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

    protected override void Awake()
    {
        base.Awake();
        NextTurn();
    }

    public void NextTurn()
    {
        TurnCounter++;
        SetTeamTurn();
        OnTurnChanged?.Invoke(this, EventArgs.Empty);
    }

    private void SetTeamTurn()
    {
        if (TurnCounter % 2 == 0) _teamTurn = Team.Team2;
        else _teamTurn = Team.Team1;
    }

    public Team GetTeamTurn() => _teamTurn;

}
