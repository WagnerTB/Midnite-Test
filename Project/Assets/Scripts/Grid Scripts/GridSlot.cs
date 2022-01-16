using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSlot : MonoBehaviour
{
    public Vector2Int index;
    public GridObject gridObject;

    /// <summary>
    /// true if some object enters in the slot
    /// Grid Object that enters or leave the slot
    /// </summary>
    public System.Action<bool, GridObject> OnSlotChanged;

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
