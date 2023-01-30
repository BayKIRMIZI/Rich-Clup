using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeciciFinishControl : MonoBehaviour
{
    [SerializeField] private GameLevel gameLevel;

    /* private void OntriggerEnter(Collider other)
     {
         Debug.Log("Finish e Geldi");
         Debug.Log(gameLevel.level);
         if (other.tag == "Player")
         {
             gameLevel.level++;
             SaveSystem.SaveLevel(gameLevel);
             Debug.Log(gameLevel.level);
         }
     }*/

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Finish");
        if (other.tag == "Player")
        {
            gameLevel.level++;
            SaveSystem.SaveLevel(gameLevel);
            Debug.Log(gameLevel.level);
        }
    }
}
