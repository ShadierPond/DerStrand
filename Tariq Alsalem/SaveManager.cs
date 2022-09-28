using UnityEngine;
using UnityEngine.InputSystem;

public class SaveManager : MonoBehaviour
{
    // Parent object for all the save points
    [SerializeField] private GameObject saveSlotContent;
    // Public access to the Script
    public static SaveManager Instance { get; private set; }
    
    // Set the Instance
    private void Awake()
    {
        Instance = this;
    }
    
    private void Start()
    {
        // Set the save slot content to the parent object in the Save System
        SaveSystem.Instance.saveSlotsContent = saveSlotContent;
    }
    
    // Regenerate the Save Slots in the UI. Public access for Game Scene
    public void RefreshSaveSlots()
    {
        SaveSystem.Instance.RefreshSaveSlots();
    }

    // Save the Game Data
    [ContextMenu("Save")]
    public void SaveGame()
    {
        // Get the Light Manager Data
        LightingManager.Instance.Save();
        // Get the Day Time Data
        TimeController.Instance.Save();
        // Get the Player Data
        Player.Instance.Save();
        // Get the Player Properties Data
        PlayerProperties.Instance.Save();
        // Save the Collected Data
        SaveSystem.Instance.SaveGame();
    }
    
    // Delete the Game Data. public access for Game Scene
    public void DeleteGame()
    {
        SaveSystem.Instance.DeleteGame();
    }
    
    // Load the Game Data. public access for Game Scene
    public void LoadGame()
    {
        SaveSystem.Instance.LoadGame();
        GameManager.Instance.LoadScene("Tariq");
    }
    
    // Start a new Game
    public void NewGame()
    {
        // Set the Game State to New Game
        SaveSystem.Instance.newGame = true;
        // Load the Game Scene
        GameManager.Instance.LoadScene("Game");
    }
}
