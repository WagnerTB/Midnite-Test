using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinWindow : MonoBehaviour
{
    public GameObject window;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.OnGameStateChanged += ShowWindow;
    }

    private void ShowWindow(GameState gameState)
    {
        if(gameState == GameState.Win)
        {
            window.SetActive(true);
        }
        else
        {
            window.SetActive(false);
        }
    }
}
