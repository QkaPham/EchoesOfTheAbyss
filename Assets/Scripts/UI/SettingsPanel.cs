using System;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using UnityEngine.EventSystems;

public class SettingsPanel : BasePanel
{
    [SerializeField]
    private AudioMixer audioMixer;

    [SerializeField]
    private Slider MasterSlider;

    [SerializeField]
    private Slider MusicSlider;

    [SerializeField]
    private Slider SFXSlider;

    [SerializeField]
    private TMP_Dropdown ResolutionDropDown;

    [SerializeField]
    private Toggle FullScreenToggle;

    [SerializeField]
    private SettingsProfileSO settingsProfile;

    private string FilePath;

    private BasePanel TurnOnSettingsPanel;

    protected override void Awake()
    {
        base.Awake();
        FilePath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SettingsData.json";

        MasterSlider.onValueChanged.AddListener(SetMasterVolume);
        MusicSlider.onValueChanged.AddListener(SetMusicVolume);
        SFXSlider.onValueChanged.AddListener(SetSFXVolume);

        ResolutionDropDown.onValueChanged.AddListener(SetResolution);
        FullScreenToggle.onValueChanged.AddListener(SetDisplay);
    }

    private void Start()
    {
        LoadFromJson();
    }

    private void Update()
    {
        if (InputManager.Instance.Cancel)
        {
            if (!isActive) return;
            OnSaveButtonClick();
        }
    }

    public void ActivateByPanel(BasePanel panel)
    {
        TurnOnSettingsPanel = panel;
        Activate(true);
    }

    protected override IEnumerator DelayActivate(bool active, float delay)
    {
        if (active)
        {
            yield return new WaitForSeconds(delay);
            isActive = true;
            EventSystem.current.SetSelectedGameObject(firstSelectedGameObject);
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            yield return new WaitForSeconds(delay);
            isActive = false;
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    private void SetDisplay(bool toggle)
    {
        Screen.fullScreen = toggle;
    }

    private void SetResolution(int index)
    {
        switch (index)
        {
            case 0:
                Screen.SetResolution(1920, 1080, settingsProfile.settingsProfile.FullScreen);
                break;
            case 1:
                Screen.SetResolution(1760, 990, settingsProfile.settingsProfile.FullScreen);
                break;
            case 2:
                Screen.SetResolution(1600, 900, settingsProfile.settingsProfile.FullScreen);
                break;
        }
    }

    private void SetMasterVolume(float value)
    {
        audioMixer.SetFloat("MasterVolume", ValueToVolume(value));
        settingsProfile.settingsProfile.MasterVolume = value;
    }

    private void SetMusicVolume(float value)
    {
        audioMixer.SetFloat("MusicVolume", ValueToVolume(value));
        settingsProfile.settingsProfile.MusicVolume = value;
    }

    private void SetSFXVolume(float value)
    {
        audioMixer.SetFloat("SFXVolume", ValueToVolume(value));
        settingsProfile.settingsProfile.SFXVolume = value;
    }

    public void OnSaveButtonClick()
    {
        SaveToJson();
        Activate(false);
        if (TurnOnSettingsPanel.GetType() == typeof(MainMenuPanel))
        {
            UIManager.Instance.MainMenuPanel.Activate(true);
            UIManager.Instance.ActiveDepthOfField(false);
        }
        else if (TurnOnSettingsPanel.GetType() == typeof(PausePanel))
        {
            UIManager.Instance.PausePanel.Activate(true);
        }
    }

    private void SaveToJson()
    {
        settingsProfile.Save(FilePath);
    }

    private void LoadFromJson()
    {
        settingsProfile.Load(FilePath);

        var settingDatas = settingsProfile.settingsProfile;

        MasterSlider.value = settingDatas.MasterVolume;
        MusicSlider.value = settingDatas.MusicVolume;
        SFXSlider.value = settingDatas.SFXVolume;
        ResolutionDropDown.value = settingDatas.Resolution;
        FullScreenToggle.isOn = settingDatas.FullScreen;

        SetMasterVolume(settingDatas.MasterVolume);
        SetMusicVolume(settingDatas.MusicVolume);
        SetSFXVolume(settingDatas.SFXVolume);
        SetResolution(settingDatas.Resolution);
        SetDisplay(settingDatas.FullScreen);
    }

    /// <summary>
    /// Turn slider value in range [0,10] to Audiomixer volume Decibel [-120,0]
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private float ValueToVolume(float value)
    {
        float volume;
        volume = Mathf.Log10(value * 0.1f + 0.0001f) * 30;
        return volume;
    }
}
