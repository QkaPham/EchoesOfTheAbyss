using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    public int slotIndex;
    public Item item;

    public TextMeshProUGUI itemNameText;

    public Image itemBackGround;
    public Image itemLight;
    public Image iconImage;
    public GameObject stars;
    public Image border;
    public GameObject require;

    public TextMeshProUGUI itemStatsText;
    public TextMeshProUGUI itemStatsValueText;

    public TextMeshProUGUI priceText;
    public Button buyButton;
    public RarityColor colors;

    private void Awake()
    {
        slotIndex = transform.GetSiblingIndex();
        buyButton = GetComponent<Button>();
    }

    public void UpdateShopSlot(Item item)
    {
        this.item = item;
        if (item != null)
        {
            iconImage.color = Color.white;
            buyButton.interactable = true;


            itemNameText.text = item.profile.itemName;
            iconImage.sprite = item.profile.icon;
            itemBackGround.color = colors.DarkColor(item.Rarity);
            itemLight.color = colors.LightColor(item.Rarity);
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
            buyButton.interactable = false;
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
}
