using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NetworkManagerServer : MonoBehaviour
{
    public int lobbiesQuantity { get; private set; }
    public bool checkForLobbies;

    [SerializeField] private TMP_Text joinButtonText;
    [SerializeField] private GameObject panelUI;

    private string joinGame = "Join Game", CreateGameAndJoin = "Create Game And Join";

    private async void Start()
    {
        await RelayManager.Instance.UnityServicesConnection();
        await RelayManager.Instance.anonymouslyAuthentication();
        lobbiesQuantity = await LobbyManager.Instance.ListLobbies();
    }

    public void ToggleChecForlobbies()
    {
        if (lobbiesQuantity != 0)
        {
            checkForLobbies = true;
        }
        else
        {
            checkForLobbies = false;
        }
    }

    public IEnumerator JoinBtnTrasition()
    {
        yield return new WaitForSecondsRealtime(3f);
        joinButtonText.text = "Looking for a lobby Game...";
        yield return new WaitForSecondsRealtime(4f);
        if (checkForLobbies)
        {
            joinButtonText.text = "Lobby Game found";
        }
        else
        {
            joinButtonText.text = "Creating new lobby game..";
        }
        yield return new WaitForSecondsRealtime(4f);
        ScenesManager.Instance.LoadLevel("Multiplayerlocal");
        yield return new WaitForSecondsRealtime(4f);
        List();
    }

    public async void List()
    {
        if (checkForLobbies)
        {
            await LobbyManager.Instance.StartGameQuick();
        }
        else
        {
            await LobbyManager.Instance.StartGameCreatingLobby();
        }
    }

    public void StartBtnTransition()
    {
        StartCoroutine("JoinBtnTrasition");
    }

}




