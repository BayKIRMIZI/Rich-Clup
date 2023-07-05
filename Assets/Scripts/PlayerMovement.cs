using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float forwardSpeed = 5;
    public float swipeSpeed = 3;
    public bool isMove;
    public bool goTarget;

    [SerializeField] private float maxClampVnum = 2.5f;
    [SerializeField] private float _roadWidth = 5f;
    [SerializeField] private float _screenWidth;
    [SerializeField] private float _lookSpeed = 50f;
    [SerializeField] private float _lookAngle = 45f;
    [SerializeField] private PlayerController playerCont;
    
    private float _inputStartX;
    private float _modelStartX;
    private float lastPosX;

    void Start()
    {
        playerCont = transform.GetComponent<PlayerController>();
    }
    
    public void PlayerMoveInit()
    {
        _screenWidth = Screen.width;
        isMove = true;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _inputStartX = Input.mousePosition.x;
            _modelStartX = transform.localPosition.x;
        }
        
        if (!LevelManager.isLevelStarted)
            return;

        if (LevelManager.isLevelLosed)
            return;

        transform.position += Vector3.forward * forwardSpeed * Time.deltaTime;
        
        if (!isMove)
            return;

        if (Input.GetMouseButton(0))
        {
            float deltaX = ((_inputStartX - Input.mousePosition.x) / _screenWidth * 2) * swipeSpeed;
            
            Vector3 pos = transform.localPosition;
            pos.x = Mathf.Clamp(_modelStartX - deltaX * _roadWidth, -maxClampVnum, maxClampVnum);
            transform.localPosition = pos;
            
            if (transform.localPosition.x < lastPosX)
            {
                playerCont.tempModel.transform.localRotation =
                    Quaternion.Lerp(
                        playerCont.tempModel.transform.localRotation, 
                        Quaternion.Euler(0,-_lookAngle,0), 
                        _lookSpeed * Time.deltaTime);
                
            }
            else if (transform.localPosition.x > lastPosX)
            {
                playerCont.tempModel.transform.localRotation =
                    Quaternion.Lerp(
                        playerCont.tempModel.transform.localRotation, 
                        Quaternion.Euler(0, _lookAngle, 0), 
                        _lookSpeed * Time.deltaTime);
            }

            lastPosX = transform.localPosition.x;  
        }
        else
        {
            playerCont.tempModel.transform.localRotation =
                Quaternion.Lerp(
                    playerCont.tempModel.transform.localRotation, 
                    Quaternion.Euler(0, 0, 0), 
                    _lookSpeed * Time.deltaTime);
        }
    }
}
