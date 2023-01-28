using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoad : MonoBehaviour
{
    [Header("Income")]
    [SerializeField] private GameObject[] Environment; 
    // 0-> bag, 1-> posMoney, 2-> negMoney, 3-> tax

    [Header("Human")]
    [SerializeField] private GameObject[] woman_chars;
    [SerializeField] private GameObject[] man_chars;

    [Header("UI")]
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject tapToStartUI;
    
    private float zPos = 5f;
    private GameObject tempClone;
    
    public void InitLevel()
    {
        Load();
    }

    private void Load()
    {
        StartBagClone();
        while (zPos < 75f)
        {
            GetRandomMethod();
        }
        
        //MoneyClone();
        //ModelClone();
    }

    private void StartBagClone()
    {
        Vector3 cloneStartPos = new Vector3(-1.75f, 1f, zPos);
        float xPosVnum = 1.75f;
        float zPosVnum = 1.5f;

        int i = 0;
        for (; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                tempClone = Instantiate(Environment[0], 
                    cloneStartPos + new Vector3(xPosVnum * j, 0f, zPosVnum * i),
                    Quaternion.identity);
                tempClone.transform.parent = this.transform;
            }
        }

        zPos += (i - 1) * zPosVnum + 5f;
    }
    
    private void GetRandomMethod()
    {
        int chance = Random.Range(0, 3);
        if (chance == 0)
        {
            RandomChoiseEnvironment();
        }
        else if(chance == 1)
        {
            ModelClone();
        }
        else
        {
            // Model çapraz sıralı method yaz
        }
    }

    private void EnvironmentClone(GameObject cloneObj, float xPos, float zPos, int count)
    {
        float yPos = 0f;

        if (cloneObj.tag == "income")
        {
            yPos = 1f;
        }

        for (int i = 0; i < count; i++)
        {
            tempClone = Instantiate(cloneObj,
                   new Vector3(xPos, yPos, zPos + 1.5f * i),
                   Quaternion.identity
                   );
            tempClone.transform.parent = this.transform;
        }
    }

    private void RandomChoiseEnvironment()
    {
        float[] xPositions = { -1.75f, 0f, 1.75f };
        int xPosCount = Random.Range(1, xPositions.Length + 1);
        float xPos = xPositions[Random.Range(0, xPositions.Length)];
        int countChoise = Random.Range(1,4);

        int i = 0;
        if (xPosCount == 3)
        {
            for (; i < xPosCount; i++)
            {
                EnvironmentClone(
                    Environment[Random.Range(1, Environment.Length)],
                    xPositions[i],
                    zPos,
                    countChoise
                );
            }
        }
        else
        {
            while (i < xPosCount)
            {
                float choise = xPositions[Random.Range(0, xPositions.Length)];
                if (xPos != choise)
                {
                    xPos = choise;

                    EnvironmentClone(
                        Environment[Random.Range(1, Environment.Length)],
                        xPos,
                        zPos,
                        countChoise
                        );

                    i++;
                }
            }
        }

        zPos += (xPosCount - 1) * 1.5f + 5f;
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
                tempClone = Instantiate(Environment[1], 
                    new Vector3(xPosVnum, 1f, zPos + i * 1.5f), 
                    Quaternion.identity);
                tempClone.transform.parent = this.transform;
            }
        }
        else
        {
            for ( ; i < moneyCount; i++)
            {
                tempClone = Instantiate(Environment[2],
                    new Vector3(xPosVnum, 1f, zPos + i * 1.5f),
                    Quaternion.identity);
                tempClone.transform.parent = this.transform;
            }
        }

        zPos += i * 1.5f + 5f; 
    }

    private void ModelClone()
    {
        float[] xPositions = { -1.75f, 0f, 1.75f };
        int xPosCount = Random.Range(1, xPositions.Length + 1);
        float xPos = xPositions[Random.Range(0, xPositions.Length)];
        
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
                float choise = xPositions[Random.Range(0, xPositions.Length)];
                if (xPos != choise)
                {
                    xPos = choise;
                    int ch = Random.Range(0,2);
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

        //zPos += i * 1.5f + 5f;
        zPos += 5f;
    }

    public void LevelReset()
    {
        int count = transform.childCount;
        
        for (int i = count - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        Load();
        gameOverUI.SetActive(false);
        tapToStartUI.SetActive(true);
    }



    private void BagClone(float xPos, float zPos, int count)
    {
        for (int i = 0; i < count; i++)
        {
            tempClone = Instantiate(Environment[0],
                   new Vector3(xPos, 0f, zPos + 1.5f * i),
                   Quaternion.identity);
            tempClone.transform.parent = this.transform;
        }
    }

    private void PosMoneyClone(float xPos, float zPos, int count)
    {
        for (int i = 0; i < count; i++)
        {
            tempClone = Instantiate(Environment[1],
                   new Vector3(xPos, 0f, zPos + 1.5f * i),
                   Quaternion.identity);
            tempClone.transform.parent = this.transform;
        }
    }

    private void NegMoneyClone(float xPos, float zPos, int count)
    {
        for (int i = 0; i < count; i++)
        {
            tempClone = Instantiate(Environment[2],
                   new Vector3(xPos, 0f, zPos + 1.5f * i),
                   Quaternion.identity);
            tempClone.transform.parent = this.transform;
        }
    }

    private void TaxMoneyClone(float xPos, float zPos, int count)
    {
        for (int i = 0; i < count; i++)
        {
            tempClone = Instantiate(Environment[3],
                   new Vector3(xPos, 0f, zPos + 1.5f * i),
                   Quaternion.identity);
            tempClone.transform.parent = this.transform;
        }
    }

}
