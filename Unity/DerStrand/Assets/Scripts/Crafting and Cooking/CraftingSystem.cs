using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    // Player Inventory
    public Inventory playerInventory;
    // Crafting Slot Prefab
    private GameObject craftingSlotPrefab;
    // Ingredient Slot Prefab
    private GameObject ingredientSlotPrefab;
    // The Panel that holds the crafting slots
    public GameObject craftingPanel;
    // The Selected Crafting Slot
    public GameObject selectedSlot;
    // The Color of craftable items (when player have required ingredients)
    public Color interactableColor;
    // The Color of uncraftable items (when player don't have required ingredients)
    public Color nonInteractableColor;
    // The Amount mltiplier for the crafting (for example, if you want to craft 10 items, you need 10 times more ingredients)
    [SerializeField] private int craftingMultiplier = 1;
    // List of all the crafting slots
    public List<InventorySlot> craftingSlotsList = new List<InventorySlot>();
    // List of all the objects of the Crafting Slots (fore interacting with them)
    public List<GameObject> craftingSlotObjects = new List<GameObject>();
    // Public access to the Crafting Class
    public static CraftingSystem Instance { get; private set; }
    
    [Header("Campfire Settings")]
    // Distance between Player and Campfire
    public float distanceToCampfire;
    // is the player near the campfire
    private bool nearCampfire;
    // Player Transform
    private Transform playerTransform;
    // Array of Campfires
    private GameObject[] campfires;
    
    // Set the Instance
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Get the Crafting Slot Prefab from the Resources Folder
        craftingSlotPrefab = Resources.Load("CraftingSlot") as GameObject;
        // Get the Ingredient Slot Prefab from the Resources Folder
        ingredientSlotPrefab = Resources.Load("IngredientSlot") as GameObject;
        // Get the Campfires from the Scene
        campfires = GameObject.FindGameObjectsWithTag("Campfire");
        // Get the Player Transform
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        CreateSlots();
        SetIngredients();
    }

    private void Update()
    {
        CheckIngredients();
        CheckPlayerNearCampfire();
    }
    // Create the Crafting Slots on Start (Instantiate the Prefab)
    private void CreateSlots()
    {
        // Loop through all the items in the Database
        foreach (var item in playerInventory.database.items)
            // If the item is craftable
            if (item.isCraftable)
                // Add the item to the Crafting Slots List
                craftingSlotsList.Add(new InventorySlot(playerInventory.database.items[item.id], 1));
        // Loop through all the items in the Crafting Slots List
        foreach (var recipe in craftingSlotsList)
        {
            // Instantiate the Crafting Slot Prefab
            var recipeObj =  Instantiate(craftingSlotPrefab, craftingPanel.transform);
            // Add the instantiated object to the Crafting Slot Objects List
            craftingSlotObjects.Add(recipeObj);
            // Set the Button Component of the instantiated object. If the button is pressed, save the object as the Selected Slot
            recipeObj.GetComponent<Button>().onClick.AddListener(() => selectedSlot = recipeObj);
            // Set the Icon of the instantiated object to the item's icon
            recipeObj.transform.GetChild(0).GetComponent<Image>().sprite = playerInventory.database.getItem[recipe.item.id].icon;
            // Set the Name of the instantiated object to the item's name
            recipeObj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = playerInventory.database.getItem[recipe.item.id].name;
            // Set the Crafting Slot to false (not craftable). Default
            ActiveCraftingSlot(recipeObj, false);
            if(recipe.item.needsCampfire)
                recipeObj.transform.GetChild(3).gameObject.SetActive(true);
            else
                recipeObj.transform.GetChild(3).gameObject.SetActive(false);
        }
    }
    // Set the Ingredients for each Crafting Slot
    private void SetIngredients()
    {
        // Loop through all the items in the Crafting Slots List
        for (int i = 0; i < craftingSlotsList.Count; i++)
        {
            // Get the Crafting Slot Object
            var recipeObj = craftingSlotObjects[i];
            
            // Loop through all the ingredients in the Crafting Slot
            for (int j = 0; j < craftingSlotsList[i].item.ingredients.Count; j++)
            {
                // Instantiate the Ingredient Slot Prefab
                var ingredientObj = Instantiate(ingredientSlotPrefab, recipeObj.transform.GetChild(2));
                // Set the Icon of the instantiated object to the ingredient's icon
                ingredientObj.GetComponentInChildren<Image>().sprite = playerInventory.database.getItem[craftingSlotsList[i].item.ingredients[j].id].icon;
                // Set the Amount of the instantiated object to the ingredient's amount * the crafting multiplier
                ingredientObj.GetComponentInChildren<TextMeshProUGUI>().text = (craftingSlotsList[i].item.ingredientAmounts[j] * craftingMultiplier).ToString();
            }
        }
    }
    
    // Check if the player have the required ingredients to craft the item
    private void CheckIngredients()
    {
        // Loop through all the items in the Crafting Slots List
        for (int i = 0; i < craftingSlotsList.Count; i++)
        {
            // Create a list of bools that contains the size of the ingredients for the item
            bool[] ingredientCheck = new bool[craftingSlotsList[i].item.ingredients.Count];
            // Get the Crafting Slot Object
            var recipeObj = craftingSlotObjects[i];
            // Loop through all the ingredients in the Crafting Slot
            for (int j = 0; j < craftingSlotsList[i].item.ingredients.Count; j++)
            {
                // Loop through all the ingredients in the Crafting Slot (again)
                for (int k = 0; k < craftingSlotsList[i].item.ingredients.Count; k++)
                {
                    // Set the Text Amount of the Ingredient Slot to the ingredient's amount * the crafting multiplier (Update the amount)
                    recipeObj.transform.GetChild(2).GetChild(k).GetComponentInChildren<TextMeshProUGUI>().text = (craftingSlotsList[i].item.ingredientAmounts[k] * craftingMultiplier).ToString();
                }
                // Loop through all the items in the Player Inventory
                foreach (var item in playerInventory.items)
                {
                    // If the Item in the Player Inventory is the same as the Ingredient in the Crafting Slot
                    if (item.item == craftingSlotsList[i].item.ingredients[j])
                    {
                        // If the Amount of the Item in the Player Inventory is greater or equal to the Amount of the Ingredient in the Crafting Slot
                        if(item.amount >= craftingSlotsList[i].item.ingredientAmounts[j] * craftingMultiplier)
                            // Set the Ingredient Check to true (the bool in the list for the ingredient)
                            ingredientCheck[j] = true;
                        else
                            // Set the Ingredient Check to false if the player don't have enough ingredients
                            ingredientCheck[j] = false;
                    }
                }
            }
            // if there is no ingredients in the Crafting Slot
            if (ingredientCheck.Length == 0)
                // Set the Crafting Slot to false (not craftable)
                ActiveCraftingSlot(recipeObj, false);
            else
            {
                // set a bool for checking if the player have all the ingredients. Default is true
                bool canCraft = true;
                // Loop through all the ingredients in the bool list (ingredientCheck)
                foreach (var check in ingredientCheck)
                {
                    // if one of the ingredients is false
                    if (!check)
                        // Set the canCraft bool to false
                        canCraft = false;
                }
                
                if(craftingSlotsList[i].item.needsCampfire && !nearCampfire)
                    canCraft = false;

                // Set the Crafting Slot to true (craftable) if the player have all the ingredients (if the list of bool (ingredientCheck) is true)
                ActiveCraftingSlot(recipeObj, canCraft);
            }
        }
    }
    // Set the Crafting Slot to craftable or not craftable
    private void ActiveCraftingSlot(GameObject recipe, bool state)
    {
        // Get the List of Images of the Ingredients in the Crafting Slot
        var objIngredientsImage = recipe.transform.GetChild(2).GetComponentsInChildren<Image>();
        // Get the List of Texts of the Ingredients in the Crafting Slot
        var objIngredientsText = recipe.transform.GetChild(2).GetComponentsInChildren<TextMeshProUGUI>();
        
        // if the state (craftable or not craftable) is false
        if (!state)
        {
            // Set the button of the Crafting Slot to non interactable
            recipe.GetComponent<Button>().interactable = false;
            // Set the Color of the Crafting Slot Image to not craftable
            recipe.transform.GetChild(0).GetComponent<Image>().color = nonInteractableColor;
            // Set the Text Color of the Crafting Slot to not craftable
            recipe.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = nonInteractableColor;
            // Loop through all the Images of the Ingredients in the Crafting Slot
            foreach (var ingredient in objIngredientsImage)
                // Set the Color of the Ingredient Image to not craftable
                ingredient.color = nonInteractableColor;
            // Loop through all the Texts of the Ingredients in the Crafting Slot
            foreach (var ingredient in objIngredientsText)
                // Set the Text Color of the Ingredient to not craftable
                ingredient.color = nonInteractableColor;
        }
        // if the state (craftable or not craftable) is true
        else
        {
            // Set the button of the Crafting Slot to interactable
            recipe.GetComponent<Button>().interactable = true;
            // Set the Color of the Crafting Slot Image to craftable
            recipe.transform.GetChild(0).GetComponent<Image>().color = nonInteractableColor;
            // Set the Text Color of the Crafting Slot to craftable
            recipe.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = nonInteractableColor;
            // Loop through all the Images of the Ingredients in the Crafting Slot
            foreach (var ingredient in objIngredientsImage)
                // Set the Color of the Ingredient Image to craftable
                ingredient.color = nonInteractableColor;
            // Loop through all the Texts of the Ingredients in the Crafting Slot
            foreach (var ingredient in objIngredientsText)
                // Set the Text Color of the Ingredient to craftable
                ingredient.color = nonInteractableColor;
        }
    }
    // Get the Multiplier of the Crafting Slot from the Input Field
    public void GetCraftingMultiplier(TMP_InputField multiplier)
    {
        // If the Input Field is not empty and doesn't contain 0 or a negative number
        if(multiplier.text != "" && multiplier.text != "0" && multiplier.text != "-")
            // Get the multiplier from the Input Field
            craftingMultiplier = int.Parse(multiplier.text);
        else
        {
            // Default the multiplier to 1
            craftingMultiplier = 1;
            // Set the Input Field to empty
            multiplier.text = "";
        }
    }
    
    // Craft the item
    public void CraftItem()
    {
        // if the selected Crafting Slot is not empty
        if (selectedSlot != null)
        {
            // Get the Item from the Crafting Slot
            var recipe = craftingSlotsList[craftingSlotObjects.IndexOf(selectedSlot)];
            // Loop through all the ingredients in the Crafting Slot
            for (int i = 0; i < recipe.item.ingredients.Count; i++)
            {
                // Remove the ingredients from the Player Inventory (subtract the amount of the ingredient * the crafting multiplier)
                playerInventory.RemoveItem(recipe.item.ingredients[i], recipe.item.ingredientAmounts[i] * craftingMultiplier);
            }
            // Add the crafted item to the Player Inventory (add the amount of the crafted item * the crafting multiplier)
            playerInventory.AddItem(recipe.item, craftingMultiplier);
        }
    }

    // Check if the player is nearby the Campfire. used for cooking in the crafting menu
    private void CheckPlayerNearCampfire()
    {
        // if there are no Campfires in the scene. set the bool to false
        if(campfires.Length == 0)
            nearCampfire = false;
        else
        {
            // Set the bool to false. standard
            nearCampfire = false;
            // Loop through all the Campfires in the scene
            foreach (var check in campfires)
                // if the player is within the radius of the Campfire
                if(Vector3.Distance(playerTransform.position, check.transform.position) <= distanceToCampfire)
                    // Set the bool to true
                    nearCampfire = true;
        }
    }
}
