using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropDownBehavior : MonoBehaviour,
    ISelectHandler, IDeselectHandler,
    IPointerEnterHandler,
    ISubmitHandler, IPointerClickHandler
{
    public bool isSelected;
    public bool isDroping;

    protected TMP_Dropdown dropdown;

    [SerializeField] protected string selectSFX = "Select";
    [SerializeField] protected string submitSFX = "Confirm";

    private void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        RectTransform template = dropdown.template;
        template.gameObject.SetActive(true);
        template.AddComponent<DropDownTemplateBehavior>();
        GameObject item = template.GetComponentInChildren<Toggle>().gameObject;
        item.AddComponent<DropDownItemBehavior>();
        template.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (!isSelected && !isDroping)
        {
            isSelected = true;
            AudioManager.Instance.PlaySFX(selectSFX);
        }
    }

    public void OnSubmit(BaseEventData eventData)
    {
        AudioManager.Instance.PlaySFX(submitSFX);
        GetComponentInChildren<DropDownTemplateBehavior>().DropDownBehavior = this;
        isDroping = true;
        isSelected = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySFX(submitSFX);
        GetComponentInChildren<DropDownTemplateBehavior>().DropDownBehavior = this;
        isDroping = true;
        isSelected = true;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        isSelected = false;
    }
}
