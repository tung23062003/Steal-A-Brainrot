using Lofelt.NiceVibrations;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : PersistantSingleton<SFXManager>
{
    public List<SFXData> sfxList = new();
    private Dictionary<string, AudioClip> sfxDictionary = new();

    private List<AudioSource> sfxAudioSources = new();
    public int maxAudioSources = 10;

    [Header("Music Settings")]
    public AudioSource musicAudioSource;
    public List<MusicData> musicList = new();
    private Dictionary<string, AudioClip> musicDictionary = new();

    [Header("Vibration Settings")]
    [Range(0f, 1f)] public float vibrationIntensity = 0.7f;
    public HapticPatterns.PresetType defaultVibrationPreset = HapticPatterns.PresetType.MediumImpact;

    private Dictionary<AudioSource, float> audioSourceBaseVolumes = new();
    private float musicBaseVolume = 1f;

    protected override void Awake()
    {
        base.Awake();

        foreach (var sfx in sfxList)
        {
            sfxDictionary[sfx.key] = sfx.clip;
        }

        foreach (var music in musicList)
        {
            musicDictionary[music.key] = music.clip;
        }

        // T?o AudioSource cho nh?c n?n
        musicAudioSource = gameObject.AddComponent<AudioSource>();
        musicAudioSource.loop = true;
        musicBaseVolume = musicAudioSource.volume;

        CreateSFXAudioSources();

        SettingsManager.OnSFXVolumeChanged += UpdateAllSFXVolume;
        SettingsManager.OnMusicVolumeChanged += UpdateMusicVolume;
    }

    private void OnDestroy()
    {
        SettingsManager.OnSFXVolumeChanged -= UpdateAllSFXVolume;
        SettingsManager.OnMusicVolumeChanged -= UpdateMusicVolume;
    }

    private void CreateSFXAudioSources()
    {
        for (int i = 0; i < maxAudioSources; i++)
        {
            AudioSource newSource = gameObject.AddComponent<AudioSource>();
            sfxAudioSources.Add(newSource);
            audioSourceBaseVolumes[newSource] = 1f; // M?c ??nh base volume là 1
        }
    }

    private AudioSource GetAvailableAudioSource()
    {
        foreach (var source in sfxAudioSources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }

        if (sfxAudioSources.Count < maxAudioSources * 2)
        {
            AudioSource newSource = gameObject.AddComponent<AudioSource>();
            sfxAudioSources.Add(newSource);
            audioSourceBaseVolumes[newSource] = 1f;
            return newSource;
        }

        Debug.LogWarning("AudioSource runs out of stock");
        return null;
    }

    #region SFX Functions
    public void PlaySFX(string key, Vector3 position, float volume = 1.0f, float spatial_blend = 1.0f,
                        bool isLoop = false, bool isStopAllActiveSound = false, bool canVibrate = false)
    {
        if (!sfxDictionary.TryGetValue(key, out AudioClip clip))
        {
            Debug.LogWarning("SFX can not found: " + key);
            return;
        }

        AudioSource source = GetAvailableAudioSource();
        if (source == null) return;

        if (isStopAllActiveSound)
            StopAllSFX();

        audioSourceBaseVolumes[source] = volume;
        float finalVolume = volume * SettingsManager.Instance.sfxVolume;

        source.clip = clip;
        source.volume = finalVolume;
        source.transform.position = position;
        source.spatialBlend = spatial_blend;
        source.loop = isLoop;
        source.Play();

        if (canVibrate)
        {
            SettingsManager.Instance.Vibrate();
        }
    }

    private void UpdateAllSFXVolume(float newVolume)
    {
        foreach (var source in sfxAudioSources)
        {
            if (audioSourceBaseVolumes.TryGetValue(source, out float baseVolume))
            {
                source.volume = baseVolume * newVolume;
            }
        }
    }

    public void StopAllSFX()
    {
        foreach (var source in sfxAudioSources)
        {
            source.Stop();
        }
    }

    public void StopSFX(string key)
    {
        if (sfxDictionary.TryGetValue(key, out AudioClip clip))
        {
            foreach (var source in sfxAudioSources)
            {
                if (source == null) continue;
                if (source.isPlaying && source.clip == clip)
                {
                    source.Stop();
                }
            }
        }
    }

    public void PauseAllSFX()
    {
        foreach (var source in sfxAudioSources)
        {
            if (source.isPlaying)
            {
                source.Pause();
            }
        }
    }

    public void ResumeAllSFX()
    {
        foreach (var source in sfxAudioSources)
        {
            if (!source.isPlaying && source.clip != null)
            {
                source.Play();
            }
        }
    }
    #endregion

    #region Music Functions
    public void PlayMusic(string key, float volume = 1f, bool loop = true)
    {
        if (!musicDictionary.TryGetValue(key, out AudioClip clip))
        {
            Debug.LogWarning("Music không tìm th?y: " + key);
            return;
        }

        musicBaseVolume = volume;
        musicAudioSource.clip = clip;
        musicAudioSource.volume = volume * SettingsManager.Instance.musicVolume;
        musicAudioSource.loop = loop;
        musicAudioSource.Play();
    }

    public void StopMusic()
    {
        musicAudioSource.Stop();
    }

    public void PauseMusic()
    {
        musicAudioSource.Pause();
    }

    public void ResumeMusic()
    {
        musicAudioSource.Play();
    }

    public void SetMusicVolume(float volume)
    {
        musicBaseVolume = volume;
        musicAudioSource.volume = volume * SettingsManager.Instance.musicVolume;
    }

    private void UpdateMusicVolume(float newVolume)
    {
        musicAudioSource.volume = musicBaseVolume * newVolume;
    }
    #endregion

    #region Vibration Functions
    public void PlayVibration(HapticPatterns.PresetType presetType)
    {
        if (!Application.isMobilePlatform || !SettingsManager.Instance.vibrationEnabled) return;
        HapticPatterns.PlayPreset(presetType);
    }

    public void PlayCustomVibration(float amplitude, float frequency, float duration = 0.1f)
    {
        if (!Application.isMobilePlatform || !SettingsManager.Instance.vibrationEnabled) return;

        amplitude = Mathf.Clamp01(amplitude) * vibrationIntensity;
        frequency = Mathf.Clamp01(frequency);

        if (DeviceCapabilities.isVersionSupported)
        {
            if (DeviceCapabilities.meetsAdvancedRequirements)
            {
                HapticPatterns.PlayConstant(amplitude, frequency, duration);
            }
            else
            {
                HapticPatterns.PlayEmphasis(amplitude, frequency);
            }
        }
    }

    public void StopVibration()
    {
        if (Application.isMobilePlatform)
        {
            HapticController.Stop();
        }
    }
    #endregion
}

[System.Serializable]
public class SFXData
{
    public string key;
    public AudioClip clip;
}

[System.Serializable]
public class MusicData
{
    public string key;
    public AudioClip clip;
}