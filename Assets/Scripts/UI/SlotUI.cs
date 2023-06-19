using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour//, ISelectHandler
{
    [SerializeField] private Image Icon;
    [SerializeField] private Image backGround;
    [SerializeField] private Image lightImage;
    [SerializeField] private Image border;
    [SerializeField] private ItemStars stars;
    [SerializeField] private RarityColor colors;
    [HideInInspector] public ItemDetailUI itemDetailUI;
    private Item item;
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
            backGround.color = colors.DarkColor(item.Rarity);
            lightImage.color = colors.LightColor(item.Rarity);
        }
        else
        {
            Icon.color = Color.clear;
            stars.ShowStar(0);
            backGround.color = Color.black;
            lightImage.color = Color.clear;
        }
    }

    //public void OnSelect(BaseEventData eventData)
    //{
    //    Debug.Log("Select");
    //    //itemDetailUI.UpdateItemDetailUI(Item);
    //}
}
