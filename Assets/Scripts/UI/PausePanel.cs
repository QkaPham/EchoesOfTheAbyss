using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;
using UnityEngine.EventSystems;
using DG.Tweening;
using Unity.VisualScripting;

public class PausePanel : BasePanel
{
    private void Update()
    {
        if (InputManager.Instance.Cancel)
        {
            if (UIManager.Instance.CompareCurrentView(View.Pause))
            {
                OnResumeButtonClick();
            }
        }
    }

    public void OnResumeButtonClick()
    {
        GameManager.Instance.Resume();
    }

    public void OnSettingsButtonClick()
    {
        UIManager.Instance.Show(View.Settings);
        UIManager.Instance.ActiveDepthOfField(true);
    }

    public void OnMainMenuButtonClick()
    {
        GameManager.Instance.ReturnToMainMenu();
    }

    public void OnQuitButtonClick()
    {
        GameManager.Instance.Quit();
    }
}