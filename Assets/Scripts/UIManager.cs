using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager uiManager;
    public Text levelInfo;
    public Text tapToStartText;
    
    public GameObject completedUI;
    public GameObject loseUI;
    
    private void Awake()
    {
        uiManager = this;
    }

    public void OpenUIinSmooth(GameObject setActiveObj, CanvasGroup canvas)
    {
        setActiveObj.SetActive(true);
        StartCoroutine(SmoothUI(canvas));
    }

    IEnumerator SmoothUI(CanvasGroup canvasGroup)
    {
        float counter = 0;
        float value = 0.01f;
        canvasGroup.alpha = 0;

        while (counter < 100)
        {
            counter ++;
            canvasGroup.alpha += value;
            yield return new WaitForSeconds(value);
        }
    }

    /*IEnumerator SmoothUI(CanvasGroup canvasGroup)
    {
        var wait = new WaitForEndOfFrame();
        float time = 0;
        canvasGroup.alpha = 0;
        float endVnum = 1;
        Debug.Log(canvasGroup.alpha);
        
        while (time < 1)
        {
            time += Time.deltaTime / 100;
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, endVnum, time);
            
            yield return wait;
        }
        
        
    }*/
}
