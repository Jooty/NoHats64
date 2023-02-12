using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameManager : MonoBehaviour
{

    // god forgive me for this one
    // SINGLETON
    public static GameManager instance;

    public LevelManager sceneLevelManager;
    /// <summary>
    /// A list of scene names that do *not* contain a level manager.
    /// </summary>
    public string[] nonLeveledScenes = new string[]
    {
        "MainMenu",
        "LoadingScreen",
        "MultiplayerLobby"
    };

    private void Start()
    {
        if (instance) Destroy(gameObject);
        instance = this;

        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;

#if UNITY_EDITOR
        // if this is the editor, chances are the scene wont be changing
        // so we set it manually here.
        if (Application.isEditor)
        {
            SetLevelManager();
        }
#endif
    }

    public void StartRun()
    {

    }

    public void ChangeScene(string newSceneName)
    {
        SceneManager.LoadScene(newSceneName);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void SceneManager_activeSceneChanged(Scene oldScene, Scene newScene)
    {
        SetLevelManager();
    }

    private void SetLevelManager()
    {
        // try to find the level manager
        if (nonLeveledScenes.Any(x => x == SceneManager.GetActiveScene().name)) return;
        sceneLevelManager = FindObjectOfType<LevelManager>();

        if (!sceneLevelManager)
        {
            Debug.LogWarning($"Loaded in level \"{SceneManager.GetActiveScene().name}\" does not contain a level manager when it should!");
            return;
        }
    }

}
