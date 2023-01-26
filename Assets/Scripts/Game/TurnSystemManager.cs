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
    public string player1name;
    public string player2name;


    private Team _teamTurn;
    public int TurnCounter { get; private set; } = 0;

    private void Start()
    {
        StartCoroutine(StartMatch());
    }

    IEnumerator StartMatch()
    {
        infoHolder.SetActive(false);

        yield return new WaitForSeconds(3.5f);

        infoHolder.SetActive(true);
        yield return new WaitForSeconds(3);
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
        if (TurnCounter % 2 == 0)
        {
            yield return new WaitForSeconds(1f);
            infoHolder.SetActive(true);
            nameText.text = player2name + " TURN";
            yield return new WaitForSeconds(2f);
            infoHolder.SetActive(false);
        }
        else
        {
            yield return new WaitForSeconds(1.5f);
            infoHolder.SetActive(true);
            nameText.text = player1name + " TURN";
            yield return new WaitForSeconds(3f);
            infoHolder.SetActive(false);
        }
    }

    public Team GetTeamTurn() => _teamTurn;

}
