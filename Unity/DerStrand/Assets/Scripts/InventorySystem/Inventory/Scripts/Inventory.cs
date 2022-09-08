using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Inventory")]
public class Inventory : ScriptableObject
{
    public InventorySlot[] items = new InventorySlot[28];
    public ItemDatabase database;

    // Loads the inventory from the save file (if it exists.) If it doesn't exist, it creates a new one.
    public void Load(string saveName, string saveLocation)
    {
        // If the save file exists, load it.
        if(File.Exists(string.Concat(saveLocation, "/", saveName, ".inventory")))
        {
            // Create a new file stream to read the save file.
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(saveLocation, "/", saveName, ".inventory"), FileMode.Open);
            // Overwrite the current inventory with the one in the save file.
            JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), this);
            // Close the file stream.
            file.Close();
        }
        else
            Debug.Log("No Inventory to load");
    }
    
    // Saves the inventory to a file that has the same name as the game´s save name with .inventory ending.
    public void Save(string saveName, string saveLocation)
    {
        // Create a new file stream to write the save file.
        BinaryFormatter bf = new BinaryFormatter();
        // Create the save file
        FileStream file = File.Create(string.Concat(saveLocation, "/", saveName, ".inventory"));
        // Serialize the inventory and write it to the save file.
        bf.Serialize(file, JsonUtility.ToJson(this));
        // Close the file stream.
        file.Close();
    }
    
    // Swaps the items in the two given slots. used for moving items around in the inventory UI.
    public void SwapItems(InventorySlot from, InventorySlot to)
    {
        InventorySlot temp = new InventorySlot(to.item, to.amount);
        to.UpdateSlot(from.item, from.amount);
        from.UpdateSlot(temp.item, temp.amount);
    }

    // Adds an item to the inventory. If the item is stackable, it will try to stack it with other items of the same type.
    // TODO: choose which method is more efficient and remove the other one.
    public void AddItem(Item item, int amount)
    {
        var itemToAdd = Array.Find(items, x => x.item == item);
        if (itemToAdd != null)
        {
            itemToAdd.amount += amount;
            return;
        }
            
        /*
        foreach (var _item in items)
            if (_item.item == item)
            {
                _item.AddAmount(amount);
                return;
            }
        */
        // If the item is not stackable, it will add it to the first empty slot.
        SetEmptySlot(item, amount);
    }
    // Removes an item from the inventory, even if it is stacked.
    public void RemoveItem(Item item, int amount)
    {
        foreach (var _item in items)
        {
            if (_item.item == item)
            {
                _item.amount -= amount;
                if (_item.amount <= 0)
                    _item.UpdateSlot(null, 0);
                return;
            }
        }
    }
    // Looks for the next empty slot and adds the item to it with the given amount. if there is no empty slot, it will do nothing.
    public InventorySlot SetEmptySlot(Item _item, int amount)
    {
        foreach (var item in items)
        {
            if (item.item == null)
            {
                item.UpdateSlot(_item, amount);
                return item;
            }
        }
        return null;
    }
}

[System.Serializable]
public class InventorySlot
{
    // The Panel that holds the item´s image and amount text.
    public InventoryUI parent;
    // The item in the slot.
    public Item item;
    // The amount of the item in the slot.
    public int amount;
    
    // Constructor for the InventorySlot class.
    public InventorySlot()
    {
        item = null;
        amount = 0;
    }
    // Sets the id, item and amount of the slot.
    public InventorySlot(Item item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }
    // Updates the id, item and amount of the slot.
    public void UpdateSlot(Item item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }
    // Adds the given amount to the slot´s amount.
    public void AddAmount(int value)
    {
        amount += value;
    }
}
