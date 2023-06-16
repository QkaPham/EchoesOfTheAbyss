using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    public List<InventorySlotUI> InventorySlots;

    [SerializeField]
    private InventorySlotUI inventorySlotPrefabs;

    private Action<Notify> OnInventoryChange;

    public void Awake()
    {
        InventorySlots = new List<InventorySlotUI>();
        InventorySlots.AddRange(GetComponentsInChildren<InventorySlotUI>());

        foreach(InventorySlotUI slot in InventorySlots)
        {
            slot.UpdateUISlot(null);
        }

        OnInventoryChange = thisNotify => { if (thisNotify is InventoryChangeNotify notify) UpdateInventoryUI(notify.items); };
    }

    private void Start()
    {
        EventManager.AddListiener(EventID.InventoryChange, OnInventoryChange);
    }

    private void UpdateInventoryUI(List<Item> items)
    {
        for (int i = 0; i < InventorySlots.Count; i++)
        {
            if (i < items.Count)
            {
                UpdateInventoryUI(items[i], i);
            }
            else
            {
                UpdateInventoryUI(null,i);
            }
        }
    }

    private void UpdateInventoryUI(Item item, int slot)
    {
        if (InventorySlots.Count - 1 >= slot)
        {
            InventorySlots[slot].UpdateUISlot(item);
        }
        else
        {
            InventorySlotUI newSlot = Instantiate(inventorySlotPrefabs, transform);
            newSlot.UpdateUISlot(item);
            InventorySlots.Add(newSlot);
        }
    }

}
