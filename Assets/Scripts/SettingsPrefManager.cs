using System;
using UnityEngine;

public class SettingsPrefManager
{
    public static SettingsPrefManager Instance { get; private set; }

    private const string SAVED_PREF_KEY = "SavedSettingsData";
    private const float DEFAULT_MUSIC_VOLUME = 0.75f;

    [Serializable]
    private class SettingsPrefData
    {
        public float musicVolume = DEFAULT_MUSIC_VOLUME;
        public float sfxVolume = DEFAULT_MUSIC_VOLUME;
    }

    private SettingsPrefData _prefData = new();

    public float MusicVolume => _prefData.musicVolume;
    public float SFXVolume => _prefData.sfxVolume;

    private SettingsPrefManager() { }

    public static SettingsPrefManager GetInstance()
    {
        if (Instance == null)
        {
            Instance = new();
            return Instance;
        }
        else
        {
            return Instance;
        }
    }

    /// <summary>
    /// Saves the current settings to <see cref="PlayerPrefs"/>.
    /// </summary>
    public void Save()
    {
        string json = JsonUtility.ToJson(_prefData);
        PlayerPrefs.SetString(SAVED_PREF_KEY, json);
        PlayerPrefs.Save();

        Debug.Log("Settings saved to PlayerPrefs.");
    }

    public void ClearSaved()
    {
        PlayerPrefs.DeleteKey(SAVED_PREF_KEY);
    }

    /// <summary>
    /// Updates and saves the music volume.
    /// </summary>
    /// <param name="volume">A normalized volume value from 0 to 1.</param>
    public void SetMusicVolume(float volume)
    {
        if (volume == _prefData.musicVolume) return;

        _prefData.musicVolume = Mathf.Clamp01(volume);
        Save();
    }

    /// <summary>
    /// Updates and saves the sound-effects volume.
    /// </summary>
    /// <param name="volume">A normalized volume value from 0 to 1.</param>
    public void SetSFXVolume(float volume)
    {
        if (volume == _prefData.sfxVolume) return;

        _prefData.sfxVolume = Mathf.Clamp01(volume);
        Save();
    }

    /// <summary>
    /// Loads settings from <see cref="PlayerPrefs"/> when saved data exists.
    /// If no saved data exists, the current in-memory settings are unchanged.
    /// </summary>
    public void Load()
    {
        if (!PlayerPrefs.HasKey(SAVED_PREF_KEY)) return;

        string json = PlayerPrefs.GetString(SAVED_PREF_KEY);
        _prefData = JsonUtility.FromJson<SettingsPrefData>(json);

        Debug.Log("Settings loaded from PlayerPrefs.");
    }
}
