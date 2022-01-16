using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDataLoader : MonoBehaviour
{
    public ObjectData objectData;
    private GameObject visual;
    public void SetObjectData(ObjectData objectData)
    {
        if(visual != null)
        {
            Destroy(visual.gameObject);
        }
        this.objectData = objectData;
        visual = Instantiate(objectData.visualPrefab, this.transform.position + new Vector3(0, .1f, 0), Quaternion.identity);
        visual.transform.SetParent(this.transform);
        this.name = objectData.name;
    }
}
