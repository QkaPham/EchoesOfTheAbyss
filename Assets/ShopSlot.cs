using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    public Shop shop;
    public int slotIndex;

    public TextMeshProUGUI itemNameText;
    public Image iconImage;
    public Image itemBackGround;
    public GameObject itemRarityStar;
    public TextMeshProUGUI itemStatsText;
    public TextMeshProUGUI itemStatsValueText;
    public TextMeshProUGUI priceText;
    public Button buyButton;

    private void Awake()
    {
        shop = GetComponentInParent<Shop>();
        slotIndex = transform.GetSiblingIndex();
        buyButton = GetComponentInChildren<Button>();
        buyButton.onClick.AddListener(OnBuyButtonClick);
    }

    public void UpdateShopSlot(Item item)
    {
        if (item != null)
        {
            iconImage.color = Color.white;
            buyButton.gameObject.SetActive(true);


            itemNameText.text = item.profile.itemName;
            iconImage.sprite = item.profile.icon;
            itemBackGround.color = item.backGroundColor;
            ShowStar(item.Rarity);
            itemStatsText.text = item.ModifierType();
            itemStatsValueText.text = item.ModifierValue();
            priceText.text = item.price.ToString();
        }
        else
        {
            itemNameText.text = " ";
            iconImage.color = Color.clear;
            itemBackGround.color = Color.clear;

            ShowStar(0);
            itemStatsText.text = " ";
            itemStatsValueText.text = " ";
            buyButton.gameObject.SetActive(false);
        }
    }

    private void OnBuyButtonClick()
    {
        shop.Buy(slotIndex);
    }

    private void ShowStar(int number)
    {
        int childCount = itemRarityStar.transform.childCount;
        //Debug.Log(childCount);
        for (int i = 0; i < childCount; i++)
        {
            //Debug.Log(itemRarityStar.transform.GetChild(i).name);
            if (i < childCount - number)
            {
                itemRarityStar.transform.GetChild(i).gameObject.SetActive(false);
                // gameObject.SetActive(false);
            }
            else
            {
                itemRarityStar.transform.GetChild(i).gameObject.SetActive(true);
                // gameObject.SetActive(true);
            }
        }
    }
}
