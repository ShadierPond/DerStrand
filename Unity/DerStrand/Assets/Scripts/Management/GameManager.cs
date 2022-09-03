using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private string[] _persistentScenes = {"Management"};
    public static GameManager Instance { get; private set; }

    public string currentSaveName;
    public string currentSavePath;
    
    private void Awake()
    {
        Instance = this;
    }

    private void UnloadAllNonPersistentScenes()
    {
        foreach (var scene in SceneManager.GetAllScenes())
        {
            if (!(Array.IndexOf(_persistentScenes, scene.name) > -1))
            {
                SceneManager.UnloadSceneAsync(scene.name);
            }
        }
    }

    public void LoadScene(string sceneName)
    {
        UnloadAllNonPersistentScenes();
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
    }

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
        //LoadScene("Game");
    }
}
