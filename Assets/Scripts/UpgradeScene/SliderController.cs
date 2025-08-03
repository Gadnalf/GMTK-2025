using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderController : MonoBehaviour, IPointerDownHandler
{
    public string sliderString;
    public Slider heatGauge;
    public TextMeshProUGUI panelText;
    public TextMeshProUGUI panelGlow;
    Slider slider;
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = UpgradeManager.instance.GetLevel(sliderString);
        slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    void ValueChangeCheck()
    {
        UpgradeManager.instance.Upgrade(sliderString, Mathf.RoundToInt(slider.value));
        heatGauge.value = UpgradeManager.money == 0 ? 0 : (float)UpgradeManager.tempMoney / UpgradeManager.money;
        if (UpgradeManager.tempMoney > UpgradeManager.money) heatGauge.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = Color.red;
        else heatGauge.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = Color.green;
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        panelText.text = sliderString;
        panelGlow.text = sliderString;
    }
}