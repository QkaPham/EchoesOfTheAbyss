using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Search;
using UnityEngine;
using DG.Tweening;

public class MainMenuPanel : BasePanel
{
    private bool showMainMenu;
    [SerializeField]
    private TextMeshProUGUI pressAnyKeyText;
    public float fadeValue;
    public float fadeTime;

    [SerializeField]
    private TextMeshProUGUI titleText;
    [SerializeField]
    private Vector3 titleMoveDirection;
    [SerializeField]
    private float titleDuration;


    [SerializeField]
    private CanvasGroup buttonsCanvasGroup;
    [SerializeField]
    private Vector3 moveDirection;
    [SerializeField]
    private float moveDuration;


    protected void Start()
    {
        titleText.transform.position -= titleMoveDirection;
        titleText.transform.DOMove(titleText.transform.position+titleMoveDirection, titleDuration);
        Color c = titleText.color;
        c.a = 0f;
        titleText.color = c;
        titleText.DOFade(1f, titleDuration);

        FlickeringPressAnyKeyText();
        Activate(true);
        buttonsCanvasGroup.transform.position = buttonsCanvasGroup.transform.position - moveDirection;
        buttonsCanvasGroup.alpha = 0;
    }

    private void Update()
    {
        if (Input.anyKeyDown && !showMainMenu && isActive)
        {
            ShowMainMenu();
        }
    }

    public override void Activate(bool active, float delay = 0)
    {
        base.Activate(active, delay);
    }


    public void OnStartGameButtonClick()
    {
        Activate(false);
        GameManager.Instance.StartGame();
        UIManager.Instance.LoadScene("GameLevel");
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
        showMainMenu = true;
        pressAnyKeyText.gameObject.SetActive(false);
        buttonsCanvasGroup.transform.DOMove(buttonsCanvasGroup.transform.position + moveDirection, moveDuration);
        buttonsCanvasGroup.DOFade(1f, moveDuration);
    }

    private void FlickeringPressAnyKeyText()
    {
        pressAnyKeyText.DOFade(fadeValue, fadeTime).SetLoops(-1, LoopType.Yoyo);
    }
}
