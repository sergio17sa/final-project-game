using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AuthenticateUI : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(async () =>
        {
            await RelayManager.instance.UnityServicesConnection();
            await RelayManager.instance.anonymouslyAuthentication();
           // hide();
        });
    }

    public void hide()
    {
        gameObject.SetActive(false);
    }
}
