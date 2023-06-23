using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : BasePanel
{
    [SerializeField] protected Button StartGameButton;
    [SerializeField] protected Button SettingsButton;
    [SerializeField] protected Button QuitButton;

    private void Start()
    {
        StartGameButton.onClick.AddListener(OnStartGameButtonClick);
        SettingsButton.onClick.AddListener(OnSettingsButtonClick);
        QuitButton.onClick.AddListener(OnQuitButtonClick);
    }

    public void OnStartGameButtonClick()
    {
        GameManager.Instance.StartGame();
    }

    public void OnSettingsButtonClick()
    {
        UIManager.Instance.Show(View.Settings);
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
    }
}
