using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using IngameDebugConsole;
using System.Collections;
using Unity.Netcode;
using Unity.Services.Authentication;

public class LobbyManager : MonoBehaviour
{

    public static LobbyManager Instance;

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
    private int maxPLayers = 2;
    private Lobby hostLobby, joinedLobby;
    private QueryResponse queryResponse;
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


    /// <summary>
    /// allow to create lobby and pass it options to determinate if the lobby is public or private,  player data like name, and lobby data to storage  
    /// variables to set the lobby state or trigger other function into the game like connect the relay server.
    /// </summary>
    /// <param name="lobbyName"></param>
    /// <returns>new Lobby </returns>
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


    /// <summary>
    /// Lobbies turn off by default every 30 seconds, this coroutine send heartbeats every 15 seconds to avoid it.
    /// </summary>
    /// <param name="lobbyId"></param>
    /// <param name="waitForSeconds"></param>

    public IEnumerator HeartBeatLobbyCoroutine(string lobbyId, float waitForSeconds)
    {
        var delay = new WaitForSecondsRealtime(15);

        while (true)
        {
            LobbyService.Instance.SendHeartbeatPingAsync(lobbyId);
            yield return delay;
        }
    }

    /// <summary>
    /// it do a query to bring a lobbies list with the filters needed, in this case
    /// only bring the lobbies with available slots and those are sort by oldest to newest
    /// </summary>
    public async Task<int> ListLobbies()
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

            queryResponse = await Lobbies.Instance.QueryLobbiesAsync(queryLobbiesOptions);

            Debug.Log($"Lobbies Quantity {queryResponse.Results.Count}");
            // foreach (Lobby lobby in queryResponse.Results)
            // {
            //     Debug.Log(lobby.Name + " " + lobby.MaxPlayers + " " + "Lobby Data: " + lobby.Data["startGame"].Value);
            // }

        }
        catch (LobbyServiceException e)
        {
            Debug.LogException(e);
        }

        return queryResponse.Results.Count;
    }

    /// <summary>
    /// allow to join lobby using a lobby code that is created in CreateLobby() function
    /// </summary>
    /// <param name="lobbyCode"></param>
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
            //  PrintPlayersData(joinedLobby);
            StartCoroutine(PollForUpdates(2));
        }

        catch (LobbyServiceException e)
        {
            Debug.LogException(e);
        }
    }

    /// <summary>
    /// Easy way to join lobby, it only search to a lobby that complies with the lobby options and if find, the player join to it.
    /// </summary>
    /// <returns></returns>
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
            Debug.Log($"Joined successfull to {lobby.Name} with lobby code: {lobby.LobbyCode} lobby ID: {lobby.Id}");
            Debug.Log($"joined lobby ID {joinedLobby.Id}");
            PrintPlayersData(joinedLobby);
            StartCoroutine(PollForUpdates(2f));
        }
        catch (LobbyServiceException e)
        {
            Debug.LogException(e);
        }
    }

    /// <summary>
    /// it Allow to update the lobby data for determinate states in to the game,
    /// in this case set up the variable "startGame" to indicate that the "startGame" variable in the lobby 
    /// has or not releay join code needed to join the allocation server. 
    /// </summary>
    /// <param name="dataToUpdate"></param>

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

    /// <summary>
    /// it set up the player data to pass this options to lobby 
    /// </summary>
    /// <returns> player data to looby</returns>
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

    /// <summary>
    /// Like lobby is not a real time connection this coroutine polling every 2 
    /// seconds doing a query to get information about changes in the lobby 
    /// and pass the information between members. 
    /// </summary>
    public IEnumerator PollForUpdates(float waitForSeconds)
    {
        var delay = new WaitForSeconds(waitForSeconds);

        while (joinedLobby != null)
        {
            var getLobbies = Task.Run(async () =>
            await Lobbies.Instance.GetLobbyAsync(joinedLobby.Id)).ContinueWith((getLobbies) =>
            {
                if (getLobbies.IsFaulted)
                {
                    Debug.Log("fallo la corrutina");
                    Debug.LogException(getLobbies.Exception);
                }
                if (getLobbies.IsCompleted)
                {
                    joinedLobby = getLobbies.Result;
                    Debug.Log($" lobbies result {getLobbies.Result}");
                    Debug.Log($" lobbies result {getLobbies.Result.Players.Count}");

                }

            });
            yield return delay;
        }
    }

    /// <summary>
    /// Return a bool to check if the player is lobby host or not
    /// </summary>
    /// <returns> bool </returns>
    public bool IsLobbyHost()
    {
        bool isHost;
        return isHost = hostLobby != null ? true : false;
    }


    /// <summary>
    /// If there are not lobbies available to join, this function allow to create 
    /// one lobby, if the player is lobby host it create the allocation server, the
    /// relay join code and it join the player to the lobby.
    /// Once the player has joined, this function call UpdateLobbyData() 
    /// passing it the relay join code to set up the variable "startGame" into de the lobby
    /// </summary>


    public async Task StartGameCreatingLobby()
    {

        string relayJoinCode;
        await CreateLobby($"Lobby relay {randomName}");

        if (IsLobbyHost())
        {
            relayJoinCode = await RelayManager.Instance.RelayCreateAllocation();

            await UpdateLobbyData(relayJoinCode);
        }
    }


    /// <summary>
    /// if there are lobbies available to join, this function allow to player join to some of this lobbies 
    /// through the QuickJoinLobby() function and then call the JoinToAllocation() function in the relay maneger 
    /// to join to the relay server.
    /// </summary>
    /// <returns></returns>
    public async Task StartGameQuick()
    {

        try
        {
            await QuickJoinLobby();
            Debug.Log(joinedLobby.Data["startGame"].Value);
            if (joinedLobby.Data["startGame"].Value != "0")
            {
                Debug.Log("SE UNIO AL LOBBY QUICKLY");
                await RelayManager.Instance.JoinToAllocation(joinedLobby.Data["startGame"].Value);
            }
        }
        catch (LobbyServiceException e)
        {
            Debug.LogException(e);
        }
    }

    public async void DeleteLobby()
    {
        try
        {
            if (hostLobby.Id != null)
            {
                await LobbyService.Instance.DeleteLobbyAsync(hostLobby.Id);
                StopCoroutine("PollForUpdates");
                

            }

            if (joinedLobby.Id != null)
            {
                await LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);
                StopCoroutine("PollForUpdates");
            }
        }
        catch (LobbyServiceException e)
        {
            Debug.Log("entro al delete");
            Debug.LogException(e);
        }
    }

    public async void RemovePlayerFromLobby()
    {
        try
        {
            //Ensure you sign-in before calling Authentication Instance
            //See IAuthenticationService interface
            string playerId = AuthenticationService.Instance.PlayerId;
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, joinedLobby.Players[1].Id);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
}
