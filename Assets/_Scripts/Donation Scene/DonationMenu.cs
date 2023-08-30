using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DonationMenu : MonoBehaviour
{
    public int donationItemNeeded = 16;
    public DonationSlot[] allDonationSlots;
    public ItemData[] allItems;
    public List<ItemData> donatedItems = new List<ItemData>();
    public int donationCount;
    public GameObject donationPanel;
    public bool isShown;

    [Header("Donation Tab")]
    public Slider donationProgressionSlider;
    public GameObject donationSlotParentGrid;
    public List<string> acceptedItemTypeList = new List<string>();

    [Header("Donation Notification")]
    public Transform donationNotificationParent;
    public Image notificationIcon;
    public Text notifItemName;
    public Text notifItemType;

    [Header("Reward Tab")]
    public GameObject rewardTab;
    public Animator rewardTabAnimator;
    public bool rewardClaimed;
    public bool rewardReady;

    private void Start() {
        Initialization();

    }

    private void Update() {
        // Turn off Interactable if donation has reached the Max Amount
        if(donationCount == 16){
            for(int i = 0; i < allDonationSlots.Length; i++){
                allDonationSlots[i].itemBtn.interactable = false;

            }

            // Show Reward Tab
            rewardTab.SetActive(true);
            rewardReady = true;

        }

        // Animation for Reward Tab
        if(rewardTab != null && !rewardClaimed && rewardReady){
            rewardTabAnimator.SetBool("playAnimation", true);

        }

        // Turn off Reward Tab animation
        if(rewardClaimed){
            rewardTabAnimator.SetBool("playAnimation", false);
        }

        GetInput();

    }

    void Initialization(){
        // Get all Donation Slot + Items in backpack
        allDonationSlots = donationSlotParentGrid.GetComponentsInChildren<DonationSlot>();
        allItems = FindObjectsOfType<ItemData>();

        // Init the Progression Slider
        donationProgressionSlider.maxValue = donationItemNeeded;
        donationProgressionSlider.value = 0;

        // Hide Reward Tab
        rewardTab.SetActive(false);

        // Disable item's that doesn't have the correct type(tag) for donation
        for(int i = 0; i < allItems.Length; i++){
            for(int j = 0; j < acceptedItemTypeList.Count; j++){
                if(!allItems[i].gameObject.CompareTag(acceptedItemTypeList[j])){
                    allItems[i].itemBtn.interactable = false;
                }
            }

        }
        
        // Hide Donation Notification
        donationNotificationParent.gameObject.SetActive(false);

    }

    void GetInput(){
        // Claim Reward
        if(Input.GetKeyUp(KeyCode.E)){
            if(rewardReady && !rewardClaimed){
                ClaimReward();
            }
        }

        // Toggle Back, Close/Open Donation Panel
        if(Input.GetKeyUp(KeyCode.Escape)){
            if(isShown){
                LeanTween.scaleY(donationPanel, 0f, 0.3f);
                isShown = false;
            }else{
                LeanTween.scaleY(donationPanel, 1f, 0.3f);
                isShown = true;
            }
        }

    }

    public void ClaimReward(){
        if(!rewardReady || rewardClaimed){
            return;
        }

        rewardClaimed = true;
        rewardReady = false;
        Debug.Log("Reward Claimed!");

    }

    public void SetDonationNotification(Sprite itemSprite, string itemName, string itemType){
        notificationIcon.sprite = itemSprite;
        notifItemName.text = itemName;
        notifItemType.text = itemType;

        // Play Animation Fade Out
        FadeOutAnimation(donationNotificationParent.gameObject);
        
    }

    void FadeOutAnimation(GameObject obj){
        LeanTween.cancel(obj);
        obj.GetComponent<CanvasGroup>().alpha = 1f;
        LeanTween.alphaCanvas(obj.GetComponent<CanvasGroup>(), 0f, 2f);

    }

}
