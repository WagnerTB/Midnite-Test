using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    public List<GridSlot> GridSlots { get { return gridSlots; } }

    private Vector2Int gridSize;
    private float cellSize;
    private Vector3 offset;
    private List<GridSlot> gridSlots = new List<GridSlot>();
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
                slot.Initialize(new Vector2Int(x,y));
                gridSlots.Add(slot);
                slot.transform.SetParent(GridSystem.Instance.slotParents);
                // debug
                
                if(UnityEngine.Random.Range(0,10) >= 9)
                {
                    var obj = GameManager.Instance.db.objects[debugIndex];
                    var gridObj = GameObject.Instantiate(GridSystem.Instance.gridObjectPrefab);
                    gridObj.transform.position = pos;
                    slot.AddToSlot(gridObj);
                    
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


