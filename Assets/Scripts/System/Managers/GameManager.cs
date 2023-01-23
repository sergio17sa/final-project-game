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

    protected SpawnManager _spawnManager;

    public enum GameMode
    {
        multiplayerMode,
        IAMode
    }

    public static GameMode gameMode;


    protected virtual void Start()
    {
        StartCoroutine(StartGame());

        Character.OnDead += Character_OnDead;

        _spawnManager = SpawnManager.Instance;
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

    public void Victory() => StatisticsManager.Instance.ToggleVictory();
    
    public void Kill () => StatisticsManager.Instance.ToggleKill();

    public void Loss () => StatisticsManager.Instance.ToggleLoss();

    protected abstract void Character_OnDead(object sender, EventArgs e);

    private void OnDisable()
    {
        Character.OnDead -= Character_OnDead;
        ScenesManager.Instance.RestartMainMenu();
    }
}
