using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RemoveMissingScripts : MonoBehaviour
{
    [MenuItem("Tools/Remove Missing Scripts")]
    public static void Remove()
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>(true); // Include inactive
        int count = 0;

        foreach (GameObject obj in allObjects)
        {
            // Remove missing scripts
            int removed = GameObjectUtility.RemoveMonoBehavioursWithMissingScript(obj);
            count += removed;
        }

        Debug.Log($"Removed {count} missing scripts.");
    }
}
