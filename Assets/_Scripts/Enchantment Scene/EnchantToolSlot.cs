using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnchantToolSlot : MonoBehaviour
{
    [Header("Slot Data")]
    public bool slotIsFilled;

    [Header("References")]
    public Image slotIcon;
    public Sprite emptySprite;
    public BackpackToolSlot toolAdded;
    public Button removeBtn;
    public GameObject enchantmentInfo;
    
    private void Start() {
        Initialization();

        enchantmentInfo.transform.localScale = new Vector3(1f, 0f, 1f);

    }

    private void Update() {
        // Handles the Interactable of the Remove Btn
        if(slotIsFilled){
            removeBtn.gameObject.SetActive(true);
            LeanTween.cancel(enchantmentInfo);
            // LeanTween.scaleY(enchantmentInfo, 1f, 0.15f);
            LeanTween.scale(enchantmentInfo, Vector3.one, 0.15f);

        }else{
            removeBtn.gameObject.SetActive(false);
            LeanTween.cancel(enchantmentInfo);
            // LeanTween.scaleY(enchantmentInfo, 0f, 0.075f);
            LeanTween.scale(enchantmentInfo, Vector3.zero, 0.15f);

        }

    }

    void Initialization(){
        slotIcon.sprite = emptySprite;
        slotIsFilled = false;
        removeBtn.gameObject.SetActive(false);

    }

    public void SetToolData(Sprite toolSprite){
        slotIcon.sprite = toolSprite;

    }

    public void RemoveTool(){
        // Set the Sprite to empty Sprite
        slotIcon.sprite = emptySprite;

        // Set the data to empty
        slotIsFilled = false;

        // Reset the BG color of the Tool in Backpack
        toolAdded.toolIconBG.color = new Color(1f, 1f, 1f, 150f / 255f);

        // Enable Interactable on Btn + reset the toolAdded
        toolAdded.toolBtn.interactable = true;
        toolAdded = null;

    }

}
