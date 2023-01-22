using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static Animator animator;

    [SerializeField] private Text moneyText;
    [SerializeField] private GameObject[] Models;

    private GameObject tempModel;
    private int upgradeIndex = 0;
    private bool isEnter;

    public static string walk;

    private void Start()
    {
        UpgradeModel(upgradeIndex);
        //animator.SetBool("isWalking", false);

        // ------------------
        animator.SetTrigger("idle");

        //------------------
    }
    
    public static void AnimControl()
    {
        //animator.SetBool("isWalking", true);
        animator.SetTrigger("poor_walk");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.GetComponent<Earn>())
            return;

        if (collision.gameObject.tag == "income")
        {
            if (collision.gameObject.GetComponent<Earn>().isEarn)
            {
                PlayerStats.Money += collision.gameObject.GetComponent<Earn>().earnMoney;

                if (upgradeIndex == 0 && PlayerStats.Money >= 20)
                {
                    UpgradeModel(1);
                }
                else if (upgradeIndex == 1 && PlayerStats.Money >= 200)
                {
                    UpgradeModel(2);
                }
                else if (upgradeIndex == 2 && PlayerStats.Money >= 500)
                {
                    UpgradeModel(3);
                }
            }
            else
            {
                PlayerStats.Money -= collision.gameObject.GetComponent<Earn>().earnMoney;

                if (upgradeIndex == 1 && PlayerStats.Money < 20)
                {
                    UpgradeModel(0);
                }
                else if(upgradeIndex == 2 && PlayerStats.Money < 200)
                {
                    UpgradeModel(1);
                }
                else if(upgradeIndex == 3 && PlayerStats.Money < 500)
                {
                    UpgradeModel(2);
                }
            }

            moneyText.text = "$" + PlayerStats.Money + "K";
            Destroy(collision.gameObject);
        }
        else
        {
            isEnter = true;
            if (PlayerStats.Money < collision.gameObject.GetComponent<Earn>().earnMoney)
            {
                if (PlayerStats.Money == 0) // DURUM 3
                {
                    Debug.Log("Defeat !!!");
                    this.GetComponent<CharacterMovement>().forwadSpeed = 0;
                    animator.SetTrigger("defeat");

                    // Biraz bekle sonra GameOver UI aç
                }
                else // DURUM 2
                {
                    Debug.Log("Force? ");

                    animator.SetTrigger("force");
                }
            }
            else // DURUM 1
            {
                Debug.Log("Tokatı Yapıştırrr");
                StartCoroutine(
                        Anim_Play(
                            collision.gameObject.GetComponent<Animator>(),
                            "isScared", true, false, 0.1f));

                animator.SetTrigger( tempModel.GetComponent<CharacterStats>().SlapTypeString() );
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(isEnter)
        {
            if (PlayerStats.Money < collision.gameObject.GetComponent<Earn>().earnMoney)
            {
                if (PlayerStats.Money != 0) // DURUM 2
                {
                    Debug.Log("Para sıfırlandı");
                    PlayerStats.Money = 0;
                }
            }
            else // DURUM 1
            {
                Debug.Log("Para ver");
                PlayerStats.Money += collision.gameObject.GetComponent<Earn>().earnMoney;

                if (upgradeIndex == 0 && PlayerStats.Money >= 20)
                {
                    UpgradeModel(1);
                }
                else if (upgradeIndex == 1 && PlayerStats.Money >= 200)
                {
                    UpgradeModel(2);
                }
                else if (upgradeIndex == 2 && PlayerStats.Money >= 500)
                {
                    UpgradeModel(3);
                }
            }

            moneyText.text = "$" + PlayerStats.Money + "K";
            Destroy(collision.gameObject, 5); // Level sonu sıfırlanacağı için gerek olmayabilir
        }

        isEnter = false;
    }

    IEnumerator Anim_Play(Animator anim, string parameter, bool boolValue, bool isBackAnim, float second) {
        yield return new WaitForSeconds(second);
        anim.SetBool(parameter, boolValue);

        if (parameter == "isWalking" && !boolValue)
        {
            this.GetComponent<CharacterMovement>().forwadSpeed = 0;
        }

        if (isBackAnim)
        {
            yield return new WaitForSeconds(second);
            anim.SetBool(parameter, !boolValue);
        }
    }

    private void UpgradeModel(int index)
    {
        // Yeni karakter geçiş efekti ekle
        Destroy(tempModel);

        tempModel = Instantiate(Models[index], transform.position, transform.rotation);
        tempModel.transform.parent = this.transform;
        upgradeIndex = index;
        animator = tempModel.GetComponent<Animator>();


        if (GameManager.isGameStarted)
        {
            animator.SetTrigger("spin");
           // this.GetComponent<CharacterMovement>().forwadSpeed = 2f;
        }
        
    }



}
