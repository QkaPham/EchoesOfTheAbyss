using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RollButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Graphic content;
    private UpgradePanel upgradePanel;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color highlightColor = new Color(94 / 255f, 224 / 255f, 221 / 255f, 255 / 255f);

    [SerializeField] private Color sufficientColor = Color.white;
    [SerializeField] private Color insufficientColor = new Color(127 / 255f, 127 / 255f, 127 / 255f, 255 / 255f);
    [SerializeField] private float fadeDuration = 0.1f;
    [SerializeField] private RollingConfig rollingConfig;

    private Action<Notify> OnCurrencyChange;

    private void Awake()
    {
        upgradePanel = GetComponentInParent<UpgradePanel>();

        OnCurrencyChange = thisNotify =>
        {
            if (thisNotify is CurrencyChangeNotify notify)
            {
                if (rollingConfig.RollingCost > notify.balance)
                {
                    Insufficient();
                }
                else
                {
                    Sufficient();
                }
            }
        };

    }

    private void OnEnable()
    {
        EventManager.Instance.AddListener(EventID.CurrencyChange, OnCurrencyChange);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(EventID.CurrencyChange, OnCurrencyChange);
    }

    public void SetPriceText(int price)
    {
        priceText.text = price.ToString();
    }

    public void Insufficient()
    {
        priceText.color = insufficientColor;
    }

    public void Sufficient()
    {
        priceText.color = sufficientColor;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        upgradePanel.OnRollButtonClick();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        content.DOColor(highlightColor, fadeDuration);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        content.DOColor(normalColor, fadeDuration);
    }
}
