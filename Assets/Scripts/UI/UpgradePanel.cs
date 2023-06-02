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
    InventorySlot,
    EquipmentSlot
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

    public static event Action OnLevelUp;

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

    private void Start()
    {

    }

    public void OnEnable()
    {
        CharacterStats.OnStatsChange += UpdateStatsTxt;
        CharacterStats.OnLevelChange += (level, cost) => UpdateLevelUpCost(cost);
        Currency.OnCurrencyChange += UpdateFragmentText;
    }

    public void OnDisable()
    {
        CharacterStats.OnStatsChange -= UpdateStatsTxt;
        CharacterStats.OnLevelChange -= (level, cost) => UpdateLevelUpCost(cost);
        Currency.OnCurrencyChange -= UpdateFragmentText;
    }
    public float moveDuration;
    public float fadeDuration;


    public Vector3 moveDir;

    public CanvasGroup statsCanvasGroup;
    public Vector3 statsMoveDir;

    public CanvasGroup itemsCanvasGroup;
    public Vector3 itemsMoveDir;

    protected void SetUp()
    {
        gameProgress.transform.position -= moveDir;

        statsCanvasGroup.transform.position -= statsMoveDir;
        statsCanvasGroup.alpha = 0;

        itemsCanvasGroup.transform.position -= itemsMoveDir;
        itemsCanvasGroup.alpha = .2f;
    }

    protected void ActiveAnimate(bool active, float delay = 0f)
    {
        if (active)
        {
            gameProgress.transform.DOMove(gameProgress.transform.position + moveDir, moveDuration);

            statsCanvasGroup.transform.DOMove(statsCanvasGroup.transform.position + statsMoveDir, moveDuration);
            statsCanvasGroup.DOFade(1f, fadeDuration);

            itemsCanvasGroup.transform.DOMove(itemsCanvasGroup.transform.position + itemsMoveDir, moveDuration);
            itemsCanvasGroup.DOFade(1f, fadeDuration);
        }
        else
        {
            gameProgress.transform.DOMove(gameProgress.transform.position - moveDir, moveDuration);

            statsCanvasGroup.transform.DOMove(statsCanvasGroup.transform.position - statsMoveDir, moveDuration);
            statsCanvasGroup.DOFade(0f, Mathf.Clamp(delay*0.5f, 0.01f, delay));

            itemsCanvasGroup.transform.DOMove(itemsCanvasGroup.transform.position - itemsMoveDir, moveDuration);
            itemsCanvasGroup.DOFade(0f, Mathf.Clamp(delay * 0.5f, 0.01f, delay));
        }
    }

    protected override IEnumerator DelayActivate(bool active, float delay)
    {
        yield return null;
        if (active)
        {
            yield return new WaitForSeconds(delay);
            isActive = true;
            EventSystem.current.SetSelectedGameObject(firstSelectedGameObject);
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            ActiveAnimate(active);
        }
        else
        {
            ActiveAnimate(active, delay);
            yield return new WaitForSeconds(delay);
            isActive = false;
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public override void Activate(bool active, float delay = 0f)
    {
        base.Activate(active, delay);
        itemDetailUI.UpdateItemDetailUI(null, SelectedSlotType);
    }

    private void Update()
    {
        if (isActive)
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
                if (SelectedSlotType == SlotType.InventorySlot)
                {
                    OnEquipButtonClick();
                }
                else if (SelectedSlotType == SlotType.EquipmentSlot)
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
            UpdateStatsTxt(stats);
        }
    }

    private void UpdateLevelUpCost(int cost)
    {
        levelUpCostTxt.text = cost.ToString();
    }


    private void UpdateStatsTxt(CharacterStats stats)
    {
        levelTxt.text = $"Lv <size=150%>{stats.Level}";
        // statsTxt.text = String.Format("{0:0.00}");
        statsTxt.text = $"{stats.Attack.Total}\n{stats.MaxHealthPoint.Total}\n{stats.Defense.Total}\n{(stats.CriticalHitChance.Total * 100).ToString("F")}%\n{stats.CriticalHitDamage.Total * 100}%\n{stats.Haste.Total * 100}%";
    }

    private void UpdateFragmentText(int currency)
    {
        currencyTxt.text = currency.ToString();
    }

    public void OnNextRoundButtonClick()
    {
        // Activate(false);
        //OnNextRound?.Invoke();
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
                inventory.Remove(SelectedSlotIndex);
            }
            DisplayCurrentSelectedItem();
        }
    }
    public void OnUnequipButtonClick()
    {
        var item = equipment.Items[SelectedSlotIndex];
        if (item != null)
        {
            if (inventory.Add(item))
            {
                equipment.Remove(SelectedSlotIndex);
            }
            DisplayCurrentSelectedItem();
        }
    }
    public void OnRecycleButtonClick()
    {
        if (SelectedSlotType == SlotType.InventorySlot)
        {
            inventory.Recycle(SelectedSlotIndex);
        }
        if (SelectedSlotType == SlotType.EquipmentSlot)
        {
            equipment.Recycle(SelectedSlotIndex);
        }
        DisplayCurrentSelectedItem();
    }

    private void DisplayCurrentSelectedItem()
    {
        if (SelectedSlotIndex < 0) return;
        var item = SelectedSlotType == SlotType.InventorySlot ? inventory.Items[SelectedSlotIndex] : equipment.Items[SelectedSlotIndex];
        itemDetailUI.UpdateItemDetailUI(item, SelectedSlotType);
    }

    public void UpdateGameProgress(int round)
    {
        gameProgress.UpdateProgress(round);
    }
}