using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory/Item/Data")]
public class ItemDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    public Item[] items;
    public Dictionary<Item, int> getId = new Dictionary<Item, int>();
    public Dictionary<int, Item> getItem = new Dictionary<int, Item>();
    public void OnBeforeSerialize()
    {
    }

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
