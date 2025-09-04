using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "objectData", menuName = "ScriptableObjects/objectData", order = 1)]
public class ObjectData : ScriptableObject
{
    public String id;
    public int formIndex;
    public GameObject mesh;
    public ObjectData nextObject;

}
