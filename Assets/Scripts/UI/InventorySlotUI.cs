using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : SlotUI
{
    //[SerializeField] private ItemDetailUI itemDetailUI;
    //[SerializeField] private Image Icon;
    //[SerializeField] private Image backGround;
    //[SerializeField] private Image lightImage;
    //[SerializeField] private Image border;
    //[SerializeField] private GameObject stars;
    //[SerializeField] private RarityColor colors;

    //public  void UpdateUISlot(Item item)
    //{
    //    //this.item = item;
    //    //if (item != null)
    //    //{
    //    //    Icon.color = Color.white;
    //    //    Icon.sprite = item.profile.icon;

    //    //    ShowStar(item.Rarity);
    //    //    backGround.color = colors.DarkColor(item.Rarity);
    //    //    lightImage.color = colors.LightColor(item.Rarity);
    //    //}
    //    //else
    //    //{
    //    //    Icon.color = Color.clear;
    //    //    ShowStar(0);
    //    //    backGround.color = Color.black;
    //    //    lightImage.color = Color.clear;

    //    //}
    //}

    //private void ShowStar(int number)
    //{
    //    int childCount = stars.transform.childCount;
    //    for (int i = 0; i < childCount; i++)
    //    {
    //        if (i < childCount - number)
    //        {
    //            stars.transform.GetChild(i).gameObject.SetActive(false);
    //        }
    //        else
    //        {
    //            stars.transform.GetChild(i).gameObject.SetActive(true);
    //        }
    //    }
    //}

    //public void OnSelect(BaseEventData eventData)
    //{
    //    itemDetailUI.UpdateItemDetailUI(item);
    //}
}
