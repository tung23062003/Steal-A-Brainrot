using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wings Data", menuName = "SO/Wings Data", order = 1)]
public class WingsDataSO : ScriptableObject
{
    public List<WingsInfo> items = new();

    public WingsInfo GetWingsByIndex(int index)
    {
        return items.Find(item => item.itemInfo.index == index);
    }

    public WingsInfo GetWingsByMapNumber(Map map)
    {
        return items.Find(item => item.appearInMap == map);
    }

    public StatBonus GetStatBonusByIndex(int index)
    {
        return items.Find(item => item.itemInfo.index == index).itemInfo.statBonus;
    }
}

[System.Serializable]
public class WingsInfo
{
    public ItemInfo itemInfo;
    public BigCurrency price;
    public float speed;
    public UnlockType unlockType;
    public Map appearInMap;
}

