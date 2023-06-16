using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    public Shop shop;
    public int slotIndex;
    public Item item;

    public TextMeshProUGUI itemNameText;

    public Image itemBackGround;
    public Image itemLight;
    public Image iconImage;
    public GameObject stars;

    public TextMeshProUGUI itemStatsText;
    public TextMeshProUGUI itemStatsValueText;

    public TextMeshProUGUI priceText;
    public Button buyButton;

    public List<Color> backGroundColor;
    public List<Color> lightColor;

    private void Awake()
    {
        shop = GetComponentInParent<Shop>();
        slotIndex = transform.GetSiblingIndex();
        buyButton = GetComponentInChildren<Button>();
        buyButton.onClick.AddListener(OnBuyButtonClick);
    }

    public void UpdateShopSlot(Item item)
    {
        this.item = item;
        if (item != null)
        {
            iconImage.color = Color.white;
            buyButton.gameObject.SetActive(true);


            itemNameText.text = item.profile.itemName;
            iconImage.sprite = item.profile.icon;
            itemBackGround.color = backGroundColor[item.Rarity - 1];
            itemLight.color = lightColor[item.Rarity - 1];
            ShowStar(item.Rarity);
            itemStatsText.text = item.ModifierStat();
            itemStatsValueText.text = item.ModifierValue();
            priceText.text = item.price.ToString();
        }
        else
        {
            itemNameText.text = "";

            iconImage.color = Color.clear;
            itemBackGround.color = Color.clear;
            itemLight.color = Color.clear;

            ShowStar(0);
            itemStatsText.text = "";
            itemStatsValueText.text = "";
            buyButton.gameObject.SetActive(false);
        }
    }

    private void OnBuyButtonClick()
    {
        shop.Buy(slotIndex);
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
}
