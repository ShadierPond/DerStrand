using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Inventory")]
public class Inventory : ScriptableObject, ISerializationCallbackReceiver
{
    public InventorySlot[] items = new InventorySlot[28];
    public ItemDatabase database;

    public void Load(string saveName, string saveLocation)
    {
        if(File.Exists(string.Concat(saveLocation, "/", saveName, ".inventory")))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(saveLocation, "/", saveName, ".inventory"), FileMode.Open);
            JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), this);
            file.Close();
        }
        else
            Debug.Log("No Inventory to load");
    }
    
    public void Save(string saveName, string saveLocation)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(saveLocation, "/", saveName, ".inventory"));
        bf.Serialize(file, JsonUtility.ToJson(this));
        file.Close();
    }
    
    public void MoveItem(InventorySlot from, InventorySlot to)
    {
        InventorySlot temp = new InventorySlot(to.id, to.item, to.amount);
        to.UpdateSlot(from.id, from.item, from.amount);
        from.UpdateSlot(temp.id, temp.item, temp.amount);
    }

    public void AddItem(Item item, int amount)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].item == item)
            {
                items[i].AddAmount(amount);
                return;
            }
        }
        SetEmptySlot(item, amount);
    }
    
    public void RemoveItem(Item item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].item == item)
            {
                items[i].UpdateSlot(-1, null, 0);
                return;
            }
        }
    }
    
    public InventorySlot SetEmptySlot(Item _item, int amount)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].id <= -1)
            {
                items[i].UpdateSlot(_item.id, _item, amount);
                return items[i];
            }
        }
        return null;
    } 

    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
        /*
        for (int i = 0; i < items.Count; i++)
        {
            items[i].item = database.getItem[items[i].id];
        }
        */
    }
}

[System.Serializable]
public class InventorySlot
{
    public InventoryUI parent;
    public int id;
    public Item item;
    public int amount;
    
    
    public InventorySlot()
    {
        this.id = -1;
        this.item = null;
        this.amount = 0;
    }
    
    public InventorySlot(int id, Item item, int amount)
    {
        this.id = id;
        this.item = item;
        this.amount = amount;
    }
    
    public void UpdateSlot(int id, Item item, int amount)
    {
        this.id = id;
        this.item = item;
        this.amount = amount;
    }
    
    public void AddAmount(int value)
    {
        amount += value;
    }
    
}
