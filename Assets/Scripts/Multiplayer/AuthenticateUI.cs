using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AuthenticateUI : MonoBehaviour
{

    // [SerializeField] private Button autheticate;
    // [SerializeField] private Button createLobby;
    [SerializeField] private Button joinLobby;
    private void Awake()
    {


        // autheticate.onClick.AddListener(async () =>
        // {
        //     await RelayManager.Instance.UnityServicesConnection();
        //     await RelayManager.Instance.anonymouslyAuthentication();
        //     hide(autheticate);
        // });


        // createLobby.onClick.AddListener(async () =>
        // {
        //     await LobbyManager.Instance.StartGameCreatingLobby();
        //     hide(createLobby);
        // });

        joinLobby.onClick.AddListener(async () =>
        {
            await LobbyManager.Instance.StartGameQuick();
            hide(joinLobby);
        });

    }


    public void hide(Button buttonToSet)
    {
        buttonToSet.gameObject.SetActive(false); ;
    }
}