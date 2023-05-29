using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : SlotUI//, ISelectHandler
{
    [SerializeField] private Image Icon;
    [field: SerializeField] public Item Item { get; private set; }
    public override SlotType SlotType => SlotType.InventorySlot;
    //public static event Action<Item, SlotType> OnItemSlotSelect;
    protected override void Awake()
    {
        base.Awake();
    }
    public override void UpdateUISlot(Item item)
    {
        if (Icon != null)
        {
            if (item != null)
            {
                HasItem = true;
                Item = item;
                Icon.sprite = item.Icon;
            }
            else
            {
                HasItem = false;
                Item = null;
                Icon.sprite = GameResources.Instance.GetItem(ItemID.NoneItem).Icon;
            }
        }
    }

    //public void OnSelect(BaseEventData eventData)
    //{
    //    OnItemSlotSelect?.Invoke(Item, SlotType);
    //}
}
