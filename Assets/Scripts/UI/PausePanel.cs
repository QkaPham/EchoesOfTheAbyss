using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;
using UnityEngine.EventSystems;

public class PausePanel : BasePanel
{
    protected override IEnumerator DelayActivate(bool active, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (active)
        {
            isActive = true;
            EventSystem.current.SetSelectedGameObject(firstSelectedGameObject);
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            isActive = false;
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
    public void OnResumeButtonClick()
    {
        StartCoroutine(GameManager.Instance.Resume());
    }

    public void OnSettingsButtonClick()
    {
        Activate(false);
        UIManager.Instance.SettingsPanel.ActivateByPanel(this);
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