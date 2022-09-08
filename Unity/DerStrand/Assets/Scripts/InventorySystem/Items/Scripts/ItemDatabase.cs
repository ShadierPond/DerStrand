using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory/DataBase")]
public class ItemDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    // List of all items in the game
    public Item[] items;
    // Dictionary to get the Item from the Item Id
    public Dictionary<int, Item> getItem = new Dictionary<int, Item>();
    public void OnBeforeSerialize()
    {
        getItem = new Dictionary<int, Item>();
    }
    // When the game starts, add all items to the dictionaries and set their Ids
    public void OnAfterDeserialize()
    {
        for (var i = 0; i < items.Length; i++)
        {
            items[i].id = i;
            getItem.Add(i, items[i]);
        }
    }
}
