using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;




public static class SaveLoadSystem 
{
    public static string folder = "/Save/";
    public const string defaultFileName = "layoutSave.json";

    public static LevelData GetLevelData()
    {
        LevelData save = new LevelData();
        var gridInfo = new GridInfo();
        save.gridObjects = new List<GridObjInfo>();

        var __grid = GridSystem.Instance.grid;
        gridInfo.cellSize = __grid.cellSize;
        gridInfo.gridSize = __grid.gridSize;
        gridInfo.offset = __grid.offset;

        var slots = __grid.GridSlots;
        foreach (var slot in slots)
        {
            if (slot.Value.gridObject != null)
            {
                var gridObjInfo = new GridObjInfo();
                gridObjInfo.index = slot.Key;
                gridObjInfo.objectIndex = GameManager.Instance.db.objects.FindIndex((x) => x == slot.Value.gridObject.ObjectDataLoaders[0].objectData);
                save.gridObjects.Add(gridObjInfo);
            }
        }
        save.gridInfo = gridInfo;
        return save;
    }

    public static bool SaveLevel()
    {
        var save = GetLevelData();

        var json = JsonUtility.ToJson(save);
        System.IO.File.WriteAllText(GetSavePath() + defaultFileName, json);
        return true;
    }

    public static string GetSavePath()
    {
        var path = Application.persistentDataPath + folder;
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        return path;
    }

    public static void LoadLevel(string fileName = defaultFileName)
    {
        var json = System.IO.File.ReadAllText(GetSavePath() + fileName);
        LevelData save = JsonUtility.FromJson<LevelData>(json);

        LevelController.ClearLevel(true);
        var gridInfo = save.gridInfo;


        GridSystem.Instance.GenerateGrid(gridInfo.gridSize, gridInfo.cellSize, gridInfo.offset);
        for (int i = 0; i < save.gridObjects.Count; i++)
        {
            var target = save.gridObjects[i];
            LevelController.CreateIngredients(target.objectIndex, target.index);
        }

        GameManager.Instance.ChangeGameState(GameState.Play);
    }

    public static void LoadLevel(LevelData levelData)
    {
        LevelController.ClearLevel(false);
        var gridInfo = levelData.gridInfo;

        for (int i = 0; i < levelData.gridObjects.Count; i++)
        {
            var target = levelData.gridObjects[i];
            LevelController.CreateIngredients(target.objectIndex, target.index);
        }

        GameManager.Instance.ChangeGameState(GameState.Play);
    }
}
