using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradePanel : BasePanel
{
    [SerializeField]
    private TextMeshProUGUI statsTxt;

    [SerializeField]
    private TextMeshProUGUI levelTxt;

    [SerializeField]
    private TextMeshProUGUI currencyTxt;

    [SerializeField]
    private ItemDetailUI itemDetailUI;

    private GameObject currentSelectedObject = null;
    private GameObject previousSelectedObject = null;

    [SerializeField]
    private Currency currency;

    [SerializeField]
    private CharacterStats stats;

    [SerializeField]
    private GameProgressUI gameProgress;
    [SerializeField]
    private Inventory inventory;

    [SerializeField] private LevelUpButton LevelUpButton;
    private Item SelectedItem
    {
        get
        {
            var go = EventSystem.current.currentSelectedGameObject;
            if (!go) return null;
            var slot = go.GetComponent<ItemSlotUI>();
            if (!slot) return null;
            return slot.Item;
        }
    }

    [SerializeField]
    private Shop shop;

    private Action<Notify> OnStatsChange, OnLevelChange, OnCurrencyChange, OnRoundChange, OnRetry;

    private void Awake()
    {
        OnStatsChange = thisNotify => { if (thisNotify is StatsChangeNotify notify) UpdateStats(notify.stats); };
        OnLevelChange = thisNotify =>
        {
            if (thisNotify is LevelChangeNotify notify)
            {
                UpdateLevelUpCost(notify.level, notify.levelUpCost);
                UpdateLevel(notify.level);
            }
        };

        OnCurrencyChange = thisNotify => { if (thisNotify is CurrencyChangeNotify notify) UpdateFragmentText(notify.balance); };
        OnRoundChange = thisNotify => { if (thisNotify is RoundChangeNotify notify) UpdateGameProgress(notify.round); };
        OnRetry = thisNotify => gameProgress.Reset();
    }

    private void OnEnable()
    {
        EventManager.Instance.AddListener(EventID.StatsChange, OnStatsChange);
        EventManager.Instance.AddListener(EventID.LevelChange, OnLevelChange);
        EventManager.Instance.AddListener(EventID.CurrencyChange, OnCurrencyChange);
        EventManager.Instance.AddListener(EventID.RoundChange, OnRoundChange);
        EventManager.Instance.AddListener(EventID.Retry, OnRetry);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(EventID.StatsChange, OnStatsChange);
        EventManager.Instance.RemoveListener(EventID.LevelChange, OnLevelChange);
        EventManager.Instance.RemoveListener(EventID.CurrencyChange, OnCurrencyChange);
        EventManager.Instance.RemoveListener(EventID.RoundChange, OnRoundChange);
        EventManager.Instance.RemoveListener(EventID.Retry, OnRetry);
    }

    private void Update()
    {
        if (UIManager.Instance.CompareCurrentView(View.Upgrade))
        {
            PreventLoseFocus();

            if (InputManager.Instance.LevelUp)
            {
                OnLevelUpButtonClick();
            }
            if (InputManager.Instance.NextRound)
            {
                OnNextRoundButtonClick();
            }
            if (InputManager.Instance.Equip)
            {
                if (SelectedItem == null) return;
                if (!SelectedItem.isEquip)
                {
                    OnEquipButtonClick();
                }
                else
                {
                    OnUnequipButtonClick();
                }
            }
            if (InputManager.Instance.Recycle)
            {
                OnRecycleButtonClick();
            }
            if (InputManager.Instance.Buy1)
            {
                Buy(0);
            }
            if (InputManager.Instance.Buy2)
            {
                Buy(1);
            }
            if (InputManager.Instance.Buy3)
            {
                Buy(2);
            }
            if (InputManager.Instance.Buy4)
            {
                Buy(3);
            }
            if (InputManager.Instance.Roll)
            {
                shop.OnRollButtonClick();
            }
            if(InputManager.Instance.Cancel)
            {
                UIManager.Instance.Show(View.Pause);
            }
        }
    }

    private void PreventLoseFocus()
    {
        currentSelectedObject = EventSystem.current.currentSelectedGameObject;
        if (currentSelectedObject != previousSelectedObject)
        {
            //TODO: play SFX
            if (currentSelectedObject != null)
            {
                previousSelectedObject = currentSelectedObject;
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(previousSelectedObject);
            }
        }
    }

    public void OnLevelUpButtonClick()
    {
        if (stats.isMaxLevel) return;

        if (currency.Use(stats.LevelUpCost))
        {
            stats.LevelUp();
            UpdateLevelUpCost(stats.Level, stats.LevelUpCost);
        }
    }

    private void UpdateLevelUpCost(int level, int cost)
    {
        if (level >= 10)
        {
            LevelUpButton.MaxLevel();
        }
        else
        {
            LevelUpButton.SetPriceText(cost);
        }
    }

    private void UpdateLevel(int level)
    {
        levelTxt.text = $"Lv <size=150%>{level}";
    }

    private void UpdateStats(CharacterStats stats)
    {
        statsTxt.text = String.Format("{0:0}\n{1:0}\n{2:0}\n{3:0.0}%\n{4:0.0}%\n{5:0.0}%", stats.Attack.Total, stats.MaxHealthPoint.Total, stats.Defense.Total, stats.CriticalHitChance.Total * 100, stats.CriticalHitDamage.Total * 100, stats.Haste.Total * 100);
    }

    private void UpdateFragmentText(int currency)
    {
        currencyTxt.text = currency.ToString();
    }

    public void OnNextRoundButtonClick()
    {
        GameManager.Instance.StartNextRound();
    }

    public void OnEquipButtonClick()
    {
        inventory.Equip(SelectedItem);
        DisplayCurrentSelectedItem();
    }
    public void OnUnequipButtonClick()
    {
        inventory.UnEquip(SelectedItem);
        DisplayCurrentSelectedItem();
    }
    public void OnRecycleButtonClick()
    {
        currency.Gain(inventory.Recycle(SelectedItem));
        shop.Recycle(SelectedItem);
        DisplayCurrentSelectedItem();
    }

    private void DisplayCurrentSelectedItem()
    {
        itemDetailUI.UpdateItemDetailUI(SelectedItem);
    }

    public void UpdateGameProgress(int round)
    {
        gameProgress.UpdateProgress(round);
    }

    public void Buy(int index)
    {
        shop.Buy(index);
        DisplayCurrentSelectedItem();
    }

    public void OnRollButtonClick()
    {
        shop.OnRollButtonClick();
    }
}