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
    [SerializeField] private float upSpeed;
    [SerializeField] private float lerpSpeed = 1f;
    [SerializeField] private GameLevel gameLevel;
    [SerializeField] private GameObject particle_gMoney;
    [SerializeField] private GameObject particle_rMoney;
    [SerializeField] private GameObject particle_Upgrade;

    public GameObject tempModel;
    private int upgradeIndex = 0;
    private PlayerStats playerSt;
    private int slapIndex = 0;
    private float coolDown;
    private bool slapTrigg = false;
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

            GameObject particleClone = Instantiate(particle_Upgrade, transform.position, transform.rotation);
            particleClone.transform.parent = this.transform;
            Destroy(particleClone, 1);
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
        float weightValue = animator.GetLayerWeight(layerIndex);
        
        if (earn)
        {
            if (earn.isEarn)
            {
                playerSt.money += earn.earnMoney;
                GetUpgrade();
                StartCoroutine(ShowPopUp(other.gameObject, posPopUp, true, earn.earnMoney.ToString()));

                if (earn.earnMoney >= 50)
                {
                    // Kapı Geçişlerinde green para particle
                    Vector3 clonePos = new Vector3(other.transform.position.x, other.transform.position.y + 1f, other.transform.position.z);
                    GameObject particleClone = Instantiate(particle_gMoney, clonePos, other.transform.rotation);
                    Destroy(particleClone, 2);
                }
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
                StartCoroutine(ShowPopUp(other.gameObject, negPopUp, false, earn.earnMoney.ToString()));
                GetDownGrade();

                if (earn.earnMoney >= 50)
                {
                    // Kapı Geçişlerinde para particle
                    Vector3 clonePos = new Vector3(other.transform.position.x, other.transform.position.y + 1f, other.transform.position.z);
                    GameObject particleClone = Instantiate(particle_rMoney, clonePos, other.transform.rotation);
                    Destroy(particleClone, 2);
                }
                
            }

           

            moneyText.text = "$" + playerSt.money + "K";
            CheckCharMoney();
            Destroy(other.gameObject);
        }

        if (distController)
        {
            if (distController.isFall)
                return;

            slapTrigg = false;

            if (isEndPlace)
            {
                StartCoroutine(PlayerUp(other.transform.position.y));

                if (playerSt.money < distController.earnMoney)
                {
                    if (stairCount > 0) // ilk basamakta anim takılması olmamalı
                    {
                        slapTrigg = true;
                        StartCoroutine(Animator_Layer_OnOff(layerIndex, weightValue, 0));
                    }
                }

            }

            if (playerSt.money > distController.earnMoney)
            {
                distAnimator.SetTrigger("scared");

                if (slapIndex == 0)
                {
                    animator.SetLayerWeight(layerIndex, 1);
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
                        //UIManager.uiManager.completedUI.SetActive(true);
                        UIManager.uiManager.OpenUIinSmooth(
                            UIManager.uiManager.completedUI,
                            UIManager.uiManager.completedUI.GetComponent<CanvasGroup>()
                            );
                    }
                    else
                    {
                        //UIManager.uiManager.loseUI.SetActive(true);
                        UIManager.uiManager.OpenUIinSmooth(
                            UIManager.uiManager.loseUI,
                            UIManager.uiManager.loseUI.GetComponent<CanvasGroup>()
                            );
                    }
                    
                }
                else
                {
                    if (playerSt.money == 0) // DURUM 3
                    {
                        movement.forwardSpeed = 0;
                        charAnimator.SetTrigger("slap");
                        animator.SetTrigger("defeat");

                        //UIManager.uiManager.loseUI.SetActive(true);
                        UIManager.uiManager.OpenUIinSmooth(
                            UIManager.uiManager.loseUI,
                            UIManager.uiManager.loseUI.GetComponent<CanvasGroup>()
                            );

                        LevelManager.isLevelLosed = true;
                    }
                    else // DURUM 2
                    {
                        charAnimator.SetTrigger("slap");
                        animator.SetTrigger("force");
                    }

                    // Model tokat attığında red money particle
                    Vector3 clonePos = new Vector3(other.transform.position.x, other.transform.position.y + 1f, other.transform.position.z);
                    GameObject particleClone = Instantiate(particle_rMoney, clonePos, other.transform.rotation);
                    Destroy(particleClone, 2);

                }
            }
            else // DURUM 1
            {
                charAnimator.SetTrigger("fall");
                charController.isFall = true;
                charController.CanvasActive();

                //animator.SetTrigger(mainCharStats.SlapTypeString());
                animator.SetLayerWeight(layerIndex, 1);

                if (slapIndex == 0)
                {
                    animator.SetTrigger("slap_down");
                    slapIndex = 1;
                    StartCoroutine(SlapCoolDown());
                    
                    if (charController)
                    {
                        StartCoroutine(
                            HeadForce(
                                charAnimator,
                                other.GetComponentsInChildren<Rigidbody>(),
                                other.GetComponentsInChildren<Collider>(),
                                true,
                                -11f,
                                0.14f
                                ));
                    }
                    
                }
                else if (slapIndex == 1)
                {
                    GameManager.isCanWalk = false;
                    animator.SetTrigger("slap_right");
                    slapIndex = 0;
                    
                    if (charController)
                    {
                        StartCoroutine(
                            HeadForce(
                                charAnimator,
                                other.GetComponentsInChildren<Rigidbody>(),
                                other.GetComponentsInChildren<Collider>(),
                                true,
                                11f,
                                0.05f
                                ));
                    }
                }

                if (isEndPlace)
                {
                    stairCount++;

                    if (stairCount == 1)
                    {
                        gameLevel.level++;
                        SaveSystem.SaveLevel(gameLevel);
                    }

                    if (stairCount == 5)
                    {
                        movement.forwardSpeed = 0;

                        //UIManager.uiManager.completedUI.SetActive(true);
                        UIManager.uiManager.OpenUIinSmooth(
                            UIManager.uiManager.completedUI,
                            UIManager.uiManager.completedUI.GetComponent<CanvasGroup>()
                            );
                    
                        animator.SetLayerWeight(layerIndex,0);
                        slapTrigg = true;
                        StartCoroutine(Animator_Layer_OnOff(layerIndex, weightValue, 0));
                        animator.SetTrigger("idle");
                    }
                }

                // karakter tokat attığında green money Particle
                StartCoroutine(MoneyParticleEffect(other.gameObject));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CharController distController = other.gameObject.GetComponent<CharController>();
        CharController charController 
            = other.gameObject.transform.parent.GetComponent<CharController>();
        Animator distAnimator = other.gameObject.GetComponentInChildren<Animator>();
        Animator charAnimator = other.gameObject.GetComponent<Animator>();
        PlayerMovement movement = this.GetComponent<PlayerMovement>();
        MainCharStats mainCharStats = tempModel.GetComponent<MainCharStats>();

        int layerIndex = animator.GetLayerIndex("body");
        float weightValue = animator.GetLayerWeight(layerIndex);

        if (distController && !distController.isFall)
        {
            distAnimator.SetTrigger("idle");
            mainCharStats.Walk();

            if (weightValue != 0)
            {
                slapTrigg = true;
                StartCoroutine(Animator_Layer_OnOff(layerIndex, weightValue, 0));
            }
            
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
                StartCoroutine(ShowPopUp(other.gameObject, negPopUp, false, charController.earnMoney.ToString()));
                GetDownGrade();
                other.enabled = false;
            }
            else // DURUM 1
            {
                playerSt.money += charController.earnMoney;
                StartCoroutine(ShowPopUp(other.gameObject, posPopUp, true, charController.earnMoney.ToString()));
                GetUpgrade();
            }

            charController.isEarnable = false;
            moneyText.text = "$" + playerSt.money + "K";
            CheckCharMoney();
        }

        if (!StairCheckPoint.checkPoint.isEndPlace && charController)
        {
            //animator.SetLayerWeight(layerIndex, 0);
            slapTrigg = true;
            StartCoroutine(Animator_Layer_OnOff(layerIndex, weightValue, 0));
        }

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

    IEnumerator Animator_Layer_OnOff(int layerIndex, float startVnum, float endVnum)
    {
        var wait = new WaitForEndOfFrame();
        float time = 0;

        // slaptrig -> layerweight değeri 1 yapılsa bile coroutine çalışmaya devam ettiğinden 
        // weight değeri 0 a doğru azalmaya devam eder.
        while (time < 1 && slapTrigg)
        {
            time += Time.deltaTime / 10 * lerpSpeed;
            startVnum = Mathf.Lerp(startVnum, endVnum, time / 5);
            animator.SetLayerWeight(layerIndex, startVnum);
             
            if (startVnum <= 0.1f)
            {
                animator.SetLayerWeight(layerIndex, 0);
                break;
            }
            yield return wait;
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

    [SerializeField] private GameObject posPopUp;
    [SerializeField] private GameObject negPopUp;
    [SerializeField] private Vector3 PopUpPosition;
    [SerializeField] private float popUpSizeVnum;
    private TextMesh popUpTextMesh;

    IEnumerator ShowPopUp(GameObject _target, GameObject popUp, bool isEarn, string money)
    {
        var wait = new WaitForEndOfFrame();

        GameObject clone = Instantiate(popUp, _target.transform.position, transform.rotation);
        
        popUpTextMesh = clone.GetComponent<TextMesh>();

        if (isEarn)
        {
            popUpTextMesh.text = "+" + money + "K";
        }
        else
        {
            popUpTextMesh.text = "-" + money + "K";
        }

        Vector3 tempPos = clone.transform.position;
        while (clone)
        {
            tempPos.z = transform.position.z + 0.5f;
            tempPos.x = transform.position.x + 0.25f;
            clone.transform.position = tempPos;
            yield return wait;
        }
    }

    IEnumerator MoneyParticleEffect(GameObject other)
    {
        yield return new WaitForSeconds(0.2f);

        Vector3 clonePos = new Vector3(other.transform.position.x, other.transform.position.y + 1f, other.transform.position.z);
        GameObject particleClone = Instantiate(particle_gMoney, clonePos, other.transform.rotation);
        Destroy(particleClone, 2);
    }

    IEnumerator HeadForce(Animator animator, Rigidbody[] ragdollBodies, Collider[] ragdollColliders, bool state, float force, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        animator.enabled = !state;

        foreach (Rigidbody rb in ragdollBodies)
        {
            rb.isKinematic = !state;
        }

        foreach (Collider collider in ragdollColliders)
        {
            collider.enabled = state;
        }

        foreach (Rigidbody rb in ragdollBodies)
        {
            rb.AddForce(Vector3.right * force, ForceMode.Impulse);
            rb.velocity = Vector3.zero; 
            rb.detectCollisions = true;
        }
    }

}
