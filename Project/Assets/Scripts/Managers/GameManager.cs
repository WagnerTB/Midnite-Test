using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    None,
    Play,
    Paused,
    Animating
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get { return _instance; } }
    private static GameManager _instance;

    public ObjectsDatabase db;

    public static GameState CurrentGameState { get { return _currentGameState; } }
    private static GameState _currentGameState;

    public delegate void GameStateEvent(GameState gameState);
    public static GameStateEvent OnGameStateChanged;

    public List<ObjectDataLoader> ingredients = new List<ObjectDataLoader>();

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

    public void ChangeGameState(GameState gameState)
    {
        _currentGameState = gameState;
        OnGameStateChanged?.Invoke(_currentGameState);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            SaveLoadSystem.LoadLevel();
        }

    }

    public void CreateIngredients(int objIndex, Vector2Int index)
    {
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
            ingredients.Add(objLoader);
    }

    public void CheckWin(GridObject gridObject)
    {
        var list = gridObject.ObjectDataLoaders;

        if (list.Count >= 3)
        {
            if (list[0].objectData.objectName == "Bread" &&
                list[list.Count - 1].objectData.objectName == "Bread")
            {
                Debug.Log("Bread on both sides!");
                bool win = true;
                GridObject ingredientGridObject = null;

                for (int i = 0; i < ingredients.Count; i++)
                {
                    var target = ingredients[i];

                    if (i == 0)
                        ingredientGridObject = ingredients[0].gridObject;
                    else
                    {
                        if (target.gridObject != ingredientGridObject)
                        {
                            win = false;
                            break;
                        }
                    }
                }

                if(win)
                WinGame();
            }

        }
    }

    public void WinGame()
    {
        Debug.Log("<color=green>WIN THE GAME!! </color>");
    }
}
