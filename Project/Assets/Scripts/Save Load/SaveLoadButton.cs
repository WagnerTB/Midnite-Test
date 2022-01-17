using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadButton : MonoBehaviour
{
    public void Save()
    {
        SaveLoadSystem.SaveLevel();
    }

    public void Load()
    {
        SaveLoadSystem.LoadLevel();
    }
}
