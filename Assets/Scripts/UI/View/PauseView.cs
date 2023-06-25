using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseView : BaseView
{
    public override View viewName => View.Pause;
    public override void Activate(float duration, float delay = 0, Action onComplete = null)
    {
        base.Activate(duration, delay, onComplete);
        UIManager.Instance.ActiveDepthOfField(true);
    }

    public override void DeActivate(Action onComplete = null, bool playSFX = true)
    {
        base.DeActivate(onComplete, playSFX);
        UIManager.Instance.ActiveDepthOfField(false);
    }
}
