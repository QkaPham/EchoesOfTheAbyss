using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public abstract class BasePanel : MonoBehaviour
{
    [SerializeField]
    protected GameObject firstSelectedGameObject;
    public bool isActive;
    protected CanvasGroup canvasGroup;

    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public virtual void Activate(bool active, float delay = 0f)
    {
        StartCoroutine(DelayActivate(active, delay));
    }

    protected virtual IEnumerator DelayActivate(bool active, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (active)
        {
            isActive = true;
            EventSystem.current.SetSelectedGameObject(firstSelectedGameObject);
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            isActive = false;
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
}
