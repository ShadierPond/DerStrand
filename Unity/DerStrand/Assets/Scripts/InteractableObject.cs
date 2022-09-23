using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class InteractableObject : MonoBehaviour
{
    private enum ObjectType
    {
        None,
        Chest,
        Water,
        Bed,
        CampFire,
        Trap,
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
    
    [Header("Campfire")]
    [SerializeField] private GameObject campfireUI;
    [SerializeField] private float campfireTransitionTime;
    
    [Header("Trap")]
    [SerializeField] private bool trapWorksWhenPlayerLooksAtIt;
    [SerializeField] private bool isTrapSet;
    [SerializeField] private bool isTrapTriggered;
    [SerializeField] private float trapRandomTimeLimit;
    [SerializeField] private float trapSetAngle; 
    [SerializeField] private Item[] resetTrapIngredients;
    [SerializeField] private int[] resetTrapIngredientAmounts;
    [SerializeField] private bool[] resetTrapIngredientCheck;
    private float timer;
    private float randomTime;
    private bool PlayerLookingAtTrap(Camera cam, GameObject target)
    {
        return GeometryUtility.CalculateFrustumPlanes(cam).All(plane => !(plane.GetDistanceToPoint(target.transform.position) < 0));
    }

    
    private void Start()
    {
        if(objectType == ObjectType.Trap)
            randomTime = Random.Range(0, trapRandomTimeLimit);
        
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
                case ObjectType.CampFire:
                    Debug.Log("Campfire used");
                    StartCoroutine(ObjectAnimationTransition(campfireUI, campfireTransitionTime));
                    break;
                case ObjectType.Trap:
                    Debug.Log("Trap used");
                    CheckTrap();
                    break;
            }
        }
        if (isBuildable && !isInteractable)
            Build();
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
        if(objectType != ObjectType.None)
            isInteractable = true;
        isBuilt = true;
        Start();
        for (var i = 0; i < requiredItems.Length; i++)
            playerInventory.RemoveItem(requiredItems[i], requiredItemAmounts[i]);

    }

    private void CheckTrap()
    {
        if (isTrapSet && isTrapTriggered)
        {
            isBuildable = true;
            Start();
            isTrapSet = false;
            isTrapTriggered = false;
            randomTime = Random.Range(0, trapRandomTimeLimit);
            Debug.Log("Trap reset");
            foreach (var item in Player.Instance.inventory.database.items)
            {
                if (item.name != "Raw Meat")
                    continue;
                Player.Instance.inventory.AddItem(item, 1);
                break;
            }
        }
        else if(!isTrapSet && !isTrapTriggered)
        {
            var playerInventory = Player.Instance.inventory;
            resetTrapIngredientCheck = new bool[resetTrapIngredients.Length];
            
            for (var i = 0; i < resetTrapIngredients.Length; i++)
                foreach (var item in playerInventory.items)
                {
                    if (item.item != resetTrapIngredients[i])
                        continue;
                
                    if (item.amount >= resetTrapIngredientAmounts[i])
                        resetTrapIngredientCheck[i] = true;
                    else 
                        resetTrapIngredientCheck[i] = false;
                }
            bool canBeReset;
            if (resetTrapIngredientCheck.Length == 0)
                canBeReset = false;
            else 
            {
                canBeReset = true;
                foreach (var check in resetTrapIngredientCheck)
                    if (!check)
                        canBeReset = false;
            }

            if (!canBeReset)
                return;
            isTrapSet = true;
            gameObject.transform.Rotate(0, 0, trapSetAngle);
            for (var i = 0; i < resetTrapIngredients.Length; i++)
                playerInventory.RemoveItem(resetTrapIngredients[i], resetTrapIngredientAmounts[i]);
            var renderers = gameObject.GetComponentsInChildren<MeshRenderer>();
            foreach (var meshRenderer in renderers)
                meshRenderer.material = objectMaterial;
        }
    }

    private void Update()
    {
        if (objectType != ObjectType.Trap)
            return;
        if(!isTrapSet)
            return;
        if (PlayerLookingAtTrap(Camera.main, gameObject) && !trapWorksWhenPlayerLooksAtIt)
            return;
        timer += Time.deltaTime;
        if (isTrapSet && !isTrapTriggered && timer >= randomTime)
        {
            Debug.Log("Trap triggered");
            isTrapTriggered = true;
            gameObject.transform.Rotate(0, 0, -trapSetAngle);
            timer = 0;
        }
    }

    private void LockMouse(bool state)
    {
        Cursor.lockState = state ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !state;
    }

}
