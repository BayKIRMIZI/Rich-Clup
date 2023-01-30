using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] levels;

    private GameObject[] levelArray;
    private GameLevel gameLevel;
    private PlayerData playerData;

    private void Awake()
    {
        gameLevel = transform.GetComponent<GameLevel>();
        //SaveSystem.SaveLevel(gameLevel);
        playerData = SaveSystem.LoadLevel(gameLevel);

        string levelPath = "Levels/Level_" + playerData.level.ToString();

        //LevelClones();
        //levelArray[playerData.level].SetActive(true);

        GameObject levelPrefab =  Resources.Load<GameObject>(levelPath);
        GameObject clone = Instantiate(levelPrefab);
        clone.transform.parent = this.transform;
    }

    private void LevelClones()
    {
        levelArray = new GameObject[levels.Length]; 

        for (int i = 0; i < levels.Length; i++)
        {
            levelArray[i] = Instantiate(levels[i]);
            levelArray[i].SetActive(false);
            levelArray[i].transform.parent = this.transform;
        }
    }
}
