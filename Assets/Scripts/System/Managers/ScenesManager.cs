using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenesManager : Singleton<ScenesManager>
{
    /// <summary>
    /// Listado de las operaciones asincronas realizadas por el GameManager
    /// </summary>
    [SerializeField] private List<AsyncOperation> _loadOperations;

    /// <summary>
    /// Nombre de la escena que se encuentra en ejecución
    /// </summary>
    string _currentLevelName;

    /// <summary>
    /// Nombre del nivel anterior
    /// </summary>
    string _lastLevelName;

    /// <summary>
    /// Barra de carga para la pantalla Loading
    /// </summary>
    [SerializeField] GameObject loadingPanel;
    Slider slider;

    /// <summary>
    /// Variable que define si el juego esta en ejecucion.
    /// </summary>
    [SerializeField] private bool is_pause;
    
    /// <summary>
    /// Propiedad que retorna el nombre de la escena que se encuentra en ejecución.
    /// </summary>
    /// <value>Nombre de la escena.</value>
    public string CurrentLevelName { get { return _currentLevelName; } }

    /// <summary>
    /// Propiedad que retorna el estado de ejecución (T&F). 
    /// </summary>
    public bool IsPaused { get { return is_pause; } }

    void Start()
    {
        _currentLevelName = string.Empty;
        _loadOperations = new List<AsyncOperation>();
        EditMenuPrefabs();
    }

    /// <summary>
    /// Método que permite editar los prefabs necesarios para la correcta ejecución del juego
    /// </summary>
    void EditMenuPrefabs()
    {
        SoundManager.Instance.CreateSoundsLevel(MusicLevel.MAINMENU);
        SoundManager.Instance.PlayNewSound("MainBackGround");
        slider = loadingPanel.GetComponentInChildren<UnityEngine.UI.Slider>();
        UIManager.Instance.panelMainButtons.SetActive(true);
    }

    /// <summary>
    /// Delegado del método LoadLevel que se ejecuta una vez se ha terminado de cargar una escena asincronamente.
    /// </summary>
    /// <param name="ao">Objeto que contiene la información de la escena cargada.</param>
    void OnLoadOperationComplete(AsyncOperation ao)
    {
        if (_loadOperations.Contains(ao))
            _loadOperations.Remove(ao);

        ValidateLevel();
        Debug.Log("[GameManager] Escena Cargada completamente");
    }

    /// <summary>
    /// Delegado del método UnLoadLevel que se ejecuta una vez se ha terminado de descargar una escena asincronamente.
    /// </summary>
    /// <param name="ao">Objeto que contiene la información de la escena cargada.</param>
    void OnUnLoadOperationComplete(AsyncOperation ao)
    {
        if (_loadOperations.Contains(ao))
            _loadOperations.Remove(ao);
        //_currentLevelName = null;
        Debug.Log("[GameManager] Escena descargada completamente");
    }

    public void RestartMainMenu()
    {
        SceneManager.LoadSceneAsync("Boot", LoadSceneMode.Single);
    }

    /// <summary>
    /// Método que permite cargar una escena de manera asincrona
    /// </summary>
    /// <param name="levelName">Nombre de la escena que se desea cargar.</param>
    public void LoadLevel(string levelName)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(levelName,LoadSceneMode.Additive);

        if (ao == null)
        {
            Debug.Log("[GameManager] Error al cargar el nivel" + levelName);
            return;
        }

        _lastLevelName = _currentLevelName;
        _currentLevelName = levelName;
        ao.completed += OnLoadOperationComplete;
        StartCoroutine(LoadingScreen(ao));
        _loadOperations.Add(ao);
    }

    /// <summary>
    /// Método que permite descargar una escena de manera asincrona.
    /// </summary>
    /// <param name="levelName">Nombre de la escena que se desea descargar.</param>
    public void UnLoadLevel(string levelName)
    {
        AsyncOperation ao = SceneManager.UnloadSceneAsync(levelName);
        ao.completed += OnUnLoadOperationComplete;
    }

    /// <summary>
    /// Método que permite salir del juego.
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }


    /// <summary>
    /// Método que permite cambiar la calidad de graficas.
    /// </summary>
    /// <param name="qualityIndex">Numero de calidad grafica segun ployect settings.</param>
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    /// <summary>
    /// Método que permite validar el nivel cargado para determinar las acciones a realizar.
    /// </summary>
    void ValidateLevel()
    {
        if (_lastLevelName != "")
            UnLoadLevel(_lastLevelName);

        SoundManager.Instance.DeleteSoundsLevel();

        switch (CurrentLevelName)
        {
            case "Game":
                SoundManager.Instance.CreateSoundsLevel(MusicLevel.GAME);
                SoundManager.Instance.PlayNewSound("LevelBackGround");
                break;

            case "Multiplayer":
                SoundManager.Instance.CreateSoundsLevel(MusicLevel.GAME);
                SoundManager.Instance.PlayNewSound("LevelBackGround");
                break;
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(_currentLevelName));

    }

    /// <summary>
    /// Método generado por si existe algun error.
    /// </summary>
    public void LoadErrorScene()
    {
        SceneManager.LoadScene("Error");
    }

    /// <summary>
    /// Pausa la ejecución de la aplicación
    /// </summary>
    public void Pause()
    {
        is_pause = !is_pause;
        SoundManager.Instance.PauseAllSounds(is_pause);
        UIManager.Instance.pausePanel.SetActive(is_pause);

        if (is_pause)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    /// <summary>
    /// Coroutine para la pantalla de carga
    /// </summary>
    /// <returns>Proceso de carga</returns>
    /// <param name="ao">Asyncronous Operation object</param>
    IEnumerator LoadingScreen(AsyncOperation ao)
    {
        loadingPanel.SetActive(true);
        ao.allowSceneActivation = false;

        while (ao.isDone == false)
        {
            slider.value = ao.progress;
            yield return new WaitUntil(() => ao.progress.Equals(0.9f));
            slider.value = 1f;
            ao.allowSceneActivation = true;
            yield return new WaitForSeconds(2f);
            loadingPanel.SetActive(false);
            UIManager.Instance.panelMainButtons.SetActive(false);
        }
    }
}