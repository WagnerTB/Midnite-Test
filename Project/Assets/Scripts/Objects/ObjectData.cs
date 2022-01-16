using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Object Data", menuName = "Scriptable Object/new Object Data")]
[System.Serializable]
public class ObjectData : ScriptableObject
{
    public string objectName;
    public GameObject visualPrefab;
}
