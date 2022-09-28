using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item/Consumable")]
public class consumableItem : Item
{
    // Value the item will add to the player's stats
    public int restoreHealthValue;
    public int restoreHungerValue;
    public int restoreThirstValue;
    private void Awake()
    {
        type = ItemType.Consumable;
    }
}
