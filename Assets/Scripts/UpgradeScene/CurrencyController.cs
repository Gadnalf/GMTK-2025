using UnityEngine;
using UnityEngine.UI;

public class CurrencyController : MonoBehaviour
{
    void Start()
    {
        GetComponent<Slider>().value = UpgradeManager.instance.money == 0 ? 0 : (float) UpgradeManager.instance.tempMoney / UpgradeManager.instance.money;
    }
}
