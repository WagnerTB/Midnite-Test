using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public struct GridInfo
{
    public Vector2Int gridSize;
    public float cellSize;
    public Vector3 offset;
}

public class CustomGrid
{
    public Dictionary<Vector2Int, GridSlot> GridSlots { get { return gridSlots; } }

    public Vector2Int gridSize { get; private set; }
    public float cellSize { get; private set; }
    public Vector3 offset { get; private set; }

    private Dictionary<Vector2Int, GridSlot> gridSlots = new Dictionary<Vector2Int, GridSlot>();
    private GridSlot gridSlotPrefab;

    public CustomGrid(Vector2Int gridSize, float cellSize, Vector3 offset, GridSlot gridSlotPrefab)
    {
        this.gridSize = gridSize;
        this.cellSize = cellSize;
        this.offset = offset;
        this.gridSlotPrefab = gridSlotPrefab;

        GenerateGridSlots();
    }

    private void GenerateGridSlots()
    {

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector3 pos = new Vector3((x * cellSize) + offset.x, offset.y, (y * cellSize) + offset.z);
                var slot = GameObject.Instantiate(gridSlotPrefab, pos, Quaternion.identity);
                var index = new Vector2Int(x, y);
                slot.Initialize(index);

                if (!gridSlots.ContainsKey(index))
                    gridSlots.Add(index, slot);

                slot.transform.SetParent(GridSystem.Instance.slotParents);
                slot.name = "Slot [" + index + "]";

                // debug



            }
        }
    }


}


