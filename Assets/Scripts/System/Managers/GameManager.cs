using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : Singleton<GameManager>
{
    [Header("Setup")]
    public bool isActive;
    
    private void Start()
    {
        StartCoroutine(StartGame());
    }

    public IEnumerator StartGame()
    {
        ScenesManager.Instance.ui.panelRuler.SetActive(true);
        yield return new WaitUntil(() => !ScenesManager.Instance.ui.panelRuler.activeInHierarchy);
        isActive = true;
    }

    public IEnumerator Winner()
    {
        isActive = false;
        SoundManager.Instance.PauseAllSounds(true);
        SoundManager.Instance.PlayNewSound("Winner");
        yield return new WaitForSeconds(5f);
        ScenesManager.Instance.ui.panelWinner.SetActive(true);
        yield return new WaitUntil(() => !ScenesManager.Instance.ui.panelWinner.activeInHierarchy);
        ScenesManager.Instance.RestartMainMenu();
    }
    public IEnumerator Losser()
    {
        isActive = false;
        SoundManager.Instance.PauseAllSounds(true);
        SoundManager.Instance.PlayNewSound("Losser");
        yield return new WaitForSeconds(5f);
        ScenesManager.Instance.ui.panelLosser.SetActive(true);
        yield return new WaitUntil(() => !ScenesManager.Instance.ui.panelLosser.activeInHierarchy);
        ScenesManager.Instance.RestartMainMenu();
    }

    
    // Update is called once per frame
    void Update()
    {
        if (!isActive)
            return;

        if (Input.GetKeyDown(KeyCode.P) && isActive || Input.GetKeyDown(KeyCode.Escape) && isActive)
        {
            ScenesManager.Instance?.Pause();
        }

    }
}
