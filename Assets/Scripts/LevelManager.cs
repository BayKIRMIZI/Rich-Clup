using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static bool isLevelStarted;
    public static CharController[] chars;

    [SerializeField] private GameObject tapToStartText;
    [SerializeField] private Text moneyText;
    [SerializeField] private GameObject player;
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

        FillCharArray();
        InitLoad();
        GameManagerInit();

    }

    private void InitLoad()
    {
        playerCont.InitPlayer();
        playerMove.PlayerMoveInit();
    }

    private void GameManagerInit()
    {
        isLevelStarted = false;
        tapToStartText.SetActive(true);

        gameLevel.LoadLevel();

        moneyText.text = "$" + playerSt.money + "K";
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
        if (isLevelStarted)
            return;

        if (Input.GetMouseButton(0))
        {
            isLevelStarted = true;
            playerCont.Anim_Control();
        }
    }
}
