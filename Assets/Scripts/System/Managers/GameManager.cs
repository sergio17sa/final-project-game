using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public abstract class GameManager : Singleton<GameManager>
{
    [Header("Setup")]
    public bool isActive;

    public enum ModeGame
    {
       multiplayerMode,
       IAMode
    }

    protected void Start()
    {
        StartCoroutine(StartGame());
        SpawnManager.OnVictory += victory;
        SpawnManager.OnKill += kill;
        SpawnManager.OnLoss += Loss;
    }

    protected abstract IEnumerator StartGame();

    public abstract void EndMatch();

    public IEnumerator Winner()
    {
        isActive = false;
        SoundManager.Instance.PauseAllSounds(true);
        SoundManager.Instance.PlayNewSound("Winner");
        yield return new WaitForSeconds(5f);
        UIManager.Instance.panelWinner.SetActive(true);
        yield return new WaitUntil(() => !UIManager.Instance.panelWinner.activeInHierarchy);
        ScenesManager.Instance.RestartMainMenu();
    }

    public IEnumerator Losser()
    {
        isActive = false;
        SoundManager.Instance.PauseAllSounds(true);
        SoundManager.Instance.PlayNewSound("Losser");
        yield return new WaitForSeconds(5f);
        UIManager.Instance.panelLosser.SetActive(true);
        yield return new WaitUntil(() => !UIManager.Instance.panelLosser.activeInHierarchy);
        ScenesManager.Instance.RestartMainMenu();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!isActive)
            return;

        if (Input.GetKeyDown(KeyCode.P) && isActive || Input.GetKeyDown(KeyCode.Escape) && isActive)
        {
            ScenesManager.Instance?.Pause();
        }
    }

    public void victory(object sender, EventArgs e)
    {
        StatisticsManager.Instance.ToggleVictory();
    }
    
    public void kill (object sender, EventArgs e)
    {
        StatisticsManager.Instance.ToggleKill();
    }
    
    public void Loss (object sender, EventArgs e)
    {
        StatisticsManager.Instance.ToggleLoss();
    }
}
