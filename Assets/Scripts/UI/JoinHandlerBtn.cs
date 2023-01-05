using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JoinHandlerBtn : MonoBehaviour
{
    [SerializeField] TMP_Text joinButtonText;

    private int lobbiesQuantity;
    private string joinGame = "Join Game", CreateGameAndJoin = "Create Game And Join";

    private void Start()
    {
        SetJoinButtonText();
        Debug.Log($"lobbies save in joinhandler are {lobbiesQuantity}");
    }

    public async void ToggleJoinMode()
    {
        Debug.Log($"entro al toggle y la cantidad de lobbies es {lobbiesQuantity}");
        if (lobbiesQuantity <= 0)
        {
            await LobbyManager.Instance.StartGameCreatingLobby();
        }
        else
        {
            await LobbyManager.Instance.StartGameQuick();
        }
    }

    public void SetJoinButtonText()
    {

      //  lobbiesQuantity = GameManager.Instance.lobbiesQuantity;
        if (lobbiesQuantity <= 0)
        {
            joinButtonText.text = CreateGameAndJoin;
        }
        else
        {
            joinButtonText.text = joinGame;
        }
    }
}
