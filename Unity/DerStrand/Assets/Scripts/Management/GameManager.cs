using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Persistent Scenes to stay loaded in the background
    private string[] _persistentScenes = {"Management"};
    // Public access to the Class
    public static GameManager Instance { get; private set; }
    // Current Save Name
    public string currentSaveName;
    // Current Save Path
    public string currentSavePath;
    // Item Database
    [SerializeField] private ItemDatabase itemDatabase;
    // Player Time to transition
    [SerializeField] private float sceneTransitionTime;
    // Waiting Time before Showing the next Scene
    [SerializeField] private float sceneWaitingTimeBeforeShowing;
    // is Game Paused
    public bool isPaused;

    private void Awake()
    {
        Instance = this;
        BuildDatabases();
    }
    
    // Set Items Prefabs and Icons in the ItemDatabase
    private void BuildDatabases()
    {
        // Loop through all the items in the database
        foreach (var item in itemDatabase.items)
        {
            // Get the item prefab from the Resources folder
            var prefab = Resources.Load<GameObject>("Items/Prefabs/" + item.name);
            // Get the item icon from the Resources folder
            var icon = Resources.Load<Sprite>("Items/Icons/" + item.name);
            // if there is a prefab, set it in the database item
            if(prefab)
                item.prefab = prefab;
            else
                Debug.LogWarning("Prefab for " + item.name + " not found. Please add prefab with the same Item name to the Resources/Items/Prefabs folder.");
            // If there is an icon, set it in the database item
            if(icon)
                item.icon = icon;
            else
                Debug.LogWarning("Icon for " + item.name + " not found. Please add icon with the same Item name to the Resources/Items/Icons folder.");
        }
    }

    // Unload all scenes except persistent scenes
    private void UnloadAllNonPersistentScenes()
    {
        // Loop through all the loaded scenes
        for (var i = 0; i < SceneManager.sceneCount; i++)
        {
            // Get the Scene Name
            var sceneName = SceneManager.GetSceneAt(i).name;
            // If the scene is not a persistent scene, unload it
            if (!(Array.IndexOf(_persistentScenes, sceneName) > -1))
                SceneManager.UnloadSceneAsync(sceneName);
        }
    }
    // Load scene with scene name
    public void LoadScene(string sceneName)
    {
        StartCoroutine(TransitionScene(sceneName));
    }
    // Load an Array of scenes
    public void LoadScene(string[] sceneNames)
    {
        UnloadAllNonPersistentScenes();
        foreach (var sceneName in sceneNames)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        } 
    }

    private void Start()
    {
        //LoadScene("Main Menu");
    }
    
    // Load the scene with the transition (fade in/out and loading screen)
    IEnumerator TransitionScene(string sceneName)
    {
        // Get the Crossfade Canvas Group
        var canvasGroup = GameObject.Find("CrossFade").GetComponent<CanvasGroup>();
        // if there is no canvas group, exit the routine
        if (!canvasGroup) 
            yield break;
        // Set the canvas group alpha to 0
        canvasGroup.alpha = 0;
        // Fade in the canvas group
        canvasGroup.DOFade(1, sceneTransitionTime);
        yield return new WaitForSeconds(sceneTransitionTime);
        // Unload all non persistent scenes
        UnloadAllNonPersistentScenes();
        // Load the Loading Scene
        SceneManager.LoadSceneAsync("Loading", LoadSceneMode.Additive);
        // Wait for the scene to load
        yield return new WaitForSeconds(2);
        // Get the Progress Bar
        var progressBar = GameObject.Find("Progress").GetComponent<Image>();
        // Get the Scene Loading
        var scene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        // While the scene is loading
        while (!scene.isDone)
        {
            // Set the progress bar fill amount
            progressBar.fillAmount = scene.progress;
            yield return null;
        }
        // Unload the Loading Scene
        SceneManager.UnloadSceneAsync("Loading");
        // Set the Loaded Scene as active
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        // Get the Canvas Group from the Loaded Scene
        canvasGroup = GameObject.Find("CrossFade").GetComponent<CanvasGroup>();
        // Set the Canvas Group Alpha to 1
        canvasGroup.alpha = 1;
        if(sceneName == "Game")
            yield return new WaitForSeconds(sceneWaitingTimeBeforeShowing);
        // Fade out the Canvas Group
        canvasGroup.DOFade(0, sceneTransitionTime);
        yield return new WaitForSeconds(sceneTransitionTime);
    }
    
    // Pause the Game
    public void PauseGame(bool pause)
    {
        //Time.timeScale = pause ? 0 : 1;
        isPaused = pause;
    }
    
    // Set the GameOver Screen
    public void GameOver()
    {
        LoadScene("GameOver");
    }
}
