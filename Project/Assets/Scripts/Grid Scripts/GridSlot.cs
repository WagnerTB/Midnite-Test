using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSlot : MonoBehaviour
{
    public Vector2Int index;
    public List<GridObject> gridObjects;

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
        if (!gridObjects.Contains(gridObject))
        {
            gridObjects.Add(gridObject);
            gridObject.SetGridSlot(this);
        }
    }

    public void RemoveFromSlot(GridObject gridObject)
    {
        if (gridObjects.Contains(gridObject))
        {

            OnSlotChanged(false, gridObject);
        }
    }
}
