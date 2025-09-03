using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Food : MonoBehaviour
{

    public ObjectData objectData;

    void Start()
    {
        //instantiate the visual mesh accordingly
        GameObject.Instantiate(objectData.mesh, Vector3.zero, Quaternion.identity);
    }
}
