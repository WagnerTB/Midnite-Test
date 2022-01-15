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

}
