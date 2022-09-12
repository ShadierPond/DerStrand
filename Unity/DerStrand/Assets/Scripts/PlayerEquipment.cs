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
            case ItemType.None:
                break;
        }
    }

    private void Eat()
    {
        var consumable = objectHeld as consumableItem;
        playerEquipmentUI.inventory.RemoveItem(objectHeld, 1);
        Debug.Log("You ate " + consumable.name + " and gained " + consumable.restoreHungerValue + " hunger");
        // player.RegenerateHunger(consumable.restoreHungerValue);
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
