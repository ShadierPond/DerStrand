using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item/Weapon")]
public class WeaponItem : Item
{
    public int damage;
    private void Awake()
    {
        type = ItemType.Weapon;
    }
}
