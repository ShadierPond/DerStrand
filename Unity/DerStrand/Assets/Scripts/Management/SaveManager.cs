using UnityEngine;
using UnityEngine.InputSystem;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private GameObject saveSlotContent;
    
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
    public void SaveGame(InputAction.CallbackContext context)
    {
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
    }
}
