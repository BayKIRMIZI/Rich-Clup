using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int level;

    public PlayerData(GameLevel gameLevel)
    {
        level = gameLevel.level;
    }

}
