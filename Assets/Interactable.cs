using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Selectable))]
[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(EventTrigger))]
public class Interactable : MonoBehaviour
{
    public bool IsInteractable { get; private set; }

    protected Image raycastTarget;
    protected Selectable navigation;
    protected CanvasGroup controller;
    protected EventTrigger eventsHandler;

    protected virtual void Awake()
    {
        raycastTarget = GetComponent<Image>();
        navigation = GetComponent<Selectable>();
        controller = GetComponent<CanvasGroup>();
        eventsHandler = GetComponent<EventTrigger>();

        raycastTarget.color = Color.clear;
        raycastTarget.raycastTarget = true;
    }

    public virtual void Enable()
    {
        controller.interactable = true;
        controller.blocksRaycasts = true;
    }

    public virtual void Disable()
    {
        controller.interactable = false;
        controller.blocksRaycasts = false;
    }

    public virtual void Visible()
    {
        controller.alpha = 1;
    }

    public virtual void Invisible()
    {
        controller.alpha = 0;
    }

    public void AddEvent(EventTriggerType eventType, Action listener)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventType;
        entry.callback.AddListener(data => listener?.Invoke());
        eventsHandler.triggers.Add(entry);
    }

}