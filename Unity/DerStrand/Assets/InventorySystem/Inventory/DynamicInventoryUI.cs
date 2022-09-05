using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicInventoryUI : InventoryUI
{
    public int slotCount;
    public override void CreateSlots()
    {
        items = new Dictionary<GameObject, InventorySlot>();

        foreach (var slot in inventory.items)
        {
            var obj = Instantiate(inventorySlotPrefab, inventoryUI.transform);
            
            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });
            
            items.Add(obj, slot);
        }
    }

    private void OnApplicationQuit()
    {
        inventory.items = new InventorySlot[slotCount];
    }
}
