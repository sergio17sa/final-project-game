using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject _gameFinishedUI;
    [SerializeField] private TextMeshProUGUI _gameFinishedText;

    private void Start()
    {
        //SpawnManager.Instance.OnGameFinished += SpawnManager_OnGameFinished;
    }

    private void SpawnManager_OnGameFinished(object sender, EventArgs e)
    {
        _gameFinishedUI.SetActive(true);
        _gameFinishedText.text = "Game Over";
    }

    private void OnDisable()
    {
       // SpawnManager.Instance.OnGameFinished -= SpawnManager_OnGameFinished;
    }
}
