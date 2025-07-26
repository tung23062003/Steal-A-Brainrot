using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gacha Egg", menuName = "SO/Gacha Egg Data")]
public class GachaEggDataSO : ScriptableObject
{
    public int eggId;
    public BigCurrency costInCoin;
    public int requiredAdsCount;

    public List<GachaPetEntry> petList;

    public object GetPetByIndex(int idPet)
    {
        return petList[idPet];
    }
}

[System.Serializable]
public class GachaPetEntry
{
    public int idPet;
    [Range(0f, 1f)] public float chance;
}
