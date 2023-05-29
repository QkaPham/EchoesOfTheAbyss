using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetailUI : MonoBehaviour
{
    [SerializeField] Image ItemImage;
    [SerializeField] TextMeshProUGUI ItemName;
    [SerializeField] TextMeshProUGUI ItemDescription;
    [SerializeField] Button EquipButton;
    [SerializeField] Button UnequipButton;
    [SerializeField] Button RecycleButton;

    public void UpdateItemDetailUI(Item item, SlotType slotType)
    {
        bool isEquipmentSlot = slotType == SlotType.EquipmentSlot;
        bool isInventorySlot = slotType == SlotType.InventorySlot;
        bool hasItem = item != null;

        ItemImage.sprite = hasItem ? item.Icon : GameResources.Instance.GetItem(ItemID.NoneItem).Icon;
        ItemName.text = hasItem ? item.Name : "";
        ItemDescription.text = hasItem ? string.Join("\n", item.Modifiers) : "";

        EquipButton.gameObject.SetActive(hasItem && isInventorySlot);
        UnequipButton.gameObject.SetActive(hasItem && isEquipmentSlot);
        RecycleButton.gameObject.SetActive(hasItem);
    }
}
