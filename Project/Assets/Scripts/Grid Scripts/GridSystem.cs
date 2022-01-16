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

        GameManager.Instance.CreateIngredients(0, new Vector2Int(1, 1));
        GameManager.Instance.CreateIngredients(0, new Vector2Int(2, 1));
        GameManager.Instance.CreateIngredients(1, new Vector2Int(1, 2));
        GameManager.Instance.CreateIngredients(2, new Vector2Int(2, 2));
        GameManager.Instance.CreateIngredients(2, new Vector2Int(1, 0));
        GameManager.Instance.CreateIngredients(1, new Vector2Int(2, 0));
        SaveLoadSystem.SaveLevel();
    }

    public void DestroyGrid()
    {
        if(grid != null)
        {
            foreach (var slot in grid.GridSlots)
            {
                if(slot.Value.gridObject != null)
                {
                    Destroy(slot.Value.gridObject.gameObject);
                }
            }

            var slotList = grid.GridSlots.ToList();

            for (int i = slotList.Count-1; i >= 0; i--)
            {
                var target = slotList[i].Value;
                if(target != null)
                {
                    Destroy(target.gameObject);
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

        Vector3 pos = new Vector3((gridSlot.index.x * cellSize)+ offset.x, .2f, (gridSlot.index.y * cellSize)+ offset.z);
        return pos;
    }
}
