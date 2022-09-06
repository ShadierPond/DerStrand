using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item/Tool")]
public class ToolItem : Item
{
    
    private void Awake()
    {
        type = ItemType.Tool;
    }
}
