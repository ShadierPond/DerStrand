using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SaveSystem : MonoBehaviour
{
    // Access the save system from anywhere in the Project
    public static SaveSystem Instance { get; private set; }
    
    [SerializeField] private bool usePersistentDataPath;
    [SerializeField] private string saveLocation;
    [HideInInspector] public GameObject saveSlotsContent;
    public SaveData saveData;
    private string selectedSaveName;

    // Reference the Instance to this script and set Save Location
    private void Awake()
    {
        Instance = this;
        if(usePersistentDataPath)
            saveLocation = Application.persistentDataPath;
    }
    
    // Save Game Data by passing in a GameData Class with the Data to be saved
    private void Save(SaveData data)
    {
        // Set Save Date and Time to current Date and Time
        data.saveDate = DateTime.Now.ToString("dd/MM/yyyy");
        data.saveTime = DateTime.Now.ToString("HH:mm:ss");
        // Set SaveName as the Date and Time and replace any : with _ (to prevent any issues with the file name)
        var saveName = (data.saveDate +"-"+ data.saveTime).Replace( ":", "_");
        // Convert the GameData Class to a JSON string
        var json = JsonUtility.ToJson(data);
        // if the path doesnt exist, create it then save the data in the path
        if(!Directory.Exists(saveLocation))
            Directory.CreateDirectory(saveLocation);
        // Write the JSON string to the save location
        File.WriteAllText(saveLocation + "/" + saveName + ".json", json);
        // Take a screenshot of the game and save it to the save location
        ScreenCapture.CaptureScreenshot(saveLocation + "/" + saveName + ".png");
    }
    
    // Load Game Data by looking for the save file name and returning the data
    private SaveData Load(string saveName)
    {
        return JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation + "/" + saveName + ".json"));
    }
    // Load Game Data´s Image by looking for the save file name and returning the Image
    private Texture LoadImage(string saveName)
    {
        // Load the image from the save location
        var bytes = File.ReadAllBytes(saveLocation + "/" + saveName + ".png");
        // Create a new Texture2D
        var texture = new Texture2D(240, 135);
        // Load the image into the Texture2D
        texture.LoadImage(bytes);
        return texture;
    }
    
    // Delete Game Data and the Image by looking for the save file name and deleting the file
    private void Delete(string saveName)
    {
        File.Delete(saveLocation + "/" + saveName + ".json");
        File.Delete(saveLocation + "/" + saveName + ".png");
    }
    
    // Return List of Save Files (name of Save File only). used for loading slots in the UI
    private List<string> GetSaveFiles()
    {
        var saveFiles = new List<string>();
        saveFiles.AddRange(Directory.GetFiles(saveLocation, "*.json").Select(Path.GetFileNameWithoutExtension));
        return saveFiles;
    }
    
    // Regenerate the Save Slots in the UI
    public void RefreshSaveSlots()
    {
        // Delete already existing Save Slots
        foreach (Transform child in saveSlotsContent.transform)
        {
            Destroy(child.gameObject);
        }
        // Get the list of Save Files
        var saveSlots = GetSaveFiles();
        // Loop through the list of Save Files and create a Save Slot for each one
        foreach (var save in saveSlots)
        {
            // Temporarily store the Save Data
            var data = Load(save);
            // Create a new Save Slot
            var saveSlot = Instantiate(Resources.Load("SaveSlot"), saveSlotsContent.transform) as GameObject;
            // Setup the Save Slot's Button. OnClick, Save the Save Slot's Save Name to the selectedSaveName variable
            saveSlot.GetComponent<Button>().onClick.AddListener(() => selectedSaveName = (data.saveDate +"-"+ data.saveTime).Replace(":", "_"));
            // If the Save Slot exists, set the Save Slot's Info Text to the Save Data's Info Text
            if (saveSlot != null)
            {
                // Load the Save Slot's Image
                saveSlot.transform.GetChild(0).GetComponent<RawImage>().texture = LoadImage((data.saveDate +"-"+ data.saveTime).Replace(":", "_"));
                // Load the Save Slot's Date
                saveSlot.transform.GetChild(1).GetComponent<TMP_Text>().text = data.saveDate;
                // Load the Save Slot's Time
                saveSlot.transform.GetChild(2).GetComponent<TMP_Text>().text = data.saveTime;
                // Load the Save Slot's Info Text
                saveSlot.transform.GetChild(3).GetComponent<TMP_Text>().text = data.daysSurvived + " days survived";
            }
        }
    }
    
    // Save the Game Data
    public void SaveGame()
    {
        Save(saveData);
        Debug.Log("Game Saved");
    }
    
    // Load the Game Data
    public void DeleteGame()
    {
        Delete(selectedSaveName);
        RefreshSaveSlots();
    }
    
    // Load the Game Data
    public void LoadGame()
    {
        saveData = Load(selectedSaveName);
        Debug.Log("Loaded save data");
    }
}
