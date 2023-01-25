using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class NetworkPlayer : NetworkBehaviour
{

    private NetworkVariable<NetString> playerServer = new NetworkVariable<NetString>(
        new NetString
        {
            nameString = "0",
            id = 0,
            isReady = false,
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

        public Vector3 vector3;


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

        };

        playerClient.OnValueChanged += (NetString previousValue, NetString newValue) =>
        {
            Debug.Log(OwnerClientId + " playerClient value: " + playerClient.Value.nameString);
        };

       
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;
        }
    }
    private void Update()
    {
        if (!IsOwner) return;
        test();
        Vector3 moveDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) moveDir.z += 1f;
        if (Input.GetKey(KeyCode.S)) moveDir.z += -1f;
        if (Input.GetKey(KeyCode.D)) moveDir.x += 1f;
        if (Input.GetKey(KeyCode.A)) moveDir.x += -1f;

        float speed = 10f;
        transform.Translate(moveDir * speed * Time.deltaTime);
    }

    public void SetPlayerName(int id, bool isReady)
    {

        if (OwnerClientId == 0)
        {
            playerServer.Value = new NetString
            {
                nameString = StatisticsManager.Instance.stats.playerName,
                id = id,
                isReady = isReady
            };
        }
        if (OwnerClientId == 1)
        {
            playerClient.Value = new NetString
            {
                nameString = StatisticsManager.Instance.stats.playerName,
                id = id,
                isReady = isReady
            };
        }
    }

   

    public void OnClientDisconnectCallback(ulong playerID)
    {
        if(IsServer){
            Debug.Log($"se desconecto {OwnerClientId} el cliente");
        }
    }

    public void test()
    {
        int randomId = Random.Range(0, 99);

        if (Input.GetKeyDown(KeyCode.F))
        {
            SetPlayerName(randomId, true);
        }
    }
}