using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="new Object Databse",menuName ="Scriptable Object/New Object Database")]
public class ObjectsDatabase : ScriptableObject
{
    public List<ObjectData> objects;

    public int GetRandomObjectIndexAtRange(int min, int max)
    {
        if (max > objects.Count)
            max = objects.Count;

        var rnd = Random.Range(min, max);
        return rnd;
    }
}
