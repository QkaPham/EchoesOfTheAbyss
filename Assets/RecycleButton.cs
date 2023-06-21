using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RecycleButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Graphic content;
    private UpgradePanel upgradePanel;
    public Color normalColor = Color.white;
    public Color highlightColor = new Color(94 / 255f, 224 / 255f, 221 / 255f, 255 / 255f);
    public float fadeDuration = 0.1f;

    public void SetPriceText(int price)
    {
        priceText.text = price.ToString();
    }

    private void Awake()
    {
        upgradePanel = GetComponentInParent<UpgradePanel>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        upgradePanel.OnRecycleButtonClick();
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
