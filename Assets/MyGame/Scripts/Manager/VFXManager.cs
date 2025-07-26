using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : PersistantSingleton<VFXManager>
{
    public List<VFXData> vfxList = new();
    private Dictionary<string, GameObject> vfxDictionary = new();

    protected override void Awake()
    {
        base.Awake();
        foreach (var vfx in vfxList)
        {
            vfxDictionary[vfx.key] = vfx.effectPrefab;
        }
    }

    public void PlayVFX(string key, Vector3 position, Quaternion rotation, float destroyTime = 2f, Transform parent = null)
    {
        if (vfxDictionary.TryGetValue(key, out GameObject prefab))
        {
            var vfxInstance = ObjectPool.Instance.Spawn(prefab);
            vfxInstance.transform.SetPositionAndRotation(position, rotation);
            if (parent != null)
                vfxInstance.transform.SetParent(parent);
            StartCoroutine(DeactiveAfterTime(vfxInstance, destroyTime));
            vfxInstance.SetActive(true);
        }
        else
        {
            Debug.LogWarning("VFX is not founded: " + key);
        }
    }

    //public void PlayVFX_Addressable(string key, Vector3 position, Quaternion rotation, float destroyTime = 2f)
    //{
    //    AddressableManager.Instance.CreateAsset<GameObject>(key, result =>
    //    {
    //        GameObject vfxInstance = ObjectPool.Instance.Spawn(result);
    //        vfxInstance.transform.SetPositionAndRotation(position, rotation);
    //        StartCoroutine(DeactiveAfterTime(vfxInstance, destroyTime));
    //        vfxInstance.SetActive(true);
    //    });
    //}

    private IEnumerator DeactiveAfterTime(GameObject prefab, float time)
    {
        yield return new WaitForSeconds(time);

        prefab.SetActive(false);
    }
}

[System.Serializable]
public class VFXData
{
    public string key;
    public GameObject effectPrefab;
}
