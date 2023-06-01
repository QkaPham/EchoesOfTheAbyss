using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class FadePanel : MonoBehaviour
{
    private Image imgFade;
    [SerializeField]
    private Color fadeColor;

    private void Awake()
    {
        imgFade = GetComponent<Image>();
        imgFade.color = fadeColor;
    }

    public void FadeOut(float value, float fadeTime, Action onFinish)
    {
        if (Time.timeScale != 1) Time.timeScale = 1;
        SetAlpha(0);
        Sequence seq = DOTween.Sequence();
        seq.Append(this.imgFade.DOFade(value, fadeTime));
        //seq.Append(this.imgFade.DOFade(0, fadeTime));
        seq.OnComplete(() =>
        {
            onFinish?.Invoke();
        });
    }

    private void SetAlpha(float alp)
    {
        Color cl = this.imgFade.color;
        cl.a = alp;
        this.imgFade.color = cl;
    }
}
