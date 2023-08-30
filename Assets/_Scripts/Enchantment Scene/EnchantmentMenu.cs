using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class EnchantmentMenu : MonoBehaviour
{
    public ItemData[] allItems;
    public EnchantMaterialSlot[] allMaterialSlot;
    public List<string> acceptedItemTypeList = new List<string>();
    public bool enchantable;
    public bool readyToEnchant;
    public bool isShown;

    [Header("References")]
    public GameObject enchantmentPanel;
    public GameObject confirmEnchantmentBtn;
    public Image confirmBtnBg;
    public Animator confirmEnchantAnimator;
    public Color confirmBtnReadytoEnchantColor;
    Color defaultConfirmBtnColor;

    [Header("Tabs")]
    public List<GameObject> allContentTabs = new List<GameObject>();
    public Image addTabBg;
    public Text addTabTxt;
    public Image upgradeTabBg;
    public Text upgradeTabTxt;
    public int currTab;
    public Color selectedColor;

    [Header("Enchant Tool Slot")]
    public EnchantToolSlot enchantToolSlot;

    [Header("Enchantment Slots")]
    public int maxMaterialCount = 5;
    public int addedMaterialCount;
    public List<ItemData> addedMaterialList = new List<ItemData>();
    public GameObject materialSlotParentGrid;
    public Slider enchantmentProgressionSlider;
    public Text currProgressionValue;
    public int maxMaterialValue = 100;

    [Header("Enchantment Info Slots")]
    public static EnchantmentInfoSlot selectedEnchantmentInfo;
    public Slider discardBtnSlider;
    public Image discardFill;
    public Color discardFillColor;
    public Animator discardBtnAnimator;
    public float discardSliderMaxValue;
    public float timeToDiscard = 2f;
    public bool canDiscard;
    public bool finishDiscard;


    private void Start() {
        Initialization();

    }

    private void Update() {
        GetInput();
        EnchantmentProgressionHandler();

        if(selectedEnchantmentInfo != null){
            canDiscard = true;
        }else{
            canDiscard = false;
        }

        if(readyToEnchant){
            confirmEnchantAnimator.SetBool("Ready", true);
        }else{
            confirmEnchantAnimator.SetBool("Ready", false);
            confirmEnchantmentBtn.transform.rotation = new Quaternion(0,0,0,0);
            confirmEnchantmentBtn.transform.localScale = Vector3.one;

        }

    }

    void Initialization(){
        // Get all Items in Backpack + all Material Slots
        allItems = FindObjectsOfType<ItemData>();
        allMaterialSlot = materialSlotParentGrid.GetComponentsInChildren<EnchantMaterialSlot>();

        if(enchantToolSlot == null){
            enchantToolSlot = FindObjectOfType<EnchantToolSlot>();
        }

        // Set the curr tab (Add Tab)
        currTab = 0;
        addTabBg.color = selectedColor;
        addTabTxt.color = Color.black;
        upgradeTabBg.color = new Color(1f, 1f, 1f, 60f / 255f);
        upgradeTabTxt.color = Color.white;

        // Disable tabs that aren't the current one
        for(int i = 0; i < allContentTabs.Count; i++){
            if(i != currTab){
                allContentTabs[i].SetActive(false);
                allContentTabs[i].transform.localScale = new Vector3(1f, 0f, 1f);
            }
        }

        // Disable item's that doesn't have the correct type(tag) for Enchanting
        for(int i = 0; i < allItems.Length; i++){
            for(int j = 0; j < acceptedItemTypeList.Count; j++){
                if(!allItems[i].gameObject.CompareTag(acceptedItemTypeList[j])){
                    allItems[i].itemBtn.interactable = false;
                }
            }

        }

        // Set the Progression Value
        enchantmentProgressionSlider.value = 0;

        // Set Discard Fill Color & Value + the Slider's Max Value
        discardFill.color = discardFillColor;
        discardBtnSlider.maxValue = discardSliderMaxValue;
        discardBtnSlider.value = 0;

        // Set the default Confirm Btn color
        defaultConfirmBtnColor = confirmBtnBg.color;

    }
    
    void GetInput(){
        // Change Tab to the left
        if(Input.GetKeyUp(KeyCode.A)){
            if(currTab == 0){
                currTab = 1;

            }else{
                currTab--;

            }

            ChangeTab(currTab);

        }

        // Change Tab to the right
        if(Input.GetKeyUp(KeyCode.D)){
            if(currTab == 1){
                currTab = 0;

            }else{
                currTab++;

            }

            ChangeTab(currTab);

        }
        
        // Back Toggle, Close/Open the Enchantment UI
        if(Input.GetKeyUp(KeyCode.Escape)){
            if(isShown){
                LeanTween.scaleY(enchantmentPanel, 0f, 0.3f);
                isShown = false;
            }else{
                LeanTween.scaleY(enchantmentPanel, 1f, 0.3f);
                isShown = true;
            }
        }
        
#region Actions in Add Tab
        if(currTab == 0){
            // Discard Btn
            if(Input.GetKey(KeyCode.X)){
                if(canDiscard && !finishDiscard){
                    discardBtnAnimator.SetBool("Discard", true);
                    discardBtnSlider.value += (discardBtnSlider.maxValue / timeToDiscard) * Time.deltaTime;
                    
                    if(discardBtnSlider.value >= discardBtnSlider.maxValue){
                        finishDiscard = true;
                        discardBtnSlider.value = 0;
                        discardBtnAnimator.SetBool("Discard", false);
                        Debug.Log("Item Discarded");

                    }
                }
            }
            if(Input.GetKeyUp(KeyCode.X)){
                discardBtnAnimator.SetBool("Discard", false);
                discardBtnSlider.value = 0;
                finishDiscard = false;

            }

            // Enchant Tool
            if(Input.GetKeyUp(KeyCode.Return) && enchantable && readyToEnchant){
                EnchantTool();

            }

        }
#endregion

    }

    void ChangeTab(int tabIdx){
        if(tabIdx == 0){
            // Disable Deselected Tab
            upgradeTabBg.color = new Color(1f, 1f, 1f, 60f / 255f);
            upgradeTabTxt.color = Color.white;
            
            allContentTabs[1].SetActive(false);
            allContentTabs[1].transform.localScale = new Vector3(1f, 0f, 1f);
            
            // Set Selected Tab
            addTabBg.color = selectedColor;
            addTabTxt.color = Color.black;
            
            allContentTabs[currTab].SetActive(true);
            LeanTween.scaleY(allContentTabs[currTab], 1f, 0.3f);


        }else if(tabIdx == 1){
            // Disable Deselected Tab
            addTabBg.color = new Color(1f, 1f, 1f, 60f / 255f);
            addTabTxt.color = Color.white;

            allContentTabs[0].SetActive(false);
            allContentTabs[0].transform.localScale = new Vector3(1f, 0f, 1f);

            // Set Selected Tab
            upgradeTabBg.color = selectedColor;
            upgradeTabTxt.color = Color.black;

            allContentTabs[currTab].SetActive(true);
            LeanTween.scaleY(allContentTabs[currTab], 1f, 0.3f);

        }

    }

    void EnchantmentProgressionHandler(){
        if(currTab != 0){
            return;
        }
        // If the added material is more than the max set it to 100 & if its 0 or less set it to 0
        // Also Set the Enchantable bool to true
        if(enchantmentProgressionSlider.value <= 0){
            enchantmentProgressionSlider.value = 0;

        }
        if(enchantmentProgressionSlider.value < 100){
            enchantable = false;
            readyToEnchant = false;

        }
        if(enchantmentProgressionSlider.value >= maxMaterialValue){
            enchantmentProgressionSlider.value = 100;
            enchantable = true;
            readyToEnchant = true;

        }

        currProgressionValue.text = enchantmentProgressionSlider.value.ToString();

    }

    void EnchantTool(){
        if(!enchantable){
            Debug.Log("Add more material until the value is 100!");
            return;
        }

        // Destroy the materials(?)

        // Set the Progression Value to 0 (the respective bool will be handled in EnchantmentProgressionHandler() )
        enchantmentProgressionSlider.value = 0;
        Debug.Log("Tool succesfully enchanted!");

    }

}
