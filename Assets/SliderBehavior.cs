using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderBehavior : MonoBehaviour,
    ISelectHandler, IPointerEnterHandler
{
    protected Slider slider;
    int lastSliderValue;

    [SerializeField] protected string selectSFX = "Select";
    [SerializeField] protected string submitSFX = "Confirm";

    protected virtual void Awake()
    {
        slider = GetComponent<Slider>();
        lastSliderValue = (int)slider.value;
    }

    private void Update()
    {
        if ((int)slider.value != lastSliderValue)
        {
            lastSliderValue = (int)slider.value;
            AudioManager.Instance.PlaySFX(selectSFX);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void OnSelect(BaseEventData eventData)
    {
        AudioManager.Instance.PlaySFX(selectSFX);
    }
}