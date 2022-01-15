using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    public Dictionary<Vector2Int, GridSlot> GridSlots { get { return gridSlots; } }

    private Vector2Int gridSize;
    private float cellSize;
    private Vector3 offset;
    private Dictionary<Vector2Int,GridSlot> gridSlots = new Dictionary<Vector2Int,GridSlot>();
    private GridSlot gridSlotPrefab;

    public Grid(Vector2Int gridSize,float cellSize,Vector3 offset,GridSlot gridSlotPrefab)
    {
        this.gridSize = gridSize;
        this.cellSize = cellSize;
        this.offset = offset;
        this.gridSlotPrefab = gridSlotPrefab;

        GenerateGridSlots();
    }

    private void GenerateGridSlots()
    {
        int debugIndex = 0;

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector3 pos = new Vector3((x * cellSize) + offset.x, offset.y, (y*cellSize) + offset.z);
                var slot = GameObject.Instantiate(gridSlotPrefab, pos, Quaternion.identity);
                var index = new Vector2Int(x, y);
                slot.Initialize(index);

                if(!gridSlots.ContainsKey(index))
                    gridSlots.Add(index,slot);

                slot.transform.SetParent(GridSystem.Instance.slotParents);
                slot.name = "Slot [" + index + "]";
                // debug
                
                if(UnityEngine.Random.Range(0,10) >= 9)
                {
                    var obj = GameManager.Instance.db.objects[debugIndex];
                    var gridObj = GameObject.Instantiate(GridSystem.Instance.gridObjectPrefab);
                    gridObj.transform.position = pos;
                    slot.AddToSlot(gridObj);

                    gridObj.SetGridSlot(slot);
                    gridObj.SetObjectData(obj);
                    gridObj.transform.SetParent(GridSystem.Instance.objectsParents);

                    debugIndex++;
                    if (debugIndex > GameManager.Instance.db.objects.Count - 1) ;
                    {
                        debugIndex = 0;
                    }
                }
                
            }
        }
    }
}


