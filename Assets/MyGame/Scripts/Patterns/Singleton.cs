using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour, ISingleton where T : MonoBehaviour
{
    public static T Instance;

    protected virtual void Awake()
    {
        if(Instance == null)
        {
            Instance = GetComponent<T>();
        }
        else if(Instance.GetInstanceID() != gameObject.GetInstanceID())
        {
            Debug.LogError("Singleton is not unique!");
            Destroy(this.gameObject);
        }
    }

    public void DestroySingleton()
    {
        if (Instance == this)
            Instance = null;

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}

public interface ISingleton
{
    public void DestroySingleton();
}
