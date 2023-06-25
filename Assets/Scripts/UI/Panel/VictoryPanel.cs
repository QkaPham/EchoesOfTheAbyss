using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryPanel : BasePanel
{
    [SerializeField] protected Button newGameButton;
    [SerializeField] protected Button mainMenuButton;

    private void Start()
    {
        newGameButton.onClick.AddListener(OnNewGameButtonClick);
        mainMenuButton.onClick.AddListener(OnMainMenuButtonClick);
    }

    public void OnNewGameButtonClick()
    {
        GameManager.Instance.RetryGame();
    }

    public void OnMainMenuButtonClick()
    {
        GameManager.Instance.ReturnToMainMenu();
    }
}
