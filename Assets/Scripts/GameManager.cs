using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static bool isGameStarted;
    public static bool isCanWalk = true;
    
    public GameObject completedUI;
    public GameObject loseUI;

    [SerializeField] private Text levelInfo;
    [SerializeField] private Text tapToStartText;
    [SerializeField] private GameObject losePanel;

    private GameLevel gameLevel;
    private PlayerData playerData;
    private GameObject levelPrefab;
    private GameObject clone;
    private string levelPath;

    private void Awake()
    {
        gameLevel = transform.GetComponent<GameLevel>();
        //SaveSystem.SaveLevel(gameLevel);
        UploadLevel();
        losePanel.SetActive(false); // sonra sil
        loseUI.SetActive(false);
        completedUI.SetActive(false);

        PlayerController.gameManager = this;

        /*playerData = SaveSystem.LoadLevel(gameLevel);
        levelInfo.text = playerData.level.ToString();
        levelPath = "Levels/Level_" + playerData.level.ToString();
        levelPrefab =  Resources.Load<GameObject>(levelPath);
        clone = Instantiate(levelPrefab);
        clone.transform.parent = this.transform;*/
    }

    private void Update()
    {
        if (isGameStarted)
            return;

        if (Input.GetMouseButton(0))
        {
            isGameStarted = true;
            tapToStartText.gameObject.SetActive(false);
        }
    }

    private void UploadLevel()
    {
        Destroy(clone);
        playerData = SaveSystem.LoadLevel(gameLevel);
        levelInfo.text = "Level " + playerData.level.ToString();
        levelPath = "Levels/Level_" + playerData.level.ToString();
        levelPrefab = Resources.Load<GameObject>(levelPath);
        clone = Instantiate(levelPrefab);
        clone.transform.parent = this.transform;
    }

    public void NextLevel()
    {
        LevelManager.isLevelStarted = false;
        UploadLevel();
        tapToStartText.gameObject.SetActive(true);
        loseUI.SetActive(false);
        completedUI.SetActive(false);
    }

}
