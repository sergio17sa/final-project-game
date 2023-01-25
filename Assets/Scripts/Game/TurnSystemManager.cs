using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public GameObject infoHolder;
    public TextMeshProUGUI nameText;


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
        StartCoroutine(ShowCommunicator());
        OnTurnChanged?.Invoke(this, EventArgs.Empty);
    }

    private void SetTeamTurn()
    {
        if (TurnCounter % 2 == 0) _teamTurn = Team.Team2;
        else _teamTurn = Team.Team1;
    }

    public IEnumerator ShowCommunicator()
    {
        infoHolder.SetActive(true);

        if (TurnCounter % 2 == 0)
        {
            nameText.text = "ENEMY " + "TURN";
            yield return new WaitForSeconds(2f);
            infoHolder.SetActive(false);

        }
        else
        {
            nameText.text = "PLAYER " + "TURN";
            yield return new WaitForSeconds(3f);
            infoHolder.SetActive(false);
        }
    }

    public Team GetTeamTurn() => _teamTurn;

}
