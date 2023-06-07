using DG.Tweening;
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
        canvasGroup.alpha = 0f;
    }

    public virtual void Activate(bool active, float delay = 0.05f)
    {
        StartCoroutine(DelayActivate(active, delay));
    }

    protected virtual IEnumerator DelayActivate(bool active, float delay)
    {
        if (active)
        {
            //canvasGroup.alpha = 1;
            Animation(active, delay);

            yield return new WaitForSeconds(delay);
            EventSystem.current.SetSelectedGameObject(firstSelectedGameObject);
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;

            yield return new WaitForSeconds(0.5f);
            isActive = true;
        }
        else
        {
            Animation(active, delay);

            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
            isActive = false;
            yield return new WaitForSeconds(delay);
            //canvasGroup.alpha = 0;
        }
    }

    protected virtual void Animation(bool active, float delay)
    {
        if (active)
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(canvasGroup.DOFade(1, delay));
        }
        else
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(canvasGroup.DOFade(0, delay));
        }
    }
}
