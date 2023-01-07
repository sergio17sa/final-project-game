using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class NetworkPlayer : NetworkBehaviour
{
    public GameObject playerPrefab;
    public int clientID;
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

        test();
    }

  
    void test()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            // UIManager.Instance.networkUI.SetPlayerName("Oscarxd", 1);
            UIManager.Instance.networkUI.SetPlayerName("Oscarxd", 1);
            UIManager.Instance.networkUI.Test(true, 1);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            // UIManager.Instance.networkUI.SetPlayerName("Andreyxd", 2);
            UIManager.Instance.networkUI.SetPlayerName("Andrey", 2);
            UIManager.Instance.networkUI.Test(false, 2);
        }
    }
}