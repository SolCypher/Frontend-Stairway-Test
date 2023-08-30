using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnchantMaterialSlot : MonoBehaviour
{
    public Image materialIcon;
    public Button materialBtn;
    public bool materialSlotFilled;
    public EnchantmentMenu enchantMenu;
    public string addedMaterialName;
    
    private void Start() {
        materialIcon.gameObject.SetActive(false);
        materialBtn.interactable = false;

    }

    public void RemoveMaterial(){
        if(enchantMenu == null){
            enchantMenu = FindObjectOfType<EnchantmentMenu>();
        }

        // Disable the Btn interactable
        materialBtn.interactable = false;

        // Reset the Slot's Icon
        if(materialSlotFilled){
            materialIcon.gameObject.SetActive(false);
            materialSlotFilled = false;

        }

        // Added Material Count in EnchantMenu.cs
        enchantMenu.addedMaterialCount--;

        // Increase the itemCount in ItemData.cs + Decrease the Progression Value
        for(int i = 0; i < enchantMenu.addedMaterialList.Count; i++){
            if(enchantMenu.addedMaterialList[i].itemName == addedMaterialName){
                enchantMenu.addedMaterialList[i].itemCount++;
                enchantMenu.enchantmentProgressionSlider.value -= enchantMenu.addedMaterialList[i].materialValue;
                break;
            }

        }

        // Remove the item from addedMaterialList in EnchantMenu.cs
        for(int i = 0; i < enchantMenu.addedMaterialList.Count; i++){
            if(enchantMenu.addedMaterialList[i].itemName == addedMaterialName){
                enchantMenu.addedMaterialList.RemoveAt(i);
                break;
            }

        }
    }

}
