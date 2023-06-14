using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : BasePanel
{
    public void OnRetryButtonClick()
    {
        GameManager.Instance.RetryGame();
    }

    public void OnMainMenuButtonClick()
    {
        GameManager.Instance.ReturnToMainMenu();
    }
}
