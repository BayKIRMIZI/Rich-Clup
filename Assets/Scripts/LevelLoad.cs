using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoad : MonoBehaviour
{
    [Header("income")]
    [SerializeField] private GameObject bag;
    [SerializeField] private GameObject posMoney;
    [SerializeField] private GameObject negMoney;
    [Header("Human")]
    [SerializeField] private GameObject[] woman_chars;
    [SerializeField] private GameObject[] man_chars;



    private float zPos = 5f;
    private GameObject tempClone;

    void Start()
    {
        Load();
    }

    private void Load()
    {
        BagClone();
        MoneyClone();
        ModelClone();
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
                tempClone = Instantiate(bag, 
                    cloneStartPos + new Vector3(xPosVnum * j, 0f, zPosVnum * i),
                    Quaternion.identity);
                tempClone.transform.parent = this.transform;
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
                tempClone = Instantiate(posMoney, 
                    new Vector3(xPosVnum, 1f, zPos + i * 1.5f), 
                    Quaternion.identity);
                tempClone.transform.parent = this.transform;
            }
        }
        else
        {
            for ( ; i < moneyCount; i++)
            {
                tempClone = Instantiate(negMoney,
                    new Vector3(xPosVnum, 1f, zPos + i * 1.5f),
                    Quaternion.identity);
                tempClone.transform.parent = this.transform;
            }
        }

        zPos += i * 1.5f + 5f; 
    }

    private void ModelClone()
    {
        int xPosCount = Random.Range(1, 4);
        float[] xPositions = { -1.75f, 0f, 1.75f };
        float xPos = xPositions[Random.Range(0, 3)];
        
        int i = 0;
        if (xPosCount == 3)
        {
            for (; i < xPosCount; i++)
            {
                int ch = Random.Range(0,2);
                if (ch == 0)
                {
                    tempClone = Instantiate(woman_chars[Random.Range(0,woman_chars.Length)],
                        new Vector3(xPositions[i], 0f, zPos),
                        Quaternion.Euler(new Vector3(0f, 180f, 0f)));
                    tempClone.transform.parent = this.transform;
                }
                else
                {
                    tempClone = Instantiate(man_chars[Random.Range(0, man_chars.Length)],
                        new Vector3(xPositions[i], 0f, zPos),
                        Quaternion.Euler(new Vector3(0f, 180f, 0f)));
                    tempClone.transform.parent = this.transform;
                }
            }
        }
        else
        {
            while (i < xPosCount)
            {
                float choise = xPositions[Random.Range(0, 3)];
                if (xPos != choise)
                {
                    xPos = choise;
                    //int ch = Random.Range(0,2);
                    int ch = 0;
                    if (ch == 0)
                    {
                        tempClone = Instantiate(
                            woman_chars[Random.Range(0,woman_chars.Length)],
                            new Vector3(xPos, 0f, zPos),
                            Quaternion.Euler(new Vector3(0f, 180f, 0f)));
                        tempClone.transform.parent = this.transform;
                    }
                    else
                    {
                        tempClone = Instantiate(
                            man_chars[Random.Range(0,man_chars.Length)],
                            new Vector3(xPos, 0f, zPos),
                            Quaternion.Euler(new Vector3(0f, 180f, 0f)));
                        tempClone.transform.parent = this.transform;
                    }
                    
                    i++;
                }
            }
        }

        zPos += i * 1.5f + 5f;
    }

    private void LevelReset()
    {
        int count = transform.childCount;
        
        for (int i = count - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
