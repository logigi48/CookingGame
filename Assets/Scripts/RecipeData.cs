using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "recipeData", menuName = "ScriptableObjects/recipeData", order = 2)]
public class RecipeData : ScriptableObject
{
    public String id;
    public ObjectData[] objectOrder;
    public float GrabRadius;

}
