using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerEquipment : MonoBehaviour
{
    [SerializeField] InventoryUI playerEquipmentUI;
    [SerializeField] public Item objectHeld;
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
            case ItemType.Weapon:
                Attack();
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

    private void Eat()
    {

        var consumable = objectHeld as consumableItem;
        playerEquipmentUI.inventory.RemoveItem(objectHeld, 1);
        PlayerProperties.Instance.RegenerateHunger(consumable.restoreHungerValue);
        Debug.Log("You ate " + consumable.name + " and gained " + consumable.restoreHungerValue + " hunger");
        // player.RegenerateHunger(consumable.restoreHungerValue);
    }

    private void Drink()
    {
        var bottle = objectHeld as WaterBottle;
        bottle.currentCapacity -= bottle.thirstRestore;
        PlayerProperties.Instance.RegenerateThirst(bottle.thirstRestore);
        Debug.Log("You drank from " + bottle.name + " and gained " + bottle.thirstRestore + " thirst" + " and now have " + bottle.currentCapacity + " left");
    }
    
    private void Attack()
    {
        var weapon = objectHeld as WeaponItem;
        Debug.Log("You attacked with " + weapon.name);
        // TODO: Add attack logic
    }

    private void UseTool()
    {
        var tool = objectHeld as ToolItem;
        Debug.Log("You used " + tool.name);
        // TODO: Add tool logic
    }

    public void SetObjectHeld(Item item)
    {
        objectHeld = item;
    }
}
