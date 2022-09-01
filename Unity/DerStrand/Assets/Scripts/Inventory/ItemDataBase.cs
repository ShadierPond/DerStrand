using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataBase : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public static ItemDataBase Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        BuildDataBase();
    }

    public Item GetItem(int id)
    {
        return items.Find(item => item.itemID == id);
    }
    
    public Item GetItem(string itemName)
    {
        return items.Find(item => item.itemName == itemName);
    }

    private void BuildDataBase()
    {
        items = new List<Item>()
        {
            new Item(0, "Stone", "Basic Material for various Activites", new Dictionary<string, int>()
            {
                {"Damage", 5}
            }),
            new Item(1, "Small Stick", "Upgrade for the Big Stick", new Dictionary<string, int>()),
            new Item(2, "Big Stick", "Women are more attracted to you, than Men with little Stick", new Dictionary<string, int>()),
            new Item(3, "liana", "Used to hang \"Stuff\" around", new Dictionary<string, int>()),
            new Item(4, "Bones", "Is there a BONE in you pocket or are you hapy to see me?", new Dictionary<string, int>()),
            new Item(5, "Dirt", "", new Dictionary<string, int>())
            


        };
    }
}
