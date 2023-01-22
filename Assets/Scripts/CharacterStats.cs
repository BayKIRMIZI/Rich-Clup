using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterType
{
    Poor,
    Daily,
    Rich
}

public class CharacterStats : MonoBehaviour
{
    public CharacterType chType;
    
    public void Walk()
    {
        PlayerController.walk = WalkTypeString();
        PlayerController.animator.SetTrigger(PlayerController.walk);
    }
    
    public string WalkTypeString()
    {
        switch (chType)
        {
            case CharacterType.Poor:
                return "poor_walk";
                break;
            case CharacterType.Daily:
                return "daily_walk";
                break;
            case CharacterType.Rich:
                return "cat_walk";
                break;
            default:
                return "poor_walk";
                break;
        }
    }

    public string SlapTypeString()
    {
        switch (chType)
        {
            case CharacterType.Poor:
                return "poor_walk_slap";
                break;
            case CharacterType.Daily:
                return "daily_walk_slap";
                break;
            case CharacterType.Rich:
                return "cat_walk_slap";
                break;
            default:
                return "poor_walk_slap";
                break;
        }
    }
}
