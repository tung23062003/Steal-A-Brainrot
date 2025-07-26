using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Map Data", menuName = "SO/Map Data", order = 6)]
public class MapDataSO : ScriptableObject
{
    public List<MapInfo> mapInfos = new();

    public MapInfo GetMapInfoByMapName(Map map)
    {
        return mapInfos.Find(item => item.map == map);
    }

    public BigCurrency GetCoinAdsReward(Map map)
    {
        var info = GetMapInfoByMapName(map);
        return info != null ? info.coinAdsReward : new BigCurrency(0);
    }
}

[System.Serializable]
public class MapInfo
{
    public Map map;
    public int cupCanCollect;
    public int cupToUnlock;
    public BigCurrency coinAdsReward;
}