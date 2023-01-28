using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float forwadSpeed = 5;
    public float swipeSpeed = 3;

    [SerializeField] private float maxClampVnum = 2.5f;
    [SerializeField] private float _roadWidth = 5f;
    [SerializeField] private float _screenWidth;

    private float _inputStartX;
    private float _modelStartX;
    
    public void PlayerMoveInit()
    {
        _screenWidth = Screen.width;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _inputStartX = Input.mousePosition.x;
            _modelStartX = transform.localPosition.x;
        }

        //if (!GameManager.isGameStarted)
        if (!LevelManager.isGameStarted)
            return;

        transform.position += Vector3.forward * forwadSpeed * Time.deltaTime;
        
        if (Input.GetMouseButton(0))
        {
            float deltaX = ((_inputStartX - Input.mousePosition.x) / _screenWidth * 2) * swipeSpeed;
            Vector3 pos = transform.localPosition;
            pos.x = Mathf.Clamp(_modelStartX - deltaX * _roadWidth, -maxClampVnum, maxClampVnum);
            transform.localPosition = pos;
        }
    }
}
