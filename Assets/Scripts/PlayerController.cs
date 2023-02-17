using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static Animator animator;
    public static GameManager gameManager;
    public bool isEndPlace;

    [SerializeField] private Text moneyText;
    [SerializeField] private GameObject[] models;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private float upSpeed;
    [SerializeField] private GameLevel gameLevel;

    private GameObject tempModel;
    private int upgradeIndex = 0;
    private PlayerStats playerSt;
    private int slapIndex = 0;
    private float coolDown;
    private int stairCount = 0;

    
    public void InitPlayer()
    {
        playerSt = GetComponent<PlayerStats>();
        MainCharacterPool();
        SetActiveModel(0, 0);
        animator.SetTrigger("idle");
        CheckCharMoney();
    }
    
    public void Anim_Control()
    {
        animator = tempModel.GetComponent<Animator>();
        animator.SetTrigger("poor_walk");
    }
    
    private void MainCharacterPool()
    {
        for (int i = 1; i < models.Length; i++)
        {
            models[i].SetActive(false);
        }
    }

    private void SetActiveModel(int lastIndex, int nextIndex)
    {
        models[lastIndex].SetActive(false);
        models[nextIndex].SetActive(true);

        tempModel = models[nextIndex];
        animator = tempModel.GetComponent<Animator>();
        
        if (LevelManager.isLevelStarted)
        {
            animator.SetTrigger("spin");
        }

        upgradeIndex = nextIndex;
    }
    
    private void GetUpgrade()
    {
        if (upgradeIndex == 0 && playerSt.money >= 20)
        {
            SetActiveModel(upgradeIndex, upgradeIndex + 1);
        }
        else if (upgradeIndex == 1 && playerSt.money >= 200)
        {
            SetActiveModel(upgradeIndex, upgradeIndex + 1);
        }
        else if (upgradeIndex == 2 && playerSt.money >= 500)
        {
            SetActiveModel(upgradeIndex, upgradeIndex + 1);
        }
    }

    private void GetDownGrade()
    {
        if (upgradeIndex == 1 && playerSt.money < 20)
        {
            SetActiveModel(upgradeIndex, upgradeIndex - 1);
        }
        else if (upgradeIndex == 2 && playerSt.money < 200)
        {
            SetActiveModel(upgradeIndex, upgradeIndex - 1);
        }
        else if (upgradeIndex == 3 && playerSt.money < 500)
        {
            SetActiveModel(upgradeIndex, upgradeIndex - 1);
        }
    }
    
    private void CheckCharMoney()
    {
        for (int i = 0; i < LevelManager.chars.Length; i++)
        {
            LevelManager.chars[i].SetCharPanel(playerSt);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Earn earn = other.gameObject.GetComponent<Earn>();

        CharController distController = other.gameObject.GetComponent<CharController>();
        Animator distAnimator = other.gameObject.GetComponentInChildren<Animator>();

        CharController charController
            = other.gameObject.transform.parent.GetComponent<CharController>();
        Animator charAnimator = other.gameObject.GetComponent<Animator>();

        PlayerMovement movement = this.GetComponent<PlayerMovement>();
        MainCharStats mainCharStats = tempModel.GetComponent<MainCharStats>();

        int layerIndex = animator.GetLayerIndex("body");
        
        if (earn)
        {
            if (earn.isEarn)
            {
                playerSt.money += earn.earnMoney;
                GetUpgrade();
            }
            else
            {
                if (playerSt.money < earn.earnMoney)
                {
                    playerSt.money = 0;
                }
                else
                {
                    playerSt.money -= earn.earnMoney;
                }
                GetDownGrade();
            }

            moneyText.text = "$" + playerSt.money + "K";
            CheckCharMoney();
            Destroy(other.gameObject);
        }

        if (distController)
        {
            if (distController.isFall)
                return;

            if (isEndPlace)
            {
                StartCoroutine(PlayerUp(other.transform.position.y));
                //animator.SetTrigger("stair");
            }

            if (playerSt.money > distController.earnMoney)
            {
                distAnimator.SetTrigger("scared");
                animator.SetLayerWeight(layerIndex, 1);

                if (slapIndex == 0)
                {
                    animator.SetTrigger("slap_up");
                }
                
            }
        }

        if (charController)
        {
            if (charController.isFall)
                return;

            if (playerSt.money < charController.earnMoney)
            {
                if (isEndPlace)
                {
                    movement.forwardSpeed = 0;
                    charAnimator.SetTrigger("slap");
                    animator.SetTrigger("defeat");

                    if (stairCount >= 1)
                    {
                        gameManager.completedUI.SetActive(true);
                    }
                    else
                    {
                        gameManager.loseUI.SetActive(true);
                    }
                    
                }
                else
                {
                    if (playerSt.money == 0) // DURUM 3
                    {
                        movement.forwardSpeed = 0;
                        charAnimator.SetTrigger("slap");
                        animator.SetTrigger("defeat");

                        gameManager.loseUI.SetActive(true);

                        // Biraz bekle sonra You Lose UI aç
                        // gameOverUI.SetActive(true);
                    }
                    else // DURUM 2
                    {
                        charAnimator.SetTrigger("slap");
                        animator.SetTrigger("force");
                    }
                }
            }
            else // DURUM 1
            {
                charAnimator.SetTrigger("fall");
                charController.isFall = true;

                //animator.SetTrigger(mainCharStats.SlapTypeString());
                //animator.Play("Anim_rich_catwalk_slap");
                animator.SetLayerWeight(layerIndex, 1);

                if (slapIndex == 0)
                {
                    animator.SetTrigger("slap_down");
                    slapIndex = 1;
                    StartCoroutine(SlapCoolDown());
                }
                else if (slapIndex == 1)
                {
                    GameManager.isCanWalk = false;
                    animator.SetTrigger("slap_right");
                    slapIndex = 0;
                }

                if (isEndPlace)
                {
                    stairCount++;

                    if (stairCount == 1)
                    {
                        gameLevel.level++;
                        if (gameLevel.level % 4 == 0)
                        {
                            gameLevel.level = 1;
                        }
                        SaveSystem.SaveLevel(gameLevel);
                    }

                    if (stairCount == 5)
                    {
                        movement.forwardSpeed = 0;
                        gameManager.completedUI.SetActive(true);
                        animator.SetTrigger("idle");
                    }
                }

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CharController distController = other.gameObject.GetComponent<CharController>();
        CharController charController 
            = other.gameObject.transform.parent.GetComponent<CharController>();
        Animator distAnimator = other.gameObject.GetComponentInChildren<Animator>();
        PlayerMovement movement = this.GetComponent<PlayerMovement>();
        MainCharStats mainCharStats = tempModel.GetComponent<MainCharStats>();
        int layerIndex = animator.GetLayerIndex("body");

        if (distController && !distController.isFall)
        {
            distAnimator.SetTrigger("idle");
            mainCharStats.Walk();
        }

        if (charController && charController.isEarnable)
        {
            if (playerSt.money < charController.earnMoney && playerSt.money != 0) // DURUM 2
            {
                if (playerSt.money < (charController.earnMoney / 2))
                {
                    playerSt.money = 0;
                }
                else
                {
                    playerSt.money -= (charController.earnMoney / 2);
                }

                GetDownGrade();
                other.enabled = false;
            }
            else // DURUM 1
            {
                playerSt.money += charController.earnMoney;

                GetUpgrade();
            }

            moneyText.text = "$" + playerSt.money + "K";
            CheckCharMoney();
        }
        
        animator.SetLayerWeight(layerIndex, 0);
    }
    
    IEnumerator SlapCoolDown()
    {
        var wait = new WaitForEndOfFrame();

        coolDown = 2;

        while (coolDown > 0)
        {
            coolDown -= Time.deltaTime;

            yield return wait;
        }

        if (coolDown <= 0)
        {
            slapIndex = 0;
        }
    }

    IEnumerator PlayerUp(float yPos)
    {
        var wait = new WaitForEndOfFrame();
        float time = 0;
        Vector3 start = transform.position;

        while (time < upSpeed)
        {
            time += Time.deltaTime;

            transform.position = 
                Vector3.Lerp(
                    start, 
                    new Vector3(transform.position.x, yPos+0.3f, transform.position.z), 
                    time);

            yield return wait;
        }
    }

}
