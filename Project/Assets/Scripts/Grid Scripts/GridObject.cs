using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class GridObject : MonoBehaviour
{
    public GridSlot gridSlot;
    private ObjectData objectData;
    private GameVisual gameVisual;
    public BoxCollider boxCollider;

    public void SetObjectData(ObjectData objectData)
    {
        this.objectData = objectData;
        gameVisual = Instantiate(objectData.gameVisualPrefab, this.transform.position + new Vector3(0, .1f, 0), Quaternion.identity);
        gameVisual.transform.SetParent(this.transform);
        this.name = objectData.name + "(" + gridSlot.index.x + "," + gridSlot.index.y + ")";
    }

    public void SetGridSlot(GridSlot gridSlot)
    {
        this.gridSlot = gridSlot;
        boxCollider.size = Vector3.one * GridSystem.Instance.cellSize;
    }

    public void MoveToDirection(Directions direction)
    {
        GridSlot targetGridSlot = null ;
        Vector2Int index;
        //Debug.Log("Origin Grid Slot Index (" + gridSlot.index + ")");
        switch (direction)
        {

            case Directions.None:
                break;

            case Directions.Left:
                index = gridSlot.index + new Vector2Int(-1, 0);
                targetGridSlot = GridSystem.Instance.GetGridSlot(index);
                break;

            case Directions.Right:
                index = gridSlot.index + new Vector2Int(1, 0);
                targetGridSlot = GridSystem.Instance.GetGridSlot(index);
                break;

            case Directions.Up:
                index = gridSlot.index + new Vector2Int(0, 1);
                targetGridSlot = GridSystem.Instance.GetGridSlot(index);
                break;

            case Directions.Down:
                index = gridSlot.index + new Vector2Int(0, -1);
                targetGridSlot = GridSystem.Instance.GetGridSlot(index);
                break;
        }


        if (this.gridSlot != null && targetGridSlot != null)
        {
            var targetPos = GridSystem.Instance.GetGridSlotWorldPosition(targetGridSlot);
            if(direction == Directions.Left || direction == Directions.Right)
            {
                transform.DOMoveX(targetPos.x, 1);
                transform.DOMoveY(2, .5f).SetLoops(2, LoopType.Yoyo);
            }
            else
            {
                transform.DOMoveZ(targetPos.z, 1);
                transform.DOMoveY(2, .5f).SetLoops(2, LoopType.Yoyo);
            }
            SetGridSlot(targetGridSlot);
        }
        else if(this.gridSlot == null)
        {
            SetGridSlot(gridSlot);
        }
    }
}
