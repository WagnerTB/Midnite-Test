using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Object Data", menuName = "Scriptable Object/new Object Data")]
public class ObjectData : ScriptableObject
{
    public string objectName;
    public GameVisual gameVisualPrefab;
}
