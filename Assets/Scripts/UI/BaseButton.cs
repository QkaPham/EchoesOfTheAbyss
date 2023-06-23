using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public class BaseButton : MonoBehaviour,
    IPointerEnterHandler,
    ISelectHandler, IDeselectHandler,
    IPointerDownHandler, IPointerUpHandler,
    IPointerClickHandler, ISubmitHandler
{
    [SerializeField] protected Graphic graphic;
    [SerializeField] protected Color normalColor = Color.white;
    [SerializeField] protected Color selectColor = new Color(94 / 255f, 224 / 255f, 221 / 255f, 255 / 255f);
    [SerializeField] protected Color submitColor = new Color(64 / 255f, 154 / 255f, 151 / 255f, 255 / 255f);
    [SerializeField] protected float fadeTime = 0.1f;

    [SerializeField] protected string selectSFX = "Select";
    [SerializeField] protected string submitSFX = "Confirm";

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public virtual void OnSelect(BaseEventData eventData)
    {
        AudioManager.Instance.PlaySFX(selectSFX);
        graphic.DOColor(selectColor, fadeTime);
    }

    public virtual void OnDeselect(BaseEventData eventData)
    {
        graphic.DOColor(normalColor, fadeTime);
    }

    public virtual void OnSubmit(BaseEventData eventData)
    {
        graphic.DOColor(submitColor, fadeTime).OnComplete(() => graphic.DOColor(selectColor, fadeTime));
        Submit();
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        Submit();
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        graphic.DOColor(submitColor, fadeTime);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            graphic.DOColor(selectColor, fadeTime);
        }
    }

    protected virtual void Submit()
    {
        AudioManager.Instance.PlaySFX(submitSFX);
    }
}
