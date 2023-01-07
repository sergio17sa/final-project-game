using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkUIController : NetworkBehaviour
{
    public NetworkVariable<NetString> player1Account = new NetworkVariable<NetString>(new NetString()
    {
        nameString = ""
    },NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);

    public NetworkVariable<NetString> player2Account = new NetworkVariable<NetString>(new NetString()
    {
        nameString = ""
    },NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

  
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        
        
            player1Account.OnValueChanged += (oldVal, newValue) =>
            {
                UIManager.Instance.player1Name.text = newValue.nameString + newValue.id;
            };

            player2Account.OnValueChanged += (oldVal, newValue) =>
            {
                UIManager.Instance.player2Name.text = newValue.nameString + newValue.id;
            };
    }

    public void SetPlayerName(string namePlayer, int playerId)
    {
        if (playerId == 1)
        {
            player1Account.Value = new NetString()
            {
                nameString = namePlayer,
                id = playerId,
                isReady = true
            };
        }

        if (playerId == 2)
        {
            player2Account.Value = new NetString()
            {
                nameString = namePlayer,
                id = playerId,
                isReady = false
            };
        }
    }

    public void Test(bool state, int playerId)
    {
        string value;

        if (state)
        {
            value = "true";
        }
        else
        {
            value = "false";
        }

    }

    public struct NetString : INetworkSerializable, System.IEquatable<NetString>
    {
        public string nameString;
        public int id;
        public bool isReady;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            //if (serializer.IsReader)
            //{
            //    var reader = serializer.GetFastBufferReader();
            //    reader.ReadValueSafe(out nameString);
            //}
            //else
            //{
            //    var writer = serializer.GetFastBufferWriter();
            //    writer.WriteValueSafe(nameString);
            //}
            serializer.SerializeValue(ref nameString);
            serializer.SerializeValue(ref id);
            serializer.SerializeValue(ref id);

        }

        public bool Equals(NetString other)
        {
            if (string.Equals(other.nameString,nameString,System.StringComparison.CurrentCultureIgnoreCase))
                return true;

            return false;
        }
    }
}
