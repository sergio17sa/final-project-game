using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : Singleton<GameManager>
{
    [Header("Setup")]
    public bool isActive;
    public List<GameObject> players = new List<GameObject>();

    public enum modeGame
    {
       multiplayerMode,
       IAMode
    }


    public void AddPlayer(GameObject newPlayer)
    {
        players.Add(newPlayer);
        Debug.Log("ADD new player");
        //foreach (GameObject item in players)
        //{
        //    item.GetComponent<NetworkPlayer>().SetPlayerName();
        //}

    }


    private void Start()
    {
        //  StartCoroutine(StartGame());
        SpawnManager.OnVictory += victory;
        SpawnManager.OnKill += kill;
        SpawnManager.OnLoss += Loss;
    }


    // public IEnumerator StartGame()
    // {
    //     ScenesManager.Instance.ui.panelRuler.SetActive(true);
    //     yield return new WaitUntil(() => !ScenesManager.Instance.ui.panelRuler.activeInHierarchy);
    //     isActive = true;
    // }

    public IEnumerator Winner()
    {
        isActive = false;
        SoundManager.Instance.PauseAllSounds(true);
        SoundManager.Instance.PlayNewSound("Winner");
        yield return new WaitForSeconds(5f);
        ScenesManager.Instance.ui.panelWinner.SetActive(true);
        yield return new WaitUntil(() => !ScenesManager.Instance.ui.panelWinner.activeInHierarchy);
        ScenesManager.Instance.RestartMainMenu();
    }
    public IEnumerator Losser()
    {
        isActive = false;
        SoundManager.Instance.PauseAllSounds(true);
        SoundManager.Instance.PlayNewSound("Losser");
        yield return new WaitForSeconds(5f);
        ScenesManager.Instance.ui.panelLosser.SetActive(true);
        yield return new WaitUntil(() => !ScenesManager.Instance.ui.panelLosser.activeInHierarchy);
        ScenesManager.Instance.RestartMainMenu();
    }


    // Update is called once per frame
    void Update()
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
