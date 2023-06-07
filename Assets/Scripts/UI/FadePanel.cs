using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadePanel : MonoBehaviour
{
    private Image imgFade;
    [SerializeField]
    private Color fadeColor;

    private void Awake()
    {
        imgFade = GetComponent<Image>();
        imgFade.color = fadeColor;
        SetAlpha(0);
    }

    public void Fade(float value, float fadeTime)
    {
        if (Time.timeScale != 1) Time.timeScale = 1;
        imgFade.DOFade(value, fadeTime);
    }

    private void SetAlpha(float alp)
    {
        Color cl = this.imgFade.color;
        cl.a = alp;
        this.imgFade.color = cl;
    }
}