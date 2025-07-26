using Cysharp.Threading.Tasks;
using KingCat.Base;
using KingCat.Base.Ads;
using KingCat.Base.Data;
using KingCat.Base.RemoteConfigs;
using KingCat.Base.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityServiceLocator;

public class GameManager : MonoSingleton<GameManager>
{
    public UnityEvent OnInited = new UnityEvent();

    //public Map map;


    protected override void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        base.Awake();

        if (!PlayerPrefs.HasKey("CharEquipedIndex"))
            PlayerPrefs.SetInt("CharEquipedIndex", 0);

        //PlayerPrefs.SetInt("MapUnlocked", 3);
        //if (!PlayerPrefs.HasKey("WingsEquipedIndex"))
        //    PlayerPrefs.SetInt("WingsEquipedIndex", 0);

        //if (!PlayerPrefs.HasKey("SkateboardEquipedIndex"))
        //    PlayerPrefs.SetInt("SkateboardEquipedIndex", 0);

        //if (!PlayerPrefs.HasKey("Checkpoint"))
        //    PlayerPrefs.SetInt("Checkpoint", 0);

    }

    public override void Init()
    {
        base.Init();
        StartCoroutine(IeInit());
    }

    private IEnumerator IeInit()
    {
        //yield return new WaitForSeconds(0.5f);
#if FIREBASE
        // Check Firebase dependencies before initializing
        var dependencyStatusTask = Firebase.FirebaseApp.CheckAndFixDependenciesAsync();
        yield return new WaitUntil(() => dependencyStatusTask.IsCompleted);

        // Handle dependency check result
        if (dependencyStatusTask.Result == Firebase.DependencyStatus.Available)
        {
            // Firebase dependencies are ready, initialize Firebase
#if REMOTE_CONFIG
            yield return RemoteConfigsManager.Instance.InitializeFirebase();
#elif ANALYTICS
            FirebaseApp app = Firebase.FirebaseApp.DefaultInstance;
#endif
        }
        else
        {
            // Log or handle the error if Firebase dependencies are not available
            Debug.LogError($"Could not resolve Firebase dependencies: {dependencyStatusTask.Result}");
        }

        yield return new WaitForSeconds(0.5f);
#endif

#if ADS
            AdsManager.Instance.Init();
#endif
        //yield return new WaitForSeconds(0.5f);
        // Load user profile and invoke the initialization event
        UserData.Instance.LoadUserProfile((userProfile) => { 
            Debug.Log(JsonUtility.ToJson(userProfile));
            if(!PlayerPrefs.HasKey("USER_DATA"))
                UserData.Instance.SetUserProfile(userProfile, null);

            //LogUtils.Log(UserData.Instance.GetParam<string>("skin"));
        });
        yield return new WaitForSeconds(0.5f);
        OnInited?.Invoke();
    }


    public void LoadScene(string sceneName)
    {
        Debug.Log("Load scene: " + sceneName);
        UILoadingManager.Instance.LoadScene(sceneName);
    }

    private void Start()
    {
        LoadData<PetDataWrapper>("Data/pet_data.json", (data) => { 
            UserData.Instance.userProfile.pets = data.pets;
            GameEvent.OnLoadDataDone?.Invoke();
        });


        
        //LoadData<ViewerDataWrapper>("Data/viewer_data.json", (viewerData) => { chatData.viewers = viewerData.viewers; });

        //ServiceLocator.Global.Register<ITest>(test = new Testttt());
        //ServiceLocator.Global.Register<PlayerMovement>(playerMovement = new PlayerMovement());
    }

    // Load Resource
    //public bool LoadData<T>(string path, Action<T> onComplete = null)
    //{
    //    string jsonText = LoaderUtility.Instance.GetText(path);

    //    if (!string.IsNullOrEmpty(jsonText))
    //    {
    //        var tempData = JsonUtility.FromJson<T>(jsonText);

    //        if (tempData != null)
    //        {
    //            onComplete?.Invoke(tempData);
    //            Debug.Log("Data loaded successfully!");
    //            return true;
    //        }
    //        else
    //        {
    //            Debug.LogError("Failed to parse JSON.");
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogError("Failed to load JSON.");
    //    }
    //    return false;
    //}

    public bool LoadData<T>(string fileName, Action<T> onComplete = null)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            onComplete?.Invoke(JsonUtility.FromJson<T>(jsonData));
            return true;
        }
        else
        {
            Debug.LogWarning("File is not existed: " + filePath);
        }
        return false;
    }

    public void SaveData<T>(string fileName, T data)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        Directory.CreateDirectory(Path.GetDirectoryName(filePath));

        string jsonData = JsonUtility.ToJson(data, prettyPrint: true);
        File.WriteAllText(filePath, jsonData);

        Debug.Log("Save into: " + filePath);
    }
}

[Serializable]
public class PetDataWrapper
{
    public List<PetSaveInfo> pets;
}