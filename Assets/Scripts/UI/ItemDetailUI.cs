using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetailUI : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private Image itemBackGround;
    [SerializeField] private Image itemLight;
    [SerializeField] private RarityColor colors;

    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI modifierStatsText;
    [SerializeField] private TextMeshProUGUI modifierValuesText;
    [SerializeField] private ItemStars stars;

    [SerializeField] private Button equipButton;
    [SerializeField] private Button unequipButton;
    [SerializeField] private Button recycleButton;

    private void Awake()
    {
        UpdateItemDetailUI(null, false);
    }

    public void UpdateItemDetailUI(Item item, bool isEquip)
    {
        if (item != null)
        {
            itemName.text = item.profile.itemName;
            stars.ShowStar(item.Rarity);

            itemIcon.color = Color.white;
            itemIcon.sprite = item.profile.icon;
            itemBackGround.color = colors.DarkColor(item.Rarity);
            itemLight.color = colors.LightColor(item.Rarity);

            modifierStatsText.text = item.ModifierStat();
            modifierValuesText.text = item.ModifierValue();

            equipButton.gameObject.SetActive(!isEquip);
            unequipButton.gameObject.SetActive(isEquip);
            recycleButton.gameObject.SetActive(true);

        }
        else
        {
            itemName.text = "";
            stars.ShowStar(0);

            itemIcon.color = Color.clear;
            itemBackGround.color = Color.clear;
            itemLight.color = Color.clear;

            modifierStatsText.text = "";
            modifierValuesText.text = "";

            equipButton.gameObject.SetActive(false);
            unequipButton.gameObject.SetActive(false);
            recycleButton.gameObject.SetActive(false);
        }
    }

}
