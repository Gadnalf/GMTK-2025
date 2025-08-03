using UnityEngine;
using UnityEngine.UI;

public class CurrencyController : MonoBehaviour
{
    void Start()
    {
        GetComponent<Slider>().value = UpgradeManager.money == 0 ? 0 : (float) UpgradeManager.tempMoney / UpgradeManager.money;
    }
}
