using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DonationSlot : MonoBehaviour
{
    public Image itemIcon;
    public Button itemBtn;
    public Sprite emptySprite;
    public bool slotFilled;
    public DonationMenu donateMenu;
    public string donatedItemName;
    
    private void Start() {
        if(donateMenu = null){
            donateMenu = FindObjectOfType<DonationMenu>();
        }
        
        itemIcon.sprite = emptySprite;
        itemBtn.interactable = false;

    }

    public void EmptySlot(){
        // Wont be able to access if donationCount has reach 16 because the button's Interactable will be disabled
        if(donateMenu == null){
            donateMenu = FindObjectOfType<DonationMenu>();
            
        }
        if(!slotFilled){
            return;
        }

        // Reset the icon of the slot
        itemIcon.sprite = emptySprite;
        slotFilled = false; 

        // Decrease the Progression Value & Donation Count in DonationMenu.cs
        donateMenu.donationCount--;
        donateMenu.donationProgressionSlider.value--;

        // Increase itemCount in ItemData.cs
        for(int i = 0; i < donateMenu.donatedItems.Count; i++){
            if(donateMenu.donatedItems[i].itemName == donatedItemName){
                donateMenu.donatedItems[i].itemCount++;
                // Debug.Log(donateMenu.donatedItems[i].gameObject.name + " Count: " + donateMenu.donatedItems[i].itemCount);
                break;
            }

        }
        // Remove the item from the DonatedItems List in DonatinoMenu.cs
        // donateMenu.donatedItems.Remove(donatedItemName);
        for(int i = 0; i < donateMenu.donatedItems.Count; i++){
            if(donateMenu.donatedItems[i].itemName == donatedItemName){
                donateMenu.donatedItems.RemoveAt(i);
                break;
            }

        }

    }

}
