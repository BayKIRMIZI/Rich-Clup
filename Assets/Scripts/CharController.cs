using UnityEngine;
using UnityEngine.UI;

public class CharController : MonoBehaviour
{
    public ulong earnMoney;
    public bool isFall;
    public bool isEarnable;

    [SerializeField] private Text money;
    [SerializeField] private GameObject red;
    [SerializeField] private GameObject green;

    private void Start()
    {
        money.text = earnMoney.ToString();
    }

    public void SetCharPanel(PlayerStats playerStats)
    {
        if (earnMoney > playerStats.money)
        {
            green.SetActive(false);
            red.SetActive(true);
        }
        else
        {
            red.SetActive(false);
            green.SetActive(true);
        }
    }
}
