using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pets Data", menuName = "SO/Pets Data", order = 2)]
public class PetsDataSO : ScriptableObject
{
    public List<PetInfo> items = new();

    public List<Sprite> slotImage = new();

    public List<Sprite> bgSelectedImage = new();

    public List<Sprite> rarityImage = new();

    public List<Sprite> rayImage = new();

    public PetInfo GetPetByIndex(int index)
    {
        return items.Find(item => item.itemInfo.index == index);
    }

    public PetInfo GetPetByRarity(Rarity rarity)
    {
        return items.Find(item => item.rarity == rarity);
    }

    public PetInfo GetPetByMapNumber(Map map)
    {
        return items.Find(item => item.appearInMap == map);
    }

    public List<PetInfo> GetPetsByIndex(int index)
    {
        return items.FindAll(item => item.itemInfo.index == index);
    }

    public StatBonus GetStatBonusByIndex(int index)
    {
        return items.Find(item => item.itemInfo.index == index).itemInfo.statBonus;
    }
}

[System.Serializable]
public class PetInfo
{
    public ItemInfo itemInfo;
    public Rarity rarity;
    public Map appearInMap;
}

[System.Serializable]
public class PetSaveInfo
{
    public string commonIndex;
    public string uniqueID;

    public PetSaveInfo(string commonIndex, string uniqueID)
    {
        this.commonIndex = commonIndex;
        this.uniqueID = uniqueID;
    }
}