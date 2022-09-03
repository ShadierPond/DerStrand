using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    public GameObject inventoryUI;
    public GameObject inventorySlotPrefab;


    Dictionary<InventorySlot, GameObject> items = new Dictionary<InventorySlot, GameObject>();

    private void Start()
    {
        CreateInventoryUI();
    }

    private void Update()
    {
        UpdateInventoryUI();
    }

    public void CreateInventoryUI()
    {
        foreach (var item in inventory.inventory)
        {
            GameObject itemObj = Instantiate(inventorySlotPrefab, inventoryUI.transform);
            itemObj.GetComponentInChildren<TextMeshProUGUI>().text = item.amount.ToString("n0");
            itemObj.GetComponentInChildren<Image>().sprite = item.item.icon;
            items.Add(item, itemObj);
        }
    }
    
    public void UpdateInventoryUI()
    {
        foreach (var item in inventory.inventory)
        {
            if(items.ContainsKey(item))
                items[item].GetComponentInChildren<TextMeshProUGUI>().text = item.amount.ToString("n0");
            else
            {
                GameObject itemObj = Instantiate(inventorySlotPrefab, inventoryUI.transform);
                itemObj.GetComponentInChildren<TextMeshProUGUI>().text = item.amount.ToString("n0");
                items.Add(item, itemObj);
            }
        }
        
    }
}
