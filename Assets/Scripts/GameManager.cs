using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static bool isGameStarted;
    public static bool isCanWalk = true;


    private UIManager ui;
    private GameLevel gameLevel;
    private PlayerData playerData;
    private GameObject levelPrefab;
    private GameObject clone;
    private string levelPath;

    private void Awake()
    {
        gameLevel = transform.GetComponent<GameLevel>();
        //SaveSystem.SaveLevel(gameLevel);
        
        
        
        PlayerController.gameManager = this;

        /*playerData = SaveSystem.LoadLevel(gameLevel);
        levelInfo.text = playerData.level.ToString();
        levelPath = "Levels/Level_" + playerData.level.ToString();
        levelPrefab =  Resources.Load<GameObject>(levelPath);
        clone = Instantiate(levelPrefab);
        clone.transform.parent = this.transform;*/
    }

    private void Start()
    {
        ui = UIManager.uiManager;
        UploadLevel();
        ui.loseUI.SetActive(false);
        ui.completedUI.SetActive(false);

    }


    private void Update()
    {
        if (GameManager.isGameStarted)
            return;

        if (Input.GetMouseButton(0))
        {
            GameManager.isGameStarted = true;

            ui.tapToStartText.gameObject.SetActive(false);
        }
    }

    private void UploadLevel()
    {
        Destroy(clone);
        playerData = SaveSystem.LoadLevel(gameLevel);

        ui.levelInfo.text = "Level " + playerData.level.ToString();

        if (playerData.level > 4)
        {
            string levelForPath = ( ( (playerData.level - 1) % 4) + 1).ToString();
            levelPath = "Levels/Level_" + levelForPath;
            Debug.Log(levelForPath);
        }
        else
        {
            levelPath = "Levels/Level_" + playerData.level.ToString();
        }
        
        levelPrefab = Resources.Load<GameObject>(levelPath);
        clone = Instantiate(levelPrefab);
        clone.transform.parent = this.transform;
    }

    public void NextLevel()
    {
        LevelManager.isLevelStarted = false;
        GameManager.isGameStarted = false;
        UploadLevel();

        ui.tapToStartText.gameObject.SetActive(true);
        ui.loseUI.SetActive(false);
        ui.completedUI.SetActive(false);
    }

}
