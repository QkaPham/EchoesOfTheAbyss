using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : BasePanel
{
    public void OnRetryButtonClick()
    {
        Activate(false);
        GameManager.Instance.RetryGame();
    }

    public void OnMainMenuButtonClick()
    {
        Activate(false);
        GameManager.Instance.ReturnToMainMenu();
    }
}
