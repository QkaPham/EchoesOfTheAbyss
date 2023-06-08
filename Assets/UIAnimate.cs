using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum AnimationType
{
    Slide,
    Fade,
    Zoom,

}


public class UIAnimate : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public RectTransform rectTransform;

    public AnimationType type;

    public Tween tween;
    public Vector3 originalPosition;
    public Vector3 offPosition;

    protected virtual void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.position;
        offPosition = rectTransform.position + 1000 * Vector3.down;
        canvasGroup = GetComponent<CanvasGroup>();
        Deactivate(0f);
    }

    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Activate(1f);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            Deactivate(1f);
        }
    }

    public bool receiveInput = true;
    public float Duration { get; set; }

    protected virtual void Activate(float duration)
    {
        if (!receiveInput) return;
        receiveInput = false;

        PreActivate();
        TurnOnAnimation(duration, PostActivate);
    }

    protected virtual void Deactivate(float duration)
    {
        if (!receiveInput) return;
        receiveInput = false;

        PreDeactivate();

        tween = rectTransform.DOMove(offPosition, duration).OnComplete(PostDeactivate);
    }

    protected virtual void PreActivate()
    {
        tween.Kill();
        canvasGroup.alpha = 1f;
    }

    protected virtual void PreDeactivate()
    {
        tween.Kill();
        canvasGroup.interactable = false;
    }

    protected virtual void PostActivate()
    {
        receiveInput = true;
        canvasGroup.interactable = true;
    }

    protected virtual void PostDeactivate()
    {
        receiveInput = true;
        canvasGroup.alpha = 0f;
    }

    protected virtual void TurnOnAnimation(float duration, TweenCallback onComplete)
    {
        tween = rectTransform.DOMove(originalPosition, duration).OnComplete(onComplete);
    }
}
