using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public List<Item> playerInventory = new List<Item>();
    public static InventorySystem Instance { get; private set; }
    public Color emptySlotColor;
    public Color fullSlotColor;
    
    private ItemDataBase _dataBase;
    
    private void Awake()
    {
        _dataBase = ItemDataBase.Instance;
        Instance = this;
    }

    private void Start()
    {
        AddItem(1);
    }
    
    public Item CheckItem(int id)
    {
        return playerInventory.Find(item => item.itemID == id);
    }

    public void AddItem(int id)
    {
        Item item = _dataBase.GetItem(id);
        playerInventory.Add(item);
        Debug.Log("Added " + item.itemName);
    }
    
    public void AddItem(string name)
    {
        Item item = _dataBase.GetItem(name);
        playerInventory.Add(item);
        Debug.Log("Added " + item.itemName);
    }

    public void RemoveItem(int id)
    {
        Item item = CheckItem(id);
        if(item != null)
        {
            playerInventory.Remove(item);
            Debug.Log("Removed " + item.itemName);
        }
        else
            Debug.Log("Item not found");
    }
}
