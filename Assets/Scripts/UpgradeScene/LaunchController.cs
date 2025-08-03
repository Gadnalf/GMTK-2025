using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LaunchController : MonoBehaviour
{
    Slider slider;

    void Start()
    {
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        if (UpgradeManager.tempMoney >= UpgradeManager.money)
        {
            GetComponent<Slider>().interactable = false;
        }
        else {
            GetComponent<Slider>().interactable = true;
        }
        if (slider.value >= 1)
        {
            UpgradeManager.instance.SetMoney(UpgradeManager.tempMoney);
            SceneManager.LoadScene("AcceleratorScene");
        }
    }
}
