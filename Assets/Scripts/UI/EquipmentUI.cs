using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class EquipmentUI : MonoBehaviour
{
    [SerializeField] private ItemDetailUI itemDetailUI;
    [SerializeField] private List<ItemSlotUI> slots;
    private Action<Notify> OnEquipmentChange, OnLevelChange, OnStartGame;
    public void Awake()
    {
        slots = new List<ItemSlotUI>();
        slots.AddRange(GetComponentsInChildren<ItemSlotUI>());
        foreach (var slot in slots)
        {
            slot.UpdateUISlot(null);
            slot.itemDetailUI = itemDetailUI;
        }

        OnEquipmentChange = thisNotify => { if (thisNotify is EquipmentChangeNotify notify) UpdateEquipmentUI(notify.items); };
        OnLevelChange = thisNotify =>
        {
            if (thisNotify is LevelChangeNotify notify)
            {
                slots[notify.level - 1].Unclock();
            }
        };

        OnStartGame = thisNotify => Init();
    }

    private void OnEnable()
    {
        EventManager.Instance.AddListener(EventID.EquipmentChange, OnEquipmentChange);
        EventManager.Instance.AddListener(EventID.LevelChange, OnLevelChange);
        EventManager.Instance.AddListener(EventID.StartGame, OnStartGame);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(EventID.EquipmentChange, OnEquipmentChange);
        EventManager.Instance.RemoveListener(EventID.LevelChange, OnLevelChange);
        EventManager.Instance.RemoveListener(EventID.StartGame, OnStartGame);
    }

    private void Init()
    {
        foreach (var slot in slots)
        {
            slot.UpdateUISlot(null);
            slot.itemDetailUI = itemDetailUI;
            slot.Clock();
        }

        slots[0].Unclock();
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
