using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [Header("Current Info")]
    public GameObject inventoryPanel;
    private bool isShown;
    [HideInInspector]
    public int currTab;
    [HideInInspector]
    public int currCategory;
    [HideInInspector]
    public CraftingData[] allCraftingRecipes;
    public static CraftingData selectedRecipe; //(To track selected recipe)
    // public CraftingData selectedRecipeDebug; // For debugging

    [Header("Inventory Tabs")]
    [Tooltip("Make sure the respective Icons & Titles have the same index")]
    public List<Image> inventoryTabIcons = new List<Image>();
    
    public List<Image> inventoryTabIconBgs = new List<Image>();
    public List<Text> inventoryTabTitle = new List<Text>();
    
    public List<GameObject> inventoryTabContent = new List<GameObject>();
    [Tooltip("Don't forget to set the alpha")]
    public Color selectedTabColor;

    [Header("Category Tabs")]
    [Tooltip("Make sure the respective BGs & Titles have the same index")]
    public List<Image> categoryTabBgs = new List<Image>();
    public List<Text> categoryTabTitles = new List<Text>();
    [Tooltip("Don't forget to set the alpha")]
    public Color selectedCategoryColor;

    [Header("Pin Mechanics")]
    public Transform craftingSlotParent;
    [HideInInspector]
    public List<GameObject> pinList = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> originalRecipeIdx = new List<GameObject>();

    [Header("Crafting Details UI")]
    public CraftingDetail craftingDetail;
    public GameObject craftingDetailParent;

    private void Start() {
        Initialization();

    }

    private void Update() {
        // Don't forget to add a condition where this functions only work if it's currently in Crafting tab
        GetInput();
        // UpdateCategory();

        // // For debugging purposes
        // if(selectedRecipe != null){
        //     selectedRecipeDebug = selectedRecipe;

        // }

    }

    void Initialization(){
        // Get all crafting recipes
        allCraftingRecipes = FindObjectsOfType<CraftingData>();

        // Init the Inventory Tabs
        if( (inventoryTabIcons.Count != 0 || inventoryTabTitle.Count != 0) && inventoryTabIcons.Count == inventoryTabTitle.Count){
            for(int i = 0; i < inventoryTabIcons.Count; i++){
                inventoryTabTitle[i].GetComponent<Text>().gameObject.SetActive(false);
                inventoryTabIconBgs[i].color = new Color(1f, 1f, 1f, 0f);

            }

        }

        // Set the current tab (in this case Crafting Tab)
        currTab = 3;
        inventoryTabIcons[3].GetComponent<Image>().gameObject.SetActive(true);
        inventoryTabIcons[3].GetComponent<Image>().color = selectedTabColor;
        inventoryTabIconBgs[3].color = new Color(1f, 1f, 1f, 1f);
        inventoryTabTitle[3].GetComponent<Text>().gameObject.SetActive(true);

        // Disable tabs that aren't the current one
        for(int i = 0; i < inventoryTabContent.Count; i++){
            if(i != currTab){
                inventoryTabContent[i].SetActive(false);
            }
        }

        // Init the Category Tabs
        if( (categoryTabBgs.Count != 0 || categoryTabTitles.Count != 0) && categoryTabBgs.Count == categoryTabTitles.Count){
            for(int i = 0; i < categoryTabBgs.Count; i++){
                categoryTabTitles[i].GetComponent<Text>().gameObject.SetActive(false);
                
                // Change the color
                Color newColors = categoryTabBgs[i].color;
                newColors.a = 0.15f;
                categoryTabBgs[i].color = newColors;

            }

        }

        // Set the current Category (in this case All Category)
        currCategory = 0;

        // Change the color
        Color newColor = categoryTabBgs[0].color;
        newColor.a = 1;
        categoryTabBgs[0].color = newColor;

        categoryTabBgs[0].GetComponent<Image>().color = selectedCategoryColor;
        categoryTabTitles[0].GetComponent<Text>().gameObject.SetActive(true);

        UpdateCategory();

        // Init the original indexes of each slot
        for(int i = 0; i < craftingSlotParent.childCount; i++){
            originalRecipeIdx.Add(craftingSlotParent.GetChild(i).gameObject);
        }

        // Set the details off
        craftingDetail.gameObject.SetActive(false);
        craftingDetailParent.transform.localScale = Vector3.zero;

    }

    void GetInput(){
        // Change tab to the Left
        if(Input.GetKeyUp(KeyCode.Q)){

            if(currTab == 0){
                currTab = inventoryTabIcons.Count-1;

            }else{
                currTab--;

            }

            ChangeInventoryTab(currTab);

        }
        // Change tab to the Right
        if(Input.GetKeyUp(KeyCode.E)){

            if(currTab == inventoryTabIcons.Count-1){
                currTab = 0;
            }else{
                currTab++;
            }

            ChangeInventoryTab(currTab);

        }

        // Back Toggle, Close/Open the Enchantment UI
        if(Input.GetKeyUp(KeyCode.Escape)){
            if(isShown){
                LeanTween.scaleY(inventoryPanel, 0f, 0.3f);
                isShown = false;
            }else{
                LeanTween.scaleY(inventoryPanel, 1f, 0.3f);
                isShown = true;
            }
        }

#region Actions in Crafting Tab
        if(currTab == 3){
            // Pin an item
            if(Input.GetKeyUp(KeyCode.F)){
                if(selectedRecipe != null){
                    // Debug.Log(SelectableItem.currSelected.gameObject.name);

                    // Check if the item isn't pinned yet
                    if(selectedRecipe.pinIcon.gameObject.activeSelf == false){
                        // Show the Pin Icon
                        selectedRecipe.pinIcon.gameObject.SetActive(true);

                        // Add the Pinned obj to the Pinned List & Move the sibling index of the Pinned obj based on the Idx from the List of Pinned obj
                        pinList.Add(selectedRecipe.gameObject);
                        selectedRecipe.transform.SetSiblingIndex(pinList.IndexOf(selectedRecipe.gameObject));

                    }else{ 
                        // Hide the Pin Icon
                        selectedRecipe.pinIcon.gameObject.SetActive(false);

                        // Set the Unpinned obj's Sibling Idx from the List of Original Idx + Remove it from the Pinned List
                        selectedRecipe.transform.SetSiblingIndex(originalRecipeIdx.IndexOf(selectedRecipe.gameObject));
                        pinList.Remove(selectedRecipe.gameObject);

                        // If there are still Pinned Obj, Sort the Unpinned Obj
                        if(pinList.Count != 0){
                            for(int i = 0; i < originalRecipeIdx.Count; i++){
                                for(int j = 0; j < pinList.Count; j++){
                                    if(originalRecipeIdx[i] != pinList[j]){
                                        originalRecipeIdx[i].transform.SetSiblingIndex(originalRecipeIdx.IndexOf(originalRecipeIdx[i]));
                                    }
                                }
                            }
                        }

                        // Move the Pinned Obj's to the front
                        for(int k = 0; k < pinList.Count; k++){
                            pinList[k].transform.SetSiblingIndex(k);
                        }
                            
                    }

                }else{
                    Debug.Log("Currently there are no selected item");

                }
                
            }

            // Craft item(s)
            if( (Input.GetKeyUp(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1) ) && selectedRecipe != null){
                CraftItem(selectedRecipe);

            }

        }
#endregion

    }

#region Inventory Tab (To change tabs in Inventory Menu)
    void ChangeInventoryTab(int tabNum){
        // currTab already updated before this function is called

        // Disable all the inventory tabs titles
        for(int i = 0; i < inventoryTabIcons.Count; i++){
            inventoryTabIcons[i].GetComponent<Image>().color = Color.white;
            inventoryTabTitle[i].GetComponent<Text>().gameObject.SetActive(false);
            inventoryTabIconBgs[i].color = new Color(1f, 1f, 1f, 0f);

        }

        // Enable the selected one
        inventoryTabIcons[tabNum].GetComponent<Image>().color = selectedTabColor;
        inventoryTabTitle[tabNum].GetComponent<Text>().gameObject.SetActive(true);
        inventoryTabIconBgs[tabNum].color = new Color(1f, 1f, 1f, 1f);

        // Change the tab (Content)
        // Disable all tabs + set the scaleY to 0
        for(int i = 0; i < inventoryTabContent.Count; i++){
            inventoryTabContent[i].SetActive(false);
            inventoryTabContent[i].transform.localScale = new Vector3(1f, 0f, 1f);
        }

        // Enable the current tab
        inventoryTabContent[currTab].SetActive(true);
        LeanTween.scaleY(inventoryTabContent[currTab], 1f, 0.3f);

    }
    
#endregion 

#region Category Tab 
    public void ChangeCategory(int categoryNum){
        // Don't forget to assign the param accordingly after assigning this method to the Button
        currCategory = categoryNum;
        
        UpdateCategory();

    }

    void UpdateCategory(){
        // Reset all indicator's color
        for(int i = 0; i < categoryTabBgs.Count; i++){
            categoryTabTitles[i].GetComponent<Text>().gameObject.SetActive(false);
        
            // Change the color
            Color newColors = categoryTabBgs[i].color;
            newColors.a = 0.15f;
            categoryTabBgs[i].color = newColors;
        }

        // Set the current selected category color
        // Change the color
        Color newselectedColor = categoryTabBgs[currCategory].color;
        newselectedColor.a = 1;
        categoryTabBgs[currCategory].color = newselectedColor;

        categoryTabBgs[currCategory].GetComponent<Image>().color = selectedCategoryColor;
        categoryTabTitles[currCategory].GetComponent<Text>().gameObject.SetActive(true);

        // Show all items/recipes
        for(int i = 0; i < allCraftingRecipes.Length; i++){
           allCraftingRecipes[i].gameObject.SetActive(true);

        }

        // Hide all items/recipes that's different from curr Category, unless its All Category (0)
        if(currCategory != 0){
            for(int i = 0; i < allCraftingRecipes.Length; i++){
                if(!allCraftingRecipes[i].gameObject.CompareTag(CategoryIntToString(currCategory))){
                    allCraftingRecipes[i].gameObject.SetActive(false);
                }

            }
        }

    }

#endregion

    void CraftItem(CraftingData data){
        if(data == null){
            return;
        }

        if(data.craftableCount > 0 && !data.isCraftingCooldown){
            data.cooldownIndicator.value = 100;
            data.isCraftingCooldown = true;

        }

    }

    string CategoryIntToString(int categoryNum){
        if(categoryNum == 0){
            return "All";

        } else if(categoryNum == 1){
            return "Category 1";

        }else if(categoryNum == 2){
            return "Category 2";

        }else if(categoryNum == 3){
            return "Category 3";

        }else if(categoryNum == 4){
            return "Category 4";

        }else if(categoryNum == 5){
            return "Category 5";

        }else if(categoryNum == 6){
            return "Category 6";

        }

        return null;
    }

    public void CraftingDetailShowAnimation(){
        LeanTween.scale(craftingDetailParent, Vector3.one, 0.3f);
        
    }

}
