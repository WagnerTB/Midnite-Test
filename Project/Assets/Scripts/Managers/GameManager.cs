using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    None,
    Play,
    Paused,
    Animating,
    Win
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get { return _instance; } }
    private static GameManager _instance;

    public ObjectsDatabase db;

    public static GameState CurrentGameState { get { return _currentGameState; } }
    private static GameState _currentGameState = GameState.Play;

    public delegate void GameStateEvent(GameState gameState);
    public static GameStateEvent OnGameStateChanged;

    public LevelController levelController;

    /// <summary>
    /// List with all ingredients
    /// </summary>
    public List<ObjectDataLoader> ingredients = new List<ObjectDataLoader>();


    /// <summary>
    /// List with all slots with a ingredient nearby
    /// </summary>
    public List<GridSlot> slotsWithObjectNearby = new List<GridSlot>();

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

        GridSlot.OnUpdateNearby += AddSlot;
        GridSlot.OnSlotTurnInvalid += RemoveSlot;
    }

    private void RemoveSlot(GridSlot gridslot)
    {
        if (slotsWithObjectNearby.Contains(gridslot))
        {
            slotsWithObjectNearby.Remove(gridslot);
        }
    }

    private void AddSlot(GridSlot gridslot)
    {
        if (!slotsWithObjectNearby.Contains(gridslot))
        {
            slotsWithObjectNearby.Add(gridslot);
        }
    }

    private void Start()
    {
        levelController.GenerateNewLevel();
    }

    public void ChangeGameState(GameState gameState)
    {
        _currentGameState = gameState;
        OnGameStateChanged?.Invoke(_currentGameState);
       // Debug.Log("[State] " + gameState);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            SaveLoadSystem.LoadLevel(SaveLoadSystem.defaultFileName);
        }

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

                if (win)
                    WinGame();
            }

        }
    }

    public void WinGame()
    {
        Debug.Log("<color=green>WIN THE GAME!! </color>");
        ChangeGameState(GameState.Win);
    }
}
