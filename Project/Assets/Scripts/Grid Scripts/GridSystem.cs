using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    public static GridSystem Instance { get { return _instance; } }
    private static GridSystem _instance;


    public Vector2Int gridSize;
    public float cellSize;
    public Vector3 offset;
    public CustomGrid grid;
    public GridSlot gridSlotPrefab;
    public GridObject gridObjectPrefab;
    public Transform slotParents;
    public Transform objectsParents;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        grid = new CustomGrid(gridSize, cellSize, offset, gridSlotPrefab);
    }

    public void DestroyGrid(bool destroyGrid = false)
    {
        if (grid != null)
        {
            foreach (var slot in grid.GridSlots)
            {
                if (slot.Value.gridObject != null)
                {
                    var target = slot.Value.gridObject;
                    slot.Value.RemoveFromSlot(target);
                    Destroy(target.gameObject);
                }
            }

            if (destroyGrid)
            {
                var slotList = grid.GridSlots.ToList();

                for (int i = slotList.Count - 1; i >= 0; i--)
                {
                    var target = slotList[i].Value;
                    if (target != null)
                    {
                        Destroy(target.gameObject);
                    }
                }
            }
        }
    }

    public void GenerateGrid(Vector2Int gridSize, float cellSize, Vector3 offset)
    {
        grid = new CustomGrid(gridSize, cellSize, offset, gridSlotPrefab);

    }

    public GridSlot GetGridSlot(Vector2Int index)
    {
        if ((index.x >= 0 && index.x < gridSize.x) &&
            (index.y >= 0 && index.y < gridSize.y))
        {
            //Debug.Log("Returned Grid index (" + index + ")");
            return grid.GridSlots[index];
        }
        else
        {
            //Debug.LogError("Out of grid bounds!! (" + index + ")");
            return null;
        }

    }

    public Vector3 GetGridSlotWorldPosition(GridSlot gridSlot)
    {
        if (gridSlot == null)
            return Vector3.zero;

        Vector3 pos = new Vector3((gridSlot.index.x * cellSize) + offset.x, .2f, (gridSlot.index.y * cellSize) + offset.z);
        return pos;
    }



    public List<GridSlot> GetSlotsAvailableAround(Vector2Int index)
    {
        List<GridSlot> slotsAvailable = new List<GridSlot>();

        var rightSlot = GetGridSlot(new Vector2Int(index.x + 1, index.y));
        var leftSlot = GetGridSlot(new Vector2Int(index.x - 1, index.y));
        var upSlot = GetGridSlot(new Vector2Int(index.x, index.y + 1));
        var downSlot = GetGridSlot(new Vector2Int(index.x, index.y - 1));

        if (rightSlot != null && rightSlot.gridObject == null)
            slotsAvailable.Add(rightSlot);

        if (leftSlot != null && leftSlot.gridObject == null)
            slotsAvailable.Add(leftSlot);

        if (upSlot != null && upSlot.gridObject == null)
            slotsAvailable.Add(upSlot);

        if (downSlot != null && downSlot.gridObject == null)
            slotsAvailable.Add(downSlot);

        return slotsAvailable;
    }
}
