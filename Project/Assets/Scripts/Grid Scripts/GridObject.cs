using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    public GridSlot gridSlot;
    private ObjectData objectData;
    private GameVisual gameVisual;

    public void SetObjectData(ObjectData objectData)
    {
        this.objectData = objectData;
        gameVisual = Instantiate(objectData.gameVisualPrefab, this.transform.position + new Vector3(0,.1f,0), Quaternion.identity);
        gameVisual.transform.SetParent(this.transform);
        this.name = objectData.name + "(" + gridSlot.index.x + "," + gridSlot.index.y + ")";
    }

    public void SetGridSlot(GridSlot gridSlot)
    {
        this.gridSlot = gridSlot;
    }
}
