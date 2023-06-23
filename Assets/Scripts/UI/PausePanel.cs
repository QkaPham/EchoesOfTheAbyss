using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : BasePanel
{
    [SerializeField] protected Button resumeButton;
    [SerializeField] protected Button settingsButton;
    [SerializeField] protected Button mainmenuButton;
    [SerializeField] protected Button quitButton;

    private void Start()
    {
        resumeButton.onClick.AddListener(OnResumeButtonClick);
        settingsButton.onClick.AddListener(OnSettingsButtonClick);
        mainmenuButton.onClick.AddListener(OnMainMenuButtonClick);
        quitButton.onClick.AddListener(OnQuitButtonClick);
    }

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