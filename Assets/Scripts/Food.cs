using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Food : MonoBehaviour
{

    public ObjectData objectData;

    GameObject currentObj;

    void Start()
    {
        //instantiate the visual mesh accordingly
        currentObj = GameObject.Instantiate(objectData.mesh, Vector3.zero, Quaternion.identity,this.transform);
    }


    void Update()
    {
        //test
        if (Input.GetMouseButtonDown(0))
        {
            ChangeObject();
        }

    }

    public void ChangeObject()
    {
        
        Debug.Log("It was " + objectData.id);
        if (objectData.nextObject == null)
            return;

        Destroy(currentObj);
        objectData = objectData.nextObject;
        currentObj = GameObject.Instantiate(objectData.mesh, Vector3.zero, Quaternion.identity,this.transform);
        Debug.Log("Now its " + objectData.id);
    }

}
