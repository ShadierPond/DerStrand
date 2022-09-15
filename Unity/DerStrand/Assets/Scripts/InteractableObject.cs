using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InteractableObject : MonoBehaviour
{
    private enum ObjectType
    {
        None,
        Chest,
        Water,
    }
    [SerializeField] private ObjectType objectType;
    [SerializeField] private bool isInteractable;
    [SerializeField] private bool isLocked;
    [SerializeField] private bool isOpen; 
    [SerializeField] private bool isBuildable;
    
    [Header("Chest")]
    [SerializeField] private GameObject chestUI;
    [SerializeField] private float chestTransitionTime;
    
    [Header("Build")]
    [SerializeField] private Material objectMaterial;
    private Material buildMaterial;
    [SerializeField] private Item[] requiredItems;
    [SerializeField] private int[] requiredItemAmounts;
    [SerializeField] private bool[] ingredientCheck;

    private void Start()
    {
        var renderers = gameObject.GetComponentsInChildren<MeshRenderer>();
        buildMaterial = Resources.Load("Materials/BuildMaterial", typeof(Material)) as Material;

        if (isBuildable)
            foreach (var meshRenderer in renderers)
                meshRenderer.material = buildMaterial;
        else
            foreach (var meshRenderer in renderers)
                meshRenderer.material = objectMaterial;
    }

    public void Interact()
    {
        if (isInteractable && !isLocked)
        {
            switch (objectType)
            {
                case ObjectType.Chest:
                    Debug.Log("Chest opened");
                    isOpen = !isOpen;
                    StartCoroutine(ChestAnimationTransition());
                    break;
                case ObjectType.Water:
                    FillWaterBottle();
                    break;
            }
        }
        if (isBuildable && !isInteractable)
        {
            Build();
        }
    }
    
    private IEnumerator ChestAnimationTransition()
    {
        var chestCanvas = chestUI.GetComponent<CanvasGroup>();
        //var inventory = chestUI.transform.GetChild(1).GetComponent<InventoryUI>();
        //inventory.inventory = chestInventory;
        //inventory.Start();
        //Debug.Log(chestUI.transform.GetChild(1).GetComponent<InventoryUI>().inventory);
        if(isOpen)
        {
            LockMouse(false);
            Debug.Log("animating chest open");
            chestCanvas.alpha = 0;
            chestUI.SetActive(true);
            chestCanvas.DOFade(1, chestTransitionTime);
            yield return new WaitForSeconds(chestTransitionTime);
        }
        else
        {
            LockMouse(true);
            Debug.Log("animating chest close");
            chestCanvas.alpha = 1;
            chestCanvas.DOFade(0, chestTransitionTime);
            yield return new WaitForSeconds(chestTransitionTime);
            chestUI.SetActive(false);
        }
    }

    private void FillWaterBottle()
    {
        var objectOnHand = PlayerEquipment.Instance.objectHeld;
        if (objectOnHand != null)
        {
            if (objectOnHand.name == "WaterBottle")
            {
                var bottle = objectOnHand as WaterBottle;
                bottle.currentCapacity = bottle.capacity;
                Debug.Log("Water bottle filled");
            }
        }
    }
    
    private void Build()
    {
        var playerInventory = Player.Instance.inventory;
        ingredientCheck = new bool[requiredItems.Length];
        
        for (var i = 0; i < requiredItems.Length; i++)
            foreach (var item in playerInventory.items)
            {
                if (item.item != requiredItems[i])
                    continue;
                
                if (item.amount >= requiredItemAmounts[i])
                    ingredientCheck[i] = true;
                else 
                    ingredientCheck[i] = false;
            }
        bool canBeBuilt;
        if (ingredientCheck.Length == 0)
            canBeBuilt = false;
        else 
        {
            canBeBuilt = true;
            foreach (var check in ingredientCheck)
                if (!check)
                    canBeBuilt = false;
        }

        if (!canBeBuilt)
            return;

        isInteractable = true;
        isBuildable = false;
        Start();
        for (var i = 0; i < requiredItems.Length; i++)
            playerInventory.RemoveItem(requiredItems[i], requiredItemAmounts[i]);

    }
    
    

    private void LockMouse(bool state)
    {
        Cursor.lockState = state ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !state;
    }

}
