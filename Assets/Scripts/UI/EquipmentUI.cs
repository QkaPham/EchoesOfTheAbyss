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
        EquipmentSlots = new List<EquipmentSlotUI>();
        EquipmentSlots.AddRange(GetComponentsInChildren<EquipmentSlotUI>());

        foreach (EquipmentSlotUI slot in EquipmentSlots)
        {
            slot.UpdateUISlot(null);
        }



        OnEquipmentChange = thisNotify => { if (thisNotify is EquipmentChangeNotify notify) UpdateEquipmentUI(notify.items); };
    }

    private void OnEnable()
    {
        EventManager.AddListiener(EventID.EquipmentChange, OnEquipmentChange);
    }

    private void UpdateEquipmentUI(List<Item> items)
    {
        for (int i = 0; i < EquipmentSlots.Count; i++)
        {
            if (i < items.Count)
            {
                EquipmentSlots[i].UpdateUISlot(items[i]);
                //UpdateEquipmentUI(items[i], i);
            }
            else
            {
                EquipmentSlots[i].UpdateUISlot(null);
                //UpdateEquipmentUI(null, i);
            }
        }
    }
}
