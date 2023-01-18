using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoad : MonoBehaviour
{

    [SerializeField] private GameObject bag;
    [SerializeField] private GameObject posMoney;
    [SerializeField] private GameObject negMoney;

    private float zPos = 5f;

    void Start()
    {
        Load();
    }

    private void Load()
    {
        BagClone();
        MoneyClone();
    }

    private void BagClone()
    {
        Vector3 cloneStartPos = new Vector3(-1.75f, 1f, zPos);
        float xPosVnum = 1.75f;
        float zPosVnum = 1.5f;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Instantiate(bag, 
                    cloneStartPos + new Vector3(xPosVnum * j, 0f, zPosVnum * i),
                    Quaternion.identity);
            }

            zPos += i * zPosVnum;
        }

        zPos += 5f; // Kullanılınca sonraki çağırmada 5 birim uzaktan başlasın
    }

    private void MoneyClone()
    {
        int xChoise = Random.Range(-1,1);
        float xPosVnum = xChoise * 1.75f;

        int choiseMoney = Random.Range(1,3);
        int moneyCount = Random.Range(1,3);

        int i = 0;
        if (choiseMoney > 1)
        {
            for ( ; i < moneyCount; i++)
            {
                Instantiate(posMoney, 
                    new Vector3(xPosVnum, 1f, zPos + i * 1.5f), 
                    Quaternion.identity);
            }
        }
        else
        {
            for ( ; i < moneyCount; i++)
            {
                Instantiate(negMoney,
                    new Vector3(xPosVnum, 1f, zPos + i * 1.5f),
                    Quaternion.identity);
            }
        }
        zPos += i * 1.5f + 5f; 

    }

    private void ModelClone()
    {

    }

}
