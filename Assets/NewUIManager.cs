using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewUIManager : Singleton<NewUIManager>
{
    public MainMenuView mainMenuPanel;
    public SettingsView settingsPanel;

    protected override void Awake()
    {
        base.Awake();
        mainMenuPanel = GetComponentInChildren<MainMenuView>();
        settingsPanel = GetComponentInChildren<SettingsView>();
    }
}
