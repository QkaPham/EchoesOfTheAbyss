using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsView : BaseView
{
    public string str;
    public override View viewName => View.Settings;

    public override void Activate(Action onComplete = null)
    {
        base.Activate(onComplete);
        UIManager.Instance.ActiveDepthOfField(true);
        AudioManager.Instance.PlaySFX(str);
    }

    public override void DeActivate(Action onComplete = null)
    {
        base.DeActivate(onComplete);
        UIManager.Instance.ActiveDepthOfField(false);
        AudioManager.Instance.PlaySFX(str);
    }
}
