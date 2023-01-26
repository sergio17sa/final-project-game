using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public abstract class GameManager : Singleton<GameManager>
{
    [Header("Setup")]
    public bool isActive;
    protected string message;

    protected SpawnManager _spawnManager;

    public enum GameMode
    {
        multiplayerMode,
        IAMode
    }

    public static GameMode gameMode;

    private void Awake()
    {
        base.Awake();
        StartGame();
    }

    protected virtual void Start()
    {
        Character.OnDead += Character_OnDead;
        _spawnManager = SpawnManager.Instance;
    }

    protected abstract void StartGame();

    public IEnumerator EndMatch()
    {
        yield return new WaitForSeconds(3.5f);
        isActive = false;
        UIManager.Instance.endPanel.SetActive(true);
        UIManager.Instance.endPanel.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = message;
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
    }
}
