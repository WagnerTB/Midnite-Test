using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="new Object Databse",menuName ="Scriptable Object/New Object Database")]
public class ObjectsDatabase : ScriptableObject
{
    public List<ObjectData> objects;
}
