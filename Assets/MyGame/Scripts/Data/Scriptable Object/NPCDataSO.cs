using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New NPC Data", menuName = "SO/NPC Data", order = 3)]
public class NPCDataSO : ScriptableObject
{
    public List<NPCInfo> npcs = new();

    public NPCInfo GetRandomNPC()
    {
        int index = Random.Range(0, npcs.Count);
        return npcs[index];
    }

    public NPCInfo GetNPCByIndex(int index)
    {
        return npcs.Find(item => item.index == index);
    }
}

[System.Serializable]
public class NPCInfo
{
    public int index;
    public GameObject prefab;
}