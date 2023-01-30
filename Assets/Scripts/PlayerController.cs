using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static Animator animator;

    [SerializeField] private Text moneyText;
    [SerializeField] private GameObject[] models;
    [SerializeField] private GameObject gameOverUI;

    private GameObject tempModel;
    private int upgradeIndex = 0;
    private PlayerStats playerSt;
    
    public void InitPlayer()
    {
        playerSt = GetComponent<PlayerStats>();
        MainCharacterPool();
        SetActiveModel(0, 0);
        animator.SetTrigger("idle");
    }
    
    public static void AnimControl()
    {
        int layerIndex = animator.GetLayerIndex("body");
        animator.SetLayerWeight(layerIndex, 0);
        animator.SetTrigger("poor_walk");
        Debug.Log("Statik Anim Control");
    }

    public void Anim_Control()
    {
        //int layerIndex = animator.GetLayerIndex("body");
        //animator.SetLayerWeight(layerIndex, 0);
        animator = tempModel.GetComponent<Animator>();
        animator.SetTrigger("poor_walk");
        Debug.Log(animator);
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
        
        //if (GameManager.isGameStarted)
        if (LevelManager.isGameStarted)
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

            if (playerSt.money > distController.earnMoney)
            {
                distAnimator.SetTrigger("scared");
                animator.SetLayerWeight(layerIndex, 1);
                animator.SetTrigger("slap_up");
            }
        }

        if (charController)
        {
            if (charController.isFall)
                return;

            if (playerSt.money < charController.earnMoney)
            {
                if (playerSt.money == 0) // DURUM 3
                {
                    Debug.Log("Defeat !!!");
                    movement.forwadSpeed = 0;
                    charAnimator.SetTrigger("slap");
                    animator.SetTrigger("defeat");

                    // Biraz bekle sonra GameOver UI aç
                    //gameOverUI.SetActive(true);
                }
                else // DURUM 2
                {
                    Debug.Log("Force? ");
                    charAnimator.SetTrigger("slap");
                    animator.SetTrigger("force");
                }
            }
            else // DURUM 1
            {
                Debug.Log("Tokatı Önce Kaldır -- Yapıştırrr");
                charAnimator.SetTrigger("fall");
                charController.isFall = true;

                //animator.SetTrigger(mainCharStats.SlapTypeString());
                //animator.Play("Anim_rich_catwalk_slap");
                animator.SetLayerWeight(layerIndex, 1);
                animator.SetTrigger("slap_down");
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

        if (charController)
        {

            if (playerSt.money < charController.earnMoney && playerSt.money != 0) // DURUM 2
            {
                Debug.Log("Para sıfırlandı");

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
                Debug.Log("Para ver");
                playerSt.money += charController.earnMoney;

                GetUpgrade();
            }

            moneyText.text = "$" + playerSt.money + "K";
            CheckCharMoney();
        }

        animator.SetLayerWeight(layerIndex, 0);
    }
    
}
