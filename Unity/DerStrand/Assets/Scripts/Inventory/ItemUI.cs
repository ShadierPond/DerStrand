using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    private InventorySystem _inventory;
    private Item _item;
    private Image _imageRenderer;
    
    private void Awake()
    {
        _inventory = InventorySystem.Instance;
        _imageRenderer = GetComponent<Image>();
        UpdateItem(null);
    }

    private void UpdateItem(Item item)
    {
        _item = item;
        if(_item != null)
        {
            _imageRenderer.color = _inventory.fullSlotColor;
            _imageRenderer.sprite = _item.itemIcon;
        }
        else
        {
            _imageRenderer.color = _inventory.emptySlotColor;
            _imageRenderer.sprite = null;
        }
    }
}
