using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using static UnityEngine.Rendering.DebugUI;

public class FadePanel : MonoBehaviour
{
    private Image imgFade;
    [SerializeField]
    private Color fadeColor;

    private Tween tween;

    private void Awake()
    {
        imgFade = GetComponent<Image>();
        imgFade.color = fadeColor;
        SetAlpha(0);
    }

    public void Fade(float value, float duration)
    {
        tween.Kill();
        tween = imgFade.DOFade(value, duration);
    }

    public void FadeIn(float duration)
    {
        Fade(0f, duration);
        tween.SetEase(Ease.InExpo);
    }

    public void FadeOut(float duration)
    {
        Fade(0f, duration);
        tween.SetEase(Ease.OutExpo);
    }

    private void SetAlpha(float alp)
    {
        Color cl = this.imgFade.color;
        cl.a = alp;
        this.imgFade.color = cl;
    }
}
