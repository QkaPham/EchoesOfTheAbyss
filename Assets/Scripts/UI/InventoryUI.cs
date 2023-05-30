using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    public List<InventorySlotUI> InventorySlots;

    [SerializeField]
    private InventorySlotUI inventorySlotPrefabs;

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

    private void OnDisable()
    {
        Inventory.OnInventoryChange -= UpdateInventoryUI;
    }
    private void UpdateInventoryUI(Item item, int slot)
    {
        //if(item==null) return;
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
