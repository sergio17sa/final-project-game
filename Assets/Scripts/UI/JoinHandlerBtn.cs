using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class JoinHandlerBtn : MonoBehaviour
{

    [SerializeField] private TMP_Text joinButtonText;
    private string lookingForLobby = "Looking for a lobby Game...", lobbyFound = "Lobby Game found", creatingLobby = "Creating new lobby game..";

    public IEnumerator JoinBtnTrasition()
    {
        yield return new WaitForSecondsRealtime(3f);
        joinButtonText.text = lookingForLobby;
        NetworkManagerServer.Instance.GetLobbies();

        yield return new WaitForSecondsRealtime(1f);
        NetworkManagerServer.Instance.ToggleCheckForlobbies();

        yield return new WaitForSecondsRealtime(3f);
        if (NetworkManagerServer.Instance.checkForLobbies)
        {
            joinButtonText.text = lobbyFound;
        }
        else
        {
            joinButtonText.text = creatingLobby;
        }

        yield return new WaitForSecondsRealtime(2f);
        SceneManager.LoadSceneAsync(1);
        NetworkManagerServer.Instance.CheckLobbiesToStart();
    }

    public void StartBtnTransition()
    {
       SceneManager.LoadSceneAsync("Multiplayer");
        //StartCoroutine("JoinBtnTrasition");
    }
}
