using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : SlotUI, ISelectHandler
{
    [SerializeField] private ItemDetailUI itemDetailUI;
    [SerializeField] private Image Icon;
    [SerializeField] private Image backGround;
    [SerializeField] private Image lightImage;
    [SerializeField] private Image border;
    [SerializeField] private GameObject stars;
    [SerializeField] private RarityColor colors;

    [field: SerializeField] public Item Item { get; private set; }
    public override SlotType SlotType => SlotType.Inventory;


    public override void UpdateUISlot(Item item)
    {
        if (item != null)
        {
            HasItem = true;
            Item = item;

            Icon.color = Color.white;
            Icon.sprite = item.profile.icon;

            ShowStar(item.Rarity);
            backGround.color = colors.DarkColor(item.Rarity);
            lightImage.color = colors.LightColor(item.Rarity);
        }
        else
        {
            Item = null;
            HasItem = false;

            Icon.color = Color.clear;
            ShowStar(0);
            backGround.color = Color.black;
            lightImage.color = Color.clear;

        }
    }

    private void ShowStar(int number)
    {
        int childCount = stars.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            if (i < childCount - number)
            {
                stars.transform.GetChild(i).gameObject.SetActive(false);
            }
            else
            {
                stars.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        itemDetailUI.UpdateItemDetailUI(Item,false);
    }
}
