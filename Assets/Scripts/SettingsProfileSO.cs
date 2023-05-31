using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "SettingsProfile", menuName = "Scriptable Object/Settings Profile")]
public class SettingsProfileSO : ScriptableObject
{
    public SettingsProfile settingsProfile;
 
    public void Save(string path)
    {
        var json = JsonUtility.ToJson(settingsProfile);
        File.WriteAllText(path, json);
    }

    public void Load(string path)
    {
        if (File.Exists(path))
        {
            var json = File.ReadAllText(path);
            settingsProfile = JsonUtility.FromJson<SettingsProfile>(json);
        }
    }

    [Serializable]
    public class SettingsProfile
    {
        public float MasterVolume;
        public float MusicVolume;
        public float SFXVolume;

        public int Resolution;
        public bool FullScreen;
    }
}
