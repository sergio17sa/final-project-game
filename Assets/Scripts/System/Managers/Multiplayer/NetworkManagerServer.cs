using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManagerServer : MonoBehaviour
{
    public static NetworkManagerServer Instance;
    public int lobbiesQuantity { get; private set; }
    public bool checkForLobbies { get; private set; }


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
    }

    private async void Start()
    {
        await RelayManager.Instance.UnityServicesConnection();
        await RelayManager.Instance.anonymouslyAuthentication();
    }

    public async void GetLobbies()
    {
        lobbiesQuantity = await LobbyManager.Instance.ListLobbies();
    }

    public void ToggleCheckForlobbies()
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

    public async void CheckLobbiesToStart()
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
}




