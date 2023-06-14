using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Search;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
