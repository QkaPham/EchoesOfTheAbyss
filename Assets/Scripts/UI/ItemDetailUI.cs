using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetailUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup contents;
    [SerializeField] private Image itemIcon;
    [SerializeField] private Image itemBackGround;
    [SerializeField] private Image itemLight;
    [SerializeField] private RarityColor colors;

    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI modifierStatsText;
    [SerializeField] private TextMeshProUGUI modifierValuesText;
    [SerializeField] private ItemStars stars;

    [SerializeField] private EquipButton equipButton;
    [SerializeField] private UnequipButton unequipButton;
    [SerializeField] private RecycleButton recycleButton;
    [SerializeField] private TextMeshProUGUI recyclePrice;


    private void Awake()
    {
        UpdateItemDetailUI(null);
    }

    public void UpdateItemDetailUI(Item item)
    {
        if (item != null)
        {
            contents.alpha = 1;
            itemName.text = item.profile?.itemName;
            stars.ShowStar(item.Rarity);

            itemIcon.color = Color.white;
            itemIcon.sprite = item.profile.icon;
            itemBackGround.color = colors.DarkColor(item.Rarity);
            itemLight.color = colors.LightColor(item.Rarity);

            modifierStatsText.text = item.ModifierStat();
            modifierValuesText.text = item.ModifierValue();

            equipButton.gameObject.SetActive(!item.isEquip);
            unequipButton.gameObject.SetActive(item.isEquip);
            recycleButton.gameObject.SetActive(true);
            recycleButton.SetPriceText(item.price);

        }
        else
        {
            contents.alpha = 0;
            //itemName.text = "";
            //stars.ShowStar(0);

            //itemIcon.color = Color.clear;
            //itemBackGround.color = Color.clear;
            //itemLight.color = Color.clear;

            //modifierStatsText.text = "";
            //modifierValuesText.text = "";

            //equipButton.gameObject.SetActive(false);
            //unequipButton.gameObject.SetActive(false);
            //recycleButton.gameObject.SetActive(false);
        }
    }

}
