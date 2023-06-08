using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;
using UnityEngine.EventSystems;
using DG.Tweening;
using Unity.VisualScripting;

public class PausePanel : BasePanel
{
    private float lastCancel;
    private void Update()
    {
        if (InputManager.Instance.Cancel)
        {
            if (!UIManager.Instance.GamePanel.isActive) return;
            if (UIManager.Instance.UpgradePanel.isActive) return;
            if (UIManager.Instance.SettingsPanel.isActive) return;
            if (Time.unscaledTime > lastCancel + 1.2f)
            {
                lastCancel = Time.unscaledTime;
                if (isActive)
                {

                    GameManager.Instance.Resume();
                }
                else
                {
                    GameManager.Instance.Pause();
                }
            }
        }
    }
    protected override IEnumerator DelayActivate(bool active, float delay)
    {
        if (active)
        {
            EventSystem.current.SetSelectedGameObject(firstSelectedGameObject);
            canvasGroup.DOFade(1, 1).SetUpdate(true).OnComplete(() =>
            {

                isActive = true;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
            );
        }
        else
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.DOFade(0, 0.5f).SetUpdate(true).OnComplete(() =>
            {
                isActive = false;
            });
        }
        yield return null;
    }

    public void OnResumeButtonClick()
    {
        GameManager.Instance.Resume();
    }

    public void OnSettingsButtonClick()
    {
        Activate(false);
        UIManager.Instance.SettingsPanel.ActivateByPanel(this);
        UIManager.Instance.ActiveDepthOfField(true);
    }

    public void OnMainMenuButtonClick()
    {
        Activate(false);
       //canvasGroup.interactable = false;
        GameManager.Instance.ReturnToMainMenu();
    }

    public void OnQuitButtonClick()
    {
        GameManager.Instance.Quit();
    }
}