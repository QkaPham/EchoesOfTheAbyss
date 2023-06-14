using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainMenuView : BaseView
{
    [SerializeField]
    protected UIAnimate buttons;
    [SerializeField]
    protected UIAnimate title;
    [SerializeField]
    protected UIAnimate pressAnykey;

    public override View viewName => View.MainMenu;

    protected override void Awake()
    {
        UIAnimates = new List<UIAnimate>();
        UIAnimates.Add(title);
        UIAnimates.Add(GetComponent<UIAnimate>());
        selectableContainer = GetComponentInChildren<SelectableContainer>();
    }

    private void Start()
    {
        Activate(1f, 0f, ShowPressAnykey);
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
            pressAnykey.Activate(.5f);
        }
    }
}
