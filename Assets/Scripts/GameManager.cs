using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool isGameStarted;

    [SerializeField] private GameObject mainCanvas;

    private void Start()
    {
        isGameStarted = false;
        mainCanvas.SetActive(true);
    }

    private void Update()
    {
        if (isGameStarted)
            return;

        if (Input.GetMouseButton(0))
        {
            isGameStarted = true;
            mainCanvas.SetActive(false);
            CharacterAnimController.AnimControl();
        }
    }
}
