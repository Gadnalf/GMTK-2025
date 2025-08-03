using System;
using System.Collections.Generic;
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
    Dictionary<string, string> sliderStringDictionary = new()
    {
        ["StartingVelocity"] = "Initial speed of particle",
        ["MaxVelocity"] = "Maximum vertical and horizontal speeds of particle",
        ["AccelerationRate"] = "Speed at which the particle can accelerate",
        ["ParticleWeight"] = "Additional bulk contributing to the particle's hit points",
        ["DashSpeed"] = "Speed of the particle's dashes",
        ["DashTime"] = "Duration of the particle's dashes",
        ["TurnRate"] = "Manoeuverability of the particle on angles"
    };
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = UpgradeManager.instance.GetLevel(sliderString);
        slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    void ValueChangeCheck()
    {
        UpgradeManager.instance.Upgrade(sliderString, Mathf.RoundToInt(slider.value));
        heatGauge.value = UpgradeManager.instance.money == 0 ? 0 : (float)UpgradeManager.instance.tempMoney / UpgradeManager.instance.money;
        if (UpgradeManager.instance.tempMoney > UpgradeManager.instance.money) heatGauge.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = Color.red;
        else heatGauge.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = Color.green;
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (sliderStringDictionary.TryGetValue(sliderString, out string value))
        {
            panelText.text = value;
            panelGlow.text = value;
        }
        else
        {
            panelText.text = "";
            panelGlow.text = "";
        }
    }
}