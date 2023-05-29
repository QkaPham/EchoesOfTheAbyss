using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIElement : MonoBehaviour, ISelectHandler, IPointerEnterHandler, ISubmitHandler, IPointerClickHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void OnSelect(BaseEventData eventData)
    {
        AudioManager.Instance.PlaySE("Select");
    }

    public void OnSubmit(BaseEventData eventData)
    {
        AudioManager.Instance.PlaySE("Confirm");
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySE("Confirm");
    }
}