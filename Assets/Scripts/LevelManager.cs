using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static bool isGameStarted;
    public static CharController[] chars;

    [SerializeField] private GameObject tapToStartText;
    [SerializeField] private Text moneyText;
    [SerializeField] private GameObject player;
    [SerializeField] private Text levelText;
    [SerializeField] private Transform humanModels;

    private PlayerStats playerSt;
    private PlayerController playerCont;
    private PlayerMovement playerMove;
    private GameLevel gameLevel;

    private void Awake()
    {
        playerSt = player.GetComponent<PlayerStats>();
        playerCont = player.GetComponent<PlayerController>();
        playerMove = player.GetComponent<PlayerMovement>();
        gameLevel = transform.GetComponent<GameLevel>();

        InitLoad();
        GameManagerInit();

        FillCharArray();
    }

    private void InitLoad()
    {
        playerCont.InitPlayer();
        playerMove.PlayerMoveInit();
    }

    private void GameManagerInit()
    {
        isGameStarted = false;
        tapToStartText.SetActive(true);

        gameLevel.LoadLevel();

        moneyText.text = "$" + playerSt.money + "K";
        levelText.text = "Level " + gameLevel.level.ToString();
    }

    private void FillCharArray()
    {
        chars = new CharController[humanModels.transform.childCount];

        for (int i = 0; i < chars.Length; i++)
        {
            chars[i] = humanModels.GetChild(i).GetComponent<CharController>();
        }
    }

    private void Update()
    {
        if (isGameStarted)
            return;

        if (Input.GetMouseButton(0))
        {
            isGameStarted = true;
            tapToStartText.SetActive(false);
            //PlayerController.AnimControl();
            playerCont.Anim_Control();
        }
    }
}
