using System;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.IO;

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

    [SerializeField] private Button saveButton;

    [SerializeField]
    private SettingsProfileSO settingsProfile;

    [SerializeField]
    private string FilePath;

    protected virtual void Awake()
    {
        FilePath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SettingsData.json";

        MasterSlider.onValueChanged.AddListener(SetMasterVolume);
        MusicSlider.onValueChanged.AddListener(SetMusicVolume);
        SFXSlider.onValueChanged.AddListener(SetSFXVolume);

        ResolutionDropDown.onValueChanged.AddListener(SetResolution);
        FullScreenToggle.onValueChanged.AddListener(SetDisplay);
    }

    private void Start()
    {
        saveButton.onClick.AddListener(OnSaveButtonClick);
        LoadFromJson();
    }

    private void Update()
    {
        if (InputManager.Instance.Cancel)
        {
            if (UIManager.Instance.CompareCurrentView(View.Settings))
            {
                OnSaveButtonClick();
            }
        }
    }

    private void SetDisplay(bool toggle)
    {
        Debug.Log("Set display "+ toggle);
        Screen.fullScreen = toggle;
        settingsProfile.settingsProfile.FullScreen = toggle;
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
        settingsProfile.settingsProfile.Resolution = index;
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
        UIManager.Instance.ShowLast();
    }

    private void SaveToJson()
    {
        settingsProfile.Save(FilePath);
    }

    private void LoadFromJson()
    {
        settingsProfile.Load(FilePath);

        var profile = settingsProfile.settingsProfile;

        MasterSlider.value = profile.MasterVolume;
        MusicSlider.value = profile.MusicVolume;
        SFXSlider.value = profile.SFXVolume;
        ResolutionDropDown.value = profile.Resolution;
        FullScreenToggle.isOn = profile.FullScreen;

        SetMasterVolume(profile.MasterVolume);
        SetMusicVolume(profile.MusicVolume);
        SetSFXVolume(profile.SFXVolume);
        SetResolution(profile.Resolution);
        SetDisplay(profile.FullScreen);
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
