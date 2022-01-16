using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSlot : MonoBehaviour
{
    public Vector2Int index;
    public GridObject gridObject;


    public void Initialize(Vector2Int index)
    {
        this.index = index;
    }

    public void AddToSlot(GridObject gridObject)
    {
        this.gridObject = gridObject;
        gridObject.gridSlot = this;
    }

    public void RemoveFromSlot(GridObject gridObject)
    {
        this.gridObject = null;   
    }
}
