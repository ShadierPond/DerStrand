using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public int itemID;
    public string itemName;
    public string itemDescription;
    public Sprite itemIcon;
    public Dictionary<string, int> ItemStats = new Dictionary<string, int>();
    
    public Item(int id, string name, string description, Dictionary<string, int> stats)
    {
        itemID = id;
        itemName = name;
        itemDescription = description;
        itemIcon = Resources.Load("Items/" + name, typeof(Sprite)) as Sprite;
        ItemStats = stats;
    }

    public Item(Item item)
    {
        itemID = item.itemID;
        itemName = item.itemName;
        itemDescription = item.itemDescription;
        itemIcon = item.itemIcon;
        ItemStats = item.ItemStats;
    }
}
