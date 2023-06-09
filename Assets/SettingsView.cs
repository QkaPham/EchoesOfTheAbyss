using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsView : BaseView
{
    public override View viewName => View.Settings;

    public override void Activate(Action onComplete = null)
    {
        base.Activate(onComplete);
        UIManager.Instance.ActiveDepthOfField(true);
    }

    public override void DeActivate(Action onComplete = null)
    {
        base.DeActivate(onComplete);
        UIManager.Instance.ActiveDepthOfField(false);
    }
}
