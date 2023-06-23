using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropDownItemBehavior : MonoBehaviour,
    ISelectHandler, IPointerEnterHandler,
    ISubmitHandler, IPointerClickHandler
{
    [SerializeField] protected string selectSFX = "Select";
    [SerializeField] protected string submitSFX = "Confirm";

    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void OnSelect(BaseEventData eventData)
    {
        AudioManager.Instance.PlaySFX(selectSFX);
    }

    public void OnSubmit(BaseEventData eventData)
    {
       AudioManager.Instance.PlaySFX(submitSFX);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySFX(submitSFX);
    }
}
