using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Search;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MainMenuPanel : BasePanel
{
    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {

    }

    protected override void Animation(bool active, float delay)
    {
        
    }

    public void OnStartGameButtonClick()
    {
        UIManager.Instance.LoadScene("GameLevel", View.Game);
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
