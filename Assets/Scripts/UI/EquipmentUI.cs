using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
    [SerializeField] public List<EquipmentSlotUI> EquipmentSlots;
    public void Awake()
    {
        foreach (Transform child in transform)
        {
            EquipmentSlots.Add(child.GetComponent<EquipmentSlotUI>());
        }
    }
    private void OnEnable()
    {
        Equipment.OnEquipmentChange += UpdateEquipmentUI;
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
