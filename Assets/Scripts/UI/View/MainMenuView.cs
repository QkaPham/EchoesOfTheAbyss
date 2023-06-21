using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainMenuView : BaseView
{
    [SerializeField] protected UIAnimate buttons;
    [SerializeField] protected UIAnimate title;
    [SerializeField] protected UIAnimate pressAnykey;

    public override View viewName => View.MainMenu;

    protected override void Awake()
    {
        UIAnimates = new List<UIAnimate>
        {
            GetComponent<UIAnimate>(),
            title
        };
        selectableContainer = GetComponentInChildren<SelectableContainer>();
    }

    public override void Activate(Action onComplete = null)
    {
        base.Activate(onComplete);
        ShowPressAnykey();
    }

    protected virtual void Update()
    {
        if (Input.anyKeyDown)
        {
            if (pressAnykey != null)
            {
                Destroy(pressAnykey.gameObject);
                buttons.Activate(1f);
                UIAnimates.Add(buttons);
            }
        }
    }

    private void ShowPressAnykey()
    {
        if (pressAnykey != null)
        {
            pressAnykey.Activate(.5f, 1f);
        }
    }
}
