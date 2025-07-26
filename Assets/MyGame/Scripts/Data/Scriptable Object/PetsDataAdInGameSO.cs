using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPetAd", menuName = "SO/PetAd")]
public class PetsDataAdInGameSO : ScriptableObject
{
    public int petID;
    public string petName;
    public Sprite petImage;
    public int requiredAdsToUnlock = 3;
}
