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



public class Relay : MonoBehaviour
{



    const int m_MaxConnections = 4;
    public string relayJoinCode;

    /// <summary>
    /// petición asincrona para conectar con unity game services cuando el juego inicie (RELAY, LOBBY)
    /// peticion asincrona para hacer una autenticación anonima del usurio que devuelve un playerID y un token de sesion
    /// </summary>
    /// <returns></returns>
    private async void Start()
    {
        // TODO HACER METODOS PARA HACER LA CONEXION CON UNITY SERVICES Y CREAR LA AUTENTICACION DEL USUARIO POR SEPARADO
        try
        {

            await UnityServices.InitializeAsync();
        }
        catch (System.Exception ex)
        {

            Debug.LogException(ex); ;
        }

        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            Debug.Log("signIn anonymoisly successfull");
            Debug.Log($"playerID: {AuthenticationService.Instance.PlayerId}");
        }
        catch (AuthenticationException ex)
        {
            //TODO Investigar Como responder con codigos de excepcion en unity ¿?
            Debug.LogException(ex);
        }

        DebugLogConsole.AddCommandStatic("relayMethod", "create allocation server and joincode", "AllocateRelayServerAndGetJoinCode", typeof(Relay));
        DebugLogConsole.AddCommandStatic("joinAllocation", "use joincode to join to allocation server ", "JoinToAllocation", typeof(Relay));

    }

    public static async Task<RelayServerData> AllocateRelayServerAndGetJoinCode(int maxConnections)
    {
        Allocation allocation;
        string createJoinCode;

        try
        {
            allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
            Debug.Log($"JoinCode: {allocation}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Relay create allocation request failed {e.Message}");
            throw;

        }

        Debug.Log($"server: {allocation.ConnectionData[0]} {allocation.ConnectionData[1]}");
        Debug.Log($"server: {allocation.AllocationId}");

        try
        {
            createJoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log($"JoinCode: {createJoinCode}");
        }
        catch
        {

            Debug.LogError("Relay create join code request failed");
            throw;
        }

        RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
        NetworkManager.Singleton.StartHost();



        return relayServerData;

    }



    public static async Task<RelayServerData> JoinToAllocation(string joinCode)
    {
        JoinAllocation joinAllocation;
        Debug.Log(joinCode);
        try
        {

            joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
        RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
        NetworkManager.Singleton.StartClient();
            Debug.Log("join to allocation successfull");
        }
        catch (System.Exception)
        {

            Debug.LogError("Relay create join code request failed");
            throw;
        }


        return new RelayServerData(joinAllocation, "dtls");;

    }


}
