using System.Collections.Generic;
using UnityEngine;

// This defines the types of actions appliances can perform
public enum FoodAction { None, Slice, Grill, Fry, Boil }

[System.Serializable]
public class FoodRecipe
{
    public FoodAction actionRequired;
    public float requiredProgress; // E.g., 3 for "3 knife hits", or 10f for "10 seconds of grilling"
    public FoodData nextStage;     // What this food turns into when progress is reached
}

[CreateAssetMenu(fileName = "New Food Data", menuName = "Cooking/Food Data")]
public class FoodData : ScriptableObject
{
    public string foodName;
    public GameObject meshPrefab;
    public Material IconMaterial;

    // This list holds all the ways this SPECIFIC food can be cooked or prepared!
    public List<FoodRecipe> possibleTransitions = new List<FoodRecipe>();

    // A helper method to quickly check if a specific action is allowed on this food
    public FoodRecipe GetRecipeForAction(FoodAction action)
    {
        foreach (var recipe in possibleTransitions)
        {
            if (recipe.actionRequired == action)
                return recipe;
        }
        return null; // Returns null if this action isn't in the recipe list
    }
}