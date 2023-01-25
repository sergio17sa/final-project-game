using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    Character character;
    private enum State
    {
        WaitForTurn,
        TakingTurn,
        Busy
    }

    private State state;
    private float timer;

    private void Awake()
    {
        state = State.WaitForTurn;
    }

    private void Update()
    {
        if (TurnSystemManager.Instance.GetTeamTurn() == Team.Team1) return;

        timer -= Time.deltaTime;

        switch (state)
        {
            case State.WaitForTurn:
                break;
            case State.TakingTurn:
                if (timer <= 0)
                {
                    if(TryTakeAIAction(TransitionToTakingTurn))
                    {
                        state = State.Busy;
                    } 
                    else
                    {
                        TurnSystemManager.Instance.NextTurn();
                    }
                }
                break;
            case State.Busy:
                break;
        }
    }

    private void Start()
    {
        TurnSystemManager.Instance.OnTurnChanged += TurnSystemManager_OnTurnChanged;
    }

    private void OnDisable()
    {
        TurnSystemManager.Instance.OnTurnChanged -= TurnSystemManager_OnTurnChanged;
    }

    private bool TryTakeAIAction(Action OnAIActionComplete)
    {
        foreach (GameObject characterAI in SpawnManager.Instance.SpawnedFutureTeam)
        {
            if (TryTakeAIAction(characterAI, OnAIActionComplete))
            {
                return true; 
            }
        }

        return false;
    }

    private bool TryTakeAIAction(GameObject characterAI, Action OnAIActionComplete)
    {
        Character character = characterAI.GetComponent<Character>();
        character.characterParticles.StartParticle(1);

        EnemyAIAction bestEnemyAIAction = null;
        BaseAction bestBaseAction = null;

        foreach(BaseAction baseAction in character.BaseActions)
        {
            if (character.ActionsTaken.Contains(baseAction)) continue;

            if(bestEnemyAIAction == null)
            {
                bestEnemyAIAction = baseAction.GetBestAIAction();
                bestBaseAction = baseAction;
            } 
            else
            {
                EnemyAIAction testAIAction = baseAction.GetBestAIAction();
                if(testAIAction != null && testAIAction.actionValue > bestEnemyAIAction.actionValue)
                {
                    bestEnemyAIAction= testAIAction;
                    bestBaseAction = baseAction;
                    character.characterParticles.StopParticle(1);
                }
            }
        }

        if(bestEnemyAIAction != null && !character.ActionsTaken.Contains(bestBaseAction))
        {
            bestBaseAction.TakeAction(bestEnemyAIAction.tilePosition, OnAIActionComplete);
            return true;
        }

        return false;
    }

    private void TransitionToTakingTurn()
    {
        timer = 0.5f;
        state= State.TakingTurn;
    }

    private void TurnSystemManager_OnTurnChanged(object sender, EventArgs e)
    {
        if(TurnSystemManager.Instance.GetTeamTurn() == Team.Team2)
        {
            state= State.TakingTurn;
            timer = 2f;
        }
    }
}
