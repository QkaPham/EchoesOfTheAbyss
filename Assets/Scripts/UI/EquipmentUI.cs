using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
    [SerializeField] public List<EquipmentSlotUI> EquipmentSlots;
    private Action<Notify> OnEquipmentChange;
    public void Awake()
    {
        foreach (Transform child in transform)
        {
            EquipmentSlots.Add(child.GetComponent<EquipmentSlotUI>());
        }
        OnEquipmentChange = thisNotify => { if (thisNotify is EquipmentChangeNotify notify) UpdateEquipmentUI(notify.isEquip, notify.item, notify.slot); };
    }

    private void OnEnable()
    {
        EventManager.AddListiener(EventID.EquipmentChange, OnEquipmentChange);
    }

    private void UpdateEquipmentUI(bool isAddItem, Item item, int slot)
    {
        if (isAddItem)
        {
            EquipmentSlots[slot].UpdateUISlot(item);
        }
        else
        {
            EquipmentSlots[slot].UpdateUISlot(null);
        }
    }
}
