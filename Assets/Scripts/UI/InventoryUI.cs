using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : SelectableContainer
{
    [SerializeField] private ItemDetailUI itemDetailUI;
    [SerializeField] private List<ItemSlotUI> slots;
    private Action<Notify> OnInventoryChange;

    protected override void Awake()
    {
        selectable = GetComponent<Selectable>();
        slots = new List<ItemSlotUI>();
        slots.AddRange(GetComponentsInChildren<ItemSlotUI>());
        foreach (var slot in slots)
        {
            slot.UpdateUISlot(null);
            slot.itemDetailUI = itemDetailUI;
            slot.Unclock();
        }

        OnInventoryChange = thisNotify => { if (thisNotify is InventoryChangeNotify notify) UpdateInventoryUI(notify.items); };
    }

    private void OnEnable()
    {
        EventManager.Instance.AddListener(EventID.InventoryChange, OnInventoryChange);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(EventID.InventoryChange, OnInventoryChange);
    }

    private void UpdateInventoryUI(List<Item> items)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (i < items.Count)
            {
                slots[i].UpdateUISlot(items[i]);
            }
            else
            {
                slots[i].UpdateUISlot(null);
            }
        }
    }
}
