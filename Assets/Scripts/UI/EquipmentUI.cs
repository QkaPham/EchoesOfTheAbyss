using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class EquipmentUI : MonoBehaviour
{
    [SerializeField] private ItemDetailUI itemDetailUI;
    [SerializeField] private List<SlotUI> slots;
    private Action<Notify> OnEquipmentChange;
    public void Awake()
    {
        slots = new List<SlotUI>();
        slots.AddRange(GetComponentsInChildren<SlotUI>());
        foreach (var slot in slots)
        {
            slot.UpdateUISlot(null);
            slot.itemDetailUI = itemDetailUI;
        }

        OnEquipmentChange = thisNotify => { if (thisNotify is EquipmentChangeNotify notify) UpdateEquipmentUI(notify.items); };
    }

    private void OnEnable()
    {
        EventManager.AddListener(EventID.EquipmentChange, OnEquipmentChange);
    }

    private void UpdateEquipmentUI(List<Item> items)
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
