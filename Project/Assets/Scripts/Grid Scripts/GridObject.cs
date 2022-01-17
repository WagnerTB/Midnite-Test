using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]

public struct GridObjInfo
{
    public Vector2Int index;
    public int objectIndex;
}


public class GridObject : MonoBehaviour
{
    public GridSlot gridSlot;
    [SerializeField]
    private List<ObjectDataLoader> objectDataLoaders = new List<ObjectDataLoader>();
    public List<ObjectDataLoader> ObjectDataLoaders { get { return objectDataLoaders; } }
    public BoxCollider boxCollider;

    private void Awake()
    {
        boxCollider.size = Vector3.one * GridSystem.Instance.cellSize;
    }

    public void AddNewObjLoader(ObjectDataLoader objectDataLoader)
    {
        objectDataLoader.SetGridObject(this);
        objectDataLoader.transform.position = this.transform.position;
        objectDataLoaders.Add(objectDataLoader);
    }

    public void AddNewGameVisual(List<ObjectDataLoader> objectDataLoaders)
    {
        for (int i = objectDataLoaders.Count-1; i >= 0 ; i--)
        {
            var target = objectDataLoaders[i];
            this.objectDataLoaders.Add(target);
        }
    }

    private void SetParent(List<ObjectDataLoader> objectDataLoaders)
    {
        for (int i = 0; i < objectDataLoaders.Count; i++)
        {
            var target = objectDataLoaders[i];
            target.SetGridObject(this);
        }
    }

    public void MoveToDirection(Directions direction)
    {
        if(GameManager.CurrentGameState == GameState.Play)
        {
            GameManager.Instance.ChangeGameState(GameState.Animating);
            StartCoroutine(CoMoveToDirection(direction));
        }
    }
    private IEnumerator CoMoveToDirection(Directions direction)
    {
        GridSlot targetGridSlot = null;
        Vector2Int index;

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

        if (targetGridSlot != null && targetGridSlot.gridObject != null)
        {
            var targetGridObject = targetGridSlot.gridObject;
            Debug.Log("Direction " + direction, this);

            if (this.gridSlot != null)
                gridSlot.RemoveFromSlot(this);

            boxCollider.enabled = false;

            targetGridObject.AddNewGameVisual(this.objectDataLoaders);

            float animationTime = .5f;
            MoveAnimation(targetGridSlot, direction, animationTime);
            yield return new WaitForSeconds(animationTime+.1f);
            GameManager.Instance.ChangeGameState(GameState.Play);
            targetGridObject.SetParent(objectDataLoaders);
            GameManager.Instance.CheckWin(targetGridObject);
            Destroy(this.gameObject);
        }
        else if (this.gridSlot == null)
        {
            targetGridSlot.AddToSlot(this);
        }

        if(GameManager.CurrentGameState != GameState.Play && GameManager.CurrentGameState != GameState.Win)
        GameManager.Instance.ChangeGameState(GameState.Play);

    }

    private void MoveAnimation(GridSlot targetGridSlot, Directions direction, float duration)
    {
        var targetPos = GridSystem.Instance.GetGridSlotWorldPosition(targetGridSlot);
        var rot = transform.localEulerAngles;
        float height = .3f;

        if (direction == Directions.Left || direction == Directions.Right)
        {
            transform.DOMoveX(targetPos.x, duration);
            transform.DOMoveY(2, duration / 2);
            transform.DOMoveY(targetPos.y + (targetGridSlot.gridObject.objectDataLoaders.Count * height) + height, duration / 2).SetDelay(duration / 2);
            if (direction == Directions.Left)
            {
                transform.DOLocalRotate(rot + new Vector3(0, 0, 180), duration, RotateMode.Fast).SetEase(Ease.OutQuad);
            }
            else
            {
                transform.DOLocalRotate(rot + new Vector3(0, 0, -180), duration, RotateMode.FastBeyond360);
            }
        }
        else
        {
            transform.DOMoveZ(targetPos.z, duration);
            transform.DOMoveY(2, duration / 2);
            transform.DOMoveY(targetPos.y+(targetGridSlot.gridObject.objectDataLoaders.Count * height) + height, duration / 2).SetDelay(duration / 2);

            if (direction == Directions.Up)
            {
                transform.DOLocalRotate(rot + new Vector3(180, 0, 0), duration, RotateMode.FastBeyond360);
            }
            else
            {
                transform.DOLocalRotate(rot + new Vector3(-180, 0, 0), duration, RotateMode.FastBeyond360);
            }
        }

    }
}
