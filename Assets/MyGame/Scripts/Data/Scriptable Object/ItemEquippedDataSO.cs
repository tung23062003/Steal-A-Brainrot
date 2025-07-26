using System.Collections.Generic;
using UnityEngine;

public class ItemEquippedDataSO : ScriptableObject
{
    public List<ItemInfo> items = new();

    public ItemInfo GetSkinInfoByIndex(int index)
    {
        return items.Find(item => item.index == index);
    }

    public StatBonus GetStatBonusByIndex(int index)
    {
        return items.Find(item => item.index == index).statBonus;
    }

/*    public BigCurrency GetPriceByIndex(int index)
    {
        return items.Find(item => item.index == index).price;
    }*/
}

[System.Serializable]
public class ItemInfo
{
    public int index;
    public string name;
    public GameObject itemPrefab;
    public Sprite image;
    public StatBonus statBonus;
}

[System.Serializable]
public class StatBonus
{
    public BigCurrency power; // day la power
    public BigCurrency coin_multiplier;
}