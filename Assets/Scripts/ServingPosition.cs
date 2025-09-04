using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServingPosition : MonoBehaviour
{

    public RecipeData recipe;

    public List<ObjectData> objectDataList;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Food>(out var objD))
        {
            objectDataList.Add(objD.objectData);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Food>(out var objD))
        {
            objectDataList.Remove(objD.objectData);
        }
    }

    public bool Compare()
    {
        ObjectData[] recipeArray = recipe.objectOrder;
        int tmpCounter = 0;
        List<ObjectData> tmpList = objectDataList;
        for (int i = 0; i < recipeArray.Length; i++)
        {
            int x = 0;
            while (x < tmpList.Count)
            {
                if (recipeArray[i].id == tmpList[x].id)
                {
                    tmpCounter++;
                    tmpList.Remove(tmpList[x]);
                }

                x++;
            }
        }
        if (tmpCounter >= recipeArray.Length)
            return true;
        else
            return false;
    }

}
