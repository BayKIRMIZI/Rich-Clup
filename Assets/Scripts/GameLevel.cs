using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevel : MonoBehaviour
{
    public int level = 1;

    public void SaveLevel()
    {
        SaveSystem.SaveLevel(this);
    }

    public void LoadLevel()
    {
        PlayerData data = SaveSystem.LoadLevel(this);
        level = data.level;
    }
}
