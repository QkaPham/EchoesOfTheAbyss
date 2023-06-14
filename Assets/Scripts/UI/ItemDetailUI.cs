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
        bool isEquipmentSlot = slotType == SlotType.Equipment;
        bool isInventorySlot = slotType == SlotType.Inventory;
        bool hasItem = item != null;

        ItemImage.sprite = hasItem ? item.profile.icon : AssetLoader.Instance.GetItem(ItemID.NoneItem).icon;
        ItemName.text = hasItem ? item.profile.itemName : "";
        ItemDescription.text = hasItem ? string.Join("\n", item.modifiers) : "";

        EquipButton.gameObject.SetActive(hasItem && isInventorySlot);
        UnequipButton.gameObject.SetActive(hasItem && isEquipmentSlot);
        RecycleButton.gameObject.SetActive(hasItem);
    }
}
