using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : BasePanel
{
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
