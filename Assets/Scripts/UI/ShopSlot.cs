using DG.Tweening;
using System;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private UpgradePanel upgradePanel;
    public int index;
    public Item item { get; set; }

    [SerializeField] public TextMeshProUGUI itemNameText;

    [SerializeField] public Image itemBackGround;
    [SerializeField] public Image itemLight;
    [SerializeField] public Image iconImage;
    [SerializeField] public GameObject stars;
    [SerializeField] public ItemStars star;

    [SerializeField] public Image border;
    [SerializeField] private Color normalColor = new Color(255 / 255f, 255 / 255f, 255 / 255f, 30 / 100f);
    [SerializeField] private Color highlightColor = Color.white;
    [SerializeField] public float fadeDurarion = 0.1f;

    [SerializeField] public GameObject require;

    [SerializeField] public TextMeshProUGUI itemStatsText;
    [SerializeField] public TextMeshProUGUI itemStatsValueText;

    [SerializeField] public TextMeshProUGUI priceText;

    [SerializeField] private Color sufficientColor = Color.white;
    [SerializeField] private Color insufficientColor = new Color(127 / 255f, 127 / 255f, 127 / 255f, 255 / 255f);

    public CanvasGroup contents;

    private Action<Notify> OnCurrencyChange;

    private void Awake()
    {
        index = transform.GetSiblingIndex();
        upgradePanel = GetComponentInParent<UpgradePanel>();
        border.color = normalColor;

        OnCurrencyChange = thisNotify =>
        {
            if (thisNotify is CurrencyChangeNotify notify)
            {
                if (item == null) return;
                if (item.Price > notify.balance)
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

    public void UpdateShopSlot(Item item)
    {
        this.item = item;
        if (item != null)
        {
            contents.alpha = 1;
            iconImage.color = Color.white;
            require.SetActive(true);


            itemNameText.text = item.profile.itemName;
            iconImage.sprite = item.profile.icon;
            itemBackGround.color = item.DarkColor;
            itemLight.color = item.LightColor;
            star.ShowStar(item.Rarity);
            itemStatsText.text = item.ModifierStat();
            itemStatsValueText.text = item.ModifierValue();
            priceText.text = item.Price.ToString();
        }
        else
        {
            contents.alpha = 0;
        }
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
        upgradePanel.Buy(index);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        border.DOColor(highlightColor, fadeDurarion);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        border.DOColor(normalColor, fadeDurarion);
    }
}
