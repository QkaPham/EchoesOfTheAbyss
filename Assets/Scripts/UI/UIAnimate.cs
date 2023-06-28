using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public enum AnimationType
{
    Slide,
    Zoom,
    Fade,
    FadeSlide
}

public enum AnimationDirection
{
    None,
    Up,
    Down,
    Left,
    Right
}

[RequireComponent(typeof(CanvasGroup))]
public class UIAnimate : MonoBehaviour
{
    protected CanvasGroup canvasGroup;
    protected RectTransform rectTransform;

    public AnimationType type;
    public AnimationDirection direction;

    [Range(0f, 1f)]
    public float slideDistance = 1;


    protected Vector3 startPosition;
    protected float startSolution;

    protected Vector3 activePosition => startPosition * Screen.width / startSolution;
    protected Vector3 deactivePosition => CalculateDeactivePosition(direction);
    protected bool isActive;

    protected virtual void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        startPosition = transform.position;
        startSolution = Screen.width;
        canvasGroup = GetComponent<CanvasGroup>();
        Deactivate(0f);
    }

    public virtual void Activate(float duration)
    {
        Activate(duration, 0f);
    }

    public virtual void Deactivate(float duration)
    {
        Deactivate(duration, 0f);
    }

    public virtual void Activate(float duration, float delay = 0f)
    {
        PreActivate();
        switch (type)
        {
            case AnimationType.Slide:
                SlideIn(duration, delay, PostActivate);
                break;
            case AnimationType.Zoom:
                ZoomIn(duration, delay, PostActivate);
                break;
            case AnimationType.Fade:
                FadeIn(duration, delay, PostActivate);
                break;
            case AnimationType.FadeSlide:
                FadeSlideIn(duration, delay, PostActivate);
                break;
            default:
                break;
        }
    }

    public virtual void Deactivate(float duration, float delay = 0f)
    {
        PreDeactivate();
        switch (type)
        {
            case AnimationType.Slide:
                SlideOut(duration, delay, PostDeactivate);
                break;
            case AnimationType.Zoom:
                ZoomOut(duration, delay, PostDeactivate);
                break;
            case AnimationType.Fade:
                FadeOut(duration, delay, PostDeactivate);
                break;
            case AnimationType.FadeSlide:
                FadeSlideOut(duration, delay, PostDeactivate);
                break;
            default:
                break;
        }
    }

    protected virtual void PreActivate()
    {
        if (type != AnimationType.Fade && type != AnimationType.FadeSlide)
        {
            canvasGroup.alpha = 1f;
        }
    }

    protected virtual void PreDeactivate()
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        isActive = false;
    }

    protected virtual void PostActivate()
    {
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        isActive = true;
    }

    protected virtual void PostDeactivate()
    {
        if (type != AnimationType.Fade && type != AnimationType.FadeSlide)
        {
            canvasGroup.alpha = 0f;
        }
    }

    protected virtual void SlideIn(float duration, float delay = 0f, Action onComplete = null)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(rectTransform.DOMove(activePosition, duration).SetDelay(delay));
        sequence.SetUpdate(true);
        sequence.OnComplete(() => onComplete());
    }
    protected virtual void SlideOut(float duration, float delay = 0f, Action onComplete = null)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(rectTransform.DOMove(deactivePosition, duration).SetDelay(delay));
        sequence.SetUpdate(true);
        sequence.OnComplete(() => onComplete());
    }

    protected virtual void ZoomIn(float duration, float delay = 0f, Action onComplete = null)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(rectTransform.DOScale(1f, duration).SetDelay(delay));
        sequence.SetUpdate(true);
        sequence.OnComplete(() => onComplete());
    }

    protected virtual void ZoomOut(float duration, float delay = 0f, Action onComplete = null)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(rectTransform.DOScale(0f, duration).SetDelay(delay));
        sequence.SetUpdate(true);
        sequence.OnComplete(() => onComplete());
    }

    protected virtual void FadeIn(float duration, float delay = 0f, Action onComplete = null)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(canvasGroup.DOFade(1, duration).SetEase(Ease.OutExpo).SetDelay(delay));
        sequence.SetUpdate(true);
        sequence.OnComplete(() => onComplete());
    }

    protected virtual void FadeOut(float duration, float delay = 0f, Action onComplete = null)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(canvasGroup.DOFade(0, duration).SetEase(Ease.InExpo).SetDelay(delay));
        sequence.SetUpdate(true);
        sequence.OnComplete(() => onComplete());
    }

    protected virtual void FadeSlideIn(float duration, float delay = 0f, Action onComplete = null)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(canvasGroup.DOFade(1f, duration).SetEase(Ease.OutExpo).SetDelay(delay));
        sequence.Join(rectTransform.DOMove(activePosition, duration).SetDelay(delay));
        sequence.SetUpdate(true);
        sequence.OnComplete(() => onComplete());
    }
    protected virtual void FadeSlideOut(float duration, float delay = 0f, Action onComplete = null)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(canvasGroup.DOFade(0, duration).SetEase(Ease.InExpo).SetDelay(delay));
        sequence.Join(rectTransform.DOMove(deactivePosition, duration).SetDelay(delay));
        sequence.SetUpdate(true);
        sequence.OnComplete(() => onComplete());
    }

    protected virtual Vector3 CalculateDeactivePosition(AnimationDirection direction)
    {
        switch (direction)
        {
            case AnimationDirection.None:
                return activePosition;
            case AnimationDirection.Up:
                return activePosition + Screen.height * Vector3.down * slideDistance;
            case AnimationDirection.Down:
                return activePosition + Screen.height * Vector3.up * slideDistance;
            case AnimationDirection.Left:
                return activePosition + Screen.width * Vector3.right * slideDistance;
            case AnimationDirection.Right:
                return activePosition + Screen.width * Vector3.left * slideDistance;
            default:
                return activePosition;
        }
    }
}
