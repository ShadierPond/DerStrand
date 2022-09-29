using UnityEngine;
using UnityEngine.InputSystem;

public class SaveManager : MonoBehaviour
{
    // Parent object for all the save points
    [SerializeField] private GameObject saveSlotContent;
    private GameManager gameManager;
    private SaveSystem saveSystem;
    // Public access to the Script
    public static SaveManager Instance { get; private set; }
    
    // Set the Instance
    private void Awake()
    {
        Instance = this;
        gameManager = GameManager.Instance;
        saveSystem = SaveSystem.Instance;
    }
    
    private void Start()
    {
        // Set the save slot content to the parent object in the Save System
        saveSystem.saveSlotsContent = saveSlotContent;
    }
    
    // Regenerate the Save Slots in the UI. Public access for Game Scene
    public void RefreshSaveSlots()
    {
        saveSystem.RefreshSaveSlots();
    }

    // Save the Game Data
    [ContextMenu("Save")]
    public void SaveGame()
    {
        // Get the Day Time Data
        LightingManager.Instance.Save();
        // Get the Player Data
        Player.Instance.Save();
        // Get the Player Properties Data
        PlayerProperties.Instance.Save();
        // Save the Collected Data
        saveSystem.SaveGame();
    }
    
    // Delete the Game Data. public access for Game Scene
    public void DeleteGame()
    {
        saveSystem.DeleteGame();
    }
    
    // Load the Game Data. public access for Game Scene
    public void LoadGame()
    {
        saveSystem.LoadGame();
        gameManager.LoadScene("Game");
        gameManager.LockMouse(true);
        gameManager.isPaused = false;
    }
    
    // Start a new Game
    public void NewGame()
    {
        // Set the Game State to New Game
        saveSystem.newGame = true;
        // Load the Game Scene
        gameManager.LoadScene("Game");
        // Lock the Cursor
        gameManager.LockMouse(true);
        gameManager.isPaused = false;
    }
}
