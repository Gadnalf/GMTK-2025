using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderController : MonoBehaviour, IPointerDownHandler
{
    public string sliderString;
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
        Debug.Log(slider.value);
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        panelText.text = sliderString;
        panelGlow.text = sliderString;
    }
}