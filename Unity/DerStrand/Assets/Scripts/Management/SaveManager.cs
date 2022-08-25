using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private GameObject saveSlotsContent;
    [SerializeField] private string selectedSaveName;

    private void GenerateSaveSlots()
    {
        var saveSlots = SaveSystem.Instance.GetSaveFiles(); 
        foreach (var save in saveSlots)
        {
            SaveData data = SaveSystem.Instance.Load(save);
            GameObject saveSlot = Instantiate(Resources.Load("SaveSlot"), saveSlotsContent.transform.GetChild(0).GetChild(0)) as GameObject;
            saveSlot.GetComponent<Button>().onClick.AddListener(() => selectedSaveName = data.saveDate + data.saveTime);
            if (saveSlot != null)
            {
                saveSlot.transform.GetChild(1).GetComponent<TMP_Text>().text = data.saveDate;
                saveSlot.transform.GetChild(2).GetComponent<TMP_Text>().text = data.saveTime;
            }
        }
    }
    
    private void RefreshSaveSlots()
    {
        foreach (Transform child in saveSlotsContent.transform.GetChild(0).GetChild(0))
        {
            Destroy(child.gameObject);
        }
        GenerateSaveSlots();
    }
}
