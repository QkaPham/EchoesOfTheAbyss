using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradePanel : BasePanel
{
    [SerializeField]
    private TextMeshProUGUI statsTxt;

    [SerializeField]
    private TextMeshProUGUI levelTxt;

    [SerializeField]
    private TextMeshProUGUI currencyTxt;

    [SerializeField]
    private TextMeshProUGUI levelUpCostTxt;

    [SerializeField]
    private ItemDetailUI itemDetailUI;

    [SerializeField]
    private GameObject currentSelectedObject = null;
    [SerializeField]
    private GameObject previousSelectedObject = null;

    [SerializeField]
    private Currency currency;

    [SerializeField]
    private CharacterStats stats;

    [SerializeField]
    private GameProgressUI gameProgress;
    [SerializeField]
    private Inventory inventory;
    private Item SelectedItem
    {
        get
        {
            var go = EventSystem.current.currentSelectedGameObject;
            if (!go) return null;
            var slot = go.GetComponent<SlotUI>();
            if (!slot) return null;
            return slot.Item;
        }
    }

    [SerializeField]
    private Shop shop;

    private Action<Notify> OnStatsChange, OnLevelChange, OnCurrencyChange;

    private void Awake()
    {
        OnStatsChange = thisNotify => { if (thisNotify is StatsChangeNotify notify) UpdateStats(notify.stats); };
        OnLevelChange = thisNotify =>
        {
            if (thisNotify is LevelChangeNotify notify)
            {
                UpdateLevelUpCost(notify.levelUpCost);
                UpdateLevel(notify.level);
            }
        };

        OnCurrencyChange = thisNotify => { if (thisNotify is CurrencyChangeNotify notify) UpdateFragmentText(notify.balance); };
    }

    public void Start()
    {
        EventManager.AddListener(EventID.StatsChange, OnStatsChange);
        EventManager.AddListener(EventID.LevelChange, OnLevelChange);
        EventManager.AddListener(EventID.CurrencyChange, OnCurrencyChange);
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
                shop.Roll();
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
        if (currency.Use(stats.LevelUpCost))
        {
            stats.LevelUp();
            UpdateLevelUpCost(stats.LevelUpCost);
        }
    }

    private void UpdateLevelUpCost(int cost)
    {
        levelUpCostTxt.text = cost.ToString();
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
}