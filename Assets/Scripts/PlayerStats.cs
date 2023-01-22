using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static ulong Money; 
    public ulong startMoney = 5; 
    
    void Start()
    {
        Money = startMoney;
    }
}
