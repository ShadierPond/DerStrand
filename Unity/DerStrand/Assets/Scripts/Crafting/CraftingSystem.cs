using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    public Inventory playerInventory;
    public GameObject craftingSlotPrefab;
    public GameObject ingredientSlotPrefab;
    public GameObject craftingPanel;
    public GameObject selectedSlot;
    public Color interactableColor;
    public Color nonInteractableColor;
    public List<InventorySlot> craftingSlotsList = new List<InventorySlot>();
    public List<GameObject> craftingSlotObjects = new List<GameObject>();

    private void Start()
    {
        craftingSlotPrefab = Resources.Load("CraftingSlot") as GameObject;
        ingredientSlotPrefab = Resources.Load("IngredientSlot") as GameObject;
        CreateSlots();
        SetIngredients();
    }

    private void Update()
    {
        CheckIngredients();
    }

    public void CreateSlots()
    {
        foreach (var item in playerInventory.database.items)
            if (item.isCraftable)
                craftingSlotsList.Add(new InventorySlot(playerInventory.database.items[item.id], 1));

        foreach (var recipe in craftingSlotsList)
        {
            var recipeObj =  Instantiate(craftingSlotPrefab, craftingPanel.transform);
            craftingSlotObjects.Add(recipeObj);
            recipeObj.GetComponent<Button>().onClick.AddListener(() => selectedSlot = recipeObj);
            recipeObj.transform.GetChild(0).GetComponent<Image>().sprite = playerInventory.database.getItem[recipe.item.id].icon;
            recipeObj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = playerInventory.database.getItem[recipe.item.id].name;
            ActiveCraftingSlot(recipeObj, false);
        }
    }
    
    public void SetIngredients()
    {
        foreach (var recipe in craftingSlotsList)
        {
            var recipeObj = craftingSlotObjects[recipe.item.id];
            for (int i = 0; i < recipe.item.ingredients.Count; i++)
            {
                var ingredientObj = Instantiate(ingredientSlotPrefab, recipeObj.transform.GetChild(2));
                ingredientObj.GetComponentInChildren<Image>().sprite = playerInventory.database.getItem[recipe.item.ingredients[i].id].icon;
                ingredientObj.GetComponentInChildren<TextMeshProUGUI>().text = recipe.item.ingredientAmounts[i].ToString();
            }
        }
    }
    
    public void CheckIngredients()
    {
        foreach (var recipe in craftingSlotsList)
        {
            bool[] ingredientCheck = new bool[recipe.item.ingredients.Count];
            var recipeObj = craftingSlotObjects[recipe.item.id];
            for (int i = 0; i < recipe.item.ingredients.Count; i++)
            {
                foreach (var item in playerInventory.items)
                {
                    if (item.item == recipe.item.ingredients[i])
                    {
                        if(item.amount >= recipe.item.ingredientAmounts[i])
                            ingredientCheck[i] = true;
                        else
                            ingredientCheck[i] = false;
                    }
                }
            }
            if (ingredientCheck.Length == 0)
                ActiveCraftingSlot(recipeObj, false);
            else
            {
                bool canCraft = true;
                foreach (var check in ingredientCheck)
                {
                    if (!check)
                        canCraft = false;
                }
                ActiveCraftingSlot(recipeObj, canCraft);
            }
        }
    }

    public void ActiveCraftingSlot(GameObject recipe, bool state)
    {
        if (!state)
        {
            recipe.GetComponent<Button>().interactable = false;
            var objIngredientsImage = recipe.transform.GetChild(2).GetComponentsInChildren<Image>();
            var objIngredientsText = recipe.transform.GetChild(2).GetComponentsInChildren<TextMeshProUGUI>();
            recipe.transform.GetChild(0).GetComponent<Image>().color = nonInteractableColor;
            recipe.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = nonInteractableColor;
            foreach (var ingredient in objIngredientsImage)
                ingredient.color = nonInteractableColor;
            foreach (var ingredient in objIngredientsText)
                ingredient.color = nonInteractableColor;
        }
        else
        {
            recipe.GetComponent<Button>().interactable = true;
            var objIngredientsImage = recipe.transform.GetChild(2).GetComponentsInChildren<Image>();
            var objIngredientsText = recipe.transform.GetChild(2).GetComponentsInChildren<TextMeshProUGUI>();
            recipe.transform.GetChild(0).GetComponent<Image>().color = nonInteractableColor;
            recipe.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = nonInteractableColor;
            foreach (var ingredient in objIngredientsImage)
                ingredient.color = nonInteractableColor;
            foreach (var ingredient in objIngredientsText)
                ingredient.color = nonInteractableColor;
        }
    }
}
