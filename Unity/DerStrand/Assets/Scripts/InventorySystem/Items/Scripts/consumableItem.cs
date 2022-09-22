using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item/Consumable")]
public class consumableItem : Item
{
    public int restoreHealthValue;
    public int restoreHungerValue;
    public int restoreThirstValue;
    private void Awake()
    {
        type = ItemType.Consumable;
    }
}
