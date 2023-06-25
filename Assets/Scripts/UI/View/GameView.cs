using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameView : BaseView
{
    public override View viewName => View.Game;
    public override void Activate(Action onComplete = null)
    {
        base.Activate(onComplete);
        InputManager.Instance.EnablePlayerInput(true);
    }

    public override void DeActivate(Action onComplete = null, bool playSFX = true)
    {
        base.DeActivate(onComplete, playSFX);
        InputManager.Instance.EnablePlayerInput(false);
    }
}
