using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingDetail : MonoBehaviour
{
    [Header("References")]
    public Image recipeDetailIcon;
    public Text recipeDetailName;
    public Text recipeDetailCategoryName;
    public Text recipeDetailesc;
    
    [HideInInspector]
    public List<GameObject> ingredientsList = new List<GameObject>();

}
