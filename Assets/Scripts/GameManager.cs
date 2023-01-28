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
        SaveSystem.SaveLevel(gameLevel);
        playerData = SaveSystem.LoadLevel(gameLevel);

        LevelClones();

        levelArray[playerData.level].SetActive(true);
        
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
