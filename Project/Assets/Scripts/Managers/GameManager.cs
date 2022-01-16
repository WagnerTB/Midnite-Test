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

}
