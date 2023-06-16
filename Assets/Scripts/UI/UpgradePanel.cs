using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SlotType
{
    Undefined,
    Inventory,
    Equipment
}

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
    private InventoryUI inventoryUI;

    [SerializeField]
    private EquipmentUI equipmentUI;

    [SerializeField]
    private ItemDetailUI itemDetailUI;

    private GameObject currentSelectedObject = null;
    private GameObject previousSelectedObject = null;

    [SerializeField]
    private Inventory inventory;

    [SerializeField]
    private Equipment equipment;

    [SerializeField]
    private Currency currency;

    [SerializeField]
    private CharacterStats stats;

    [SerializeField]
    private GameProgressUI gameProgress;

    private SlotType SelectedSlotType
    {
        get
        {
            SlotUI slotUI = null;
            if (currentSelectedObject != null)
            {
                slotUI = currentSelectedObject.GetComponent<SlotUI>();
            }
            if (slotUI != null)
            {
                return slotUI.SlotType;
            }
            return SlotType.Undefined;
        }
    }
    private int SelectedSlotIndex
    {
        get
        {
            if (currentSelectedObject != null)
            {
                if (currentSelectedObject.GetComponent<SlotUI>())
                {
                    return currentSelectedObject.GetComponent<SlotUI>().Index;
                }
            }
            return -1;
        }
    }

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
        EventManager.AddListiener(EventID.StatsChange, OnStatsChange);
        EventManager.AddListiener(EventID.LevelChange, OnLevelChange);
        EventManager.AddListiener(EventID.CurrencyChange, OnCurrencyChange);
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
                if (SelectedSlotType == SlotType.Inventory)
                {
                    OnEquipButtonClick();
                }
                else if (SelectedSlotType == SlotType.Equipment)
                {
                    OnUnequipButtonClick();
                }
            }
            if (InputManager.Instance.Recycle)
            {
                OnRecycleButtonClick();
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

    private void PreventLoseFocus()
    {
        currentSelectedObject = EventSystem.current.currentSelectedGameObject;
        if (currentSelectedObject != previousSelectedObject)
        {
            //TODO: play SFX
            if (currentSelectedObject != null)
            {
                previousSelectedObject = currentSelectedObject;
                DisplayCurrentSelectedItem();
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(previousSelectedObject);
            }
        }
    }

    public void OnEquipButtonClick()
    {
        var item = inventory.Items[SelectedSlotIndex];
        if (item != null)
        {
            if (equipment.Add(item))
            {
                inventory.Remove(item);
            }
            DisplayCurrentSelectedItem();
        }
    }
    public void OnUnequipButtonClick()
    {
        var item = equipment.Items[SelectedSlotIndex];
        if (item != null)
        {
            //inventory.MergeAdd(item);
            equipment.Remove(SelectedSlotIndex);
            DisplayCurrentSelectedItem();
        }
    }
    public void OnRecycleButtonClick()
    {
        if (SelectedSlotType == SlotType.Inventory)
        {
            int currency = inventory.Recycle(SelectedSlotIndex);
            this.currency.Gain(currency);
        }
        if (SelectedSlotType == SlotType.Equipment)
        {
            equipment.Recycle(SelectedSlotIndex);
        }
        DisplayCurrentSelectedItem();
    }

    private void DisplayCurrentSelectedItem()
    {
        if (SelectedSlotIndex < 0) return;
        var item = SelectedSlotType == SlotType.Inventory ? inventory.Items[SelectedSlotIndex] : equipment.Items[SelectedSlotIndex];
        //itemDetailUI.UpdateItemDetailUI(item, SelectedSlotType);
    }

    public void UpdateGameProgress(int round)
    {
        gameProgress.UpdateProgress(round);
    }

    public void Buy()
    {

    }

    public void Roll()
    {

    }
}