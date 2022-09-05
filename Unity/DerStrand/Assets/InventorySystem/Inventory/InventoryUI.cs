using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    public GameObject inventoryUI;
    public GameObject inventorySlotPrefab;
    public MouseItem mouseItem = new MouseItem();


    Dictionary<GameObject, InventorySlot> items = new Dictionary<GameObject, InventorySlot>();

    private void Start()
    {
        CreateSlots();
    }

    private void Update()
    {
        UpdateInventoryUI();
    }

    public void CreateSlots()
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
    
    public void UpdateInventoryUI()
    {
        foreach (var slot in items)
        {
            var obj = slot.Key.transform;
            if (slot.Value.id >= 0)
            {
                obj.GetChild(0).GetComponent<Image>().sprite = inventory.database.getItem[slot.Value.id].icon;
                obj.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
                obj.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = slot.Value.amount == 1 ? "" : slot.Value.amount.ToString("n0");
                obj.GetChild(1).GetComponent<Image>().color = slot.Value.amount == 1 ? new Color(1, 0, 0, 0) : new Color(1, 0, 0, 1);
            }
            else
            {
                obj.GetChild(0).GetComponent<Image>().sprite = null;
                obj.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0);
                obj.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
                obj.GetChild(1).GetComponent<Image>().color = new Color(1, 0, 0, 0);
            }
        }
    }

    private void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }
    
    private void OnEnter(GameObject obj)
    {
        mouseItem.hoverObj = obj;
        if (items.ContainsKey(obj))
            mouseItem.hoverSlot = items[obj];
    }
    
    private void OnExit(GameObject obj)
    {
        mouseItem.hoverObj = null;
            mouseItem.hoverSlot = null;
    }
    
    private void OnDragStart(GameObject obj)
    {
        Debug.Log("Drag Start");
        var mouseObj = new GameObject();
        mouseObj.AddComponent<RectTransform>().sizeDelta = new Vector2(80, 80);
        mouseObj.transform.SetParent(transform);
        if (items[obj].id >= 0)
        {
            Debug.Log("Drag Start 2");
            mouseObj.AddComponent<Image>().sprite = inventory.database.getItem[items[obj].id].icon;
            mouseObj.GetComponent<Image>().raycastTarget = false;
        }
        mouseItem.obj = mouseObj;
        mouseItem.item = items[obj];
    }
    
    private void OnDragEnd(GameObject obj)
    {
        Debug.Log("Drag End");
        if (mouseItem.hoverObj)
        {
            inventory.MoveItem(items[obj], items[mouseItem.hoverObj]);
        }
        else
        {
            inventory.RemoveItem(items[obj].item);
        }
        Destroy(mouseItem.obj);
        mouseItem.item = null;
    }
    
    private void OnDrag(GameObject obj)
    {
        Debug.Log("Drag");
        if(mouseItem.obj != null)
            mouseItem.obj.GetComponent<RectTransform>().position = Input.mousePosition;
    }
    
}

public class MouseItem
{
    public GameObject obj;
    public InventorySlot item;
    public InventorySlot hoverSlot;
    public GameObject hoverObj;
}
