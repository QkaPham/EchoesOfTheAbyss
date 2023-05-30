using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollViewAutoScroll : MonoBehaviour, IScrollHandler, IDragHandler
{
    protected Vector2 scrollViewSize;

    [SerializeField]
    protected RectTransform Content;
    protected float contentPositionY;
    protected GridLayoutGroup contentGridLayoutGroup;
    protected float cellHeight;


    protected GameObject previousSelect;
    protected GameObject select;

    // protected float height;
    protected float topOffset;
    protected float bottomOffset;

    protected bool startAutoScroll;
    protected float toValue;
    [SerializeField]
    protected float scrollSpeed = 0.1f;
    private void Awake()
    {
        scrollViewSize = GetComponent<RectTransform>().rect.size;
        contentGridLayoutGroup = Content.GetComponent<GridLayoutGroup>();
        topOffset = contentGridLayoutGroup.padding.top;
        bottomOffset = contentGridLayoutGroup.padding.bottom;
        cellHeight = contentGridLayoutGroup.cellSize.y;
    }


    private void Update()
    {
        select = EventSystem.current.currentSelectedGameObject;
        contentPositionY = Content.transform.localPosition.y;

        if (select != null && select.transform.IsChildOf(Content) && select != previousSelect)
        {
            previousSelect = select;

            RectTransform slot = select.GetComponent<RectTransform>();
            Vector2 size = slot.rect.size;
            Vector2 anchor = slot.anchoredPosition;
            Vector2 pivot = slot.pivot;


            float top = -(anchor.y + (1 - pivot.y) * size.y);
            float bottom = -(anchor.y - pivot.y * size.y);

            if (top < contentPositionY)
            {
                startAutoScroll = true;
                toValue = top - topOffset;
            }

            if (bottom - scrollViewSize.y > contentPositionY)
            {
                startAutoScroll = true;
                toValue = bottom - scrollViewSize.y + topOffset;
            }
        }


        if (startAutoScroll)
        {
            Scroll(toValue);
        }

        //Stop scroll when distance <= 1% of cell height
        if (Mathf.Abs(Content.transform.localPosition.y - toValue) <= cellHeight / 100)
        {
            Content.transform.localPosition = new Vector3(Content.transform.localPosition.x, toValue, 0f);
            startAutoScroll = false;
        }
    }

    public void Scroll(float toValue)
    {
        //Increase scroll speed if distance to scroll is high
        float distanceScroll = Mathf.Abs(Content.transform.localPosition.y - toValue);
        float minScroll = (cellHeight + contentGridLayoutGroup.spacing.y);
        float desireScroolSpeed = Mathf.Max(distanceScroll, minScroll) * scrollSpeed;

        float y = Mathf.Lerp(Content.transform.localPosition.y, toValue, Time.deltaTime * desireScroolSpeed);
        Content.transform.localPosition = new Vector3(Content.transform.localPosition.x, y, 0f);
    }

    public void OnScroll(PointerEventData eventData)
    {
        startAutoScroll = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        startAutoScroll = false;
    }
}
