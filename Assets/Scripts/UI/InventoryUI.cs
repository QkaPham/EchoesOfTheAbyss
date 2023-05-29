using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] public List<InventorySlotUI> InventorySlots;
    public void Awake()
    {
        foreach (Transform child in transform)
        {
            InventorySlots.Add(child.GetComponent<InventorySlotUI>());
        }
    }
    public void OnEnable()
    {
       Inventory.OnInventoryChange += UpdateInventoryUI;
    }
    private void UpdateInventoryUI(Item item, int slot)
    {
        InventorySlots[slot].UpdateUISlot(item);
    }
}
