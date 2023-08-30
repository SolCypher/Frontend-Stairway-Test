using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingData : MonoBehaviour
{
    [Header("Crafting Data")]
    [Tooltip("Set to true to make it Interactable")]
    public bool isValid; // Mostly for detecting if the Recipe Slot is not empty and useable for pin, clicking, etc
    public string recipeName;
    [Tooltip("Don't forget to add the tag in Unity's Tag List")]
    public string categoryName;
    public string recipeDescription;
    public int craftableCount;
    public float craftingCooldown = 0.75f;
    public bool isCraftingCooldown = false;

    [Header("Crafting Details UI")]
    public CraftingDetail craftingDetail;
 
    [Header("Ingredient's Data")]
    public Transform ingredientUIGridParent;
    public GameObject ingredientUIPrefab;
    
    public int ingredientCount;
    public List<string> ingredientNames = new List<string>();
    public List<Sprite> ingredientSpriteIcons = new List<Sprite>();
    public List<int> ingredientMaxNeeded = new List<int>();
    public List<int> ingredientAvailable = new List<int>();

    [Header("Recipe's Sprites")]
    public Sprite recipeSprite;
    public Sprite pinSprite;
    public Sprite craftableSprite;
    public Sprite emptyRecipe;

    [Header("References")]
    public Image recipeInnerSlot;
    public Image recipeOutline;
    public Image recipeIcon;
    public Image pinIcon;
    public Image craftableIcon;
    public Image craftableIconBg;
    public Slider cooldownIndicator;
    public Text recipeNameText;
    public GameObject amountPopup;
    public Inventory inventory;

    private void OnValidate() {
        while (ingredientNames.Count < ingredientCount)
        {
            ingredientNames.Add("Ingredient " + (ingredientNames.Count + 1) + " Name");
            ingredientSpriteIcons.Add(null);
            ingredientMaxNeeded.Add(0);
            ingredientAvailable.Add(0);
        }
        while (ingredientNames.Count > ingredientCount)
        {
            if(ingredientCount == 0){
                ingredientNames.Clear();
                ingredientSpriteIcons.Clear();
                ingredientMaxNeeded.Clear();
                ingredientAvailable.Clear();
            }else{
                ingredientNames.RemoveAt(ingredientNames.Count - 1);
                ingredientSpriteIcons.RemoveAt(ingredientSpriteIcons.Count - 1);
                ingredientMaxNeeded.RemoveAt(ingredientMaxNeeded.Count - 1);
                ingredientAvailable.RemoveAt(ingredientAvailable.Count - 1);

            }
        }

        if(recipeDescription.Length > 150){
            // recipeDescription.Substring(0, 149);
            Debug.LogError("Description can only be 150 long");
        }

    }

    void Start()
    {
        Initialization();

        // Set if it's valid or not
        // [insert code here]

        if(!isValid){
            recipeInnerSlot.color = new Color(1f, 1f, 1f, 45f / 255f);
            recipeIcon.sprite = emptyRecipe;
            recipeIcon.color = new Color(1f, 1f, 1f, 75f / 255f); 
            craftableIconBg.gameObject.SetActive(false);

            return;

        }


    }

    private void Update() {
        if(!isValid){
            return;       
        }
        
        // Handle Craftable Icon UI
        if(craftableCount <= 0){
            craftableIconBg.gameObject.SetActive(false);
            recipeInnerSlot.color = new Color(1f, 1f, 1f, 100f / 255f);

        }else{
            craftableIconBg.gameObject.SetActive(true);
            recipeInnerSlot.color = new Color(1f, 1f, 1f, 200f / 255f);

        }

        CraftCooldownIndicatorHandler();

    }

    void Initialization(){
        // Set inventory
        if(inventory == null){
            inventory = FindObjectOfType<Inventory>();
            
        }

        // Assign the tag (For Category)
        gameObject.tag = categoryName;

        // Init popup
        amountPopup.gameObject.transform.localScale = Vector3.zero;

        // Init all icons + name
        recipeOutline.gameObject.SetActive(false);
        pinIcon.sprite = pinSprite;
        craftableIcon.sprite = craftableSprite;
        recipeIcon.sprite = recipeSprite;
        recipeNameText.text = recipeName;

        // Disable pin icons & check if craftable
        pinIcon.gameObject.SetActive(false);

        // Insert condition to check if recipe is craftable
        if(craftableCount > 0){
            craftableIcon.gameObject.SetActive(true);
            
        }else{
            craftableIcon.gameObject.SetActive(false);

        }

        // Set the Cooldown Indicator value
        cooldownIndicator.value = 0;

    }

    public void ScaleInPopup(){
        if(!isValid){
            return;       
        }

        LeanTween.scale(amountPopup.GetComponent<RectTransform>(), Vector3.one, 0.2f);

    }
    public void ScaleOutPopup(){
        if(!isValid){
            return;       
        }
        
        LeanTween.scale(amountPopup.GetComponent<RectTransform>(), Vector3.zero, 0.2f);

    }

    public void ShowOutline(){
        if(!isValid){
            return;       
        }
        
        // Show the outline
        recipeOutline.gameObject.SetActive(true);
        Inventory.selectedRecipe = this;

    }
    public void HideOutline(){
        if(!isValid){
            return;       
        }
        
        // Hide the outline
        recipeOutline.gameObject.SetActive(false);
        Inventory.selectedRecipe = null;
        
    }

    public void ChangeDetails(){
        if(!isValid){
            return;       
        }
        
        // // Turn on the details if its off
        // if(craftingDetail.gameObject.activeSelf == false){
        //     craftingDetail.gameObject.SetActive(true);
        // }

        // Set details
        craftingDetail.recipeDetailIcon.sprite = recipeSprite;
        craftingDetail.recipeDetailName.text = recipeName;
        craftingDetail.recipeDetailCategoryName.text = categoryName;
        craftingDetail.recipeDetailesc.text = recipeDescription;

        // If IngredientsList not empty
        if(craftingDetail.ingredientsList.Count > 0){
            // Reset the Ingredient Lists (because we're changing details)
            for(int i = 0; i < craftingDetail.ingredientsList.Count; i++){
                Destroy(craftingDetail.ingredientsList[i].gameObject);
            }

            craftingDetail.ingredientsList.Clear();
        }
        

        // Set the Ingredient's Infos
        for(int i = 0; i < ingredientCount; i++){
            craftingDetail.ingredientsList.Add(Instantiate(ingredientUIPrefab, ingredientUIGridParent));
            craftingDetail.ingredientsList[i].GetComponent<IngredientInfo>().ingredientName.text = ingredientNames[i];
            craftingDetail.ingredientsList[i].GetComponent<IngredientInfo>().ingredientIcon.sprite = ingredientSpriteIcons[i];
            craftingDetail.ingredientsList[i].GetComponent<IngredientInfo>().ingredientMaxNeededAmount.text = "/ "+ ingredientMaxNeeded[i].ToString();

            // Set color according to the Available amount
            if(ingredientMaxNeeded[i] > ingredientAvailable[i]){
                craftingDetail.ingredientsList[i].GetComponent<IngredientInfo>().ingredientAvailableAmount.color = Color.red;
            }else{
                craftingDetail.ingredientsList[i].GetComponent<IngredientInfo>().ingredientAvailableAmount.color = Color.black;
            }

            craftingDetail.ingredientsList[i].GetComponent<IngredientInfo>().ingredientAvailableAmount.text = ingredientAvailable[i].ToString();

        }

        // Show Detail
        craftingDetail.gameObject.SetActive(true);
        inventory.CraftingDetailShowAnimation();

    }

    void CraftCooldownIndicatorHandler(){
        if(isCraftingCooldown){
            cooldownIndicator.value -= (cooldownIndicator.maxValue / craftingCooldown) * Time.deltaTime;

            if(cooldownIndicator.value <= 0){
                craftableCount--;
                Debug.Log("Item Crafted");
                isCraftingCooldown = false;

            }

        }

    }


}
