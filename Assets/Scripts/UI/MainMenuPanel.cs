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
    [SerializeField]
    private float titleDuration;

    [Header("Buttons")]
    [SerializeField]
    private CanvasGroup buttonsCanvasGroup;
    [SerializeField]
    private Vector3 moveDirection;
    [SerializeField]
    private float moveDuration;

    [SerializeField]
    protected Image image;

    protected override void Awake()
    {
        base.Awake();
        image.color = Color.black;
        image.DOFade(0, 3f);
    }

    private void Update()
    {
        if (Input.anyKeyDown && pressAnyKeyText.gameObject.activeSelf && isActive)
        {
            ShowMainMenu();
        }
    }

    public override void Activate(bool active, float delay = 0)
    {
        base.Activate(active, delay);
        if (active)
        {
            SetUp();
            MainMenuAnimate();
        }
    }


    public void OnStartGameButtonClick()
    {
        Activate(false);
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
        buttonsCanvasGroup.transform.DOMove(buttonsCanvasGroup.transform.position + moveDirection, moveDuration);
        buttonsCanvasGroup.DOFade(1f, moveDuration);
    }


    private void SetUp()
    {
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

    private void MainMenuAnimate()
    {
        Sequence seq = DOTween.Sequence();
        seq.SetDelay(.8f);
        seq.Append(titleText.transform.DOMove(titleText.transform.position + titleMoveDirection, titleDuration));
        seq.Join(titleText.DOFade(1f, titleDuration));
        if (pressAnyKeyText.gameObject.activeSelf)
        {
            seq.OnComplete(() =>
                pressAnyKeyText.DOFade(0.8f, fadeTime).SetDelay(0.5f).OnComplete(() =>
                            pressAnyKeyText.DOFade(fadeValue, fadeTime).SetLoops(-1, LoopType.Yoyo)
                            )
            );
        }
        else
        {
            seq.Join(buttonsCanvasGroup.transform.DOMove(buttonsCanvasGroup.transform.position + moveDirection, moveDuration).SetDelay(0.3f));
            seq.Join(buttonsCanvasGroup.DOFade(1f, moveDuration).SetDelay(0.3f));
        }
    }
}
