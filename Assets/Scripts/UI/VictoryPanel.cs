using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryPanel : BasePanel
{
    public void OnNewGameButtonClick()
    {
        GameManager.Instance.StartGame();
        Activate(false);
    }

    public void OnMainMenuButtonClick()
    {
        GameManager.Instance.ReturnToMainMenu();
    }
}
