#if REMOTE_CONFIG
using Firebase;
using Firebase.Extensions;
using Firebase.RemoteConfig;
#endif
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace KingCat.Base.RemoteConfigs
{
    public class RemoteConfigsManager : MonoSingleton<RemoteConfigsManager>
    {
#if REMOTE_CONFIG
        private bool isFirebaseInitialized = false;

        public UnityEvent<bool> onLoadRemoteConfigs = new UnityEvent<bool>();

        public Task InitializeFirebase()
        {
            return FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                isFirebaseInitialized = true;
                Debug.Log("Firebase initialized successfully");

                FetchRemoteConfigDataAsync();
            });
        }

        public Task FetchRemoteConfigDataAsync()
        {
            if (!isFirebaseInitialized)
            {
                Debug.LogWarning("Firebase is not initialized.");
                onLoadRemoteConfigs?.Invoke(false); // Invoke failure since Firebase is not initialized
                return Task.FromResult(0);
            }

            // Fetch data from the Firebase Remote Config server
            var fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
            return fetchTask.ContinueWithOnMainThread(fetchTask =>
            {
                if (fetchTask.IsCompleted && !fetchTask.IsFaulted && !fetchTask.IsCanceled)
                {
                    FirebaseRemoteConfig.DefaultInstance.ActivateAsync().ContinueWithOnMainThread(activateTask =>
                    {
                        if (activateTask.IsCompleted && !activateTask.IsFaulted && !activateTask.IsCanceled)
                        {
                            Debug.Log("Remote Config data activated successfully.");
                            DisplayRemoteConfigValues();
                        }
                        else
                        {
                            Debug.LogError("Failed to activate Remote Config data.");
                            onLoadRemoteConfigs?.Invoke(false); // Invoke failure if activation fails
                        }
                    });
                }
                else
                {
                    Debug.LogError("Failed to fetch Remote Config data.");
                    onLoadRemoteConfigs?.Invoke(false); // Invoke failure if fetching fails
                }
            });
        }

        private void DisplayRemoteConfigValues()
        {
            // Access remote config values
            string gameConfigStr = FirebaseRemoteConfig.DefaultInstance.GetValue("game_configs").StringValue;
            Debug.Log($"On fetch remote config: {gameConfigStr}");
            ParseGameConfig(gameConfigStr);
        }

        private void ParseGameConfig(string gameConfigStr)
        {
            Type gameConstsType = typeof(GameConfigs);
            if (!string.IsNullOrEmpty(gameConfigStr))
            {
                JObject gameConfig = JObject.Parse(gameConfigStr);
                foreach (var kv in gameConfig)
                {
                    try
                    {
                        PropertyInfo property = gameConstsType.GetProperty(kv.Key);
                        FieldInfo field = gameConstsType.GetField(kv.Key);

                        if (property != null)
                        {
                            SetValue(property, null, kv.Value);
                        }
                        else if (field != null)
                        {
                            SetValue(field, null, kv.Value);
                        }
                        else
                        {
                            Debug.Log($"No matching field or property found for key: {kv.Key}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.Log($"Error setting value for {kv.Key}: {ex.Message}");
                    }
                }
            }
            else
            {
                Debug.LogWarning("Remote config string is null or empty.");
                onLoadRemoteConfigs?.Invoke(false); // Invoke failure if config string is invalid
            }

            // Invoke success after successfully loading the config
            onLoadRemoteConfigs?.Invoke(true);
        }

        private void SetValue(MemberInfo member, object target, JToken value)
        {
            Type memberType = null;

            if (member is PropertyInfo property)
            {
                memberType = property.PropertyType;
            }
            else if (member is FieldInfo field)
            {
                memberType = field.FieldType;
            }

            if (memberType != null)
            {
                object convertedValue = Convert.ChangeType(value.ToObject(memberType), memberType);

                if (member is PropertyInfo prop)
                {
                    prop.SetValue(target, convertedValue);
                    Debug.Log($"{prop.Name} set to: {convertedValue}");
                }
                else if (member is FieldInfo fld)
                {
                    fld.SetValue(target, convertedValue);
                    Debug.Log($"{fld.Name} set to: {convertedValue}");
                }
            }
        }
#endif
    }

}

