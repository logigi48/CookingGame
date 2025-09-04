using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{

    public RecipeData recipeData;
    ObjectData[] objectDatas;
    public ServingPosition servingPosition;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //if (CompareArrays(recipeData.objectOrder, objectDatas))
            //   Debug.Log("Aynı");
            Debug.Log(servingPosition.Compare());
        }
    }

    /**
    // Dinamik ekleme
    ObjectData[] AddElement(ObjectData[] array, ObjectData element)
    {
        if (array == null)
        {
            array = new ObjectData[1];
            array[0] = element;
            return array;
        }

        // Array'i 1 eleman büyüt
        ObjectData[] newArray = new ObjectData[array.Length + 1];

        // Eski elemanları kopyala
        for (int i = 0; i < array.Length; i++)
        {
            newArray[i] = array[i];
        }

        // Yeni elemanı sona ekle
        newArray[array.Length] = element;

        return newArray;
    }

    // Eleman çıkarma
    ObjectData[] RemoveElement(ObjectData[] array, int index)
    {
        if (array == null || array.Length == 0 || index < 0 || index >= array.Length)
            return array;

        ObjectData[] newArray = new ObjectData[array.Length - 1];

        int j = 0;
        for (int i = 0; i < array.Length; i++)
        {
            if (i == index) continue;
            newArray[j] = array[i];
            j++;
        }

        return newArray;
    }

    // İki array karşılaştırma
    bool CompareArrays(ObjectData[] a, ObjectData[] b)
    {
        if (a == null || b == null) return false;
        if (a.Length != b.Length) return false;

        for (int i = 0; i < a.Length; i++)
        {
            if (!a[i].Equals(b[i]))
                return false;
        }

        return true;
    }

    ObjectData[] ClearArray()
    {
        return new ObjectData[0];
    }
    **/
}
