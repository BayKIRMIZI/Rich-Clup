using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTurns : MonoBehaviour
{
    [SerializeField] private float turnY;
    
    void Update()
    {
        transform.Rotate(new Vector3(0, turnY, 0) * Time.deltaTime);
    }
}
