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
        /* if (UpgradeManager.instance.currentPoints > UpgradeManager.instance.totalPoints)
        {
            GetComponent<Slider>().interactable = false;
        } */
        /*else */if (slider.value >= 1)
        {
            SceneManager.LoadScene("AcceleratorScene");
        }
    }
}
