using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private string[] _persistentScenes = {"Management"};
    public static GameManager Instance { get; private set; }

    public string currentSaveName;
    public string currentSavePath;
    
    [SerializeField] private ItemDatabase itemDatabase;
    [SerializeField] private float sceneTransitionTime;
    public bool isPaused;

    private void Awake()
    {
        Instance = this;
        BuildDatabases();
    }
    
    // Set Items Prefabs and Icons in the ItemDatabase
    private void BuildDatabases()
    {
        foreach (var item in itemDatabase.items)
        {
            var prefab = Resources.Load<GameObject>("Items/Prefabs/" + item.name);
            var icon = Resources.Load<Sprite>("Items/Icons/" + item.name);
            if(prefab)
                item.prefab = prefab;
            else
                Debug.LogWarning("Prefab for " + item.name + " not found. Please add prefab with the same Item name to the Resources/Items/Prefabs folder.");
            if(icon)
                item.icon = icon;
            else
                Debug.LogWarning("Icon for " + item.name + " not found. Please add icon with the same Item name to the Resources/Items/Icons folder.");
        }
    }

    // Unload all scenes except persistent scenes
    private void UnloadAllNonPersistentScenes()
    {
        for (var i = 0; i < SceneManager.sceneCount; i++)
        {
            var sceneName = SceneManager.GetSceneAt(i).name;
            if (!(Array.IndexOf(_persistentScenes, sceneName) > -1))
                SceneManager.UnloadSceneAsync(sceneName);
        }
    }
    // Load scene with scene name
    public void LoadScene(string sceneName)
    {
        //UnloadAllNonPersistentScenes();
        //SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        //SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
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
        
        var canvasGroup = GameObject.Find("CrossFade").GetComponent<CanvasGroup>();
        if (!canvasGroup) 
            yield break;
        
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, sceneTransitionTime);
        yield return new WaitForSeconds(sceneTransitionTime);

        UnloadAllNonPersistentScenes();
        SceneManager.LoadSceneAsync("Loading", LoadSceneMode.Additive);
        yield return new WaitForSeconds(2);
        var progressBar = GameObject.Find("Progress").GetComponent<Image>();
        
        var scene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!scene.isDone)
        {
            progressBar.fillAmount = scene.progress;
            yield return null;
        }
        SceneManager.UnloadSceneAsync("Loading");
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
            
        canvasGroup = GameObject.Find("CrossFade").GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1;
        canvasGroup.DOFade(0, sceneTransitionTime);
        yield return new WaitForSeconds(sceneTransitionTime);
    }
    
    public void PauseGame(bool pause)
    {
        //Time.timeScale = pause ? 0 : 1;
        isPaused = pause;
    }
    
    public void GameOver()
    {
        LoadScene("GameOver");
    }
}
