using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item/Consumable")]
public class consumableItem : Item
{
    public int restoreHealthValue;
    private void Awake()
    {
        type = ItemType.Consumable;
    }
}
