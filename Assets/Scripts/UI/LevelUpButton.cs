using DG.Tweening;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelUpButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Graphic interactArea;
    public Image fragmentImage;

    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Graphic content;
    private UpgradePanel upgradePanel;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color highlightColor = new Color(94 / 255f, 224 / 255f, 221 / 255f, 255 / 255f);

    [SerializeField] private Color sufficientColor = Color.white;
    [SerializeField] private Color insufficientColor = new Color(127 / 255f, 127 / 255f, 127 / 255f, 255 / 255f);
    [SerializeField] private float fadeDuration = 0.1f;

    [SerializeField] CharacterStats stats;

    private Action<Notify> OnCurrencyChange, OnStartGame;

    private void Awake()
    {
        upgradePanel = GetComponentInParent<UpgradePanel>();
        Enable();

        OnCurrencyChange = thisNotify =>
        {
            if (thisNotify is CurrencyChangeNotify notify)
            {
                if (stats.LevelUpCost > notify.balance)
                {
                    Insufficient();
                }
                else
                {
                    Sufficient();
                }
            }
        };

        OnStartGame = thisNotify => Init();
    }

    private void OnEnable()
    {
        EventManager.Instance.AddListener(EventID.CurrencyChange, OnCurrencyChange);
        EventManager.Instance.AddListener(EventID.StartGame, OnStartGame);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(EventID.CurrencyChange, OnCurrencyChange);
        EventManager.Instance.RemoveListener(EventID.StartGame, OnStartGame);
    }

    private void Init()
    {
        (content as TextMeshProUGUI).text = "Level Up";
        Enable();
    }

    public void MaxLevel()
    {
        (content as TextMeshProUGUI).text = "Max Level";
        Disable();
    }

    public void Enable()
    {
        interactArea.raycastTarget = true;
        priceText.gameObject.SetActive(true);
        fragmentImage.gameObject.SetActive(true);
    }
    public void Disable()
    {
        interactArea.raycastTarget = false;
        priceText.gameObject.SetActive(false);
        fragmentImage.gameObject.SetActive(false);
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
        upgradePanel.OnLevelUpButtonClick();
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
