using UnityEngine;

[CreateAssetMenu(fileName = "New Water Bottle", menuName = "Inventory/Item/WaterBottle")]
public class WaterBottle : Item
{
    private void Awake()
    {
        type = ItemType.WaterBottle;
    }
    
    public int thirstRestore;
    public int capacityDrank;
    public float capacity;
    public float currentCapacity;
}
