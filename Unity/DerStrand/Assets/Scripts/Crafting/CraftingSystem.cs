using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    public Inventory craftingInventory;
    public Inventory playerInventory;
    public GameObject craftingSlotPrefab;
    public GameObject craftingPanel;
    public GameObject selectedSlot;
    public int recipeCount;
    
    public Dictionary<GameObject, InventorySlot> craftingSlots = new Dictionary<GameObject, InventorySlot>();

    private void Start()
    {
        craftingSlotPrefab = Resources.Load("CraftingSlot") as GameObject;
        CreateSlots();
    }

    private void Update()
    {
        UpdateSlots();
    }

    public void CreateSlots()
    {
        CreatCraftingList();
        
        foreach (var recipe in craftingInventory.items)
        {
            var obj = Instantiate(craftingSlotPrefab, craftingPanel.transform);
            obj.GetComponent<Button>().onClick.AddListener(() => selectedSlot = obj);
            craftingSlots.Add(obj, recipe);
        }
    }
    
    public void CreatCraftingList()
    {
        foreach (var item in craftingInventory.database.items)
        {
            Debug.Log(item.isCraftable);
            if (item.isCraftable)
                recipeCount++;
        }
        
        craftingInventory.items = new InventorySlot[recipeCount];
        
        for (int i = 0; i < craftingInventory.database.items.Length; i++)
        {
            if (craftingInventory.database.items[i].isCraftable)
            {
                craftingInventory.AddItem(craftingInventory.database.items[i], 1);
            }
        }
    }

    public void UpdateSlots()
    {
        foreach (var slot in craftingSlots)
        {
            var obj = slot.Key.transform;
            var recipe = slot.Value;
            var objImage = obj.GetChild(0).GetComponent<Image>();
            var objText = obj.GetChild(1).GetComponent<TextMeshProUGUI>();
            objImage.sprite = craftingInventory.database.getItem[slot.Value.item.id].icon;
            objText.text = craftingInventory.database.getItem[slot.Value.item.id].name;
        }
    }
}
