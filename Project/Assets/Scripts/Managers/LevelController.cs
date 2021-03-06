using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LevelData
{
    public GridInfo gridInfo;
    public List<GridObjInfo> gridObjects;
}

public class LevelController : MonoBehaviour
{
    public int dificulty=2;
    public LevelData currentLevelData;
    private bool alreadyGenerated = false;

    public void GenerateNewLevel()
    {
        StartCoroutine(CoGenerateNewLevel());
    }

    private IEnumerator CoGenerateNewLevel()
    {
        if (alreadyGenerated)
            ClearLevel();

        //Generate First Bread
        var indexX = Random.Range(0, GridSystem.Instance.gridSize.x);
        var indexY = Random.Range(0, GridSystem.Instance.gridSize.y);
        Vector2Int index = new Vector2Int(indexX, indexY);
        CreateIngredients(0, index);

        yield return new WaitForSeconds(.05f);
        //Generate second bread close to the first
        var slotsAvailable = GameManager.Instance.slotsWithObjectNearby;
        //var slotsAvailable = GridSystem.Instance.GetSlotsAvailableAround(index);
        var selectedIndex = Random.Range(0, slotsAvailable.Count - 1);
        CreateIngredients(0, slotsAvailable[selectedIndex].index);
        yield return new WaitForSeconds(.05f);


        for (int i = 0; i < dificulty; i++)
        {
            var slotIndex = Random.Range(0, slotsAvailable.Count - 1);
            var slot = slotsAvailable[slotIndex];
            var obj = GameManager.Instance.db.GetRandomObjectIndexAtRange(1, 99);
            LevelController.CreateIngredients(obj, slot.index);
            yield return new WaitForSeconds(.05f);
        }

        alreadyGenerated = true;
        currentLevelData = SaveLoadSystem.GetLevelData();
        GameManager.Instance.ChangeGameState(GameState.Play);
    }

    /// <summary>
    /// Get the object data from database, and spawn at
    /// the grid slot at the index
    /// </summary>
    /// <param name="objIndex"></param>
    /// <param name="index"></param>
    public static void CreateIngredients(int objIndex, Vector2Int index)
    {
        if (objIndex < 0 || objIndex > GameManager.Instance.db.objects.Count - 1) return;
        if ((index.x < 0 || index.x > GridSystem.Instance.gridSize.x - 1) ||
            (index.y < 0 || index.y > GridSystem.Instance.gridSize.y - 1)) return;

        var obj = GameManager.Instance.db.objects[objIndex];
        var gridObj = GameObject.Instantiate(GridSystem.Instance.gridObjectPrefab);
        var slot = GridSystem.Instance.GetGridSlot(index);


        gridObj.transform.position = GridSystem.Instance.GetGridSlotWorldPosition(slot);
        slot.AddToSlot(gridObj);

        var objLoader = new GameObject("loader").AddComponent<ObjectDataLoader>();
        objLoader.SetObjectData(obj);
        gridObj.AddNewObjLoader(objLoader);
        gridObj.transform.SetParent(GridSystem.Instance.objectsParents);

        if (obj.objectName != "Bread")
            GameManager.Instance.ingredients.Add(objLoader);
    }

    /// <summary>
    /// Clear all the grid slots and clear the ingredients list
    /// Can destroy the entire grid for loading a level with a 
    /// grid with different specs
    /// </summary>
    /// <param name="destroyGrid"></param>
    public static void ClearLevel(bool destroyGrid = false)
    {
        GridSystem.Instance.DestroyGrid(destroyGrid);
        GameManager.Instance.ingredients.Clear();
    }

    /// <summary>
    /// Just reload the current level data
    /// </summary>
    public void ResetLevel()
    {
        SaveLoadSystem.LoadLevel(currentLevelData);
    }

    public void SetDificulty(string dificulty)
    {
        int result;
        int.TryParse(dificulty,out result);
        this.dificulty = result;
    }
}
