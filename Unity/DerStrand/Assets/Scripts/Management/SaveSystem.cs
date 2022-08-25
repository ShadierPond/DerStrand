using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    // Access the save system from anywhere in the game
    public static SaveSystem Instance { get; private set; }
    
    [SerializeField] private bool usePersistentDataPath;
    [SerializeField] private string saveLocation;

    // Reference the Instance to this script
    private void Awake()
    {
        Instance = this;
    }
    
    // Save Game Data by passing in a GameData Class with the Data to be saved
    public void Save(SaveData data)
    {
        var saveName = data.saveDate + data.saveTime;
        var json = JsonUtility.ToJson(data);
        
        if(usePersistentDataPath)
            File.WriteAllText(Application.persistentDataPath + "/" + saveName + ".json", json);
        
        // if the path doesnt exist, create it then save the data in the path
        else
        {
            if(!Directory.Exists(saveLocation))
                Directory.CreateDirectory(saveLocation);
            File.WriteAllText(saveLocation + "/" + saveName + ".json", json);
        }
    }
    
    // Load Game Data by looking for the save file name and returning the data
    public SaveData Load(string saveName)
    {
        if(usePersistentDataPath)
            return JsonUtility.FromJson<SaveData>(File.ReadAllText(Application.persistentDataPath + "/" + saveName + ".json"));
        
        else
            return JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation + "/" + saveName + ".json"));
    }
    
    // Delete Game Data by looking for the save file name and deleting the file
    public void Delete(string saveName)
    {
        if(usePersistentDataPath)
            File.Delete(Application.persistentDataPath + "/" + saveName + ".json");
        
        else
            File.Delete(saveLocation + "/" + saveName + ".json");
    }
    
    // Return List of Save Files (name of Save File only). used for loading slots in the UI
    public List<string> GetSaveFiles()
    {
        var saveFiles = new List<string>();
        if(usePersistentDataPath)
            saveFiles.AddRange(Directory.GetFiles(Application.persistentDataPath, "*.json").Select(Path.GetFileNameWithoutExtension));
        else
            saveFiles.AddRange(Directory.GetFiles(saveLocation, "*.json").Select(Path.GetFileNameWithoutExtension));
        
        return saveFiles;
    }
}
