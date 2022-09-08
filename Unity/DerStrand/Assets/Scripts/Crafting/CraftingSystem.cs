using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    public Inventory playerInventory;
    private GameObject craftingSlotPrefab;
    private GameObject ingredientSlotPrefab;
    public GameObject craftingPanel;
    public GameObject selectedSlot;
    public Color interactableColor;
    public Color nonInteractableColor;
    [SerializeField] private int craftingMultiplier = 1;
    public List<InventorySlot> craftingSlotsList = new List<InventorySlot>();
    public List<GameObject> craftingSlotObjects = new List<GameObject>();

    public static CraftingSystem Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

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

        for (int i = 0; i < craftingSlotsList.Count; i++)
        {
            var recipeObj = craftingSlotObjects[i];
            
            for (int j = 0; j < craftingSlotsList[i].item.ingredients.Count; j++)
            {
                var ingredientObj = Instantiate(ingredientSlotPrefab, recipeObj.transform.GetChild(2));
                ingredientObj.GetComponentInChildren<Image>().sprite = playerInventory.database.getItem[craftingSlotsList[i].item.ingredients[j].id].icon;
                ingredientObj.GetComponentInChildren<TextMeshProUGUI>().text = (craftingSlotsList[i].item.ingredientAmounts[j] * craftingMultiplier).ToString();
            }
        }
    }

    public void CheckIngredients()
    {
        for (int i = 0; i < craftingSlotsList.Count; i++)
        {
            bool[] ingredientCheck = new bool[craftingSlotsList[i].item.ingredients.Count];
            var recipeObj = craftingSlotObjects[i];
            for (int j = 0; j < craftingSlotsList[i].item.ingredients.Count; j++)
            {
                for (int k = 0; k < craftingSlotsList[i].item.ingredients.Count; k++)
                {
                    recipeObj.transform.GetChild(2).GetChild(k).GetComponentInChildren<TextMeshProUGUI>().text = (craftingSlotsList[i].item.ingredientAmounts[k] * craftingMultiplier).ToString();
                }
                
                foreach (var item in playerInventory.items)
                {
                    if (item.item == craftingSlotsList[i].item.ingredients[j])
                    {
                        if(item.amount >= craftingSlotsList[i].item.ingredientAmounts[j] * craftingMultiplier)
                            ingredientCheck[j] = true;
                        else
                            ingredientCheck[j] = false;
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
    
    public void GetCraftingMultiplier(TMP_InputField multiplier)
    {
        if(multiplier.text != "" && multiplier.text != "0" && multiplier.text != "-")
            craftingMultiplier = int.Parse(multiplier.text);
        else
        {
            craftingMultiplier = 1;
            multiplier.text = "";
        }
    }
    
    public void CraftItem()
    {
        if (selectedSlot != null)
        {
            var recipe = craftingSlotsList[craftingSlotObjects.IndexOf(selectedSlot)];
            for (int i = 0; i < recipe.item.ingredients.Count; i++)
            {
                playerInventory.RemoveItem(recipe.item.ingredients[i], recipe.item.ingredientAmounts[i] * craftingMultiplier);
            }
            playerInventory.AddItem(recipe.item, craftingMultiplier);
        }
    }
}
