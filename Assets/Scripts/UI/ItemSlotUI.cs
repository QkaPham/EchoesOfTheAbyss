using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour, ISelectHandler
{
    [SerializeField] private Image Icon;
    [SerializeField] private Image backGround;
    [SerializeField] private Image lightImage;
    [SerializeField] private Image border;
    [SerializeField] private ItemStars stars;
    [SerializeField] private Color lockedColor = new Color(35 / 255f, 35 / 255f, 35 / 255f, 1);
    [SerializeField] private Color unlockedColor = new Color(69 / 255f, 69 / 255f, 69 / 255f, 1);
    [HideInInspector] public ItemDetailUI itemDetailUI;
    private bool unlocked;
    public Item Item { get; private set; }
    private void Start()
    {
        Item = null;
    }
    public void UpdateUISlot(Item item)
    {
        this.Item = item;
        if (item != null)
        {
            Icon.color = Color.white;
            Icon.sprite = item.profile.icon;

            stars.ShowStar(item.Rarity);
            backGround.color = item.DarkColor;
            lightImage.color = item.LightColor;
        }
        else
        {
            Icon.color = Color.clear;
            stars.ShowStar(0);
            if (unlocked)
            {
                backGround.color = unlockedColor;
            }
            else
            {
                backGround.color = lockedColor;
            }
            lightImage.color = Color.clear;
        }
    }

    public void Unclock()
    {
        unlocked = true;
        backGround.color = unlockedColor;
    }

    public void OnSelect(BaseEventData eventData)
    {
        itemDetailUI.UpdateItemDetailUI(Item);
    }
}
