using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharType
{
    Poor,
    Daily,
    Rich
}

public class MainCharStats : MonoBehaviour
{
    public CharType chType;

    public void Walk()
    {
        PlayerController.animator.SetTrigger(WalkTypeString());
    }

    public string WalkTypeString()
    {
        switch (chType)
        {
            case CharType.Poor:
                return "poor_walk";
                break;
            case CharType.Daily:
                return "daily_walk";
                break;
            case CharType.Rich:
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
            case CharType.Poor:
                return "poor_walk_slap";
                break;
            case CharType.Daily:
                return "daily_walk_slap";
                break;
            case CharType.Rich:
                return "cat_walk_slap";
                break;
            default:
                return "poor_walk_slap";
                break;
        }
    }
}
