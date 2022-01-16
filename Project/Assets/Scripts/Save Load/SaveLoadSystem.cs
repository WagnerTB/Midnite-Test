using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;


[System.Serializable]
public struct Save
{
    public GridInfo gridInfo;
    public List<GridObjInfo> gridObjects;
}

public static class SaveLoadSystem 
{
    public static string folder = "/Save/";
    public static string fileName = "layoutSave.json";

    public static bool SaveLevel()
    {
        Save save = new Save();
        var gridInfo = new GridInfo();
        save.gridObjects = new List<GridObjInfo>();

        var __grid = GridSystem.Instance.grid;
        gridInfo.cellSize = __grid.cellSize;
        gridInfo.gridSize = __grid.gridSize;
        gridInfo.offset = __grid.offset;

        var slots = __grid.GridSlots;
        foreach (var slot in slots)
        {
            if(slot.Value.gridObject != null)
            {
                var gridObjInfo = new GridObjInfo();
                gridObjInfo.index = slot.Key;
                gridObjInfo.objectIndex = GameManager.Instance.db.objects.FindIndex((x) => x == slot.Value.gridObject.ObjectDataLoaders[0].objectData);
                save.gridObjects.Add(gridObjInfo);
            }
        }
        save.gridInfo = gridInfo;

        var json = JsonUtility.ToJson(save);
        System.IO.File.WriteAllText(GetSavePath() + fileName, json);
        return true;
    }

    public static string GetSavePath()
    {
        var path = Application.persistentDataPath + folder;
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        return path;
    }

    public static void LoadLevel()
    {
        var json = System.IO.File.ReadAllText(GetSavePath() + fileName);
        Save save = JsonUtility.FromJson<Save>(json);

        GridSystem.Instance.DestroyGrid();
        var gridInfo = save.gridInfo;
        GameManager.Instance.ingredients.Clear();


        GridSystem.Instance.GenerateGrid(gridInfo.gridSize, gridInfo.cellSize, gridInfo.offset);
        for (int i = 0; i < save.gridObjects.Count; i++)
        {
            var target = save.gridObjects[i];
            GameManager.Instance.CreateIngredients(target.objectIndex, target.index);
        }

        GameManager.Instance.ChangeGameState(GameState.Play);
    }
}
