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
        Bed,
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
    [Rename("Is Built? (Debug)"), SerializeField] private bool isBuilt;
    [SerializeField] private Material objectMaterial;
    private Material buildMaterial;
    [SerializeField] private Item[] requiredItems;
    [SerializeField] private int[] requiredItemAmounts;
    [SerializeField] private bool[] ingredientCheck;
    
    [Header("Bed")]
    [SerializeField] private GameObject bedUI;
    [SerializeField] private float bedTransitionTime;
    

    private void Start()
    {
        if (!isBuildable) 
            return;
        
        var renderers = gameObject.GetComponentsInChildren<MeshRenderer>();
        buildMaterial = Resources.Load("Materials/BuildMaterial", typeof(Material)) as Material;
            
        if (isBuilt)
            foreach (var meshRenderer in renderers)
                meshRenderer.material = objectMaterial;
        else
            foreach (var meshRenderer in renderers)
                meshRenderer.material = buildMaterial;
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
                    StartCoroutine(ObjectAnimationTransition(chestUI, chestTransitionTime));
                    break;
                case ObjectType.Water:
                    Debug.Log("Water Bottle filled");
                    FillWaterBottle();
                    break;
                case ObjectType.Bed:
                    Debug.Log("Bed used");
                    isOpen = !isOpen;
                    StartCoroutine(ObjectAnimationTransition(bedUI, bedTransitionTime));
                    break;
            }
        }
        if (isBuildable && !isInteractable)
        {
            Build();
        }
    }

    private IEnumerator ObjectAnimationTransition(GameObject targetObject, float transitionTime)
    {
        var objectCanvas = targetObject.GetComponent<CanvasGroup>();
        if(isOpen)
        {
            LockMouse(false);
            GameManager.Instance.PauseGame(true);
            objectCanvas.alpha = 0;
            targetObject.SetActive(true);
            objectCanvas.DOFade(1, transitionTime);
            yield return new WaitForSeconds(transitionTime);
        }
        else
        {
            LockMouse(true);
            GameManager.Instance.PauseGame(false);
            objectCanvas.alpha = 1;
            objectCanvas.DOFade(0, transitionTime);
            yield return new WaitForSeconds(transitionTime);
            targetObject.SetActive(false);
        }
    }

    private void FillWaterBottle()
    {
        var objectOnHand = PlayerEquipment.Instance.objectHeld;
        if (objectOnHand != null)
        {
            if (objectOnHand.name == "Water Bottle")
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
        isBuilt = true;
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
