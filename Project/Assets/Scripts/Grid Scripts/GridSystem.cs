using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    public static GridSystem Instance { get { return _instance; } }
    private static GridSystem _instance;


    public Vector2Int gridSize;
    public float cellSize;
    public Vector3 offset;
    public Grid grid;
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
        grid = new Grid(gridSize, cellSize, offset, gridSlotPrefab);
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

        Vector3 pos = new Vector3((gridSlot.index.x * cellSize)+ offset.x, .1f, (gridSlot.index.y * cellSize)+ offset.z);
        return pos;
    }
}
