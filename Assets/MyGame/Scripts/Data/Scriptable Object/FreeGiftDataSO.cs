using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/FreeGiftData")]
public class FreeGiftDataSO : ScriptableObject
{
    public List<FreeGiftItemData> giftItems;

    public FreeGiftItemData GetGiftById(string id)
    {
        return giftItems.Find(g => g.id == id);
    }

}


[System.Serializable]
public class FreeGiftItemData
{
    public string id;
    public Sprite icon;
    public float cooldownSeconds;
    public GiftType giftType;
    public BigCurrency amount;
}
