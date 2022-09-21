using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item/Consumable")]
public class consumableItem : Item
{
    public int restoreHealthValue;
    public int restoreHungerValue;
    public int restoreThirstValue;
    public bool needsCampfire;
    public bool isCooked;
    private void Awake()
    {
        type = ItemType.Consumable;
    }
}
