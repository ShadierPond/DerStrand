using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item/Default")]
public class defaultItem : Item
{
    private void Awake()
    {
        type = ItemType.None;
    }
}
