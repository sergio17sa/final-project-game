using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class NetworkPlayer : NetworkBehaviour
{

    private NetworkVariable<NetString> player1Account = new NetworkVariable<NetString>(
        new NetString
        {
            nameString = "test",
            id = 0,
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
    public GameObject playerPrefab;
    public int clientID;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        player1Account.OnValueChanged += (NetString previousValue, NetString newValue) =>
        {

            Debug.Log(OwnerClientId + " player1Account value: " + player1Account.Value.nameString + " " + player1Account.Value.isReady + " " + player1Account.Value.id);
            UIManager.Instance.player1Name.text = player1Account.Value.nameString + player1Account.Value.isReady.ToString() + player1Account.Value.id.ToString();

        };

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

    public NetString SetPlayerName()
    {

        return player1Account.Value = new NetString {
            nameString = "points",
            id = Random.Range(0,100),
            isReady = true
        };
        // {
        //     player1Account.Value = new NetString()
        //     {
        //        // nameString = namePlayer,
        //         id = playerId,
        //      //   isReady = true
        //     };
        // }

        // if (playerId == 2)
        // {
        //     player2Account.Value = new NetString()
        //     {
        //         nameString = namePlayer,
        //         id = playerId,
        //         isReady = false
        //     };
        // }
    }


    void test()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            //UIManager.Instance.networkUI.SetPlayerName("Oscarxd", 1);
            //UIManager.Instance.networkUI.SetPlayerName( 1);
            // UIManager.Instance.networkUI.Test(true, 1);
            SetPlayerName();
        }

        // if (Input.GetKeyDown(KeyCode.S))
        // {
        //     // UIManager.Instance.networkUI.SetPlayerName("Andreyxd", 2);
        //     //UIManager.Instance.networkUI.SetPlayerName( 2);
        //    // UIManager.Instance.networkUI.Test(false, 2);
        // }
    }
}