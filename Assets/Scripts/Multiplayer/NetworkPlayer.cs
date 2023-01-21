using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;
using Unity.Networking;

public class NetworkPlayer : NetworkBehaviour
{
    private NetworkVariable<NetString> playerServer = new NetworkVariable<NetString>(
        new NetString
        {
            nameString = "0",
            id = 0,
            isReady = false
        }, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private NetworkVariable<NetString> playerClient = new NetworkVariable<NetString>(
        new NetString
        {
            nameString = "1",
            id = 1,
            isReady = false
        }, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public struct NetString : INetworkSerializable
    {
        public string nameString;
        public int id;
        public bool isReady;


        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref nameString);
            serializer.SerializeValue(ref id);
            serializer.SerializeValue(ref isReady);
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        playerServer.OnValueChanged += (NetString previousValue, NetString newValue) =>
        {
            Debug.Log(OwnerClientId + " playerServer value: " + playerServer.Value.nameString);
            UIManager.Instance.playerServer.text = $"{playerServer.Value.nameString} {playerServer.Value.id.ToString()} {playerServer.Value.isReady.ToString()}";
        };

        playerClient.OnValueChanged += (NetString previousValue, NetString newValue) =>
        {
            Debug.Log(OwnerClientId + " playerClient value: " + playerClient.Value.nameString);
            UIManager.Instance.playerClient.text = $"{playerClient.Value.nameString} {playerClient.Value.id.ToString()} {playerClient.Value.isReady.ToString()}";
        };


    }

    private void Start()
    {
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;
        }
    }
    private void Update()
    {
        if (!IsOwner) return;
        test();

        if (NetworkManager.ShutdownInProgress)
        {
            Debug.Log("Network shutdown in progress");
            UIManager.Instance.connection.text = "el rival ha perdido la conexion";
            NetworkManager.Shutdown();
            LobbyManager.Instance.StopAllCoroutines();
        }
        else
        {
            //Debug.Log("esta vivo el server");
        }
        Vector3 moveDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) moveDir.z += 1f;
        if (Input.GetKey(KeyCode.S)) moveDir.z += -1f;
        if (Input.GetKey(KeyCode.D)) moveDir.x += 1f;
        if (Input.GetKey(KeyCode.A)) moveDir.x += -1f;

        float speed = 10f;
        transform.Translate(moveDir * speed * Time.deltaTime);
    }

    public override void OnDestroy()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnectCallback;
    }
    public void OnClientDisconnectCallback(ulong playerID)
    {
        NetworkManager.Shutdown();
        NetworkManagerServer.Instance.StopAllCoroutines();
    }


    public void SetPlayerName(int id, bool isReady)
    {

        if (OwnerClientId == 0)
        {
            playerServer.Value = new NetString
            {
                nameString = "Player Server",
                id = id,
                isReady = isReady
            };
        }
        if (OwnerClientId == 1)
        {
            playerClient.Value = new NetString
            {
                nameString = "Player Client",
                id = id,
                isReady = isReady
            };
        }
    }

    void test()
    {
        int randomId = Random.Range(0, 99);

        if (Input.GetKeyDown(KeyCode.F))
        {
            SetPlayerName(randomId, true);
        }
    }
}