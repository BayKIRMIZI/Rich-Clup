using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static bool isGameStarted;

    [SerializeField] private GameObject mainCanvas;
    [SerializeField] private Text moneyText;

    private void Start()
    {
        isGameStarted = false;
        mainCanvas.SetActive(true);
        moneyText.text = "$" + PlayerStats.Money + "K";
    }

    private void Update()
    {
        if (isGameStarted)
            return;

        if (Input.GetMouseButton(0))
        {
            isGameStarted = true;
            mainCanvas.SetActive(false);
            PlayerController.AnimControl();
        }
    }
}
