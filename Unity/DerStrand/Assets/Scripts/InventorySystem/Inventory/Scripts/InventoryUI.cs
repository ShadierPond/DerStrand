using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    // the inventory we are displaying
    public Inventory inventory;
    // the panel we are displaying the inventory on
    public GameObject inventoryPanel;
    // the slot prefab
    public GameObject inventorySlotPrefab;
    // the place where the held item will be created. this fixes layering issues
    public GameObject objectHolder;
    // the amount of slots in the inventory UI
    public int slotCount;
    // If it is the Equipment UI
    public bool isEquipmentInventory;
    public int activeSlot = 0;

    // the list of slots in the inventory UI
    public Dictionary<GameObject, InventorySlot> items = new Dictionary<GameObject, InventorySlot>();

    public void Start()
    {
        // Get the slot prefab from the resources folder
        inventorySlotPrefab = Resources.Load("InventorySlot") as GameObject;
        // Set the parent of the slot to the parent inventory panel
        foreach (var slot in inventory.items)
            slot.parent = this;
        // Create the slots
        CreateSlots();
        // Add Events to the Panel.
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEvent(gameObject, "OnEnterInterface"); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnEvent(gameObject, "OnExitInterface"); });
    }

    public void Update()
    {
        UpdateInventoryUI();
        GetActiveSlot();
    }

    public void CreateSlots()
    {
        // Create new Dictionary
        items = new Dictionary<GameObject, InventorySlot>();
        // Loop through the amount of slots
        foreach (var slot in inventory.items)
        {
            // Create a new slot
            var obj = Instantiate(inventorySlotPrefab, inventoryPanel.transform);
            // Add Events to the slot
            // When the mouse enters the slot
            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEvent(obj, "OnEnter"); });
            // When the mouse exits the slot
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnEvent(obj, "OnExit"); });
            // When the mouse clicks the slot (begins dragging)
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnEvent(obj, "OnDragStart"); });
            // When the mouse stops clicking the slot (stops dragging)
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnEvent(obj, "OnDragEnd"); });
            // While the mouse is dragging the slot
            AddEvent(obj, EventTriggerType.Drag, delegate { OnEvent(obj, "OnDrag"); });
            // Add the slot to the dictionary with the object as the key
            items.Add(obj, slot);
        }
    }
    
    // Update the inventory UI
    public void UpdateInventoryUI()
    {
        // Loop through the slots
        foreach (var slot in items)
        {
            // Temporary variables. These are used to make the code more readable and saves resources in unity
            var obj = slot.Key.transform;
            var objImage = obj.GetChild(0).GetComponent<Image>();
            var objText = obj.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            var objTextImage = obj.GetChild(1).GetComponent<Image>();
            // If the slot is not empty
            if (slot.Value.item != null)
            {
                // Set the image of the slot to the image of the item
                objImage.sprite = inventory.database.getItem[slot.Value.item.id].icon;
                // Set the Background color of the slot to white (used for Alpha blending. shows the image)
                objImage.color = new Color(1, 1, 1, 1);
                // Set the text of the slot to the amount of the item
                objText.text = slot.Value.amount == 1 ? "" : slot.Value.amount.ToString("n0");
                // Set the text image background color of the slot to Red (used for Alpha blending. show the image)
                objTextImage.color = slot.Value.amount == 1 ? new Color(1, 0, 0, 0) : new Color(1, 0, 0, 1);
            }
            // If the Slot is empty
            else
            {
                // Set the image of the slot to null
                objImage.sprite = null;
                // Set the Background color of the slot to clear (used for Alpha blending. hides the image)
                objImage.color = new Color(1, 1, 1, 0);
                // Set the text of the slot to nothing
                objText.text = "";
                // Set the text image background color of the slot to clear (used for Alpha blending. hides the image)
                objTextImage.color = new Color(1, 0, 0, 0);
            }
        }
    }
    
    // Add an event to the Slot with Event Trigger Type and an Action (aka a function)
    public void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        // Get the Event Trigger component of the slot
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        // Create a new entry
        var eventTrigger = new EventTrigger.Entry();
        // Set the entry type to the type we want
        eventTrigger.eventID = type;
        // Add the action that should be called to the entry when the event is triggered
        eventTrigger.callback.AddListener(action);
        // Add the entry to the Event Trigger
        trigger.triggers.Add(eventTrigger);
    }

    // The function that is called when an event is triggered
    public void OnEvent(GameObject obj, string eventType)
    {
        switch (eventType)
        {
            // When the mouse enters the slot
            case "OnEnter":
                // get the slot
                MouseData.slotHoveredOver = obj;
                // if the slot is not empty, get the item
                if (items.ContainsKey(obj))
                    MouseData.hoverSlot = items[obj];
                break;
            // When the mouse exits the slot
            case "OnExit":
                // Set Mouse Data Variables to null
                MouseData.slotHoveredOver = null;
                MouseData.hoverSlot = null;
                break;
            // When the mouse enters the inventory panel
            case "OnEnterInterface":
                // Get the Panel hovered over
                MouseData.interfaceMouseIsOver = obj.GetComponent<InventoryUI>();
                break;
            // When the mouse exits the inventory panel
            case "OnExitInterface":
                // Set the Panel hovered over to null
                MouseData.interfaceMouseIsOver = null;
                break;
            // When the mouse begins dragging the slot
            case "OnDragStart":
                // Create a new slot GameObject
                var mouseObj = new GameObject();
                // Set the parent of the slot to the object holder. fixes the layering issue
                mouseObj.transform.SetParent(objectHolder.transform);
                // Set the Size of the Slot
                mouseObj.AddComponent<RectTransform>().sizeDelta = new Vector2(80, 80);
                // if the slot is not empty
                if (items[obj].item != null)
                {
                    // Set the image of the slot to the image of the item
                    mouseObj.AddComponent<Image>().sprite = inventory.database.getItem[items[obj].item.id].icon;
                    // Disable the raycast target so the mouse can interact with the slot and panel.
                    mouseObj.GetComponent<Image>().raycastTarget = false;
                }
                // Set the slot to the mouse object
                MouseData.tempItemBeingDragged = mouseObj;
                // Set the slot to the slot being dragged
                MouseData.item = items[obj];
                break;
            // While the mouse is dragging the slot
            case "OnDrag":
                // If the Slot being dragged is not empty
                if(MouseData.tempItemBeingDragged != null)
                    // Set the position of the slot to the mouse position
                    MouseData.tempItemBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
                break;
            // When the mouse stops dragging the slot
            case "OnDragEnd":
                // If the Interface the mouse is over is not empty. meaning the mouse is over a panel
                if (MouseData.interfaceMouseIsOver != null)
                {
                    // If the mouse is over a slot
                    if (MouseData.slotHoveredOver)
                        // Swap the items in the slots
                        inventory.SwapItems(items[obj], MouseData.hoverSlot.parent.items[MouseData.slotHoveredOver]);
                }
                // If the Interface the mouse is over is empty. meaning the mouse is not over a panel
                else
                {
                    // Instantiate the item in the world
                    var item = Instantiate( inventory.database.getItem[items[obj].item.id].prefab, Player.Instance.transform.position + Vector3.forward, Quaternion.identity);
                    // Set the amount of the item
                    item.GetComponent<ItemObject>().amount = items[obj].amount;
                    // Set the item in the Database to the item in the world
                    item.GetComponent<ItemObject>().item = items[obj].item;
                    // Remove the item from the inventory
                    inventory.RemoveItem(items[obj].item, items[obj].amount);
                }
                // Destroy the dragged slot
                Destroy(MouseData.tempItemBeingDragged);
                // Set the dragged item to null
                MouseData.item = null;
                break;
            // When the Given Event is not found
            default:
                Debug.Log("Event not found");
                break;
        }
    }

    // Get slot Id from player Equippment Slots (the active one, that were chosen with scroll wheel)
    public void GetActiveSlot()
    {
        // Return if not EquipmentInventoryPanel
        if(!isEquipmentInventory)
            return;
        // Get Scroll Wheel Input
        var scroll = Input.mouseScrollDelta;
        // If the scroll wheel is scrolled up, add 1 to the active slot, else subtract 1.
        activeSlot = scroll.y switch
        {
            > 0 => activeSlot == 0 ? slotCount - 1 : activeSlot - 1,
            < 0 => activeSlot == slotCount - 1 ? 0 : activeSlot + 1,
            _ => activeSlot
        };
        // Set the active slot in the UI to the active slot in the inventory
        for (var i = 0; i < inventoryPanel.transform.childCount; i++)
        {
            // Get the Image component of the slot
            var image = inventoryPanel.transform.GetChild(i).GetComponent<Image>();
            // If the slot is the active slot, set the color to white, else set the color to gray-ish
            image.color = i == activeSlot ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0.5f);
        }
    }
    
    
    
    // When exiting the game. set the inventory slots to the given amount. Used for different sizes of inventories for player, chests, etc.
    private void OnApplicationQuit()
    {
        inventory.items = new InventorySlot[slotCount];
    }
}

public static class MouseData
{
    // The panel the mouse is over
    public static InventoryUI interfaceMouseIsOver;
    // The Temporary item being dragged
    public static GameObject tempItemBeingDragged;
    // The Item being dragged
    public static InventorySlot item;
    // The Slot the mouse is over
    public static InventorySlot hoverSlot;
    // The Slot the mouse is over as a GameObject
    public static GameObject slotHoveredOver;
}
