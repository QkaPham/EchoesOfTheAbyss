using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum View
{
    None,
    MainMenu,
    Settings,
    Load,
    Game,
    Pause,
}

public abstract class BaseView : MonoBehaviour
{
    public abstract View viewName { get; }
    protected List<UIAnimate> UIAnimates;
    public bool isActive;
    protected SelectableContainer selectableContainer;
    public float defaultDuration = 1f;
    public float defaultDelay = 0f;
    protected virtual void Awake()
    {
        UIAnimates = GetComponentsInChildren<UIAnimate>().ToList();
        selectableContainer = GetComponentInChildren<SelectableContainer>();
    }

    public virtual void Activate(Action onComplete = null)
    {
        Activate(defaultDuration, defaultDelay, onComplete);
    }

    public virtual void DeActivate(Action onComplete = null)
    {
        DeActivate(defaultDuration, defaultDelay, onComplete);
    }

    public virtual void Activate(float duration, float delay = 0f, Action onComplete = null)
    {
        StartCoroutine(DelayActivate(duration, delay, onComplete));
    }

    public virtual void DeActivate(float duration, float delay = 0f, Action onComplete = null)
    {
        StartCoroutine(DelayDeActivate(duration, delay, onComplete));
    }

    protected virtual IEnumerator DelayActivate(float duration, float delay = 0, Action onComplete = null)
    {
        InputManager.Instance.EnableUIInput(false);

        yield return new WaitForSeconds(delay);
        foreach (var item in UIAnimates)
        {
            item.Activate(duration);
        }
        yield return new WaitForSeconds(duration);

        isActive = true;
        InputManager.Instance.EnableUIInput(true);
        if (selectableContainer != null) EventSystem.current.SetSelectedGameObject(selectableContainer.gameObject);

        onComplete?.Invoke();
    }

    protected virtual IEnumerator DelayDeActivate(float duration, float delay = 0, Action onComplete = null)
    {
        isActive = false;
        InputManager.Instance.EnableUIInput(false);
        EventSystem.current.SetSelectedGameObject(null);

        yield return new WaitForSeconds(delay);
        foreach (var item in UIAnimates)
        {
            item.Deactivate(duration);
        }
        yield return new WaitForSeconds(duration);

        InputManager.Instance.EnableUIInput(true);

        onComplete?.Invoke();
    }
}
