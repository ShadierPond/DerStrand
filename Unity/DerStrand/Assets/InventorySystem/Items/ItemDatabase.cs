using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory/Item/Data")]
public class ItemDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    // List of all items in the game
    public Item[] items;
    // Dictionary to get the Item Id from the Item
    public Dictionary<Item, int> getId = new Dictionary<Item, int>();
    // Dictionary to get the Item from the Item Id
    public Dictionary<int, Item> getItem = new Dictionary<int, Item>();
    public void OnBeforeSerialize()
    {
    }
    // When the game starts, add all items to the dictionaries and set their Ids
    public void OnAfterDeserialize()
    {
        getId = new Dictionary<Item, int>();
        getItem = new Dictionary<int, Item>();
        for (var i = 0; i < items.Length; i++)
        {
            getId.Add(items[i], i);
            getItem.Add(i, items[i]);
        }
    }
}
