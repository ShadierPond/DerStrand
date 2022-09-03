using UnityEngine;
using UnityEngine.InputSystem;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private GameObject saveSlotContent;
    
    public static SaveManager Instance { get; private set; }
    
    private void Awake()
    {
        Instance = this;
    }
    
    private void Start()
    {
        SaveSystem.Instance.saveSlotsContent = saveSlotContent;
    }
    
    // Regenerate the Save Slots in the UI
    public void RefreshSaveSlots()
    {
        SaveSystem.Instance.RefreshSaveSlots();
    }

    // Save the Game Data
    [ContextMenu("Save")]
    public void SaveGame()
    {
        Debug.Log("Game Saved");
        TimeController.Instance.Save();
        Player.Instance.Save();
        SaveSystem.Instance.SaveGame();
    }
    
    // Load the Game Data
    public void DeleteGame()
    {
        SaveSystem.Instance.DeleteGame();
    }
    
    // Load the Game Data
    public void LoadGame()
    {
        SaveSystem.Instance.LoadGame();
        GameManager.Instance.LoadScene("Tariq");
    }

    public void NewGame()
    {
        SaveSystem.Instance.newGame = true;
        GameManager.Instance.LoadScene("Game");
    }
}
