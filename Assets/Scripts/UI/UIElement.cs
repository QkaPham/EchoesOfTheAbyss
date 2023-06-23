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
        AudioManager.Instance.PlaySFX("Select");
    }

    public void OnSubmit(BaseEventData eventData)
    {
        AudioManager.Instance.PlaySFX("Confirm");
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySFX("Confirm");
    }
}