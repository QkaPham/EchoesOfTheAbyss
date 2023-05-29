using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPanel : BasePanel
{
    protected void Start()
    {
        Activate(true);
    }

    public void OnStartGameButtonClick()
    {
        GameManager.Instance.StartGame();
        //UIManager.Instance.LoadingPanel.gameObject.SetActive(true);
        //UIManager.Instance.LoadingPanel.Activate();
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
}
