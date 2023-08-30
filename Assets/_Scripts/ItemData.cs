using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemData : MonoBehaviour
{
    [Header("Item's Data")]
    public Sprite itemSprite;
    public int itemCount;
    public string itemName;
    public string itemCategory;
    [Tooltip("Only for item with 'Material' Tag (Used in Enchantment)")]
    public int materialValue;
    
    [Header("References")]
    public Image itemIcon;
    public GameObject itemCountBox;
    public Text itemCountText;
    public DonationMenu donateMenu;
    public Button itemBtn;
    public EnchantmentMenu enchantMenu;

    void Start()
    {
        // Check Item Count
        if(itemCount < 2){
            itemCountBox.SetActive(false);

        }

        // Check Item Sprite
        if(itemSprite == null){
            itemIcon.gameObject.SetActive(false);
            
        }

        Initialization();

    }

    private void Update() {
        UpdateUI();

    }

    void Initialization(){
        // Set the item's data 
        itemCountText.text = itemCount.ToString();
        itemIcon.sprite = itemSprite;

        if(itemCategory != null && itemCategory != ""){
            gameObject.tag = itemCategory;
        }
        

    }

    void UpdateUI(){
        itemCountText.text = itemCount.ToString();

        if(itemCount <= 0){
            // No Item
            itemIcon.color = new Color(1f, 1f, 1f, 60f / 255f);
            itemBtn.interactable = false;

        }else{
            itemIcon.color = new Color(1f, 1f, 1f, 1f);
            itemBtn.interactable = true;

        }

    }

#region Donate Menu
    public void DonateItem(){
        if(donateMenu == null){
            Debug.Log("donateMenu hasn't been assigned");
            return;
        }

        if(donateMenu.donationCount == 16){
            Debug.Log("Not accepting any more donation!");
            return;
        }

        // Add the Donated Item Name to the DonationMenu.cs DonatedItems List
        donateMenu.donatedItems.Add(this);

        // Set the Donation Slot's Sprite to be the Sprite of this Item + set the name of the DonatedItemName in DonationSlots.cs
        // Also set the Btn interactable of the slot to true
        for(int i = 0; i < donateMenu.allDonationSlots.Length; i++){
            if(!donateMenu.allDonationSlots[i].slotFilled){
                donateMenu.allDonationSlots[i].itemIcon.sprite = itemSprite;
                donateMenu.allDonationSlots[i].donatedItemName = donateMenu.donatedItems[donateMenu.donatedItems.Count-1].itemName;
                donateMenu.allDonationSlots[i].slotFilled = true;
                donateMenu.allDonationSlots[i].itemBtn.interactable = true;
                // Debug.Log("Slot Name: " + donateMenu.allDonationSlots[i].gameObject.name);
                break;

            }

        }
        
        // Increase the Progression Value & Count of donated Item in DonationMenu.cs
        donateMenu.donationCount++;
        if(donateMenu.donationCount == 16){
            for(int i = 0; i < donateMenu.allDonationSlots.Length; i++){
                donateMenu.allDonationSlots[i].itemBtn.interactable = false;

            }
        }
        donateMenu.donationProgressionSlider.value++;

        // Decrease itemCount
        itemCount--;

        // Show Donation Notification + animation Fade Out after 1-2s (?) animation duration 2s?
        ShowDonationNotification();

    }

    void ShowDonationNotification(){
        // Show Notification
        donateMenu.donationNotificationParent.gameObject.SetActive(true);

        // Set the Donated Item's Data to Notification
        donateMenu.SetDonationNotification(itemSprite, itemName, itemCategory);
        
        // Play Fade Out Animation automatically after calling SetDonationNotification()

    }

#endregion

#region Enchantment Menu
    public void AssignMaterial(){
        if(enchantMenu.currTab != 0){
            return;
        }
        if(enchantMenu == null){
            Debug.Log("EnchantMenu.cs hasn't been assigned");
            return;
        }

        if(enchantMenu.addedMaterialCount == enchantMenu.maxMaterialCount){
            Debug.Log("Material has reached it's Max Amount");
            return;
        }

        if(!enchantMenu.enchantToolSlot.slotIsFilled){
            Debug.Log("Add a tool that want to be enchanted first!");
            return;
        }

        // Add the Material into the List
        enchantMenu.addedMaterialList.Add(this);

        // Set the Material Slot's added material Name + this item's sprite + Set the slot's Button.interactable to true + the Material Icons set to active
        for(int i = 0; i < enchantMenu.allMaterialSlot.Length; i++){
            if(!enchantMenu.allMaterialSlot[i].materialSlotFilled){
                enchantMenu.allMaterialSlot[i].materialIcon.sprite = itemSprite;
                enchantMenu.allMaterialSlot[i].addedMaterialName = itemName;
                enchantMenu.allMaterialSlot[i].materialSlotFilled = true;
                enchantMenu.allMaterialSlot[i].materialBtn.interactable = true;
                enchantMenu.allMaterialSlot[i].materialIcon.gameObject.SetActive(true);
                
                break;

            }
            
        }

        // Increase the Added Material Count & the material Progression Value
        enchantMenu.addedMaterialCount++;
        enchantMenu.enchantmentProgressionSlider.value += materialValue;

        // Enable the Slot's Icon
        enchantMenu.allMaterialSlot[enchantMenu.addedMaterialCount-1].materialIcon.gameObject.SetActive(true);

        // Decrease this item's Count
        itemCount--;

    }
#endregion
}
