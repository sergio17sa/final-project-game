using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using IngameDebugConsole;
using System.Collections;

public class LobbyManager : MonoBehaviour
{

    private int maxPLayers = 2;
    private Lobby hostLobby;


    private void Start()
    {
        DebugLogConsole.AddCommandInstance("CreateLobby", "create lobby", "CreateLobby", this);
        DebugLogConsole.AddCommandInstance("ListLobbies", "List lobbies", "ListLobbies", this);
        DebugLogConsole.AddCommandInstance("JoinLobby", "JoinLobby", "JoinLobby", this);
    }

    public async void CreateLobby(string lobbyName)
    {
        try
        {
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPLayers);
            hostLobby = lobby;

            StartCoroutine(HeartBeatLobbyCoroutine(lobby.Id, 15));

            Debug.Log($"lobby created with name {lobby.Name}   quianty players: {lobby.MaxPlayers}");
        }
        catch (LobbyServiceException e)
        {
            Debug.LogException(e);
        }
    }

    public IEnumerator HeartBeatLobbyCoroutine(string lobbyId, float waitForSeconds)
    {
        var delay = new WaitForSecondsRealtime(15);

        while (true)
        {
            LobbyService.Instance.SendHeartbeatPingAsync(lobbyId);
            yield return delay;
        }
    }

    public async void ListLobbies()
    {
        try
        {

            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
            {
                // Determina la cantidad de lobbies del listado y muestra solo los que tienen espacios disponibles 
                Count = 15,
                Filters = new List<QueryFilter> {
                  new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT)
               },
                // Lobbies serán ordenados del mas antiguo al mas reciente 
                Order = new List<QueryOrder> {
                  new QueryOrder(false, QueryOrder.FieldOptions.Created)
               }

            };

            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync(queryLobbiesOptions);

            Debug.Log($"Lobbies Quantity {queryResponse.Results.Count}");
            foreach (Lobby lobby in queryResponse.Results)
            {
                Debug.Log(lobby.Name + " " + lobby.MaxPlayers);
            }

        }
        catch (LobbyServiceException e)
        {
            Debug.LogException(e);
        }
    }

    private async void JoinLobby()
    {
        try
        {
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

            Lobby lobby = await Lobbies.Instance.JoinLobbyByIdAsync(queryResponse.Results[0].Id);
            Debug.Log($"Joined successfull to {lobby.Name}");
        }
        catch (LobbyServiceException e)
        {
            Debug.LogException(e);
        }
    }
}


