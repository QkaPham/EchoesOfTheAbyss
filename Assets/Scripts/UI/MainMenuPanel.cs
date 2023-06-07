using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Search;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MainMenuPanel : BasePanel
{
    [Header("Press any key")]
    [SerializeField]
    private TextMeshProUGUI pressAnyKeyText;
    [SerializeField]
    private float fadeValue;
    [SerializeField]
    private float fadeTime;

    [Header("Title")]
    [SerializeField]
    private TextMeshProUGUI titleText;
    [SerializeField]
    private Vector3 titleMoveDirection;

    [Header("Buttons")]
    [SerializeField]
    private CanvasGroup buttonsCanvasGroup;
    [SerializeField]
    private Vector3 moveDirection;

    [SerializeField]
    protected Image image;

    protected override void Awake()
    {
        base.Awake();
        image.color = Color.black;
        image.DOFade(0, 3f);
        titleText.transform.position -= titleMoveDirection;

        Color c = titleText.color;
        c.a = 0f;
        titleText.color = c;

        c = pressAnyKeyText.color;
        c.a = 0f;
        pressAnyKeyText.color = c;

        buttonsCanvasGroup.alpha = 0;
        buttonsCanvasGroup.transform.position = buttonsCanvasGroup.transform.position - moveDirection;
    }

    private void Update()
    {
        if (Input.anyKeyDown && pressAnyKeyText.gameObject.activeSelf)
        {
            ShowMainMenu();
        }
    }

    protected override void Animation(bool active, float delay)
    {
        if (active)
        {
            canvasGroup.alpha = 1;
            Sequence seq = DOTween.Sequence();
            float duration = 0.5f * delay;
            seq.Append(titleText.transform.DOMove(titleText.transform.position + titleMoveDirection, duration).SetDelay(delay - duration));
            seq.Join(titleText.DOFade(1f, delay));
            if (pressAnyKeyText.gameObject.activeSelf)
            {
                seq.OnComplete(() =>
                    pressAnyKeyText.DOFade(0.8f, fadeTime).SetDelay(0.5f).OnComplete(() =>
                                pressAnyKeyText.DOFade(fadeValue, fadeTime).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo)
                                )
                );
            }
            else
            {
                duration = 0.8f * delay;
                seq.Join(buttonsCanvasGroup.transform.DOMove(buttonsCanvasGroup.transform.position + moveDirection, duration).SetDelay(delay - duration));
                seq.Join(buttonsCanvasGroup.DOFade(1f, duration).SetDelay(delay - duration));
            }
        }
        else
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(buttonsCanvasGroup.transform.DOMove(buttonsCanvasGroup.transform.position - moveDirection, delay).SetEase(Ease.Linear));
            seq.Join(buttonsCanvasGroup.DOFade(0, delay).SetEase(Ease.OutExpo));
            seq.OnComplete(() =>
            {
                canvasGroup.alpha = 0;
                titleText.transform.position -= titleMoveDirection;

                Color c = titleText.color;
                c.a = 0f;
                titleText.color = c;

                c = pressAnyKeyText.color;
                c.a = 0f;
                pressAnyKeyText.color = c;

                //buttonsCanvasGroup.alpha = 0;
                //buttonsCanvasGroup.transform.position = buttonsCanvasGroup.transform.position - moveDirection;
            });
        }
    }

    public void OnStartGameButtonClick()
    {
        Activate(false, 2f);
        GameManager.Instance.StartGame();
    }

    public void OnSettingsButtonClick()
    {
        Activate(false);
        UIManager.Instance.ActiveDepthOfField(true);
        UIManager.Instance.SettingsPanel.ActivateByPanel(this);
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
    }

    private void ShowMainMenu()
    {
        pressAnyKeyText.gameObject.SetActive(false);
        buttonsCanvasGroup.transform.DOMove(buttonsCanvasGroup.transform.position + moveDirection, 1f);
        buttonsCanvasGroup.DOFade(1f, 1f);
    }
}
