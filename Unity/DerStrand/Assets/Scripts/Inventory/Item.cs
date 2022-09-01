using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int itemID;
    public string itemName;
    public string itemDescription;
    public Sprite itemIcon;
    public Dictionary<string, int> ItemStats = new Dictionary<string, int>();
}
