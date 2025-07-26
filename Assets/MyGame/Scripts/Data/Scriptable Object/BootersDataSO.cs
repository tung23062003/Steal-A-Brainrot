using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New User Data", menuName = "SO/User Data", order = 5)]
public class BootersDataSO : ScriptableObject
{
    public List<BoostersInfo> boosters = new();

    public BoostersInfo GetSkinInfoByIndex(int index)
    {
        return boosters.Find(item => item.index == index);
    }

    public StatBonus GetStatBonusByIndex(int index)
    {
        return boosters.Find(item => item.index == index).statBonus;
    }
}

[System.Serializable]
public class BoostersInfo
{
    public int index;
    public Sprite image;
    public StatBonus statBonus;
}