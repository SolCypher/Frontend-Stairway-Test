using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class BackpackToolSlot : MonoBehaviour
{
    [Header("Tool Data")]
    public Sprite toolSprite;
    public Sprite enchantCountFilledSprite;
    public Sprite enchantCountEmptySprite;
    public string toolName;
    [Tooltip("This is for the tag, it should always be 'Tool' ")]
    public string toolType = "Tool";

    [Header("Tool Enchantments Data")]
    public int maxEnchant = 3;
    public int enchantCount;
    public List<string> enchantmentName = new List<string>();
    public List<string> enchantmentRarity = new List<string>();

    [Header("Reference")]
    public Button toolBtn;
    public Image toolIcon;
    public Image toolIconBG;
    public List<Image> toolEnchantCountIcon = new List<Image>();
    [Tooltip("The most left Icon should be the first on the list")]
    public List<Image> enchantIcons = new List<Image>();
    public EnchantToolSlot enchantToolSlot;
    public EnchantmentMenu enchantmentMenu;
    
    private void Start() {
        Initialization();

    }

    void Initialization(){
        // Get enchantToolSlot.cs & enchantmenu.cs
        if(enchantToolSlot == null){
            enchantToolSlot = FindObjectOfType<EnchantToolSlot>();
        }
        if(enchantmentMenu == null){
            enchantmentMenu = FindObjectOfType<EnchantmentMenu>();
        }

        // Set the sprite of the slot
        toolIcon.sprite = toolSprite;
        gameObject.tag = toolType;

        // Set each toolEnchantCountIcon
        SetEnchantCountIcon();

        if(enchantToolSlot == null){
            enchantToolSlot = FindObjectOfType<EnchantToolSlot>();
        }
        
        if(enchantmentMenu == null){
            enchantmentMenu = FindObjectOfType<EnchantmentMenu>();
        }

    }

    void SetEnchantCountIcon(){
        // Set all the icons to use Empty Sprite
        for(int i = 0; i < toolEnchantCountIcon.Count; i++){
            toolEnchantCountIcon[i].sprite = enchantCountEmptySprite;
        }
        
        // If there are enchantment(s), change it to the Filled Sprite based on the EnchantCount
        if(enchantCount > 0){
            for(int i = 0; i < enchantCount; i++){
                toolEnchantCountIcon[i].sprite = enchantCountFilledSprite;
            }

        }
    }

    public void AssignTool(){
        if(enchantmentMenu.currTab != 0){
            return;
        }

        if(enchantToolSlot.slotIsFilled){
            Debug.Log("Remove the tool from the slot first");
            return;

        }
        
        // Assign the tool to the Enchant Tool Slot
        enchantToolSlot.slotIsFilled = true;
        enchantToolSlot.toolAdded = this;
        enchantToolSlot.SetToolData(toolSprite);

        // Set the BG color of this tool
        toolIconBG.color = new Color(1f, 1f, 1f, 1f);

        // Disable Interactable on Btn
        toolBtn.interactable = false;

    }

}
