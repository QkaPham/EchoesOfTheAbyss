using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : BasePanel
{
    [SerializeField] protected Button retryButton;
    [SerializeField] protected Button mainmenuButton;

    private void Start()
    {
        retryButton.onClick.AddListener(OnRetryButtonClick);
        mainmenuButton.onClick.AddListener(OnMainMenuButtonClick);
    }

    public void OnRetryButtonClick()
    {
        GameManager.Instance.RetryGame();
    }

    public void OnMainMenuButtonClick()
    {
        GameManager.Instance.ReturnToMainMenu();
    }
}
