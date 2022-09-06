using UnityEngine;

// Types of Items
public enum ItemType
{
    None,
    Weapon,
    Tool,
    Consumable
}

// Base class for all items
public abstract class Item : ScriptableObject
{
    // Item id
    public int id;
    // Item Icon for inventory UI
    public Sprite icon;
    // Prefab for item for instantiating in the world
    public GameObject prefab;
    // Item type
    public ItemType type;
    // Item description for inventory UI on mouse Hover
    [TextArea(15, 20)]
    public string description;
    
}
