using KingCat.Base.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace KingCat.Base
{
    public class SoundManager : MonoSingleton<SoundManager>
    {
#if UNITY_EDITOR
        private void OnValidate()
        {
            // Automatically load all audio clips from a specific folder in the Assets directory
            string[] guids = AssetDatabase.FindAssets("t:AudioClip", new[] { "Assets" });

            List<AudioClip> audioClipList = new List<AudioClip>();

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(path);
                if (clip != null)
                {
                    audioClipList.Add(clip);
                }
            }

            sounds = audioClipList.ToArray();
        }
#endif

        [SerializeField] private AudioClip[] sounds;
        private Dictionary<string, AudioClip> dicSounds = new Dictionary<string, AudioClip>();
        private AudioSource musicSource;
        private AudioSource soundSource;
        public bool IsMuteSound;
        public bool IsMuteMusic;
        public bool IsMuteVibrate;

        public const string SETTING_MUTEMUSIC_KEY = "muteMusic";
        public const string SETTING_MUTESOUND_KEY = "muteSound";
        public const string SETTING_MUTEVIBRATE_KEY = "muteVibrate";

        private float soundVolume;
        private float musicVolume;

        public override void Init()
        {
            base.Init();

            dicSounds.Clear();
            foreach (var sound in sounds)
            {
                dicSounds[sound.name.ToLower()] = sound;
            }

            var musicObj = new GameObject();
            musicObj.transform.SetParent(transform, false);
            musicObj.transform.localPosition = Vector3.zero;
            musicObj.name = "Music Source";
            musicSource = musicObj.AddComponent<AudioSource>();
            musicSource.loop = true;

            var soundObj = new GameObject();
            soundObj.transform.SetParent(transform, false);
            soundObj.transform.localPosition = Vector3.zero;
            soundObj.name = "Sound Source";
            soundSource = soundObj.AddComponent<AudioSource>();
            soundSource.loop = false;

            //Vibration.Init();
        }

        private void Start()
        {
            LoadSetting();
        }

        public void OnMuteSound(bool isMute)
        {
            UserData.Instance.SetParam(SoundManager.SETTING_MUTESOUND_KEY, isMute);
            LoadSetting();
        }

        public void OnMuteMusic(bool isMute)
        {
            UserData.Instance.SetParam(SoundManager.SETTING_MUTEMUSIC_KEY, isMute);
            LoadSetting();
        }

        public void OnMuteVibrate(bool isMute)
        {
            UserData.Instance.SetParam(SoundManager.SETTING_MUTEVIBRATE_KEY, isMute);
            LoadSetting();
        }

        public void LoadSetting()
        {
            IsMuteSound = UserData.Instance.GetParam<bool>(SoundManager.SETTING_MUTESOUND_KEY);
            IsMuteMusic = UserData.Instance.GetParam<bool>(SoundManager.SETTING_MUTEMUSIC_KEY);
            IsMuteVibrate = UserData.Instance.GetParam<bool>(SoundManager.SETTING_MUTEVIBRATE_KEY);

            musicSource.volume = IsMuteMusic ? 0 : musicVolume;
            soundSource.volume = IsMuteSound ? 0 : soundVolume;
        }

        public void PlayMusic(string soundName, float volume = 1f)
        {
            soundName = soundName.ToLower();
            if (!IsMuteMusic && dicSounds.ContainsKey(soundName))
            {
                AudioClip musicClip = dicSounds[soundName];
                if (musicSource.isPlaying)
                {
                    musicSource.Stop();
                }
                musicSource.volume = volume;
                musicSource.clip = musicClip;
                musicSource.Play();

                musicVolume = volume;
            }
        }

        public void PlaySound(string soundName, float volume = 1f)
        {
            soundName = soundName.ToLower();
            if (!IsMuteSound && dicSounds.ContainsKey(soundName))
            {
                AudioClip soundClip = dicSounds[soundName];
                soundSource.volume = volume;
                soundSource.PlayOneShot(soundClip);

                soundVolume = volume;
            }
        }

        public void PlayVibratePop()
        {
            if (IsMuteVibrate) return;
            if (Application.isMobilePlatform)
            {
#if UNITY_ANDROID
                //Vibration.VibrateAndroid(10);
#endif
            }
        }

        public void PlayVibratePeek()
        {
            if (IsMuteVibrate) return;
            if (Application.isMobilePlatform)
            {
#if UNITY_ANDROID
                //Vibration.VibrateAndroid(20);
#endif
            }
        }

        public void PlayVibrateNope()
        {
            if (IsMuteVibrate) return;
            if (Application.isMobilePlatform)
            {
#if UNITY_ANDROID
                long[] pattern = { 0, 10, 10};
                //Vibration.VibrateAndroid(pattern, -1);
#endif
            }
        }
    }
}

