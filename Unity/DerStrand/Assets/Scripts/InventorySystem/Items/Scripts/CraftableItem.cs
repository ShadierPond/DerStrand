using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Craftable Item", menuName = "Inventory/Item/Craftable")]
public class CraftableItem : Item
{
    public Item[] ingredients;
    public int[] ingredientAmounts;
    public Item result;
    
    private void Awake()
    {
        type = ItemType.Craftable;
    }
    
    
}
