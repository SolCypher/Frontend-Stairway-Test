using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnchantmentInfoSlot : MonoBehaviour
{
    public Image outline;
    public Color selectedColor;
    public Color deselectedColor;

    public void ChangeOutlineSelected(){
        outline.color = selectedColor;
        EnchantmentMenu.selectedEnchantmentInfo = this;

    }

    public void ChangeOutlineDeselected(){
        outline.color = deselectedColor;
        EnchantmentMenu.selectedEnchantmentInfo = null;

    }

}
