using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using IngameDebugConsole;
using System.Collections;

public class LobbyManager : MonoBehaviour
{

    private int maxPLayers = 2;
    private Lobby hostLobby, joinedLobby;
    private int randomName;
    private string playerName = "playernametest1", startGame = "0";

    private void Start()
    {
        randomName = UnityEngine.Random.Range(10, 100);
        DebugLogConsole.AddCommandInstance("CreateLobby", "create lobby", "CreateLobby", this);
        DebugLogConsole.AddCommandInstance("ListLobbies", "List lobbies", "ListLobbies", this);
        DebugLogConsole.AddCommandInstance("JoinLobby", "JoinLobby", "JoinLobbyByLoobyCode", this);
        DebugLogConsole.AddCommandInstance("QuickJoinLobby", "QuickJoinLobby", "QuickJoinLobby", this);
        DebugLogConsole.AddCommandInstance("UpdateLobbyData", "UpdateLobbyData", "UpdateLobbyData", this);
        DebugLogConsole.AddCommandInstance("PrintPlayers", "PrintPlayers", "PrintPlayers", this);
        DebugLogConsole.AddCommandInstance("StartGameCreatingLobby", "StartGameCreatingLobby", "StartGameCreatingLobby", this);
        DebugLogConsole.AddCommandInstance("StartGameQuick", "StartGameQuick", "StartGameQuick", this);
    }

    public async Task CreateLobby(string lobbyName)
    {
        try
        {
            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
            {
                IsPrivate = false,
                Player = GetPlayer(),
                Data = new Dictionary<string, DataObject>{
                    {"startGame", new DataObject( DataObject.VisibilityOptions.Member, startGame)}
                }
            };
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPLayers, createLobbyOptions);
            hostLobby = lobby;
            joinedLobby = hostLobby;


            StartCoroutine(HeartBeatLobbyCoroutine(lobby.Id, 15));
            StartCoroutine(PollForUpdates(2f));

            Debug.Log($"lobby created with name {lobby.Name}   quianty players: {lobby.MaxPlayers} Lobby ID: {lobby.Id}  lobby Code: {lobby.LobbyCode}");
            PrintPlayersData(hostLobby);
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

    public async Task ListLobbies()
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
                Debug.Log(lobby.Name + " " + lobby.MaxPlayers + " " + "Lobby Data: " + lobby.Data["startGame"].Value);
            }

        }
        catch (LobbyServiceException e)
        {
            Debug.LogException(e);
        }
    }

    private async Task JoinLobbyByLoobyCode(string lobbyCode)
    {
        try
        {
            //QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();
            JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions
            {
                Player = GetPlayer()
            };
            Lobby lobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, joinLobbyByCodeOptions);
            joinedLobby = lobby;
            Debug.Log($"Joined successfull to {lobby.Name} with lobby code: {lobbyCode}");
            PrintPlayersData(joinedLobby);
            StartCoroutine(PollForUpdates(2));
        }

        catch (LobbyServiceException e)
        {
            Debug.LogException(e);
        }
    }

    public async Task QuickJoinLobby()
    {
        try
        {
            QuickJoinLobbyOptions quickJoinLobbyOptions = new QuickJoinLobbyOptions
            {
                Player = GetPlayer()
            };
            Lobby lobby = await Lobbies.Instance.QuickJoinLobbyAsync(quickJoinLobbyOptions);
            joinedLobby = lobby;
            Debug.Log($"Joined successfull to {lobby.Name} with lobby code: {lobby.LobbyCode}");
            PrintPlayersData(joinedLobby);
            StartCoroutine(PollForUpdates(2f));
        }
        catch (LobbyServiceException e)
        {
            Debug.LogException(e);
        }
    }

    public async Task UpdateLobbyData(string dataToUpdate)
    {
        UpdateLobbyOptions updateLobbyOptions = new UpdateLobbyOptions
        {
            Data = new Dictionary<string, DataObject>{
                {"startGame", new DataObject(DataObject.VisibilityOptions.Member, dataToUpdate)}
            }
        };

        try
        {
            hostLobby = await Lobbies.Instance.UpdateLobbyAsync(hostLobby.Id, updateLobbyOptions);
            joinedLobby = hostLobby;

        }
        catch (LobbyServiceException e)
        {
            Debug.LogException(e);
        }
    }


    public Player GetPlayer()
    {
        return new Player
        {
            Data = new Dictionary<string, PlayerDataObject>{
                        {"PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, playerName + randomName)}
                    }
        };
    }

    public void PrintPlayers()
    {
        PrintPlayersData(joinedLobby);
    }

    public void PrintPlayersData(Lobby lobby)
    {
        foreach (Player player in lobby.Players)
        {
            Debug.Log($"player id: {player.Id} joined to lobby {lobby.Name} - palyer Name: {player.Data["PlayerName"].Value} - Lobby data: {lobby.Data["startGame"].Value}");
        }
    }


    public IEnumerator PollForUpdates(float waitForSeconds)
    {
        Debug.Log("ENTRO A LA COROUTINE POLLING");
        var delay = new WaitForSeconds(waitForSeconds);

        while (joinedLobby != null)
        {
            var getLobbies = Task.Run(async () =>
            await Lobbies.Instance.GetLobbyAsync(joinedLobby.Id)).ContinueWith((getLobbies) =>
            {
                if (getLobbies.IsFaulted)
                {
                    Debug.LogException(getLobbies.Exception);
                }
                if (getLobbies.IsCompleted)
                {
                    joinedLobby = getLobbies.Result;
                    Debug.Log(getLobbies.Result);

                }

            });
            yield return delay;
        }
    }

    public bool IsLobbyHost()
    {
        bool isHost;
        return isHost = hostLobby != null ? true : false;
    }


    public async Task StartGameCreatingLobby()
    {

        string relayJoinCode;
        await CreateLobby($"Lobby relay {randomName}");

        if (IsLobbyHost())
        {
            relayJoinCode = await RelayManager.instance.RelayCreateAllocation();

            await UpdateLobbyData(relayJoinCode);
        }
    }

    public async void StartGameQuick()
    {

        try
        {
            await QuickJoinLobby();
            Debug.Log(joinedLobby.Data["startGame"].Value);
            if (joinedLobby.Data["startGame"].Value != "0")
            {
                Debug.Log("SE UNIO AL LOBBY QUICKLY");
                await RelayManager.instance.JoinToAllocation(joinedLobby.Data["startGame"].Value);
            }
        }
        catch (LobbyServiceException e)
        {

            Debug.Log("esta fallando aca");
            Debug.LogException(e);
        }
    }
}


