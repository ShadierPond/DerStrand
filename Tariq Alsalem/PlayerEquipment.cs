using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerEquipment : MonoBehaviour
{
    [SerializeField] InventoryUI playerEquipmentUI;
    [SerializeField] public Item objectHeld;
    [SerializeField] GameObject[] weapons;
    private PlayerProperties player;
    public static PlayerEquipment Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    
    private void Start()
    {
        player = PlayerProperties.Instance;
    }

    private void Update()
    {
        //if the player is holding an object in the equipment slot, it will be displayed on the screen
        if (!objectHeld)
        {
            foreach(var item in weapons)
            {
                item.SetActive(false);
            }
        }
        
        if(objectHeld)
            switch (objectHeld.type)
            {
            
                case ItemType.Weapon:
                    ShowWeapon();
                    break;
            
            }
    }
    // shows the weapon on the screen
    private void ShowWeapon()
    {
        var weapon = objectHeld as WeaponItem;
        switch (weapon.name)
        {
            case "Spear":
                foreach (var item in weapons)
                {
                    if (item.name == weapon.name)
                        item.SetActive(true);
                    else item.SetActive(false);
                }
                break;
        }    
    }
    //when the player presses the Primary button (left click), different actions will be performed depending on the type of item
    public void PrimaryAction(InputAction.CallbackContext context)
    {
        if (objectHeld == null)
            return;
        if(!context.performed)
            return;

        switch (objectHeld.type)
        {
            case ItemType.Consumable:
                Eat();
                break;
            case ItemType.Tool:
                UseTool();
                break;
            case ItemType.WaterBottle:
                Drink();
                break;
            case ItemType.None:
                break;
        }
    }
    //Eat the held item
    private void Eat()
    {

        var consumable = objectHeld as consumableItem;
        playerEquipmentUI.inventory.RemoveItem(objectHeld, 1);
        PlayerProperties.Instance.RegenerateHunger(consumable.restoreHungerValue);
        Debug.Log("You ate " + consumable.name + " and gained " + consumable.restoreHungerValue + " hunger");
        // player.RegenerateHunger(consumable.restoreHungerValue);
    }
    //Drink the held item
    private void Drink()
    {
        var bottle = objectHeld as WaterBottle;
        bottle.currentCapacity -= bottle.thirstRestore;
        PlayerProperties.Instance.RegenerateThirst(bottle.thirstRestore);
        Debug.Log("You drank from " + bottle.name + " and gained " + bottle.thirstRestore + " thirst" + " and now have " + bottle.currentCapacity + " left");
    }
    //Use the held item
    private void UseTool()
    {
        var tool = objectHeld as ToolItem;
        Debug.Log("You used " + tool.name);
        if(tool.name == "Axe")
        {
            Debug.Log("You chopped down a tree");
            TreeController.Instance.ChopDownTree();
        }
    }
    // set the object held
    public void SetObjectHeld(Item item)
    {
        objectHeld = item;
    }
}
