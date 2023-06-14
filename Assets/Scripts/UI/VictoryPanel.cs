using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryPanel : BasePanel
{
    public void OnNewGameButtonClick()
    {
        UIManager.Instance.ShowLast();
        //GameManager.Instance.StartGame();
    }

    public void OnMainMenuButtonClick()
    {
        GameManager.Instance.ReturnToMainMenu();
    }
}
