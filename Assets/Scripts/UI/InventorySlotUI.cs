using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : SlotUI
{
    [SerializeField] private Image Icon;
    [field: SerializeField] public Item Item { get; private set; }
    public override SlotType SlotType => SlotType.Inventory;
    public override void UpdateUISlot(Item item)
    {
        if (Icon != null)
        {
            if (item != null)
            {
                HasItem = true;
                Item = item;
                Icon.sprite = item.profile.icon;
            }
            else
            {
                HasItem = false;
                Item = null;
                Icon.sprite = AssetLoader.Instance.GetItem(ItemID.NoneItem).icon;
            }
        }
    }
}
