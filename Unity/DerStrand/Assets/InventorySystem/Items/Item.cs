using UnityEngine;

public enum ItemType
{
    None,
    Weapon,
    Tool,
    Consumable
}

public abstract class Item : ScriptableObject
{
    public int id;
    public Sprite icon;
    public ItemType type;
    [TextArea(15, 20)]
    public string description;
    
}
