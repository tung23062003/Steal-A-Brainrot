using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : PersistantSingleton<SettingsManager>
{
    // Cài ??t âm thanh
    [Range(0, 1)] public float musicVolume = 1f;
    [Range(0, 1)] public float sfxVolume = 1f;
    public bool vibrationEnabled = true;

    // Các event ?? thông báo thay ??i
    public static event Action<float> OnMusicVolumeChanged;
    public static event Action<float> OnSFXVolumeChanged;
    public static event Action<bool> OnVibrationChanged;

    protected override void Awake()
    {
        base.Awake();
        LoadSettings();
    }

    public void ToggleVibration()
    {
        vibrationEnabled = !vibrationEnabled;
        SaveSettings();
        OnVibrationChanged?.Invoke(vibrationEnabled);

        if (vibrationEnabled)
        {
            Vibrate();
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        SaveSettings();
        OnMusicVolumeChanged?.Invoke(musicVolume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        SaveSettings();
        OnSFXVolumeChanged?.Invoke(sfxVolume);
    }

    public void Vibrate()
    {
        if (!vibrationEnabled) return;

#if UNITY_ANDROID || UNITY_IOS
        Handheld.Vibrate();
#endif
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("VibrationEnabled", vibrationEnabled ? 1 : 0);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.Save();
    }

    public void LoadSettings()
    {
        vibrationEnabled = PlayerPrefs.GetInt("VibrationEnabled", 1) == 1;
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
    }
}
