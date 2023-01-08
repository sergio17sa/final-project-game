using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkUIController : NetworkBehaviour
{
    // private NetworkVariable<NetString> playerServer = new NetworkVariable<NetString>(
    //   new NetString
    //   {
    //       nameString = "0",
    //       id = 0,
    //       isReady = false
    //   }, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    // private NetworkVariable<NetString> playerClient = new NetworkVariable<NetString>(
    //     new NetString
    //     {
    //         nameString = "1",
    //         id = 1,
    //         isReady = false
    //     }, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    // public struct NetString : INetworkSerializable
    // {
    //     public string nameString;
    //     public int id;
    //     public bool isReady;

    //     public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    //     {
    //         serializer.SerializeValue(ref nameString);
    //         serializer.SerializeValue(ref id);
    //         serializer.SerializeValue(ref isReady);
    //     }
    // }

    // public override void OnNetworkSpawn()
    // {
    //     base.OnNetworkSpawn();
    //     playerServer.OnValueChanged += (NetString previousValue, NetString newValue) =>
    //     {
    //         Debug.Log(OwnerClientId + " playerServer value: " + playerServer.Value.nameString);
    //         UIManager.Instance.playerServer.text = $"{playerServer.Value.nameString} {playerServer.Value.id.ToString()} {playerServer.Value.isReady.ToString()}";
    //     };

    //     playerClient.OnValueChanged += (NetString previousValue, NetString newValue) =>
    //     {
    //         Debug.Log(OwnerClientId + " playerClient value: " + playerClient.Value.nameString);
    //         UIManager.Instance.playerClient.text = $"{playerClient.Value.nameString} {playerClient.Value.id.ToString()} {playerClient.Value.isReady.ToString()}";
    //     };

    // }

    // public void SetPlayerName(int id, bool isReady)
    // {

    //     if (OwnerClientId == 0)
    //     {
    //         playerServer.Value = new NetString
    //         {
    //             nameString = "Player Server",
    //             id = id,
    //             isReady = isReady
    //         };
    //     }
    //     if (OwnerClientId == 1)
    //     {
    //         playerClient.Value = new NetString
    //         {
    //             nameString = "Player Client",
    //             id = id,
    //             isReady = isReady
    //         };
    //     }
    // }

    // public void test()
    // {
    //     int randomId = Random.Range(0, 99);

    //     if (Input.GetKeyDown(KeyCode.F))
    //     {
    //         SetPlayerName(randomId, true);
    //     }
    // }
}
