using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeciciFinishControl : MonoBehaviour
{
   /* public static GeciciFinishControl finishControl;
    [SerializeField] private GameLevel gameLevel;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float endTime;
    [SerializeField] private float forwardSpeed;

    public bool isEndPlace;

    private void Awake()
    {
        finishControl = this;
        isEndPlace = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(PlayerGetCenter(other.transform));
            
            playerMovement.forwardSpeed = 2;
            playerMovement.isMove = false;
            playerController.isEndPlace = true;

            isEndPlace = true;

            //SaveLevel();
        }
    }

    public void SaveLevel()
    {
        gameLevel.level++;
        SaveSystem.SaveLevel(gameLevel);
        Debug.Log("Level Kaydedildi. Sıradaki level: " + gameLevel.level);
    }

    IEnumerator PlayerGetCenter(Transform target)
    {
        var wait = new WaitForEndOfFrame();
        float time = 0;
        Vector3 start = target.position;
        float startx = start.x;
        float currentx = startx;


        while (time < endTime)
        {
            time += Time.deltaTime;
            currentx = Mathf.Lerp(startx, 0f, time / endTime);
            Vector3 tempVec = target.transform.position;
            tempVec.x = currentx;
            target.position = tempVec;
            target.position += Vector3.forward * forwardSpeed * Time.deltaTime;
            yield return wait;
        }
    }*/
}
