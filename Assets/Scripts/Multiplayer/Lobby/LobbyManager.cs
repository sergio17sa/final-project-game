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
    }

    public async void CreateLobby(string lobbyName)
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
                Debug.Log(lobby.Name + " " + lobby.MaxPlayers + " " + "Lobby Data: " + lobby.Data["startGame"].Value);
            }

        }
        catch (LobbyServiceException e)
        {
            Debug.LogException(e);
        }
    }

    private async void JoinLobbyByLoobyCode(string lobbyCode)
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
            PrintPlayersData(lobby);
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
            PrintPlayersData(lobby);
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
}


