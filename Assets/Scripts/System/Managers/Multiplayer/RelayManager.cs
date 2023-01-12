using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Services.Relay.Http;
using Unity.Services.Relay.Models;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport;
using Unity.Networking.Transport.Relay;
using NetworkEvent = Unity.Networking.Transport.NetworkEvent;
using IngameDebugConsole;


public class RelayManager : MonoBehaviour
{
    private string relayJoinCode;
    private Allocation allocation;
    private JoinAllocation joinAllocation;
    int maxConnections = 1, randomNumber;
    string playerName = "PlayerName";

    public static RelayManager Instance;

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

    private void Start()
    {
        //  await UnityServicesConnection();
        //  await anonymouslyAuthentication();
        randomNumber = UnityEngine.Random.Range(1, 100);
        DebugLogConsole.AddCommandInstance("relayAllocation", "create allocation server and joincode", "RelayCreateAllocation", this);
        DebugLogConsole.AddCommandInstance("joinAllocation", "use joincode to join to allocation server ", "JoinToAllocation", this);
    }

    /// <summary>
    /// Initialize the Unity Services engine
    /// </summary>
    public async Task UnityServicesConnection()
    {
        // Temporary variable to test multiple lobbies in the same device 
        var initializationOptions = new InitializationOptions();
        initializationOptions.SetProfile(playerName + randomNumber);

        try
        {

            await UnityServices.InitializeAsync(initializationOptions);
            await UnityServices.InitializeAsync();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"It was not possible to connect with unity services {e.Message}");
            Debug.LogException(e);
        }
    }

    /// <summary>
    /// Anonymously Auyhentication player
    /// </summary>
    public async Task anonymouslyAuthentication()
    {
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            //If not already logged, log the user in
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();

                Debug.Log("signIn anonymously successfull");
                Debug.Log($"playerID: {AuthenticationService.Instance.PlayerId}");
            }
            catch (AuthenticationException e)
            {
                Debug.LogError("Anonymously authentication failed");
                Debug.LogException(e);
            }
        }
    }

    /// <summary>
    /// ask Unity Services to allocate a Relay server and passing quantity connections like parameter
    /// </summary>
    /// <param name="maxConnections"></param>
    /// <returns> allocation </returns>
    public async Task<String> RelayCreateAllocation()
    {
        string joinCodeData;
        try
        {
            allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
            Debug.Log($"Allocation: {allocation}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Relay create allocation request failed {e.Message}");
            throw;
        }

        Debug.Log($"server: {allocation.ConnectionData[0]} {allocation.ConnectionData[1]}");
        Debug.Log($"server: {allocation.AllocationId}");

        joinCodeData = await GetJoinCode();

        return joinCodeData;
    }


    /// <summary>
    /// Retrieve the Relay join code for our clients to join our game
    /// </summary>
    /// <returns></returns>
    public async Task<String> GetJoinCode()
    {
        try
        {
            relayJoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log($"JoinCode: {relayJoinCode}");
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            Debug.LogError(e.Message);
            throw;
        }
 
        ConfigureTransportAndStartNgoAsHost();

        return relayJoinCode;
    }

    /// <summary>
    /// RelayServerData represents the necessary information
    /// for a Host to host a game on a Relay and start the game like host
    /// </summary>
    public RelayServerData ConfigureTransportAndStartNgoAsHost()
    {

        RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
        //Retrieve the Unity transport used by the NetworkManager
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
        NetworkManager.Singleton.StartHost();

        Debug.Log(relayJoinCode);

        return relayServerData;
    }

    /// <summary>
    /// Join a Relay server based on the JoinCode received from the Host or Server
    /// </summary>
    public async Task<JoinAllocation> JoinToAllocation(string joinCode)
    {
        try
        {
            //Ask Unity Services to join a Relay allocation based on our join code
            joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            Debug.Log("join to allocation successfull");
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            Debug.LogError(e.Message);

        }

        ConfigureTransportAndStartNgoAsPlayer();
        return joinAllocation;
    }

    /// <summary>
    /// RelayServerData represents the necessary information
    /// for a player to join a game on a Relay and start the game like client
    /// </summary>
    public RelayServerData ConfigureTransportAndStartNgoAsPlayer()
    {
        Debug.Log(joinAllocation.AllocationId);
        RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
        //Retrieve the Unity transport used by the NetworkManager
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
        NetworkManager.Singleton.StartClient();

        return new RelayServerData(joinAllocation, "dtls");
    }
}
