using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodDropper : MonoBehaviour, Interfaces.ButtonParent
{

    public Transform spawnPosition;
    public FoodData selectedFoodData;
    public GameObject foodPrefab;

    public void Pressed()
    {
        GameObject tmp = Instantiate(foodPrefab, spawnPosition.position, foodPrefab.transform.rotation);
        tmp.GetComponent<FoodItem>().currentData = selectedFoodData;
    }
}
