using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSlot : MonoBehaviour
{
    public Vector2Int index;
    public GridObject gridObject;
    public delegate void GridSlotEvent(GridSlot gridslot);
    public static GridSlotEvent OnUpdateNearby;
    public static GridSlotEvent OnSlotTurnInvalid;

    public void Initialize(Vector2Int index)
    {
        this.index = index;
    }

    /// <summary>
    /// Add a grid object to slot
    /// </summary>
    /// <param name="gridObject"></param>
    public void AddToSlot(GridObject gridObject)
    {
        this.gridObject = gridObject;
        gridObject.gridSlot = this;
        OnSlotTurnInvalid?.Invoke(this);
        UpdateSlotsAround();
    }

    /// <summary>
    /// Update slots in the left,right,up and dow to register as available to 
    /// spawn a igredient there
    /// </summary>
    private void UpdateSlotsAround()
    {
        var rightSlot = GridSystem.Instance.GetGridSlot(new Vector2Int(index.x + 1, index.y));
        var leftSlot = GridSystem.Instance.GetGridSlot(new Vector2Int(index.x - 1, index.y));
        var upSlot = GridSystem.Instance.GetGridSlot(new Vector2Int(index.x, index.y + 1));
        var downSlot = GridSystem.Instance.GetGridSlot(new Vector2Int(index.x, index.y - 1));

        if (rightSlot != null && rightSlot.gridObject == null)
            OnUpdateNearby?.Invoke(rightSlot);

        if (leftSlot != null && leftSlot.gridObject == null)
            OnUpdateNearby?.Invoke(leftSlot);


        if (upSlot != null && upSlot.gridObject == null)
            OnUpdateNearby?.Invoke(upSlot);


        if (downSlot != null && downSlot.gridObject == null)
            OnUpdateNearby?.Invoke(downSlot);

    }

    /// <summary>
    /// Remove grid object from slots available to spawn ingredients
    /// </summary>
    /// <param name="gridObject"></param>
    public void RemoveFromSlot(GridObject gridObject)
    {
        this.gridObject = null;

        var rightSlot = GridSystem.Instance.GetGridSlot(new Vector2Int(index.x + 1, index.y));
        var leftSlot = GridSystem.Instance.GetGridSlot(new Vector2Int(index.x - 1, index.y));
        var upSlot = GridSystem.Instance.GetGridSlot(new Vector2Int(index.x, index.y + 1));
        var downSlot = GridSystem.Instance.GetGridSlot(new Vector2Int(index.x, index.y - 1));

        if (rightSlot != null)
            OnSlotTurnInvalid?.Invoke(rightSlot);

        if (leftSlot != null)
            OnSlotTurnInvalid?.Invoke(leftSlot);

        if (upSlot != null)
            OnSlotTurnInvalid?.Invoke(upSlot);


        if (downSlot != null)
            OnSlotTurnInvalid?.Invoke(downSlot);

    }
}
