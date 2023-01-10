using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class NetworkPlayer : NetworkBehaviour
{
    public void Start()
    {
        SetPlayerName();
    }


    private NetworkVariable<NetString> playerServer = new NetworkVariable<NetString>(
        new NetString
        {
            nameString = "0",
        }, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private NetworkVariable<NetString> playerClient = new NetworkVariable<NetString>(
        new NetString
        {
            nameString = "1",
        }, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public struct NetString : INetworkSerializable
    {
        public string nameString;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref nameString);
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        playerServer.OnValueChanged += (NetString previousValue, NetString newValue) =>
        {
            Debug.Log(OwnerClientId + " playerServer value: " + playerServer.Value.nameString);
            UIManager.Instance.playerServer.text = $"{playerServer.Value.nameString}";
           
        };

        playerClient.OnValueChanged += (NetString previousValue, NetString newValue) =>
        {
            Debug.Log(OwnerClientId + " playerClient value: " + playerClient.Value.nameString);
            UIManager.Instance.playerClient.text = $"{playerClient.Value.nameString}";
        };
    }
    private void Update()
    {
        if (!IsOwner) return;
        Vector3 moveDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) moveDir.z += 1f;
        if (Input.GetKey(KeyCode.S)) moveDir.z += -1f;
        if (Input.GetKey(KeyCode.D)) moveDir.x += 1f;
        if (Input.GetKey(KeyCode.A)) moveDir.x += -1f;

        float speed = 10f;
        transform.Translate(moveDir * speed * Time.deltaTime);
    }

    public void SetPlayerName()
    {
        if (OwnerClientId == 0)
        {
            playerServer.Value = new NetString
            {
                nameString = StatisticsManager.Instance.stats.playerName,
            };
        }
        if (OwnerClientId == 1)
        {
            playerClient.Value = new NetString
            {
                nameString = StatisticsManager.Instance.stats.playerName,
            };
        }
    }
}