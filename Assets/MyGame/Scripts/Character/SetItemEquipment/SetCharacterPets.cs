using Cysharp.Threading.Tasks;
using KingCat.Base.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SetCharacterPets : SetItemEquipment
{
    [SerializeField] private PetsDataSO petsData;
    [SerializeField] private float petSpacing = 1.5f;

    [HideInInspector] private Dictionary<string, GameObject> pets = new();

    private BigCurrency totalCoinMultiplier = new(0);
    private PlayerStat playerStat;

    private void Awake()
    {
        GameEvent.OnEquipPet.AddListener(OnEquipPet);
        GameEvent.OnUnequipPet.AddListener(OnUnequipPet);
        GameEvent.OnArrangePet.AddListener(ArrangePets);

        playerStat = GetComponent<PlayerStat>();
    }

    private void OnDestroy()
    {
        GameEvent.OnEquipPet.RemoveAllListeners();
        GameEvent.OnUnequipPet.RemoveAllListeners();
        GameEvent.OnArrangePet.RemoveListener(ArrangePets);
    }

    protected override async void SetSkin()
    {
        var petsEquiped = UserData.Instance.userProfile.petsEquiped;
        if (petsEquiped.Count == 0) return;

        if (skinParent.childCount > 0)
        {
            Destroy(skinParent.GetChild(0).gameObject);
        }

        foreach (var index in petsEquiped)
        {
            SpawnPet(index);
            await UniTask.Yield();
        }
        //await UniTask.Delay(2000);
        //await UniTask.Yield();

        SetStatBonus();

        await UniTask.Delay(200);
        ArrangePets();

        await UniTask.Yield();

        if (animator != null)
        {
            animator.Rebind();
            //animator.Update(0f);
        }
    }

    public void SpawnPet(PetSaveInfo petSaveInfo)
    {
        //int petIndex = MyUtils.SubAsterisk(index);
        GameObject p;
        int petIndex = 0;
        if (petSaveInfo.commonIndex.Contains("*"))
        {
            petIndex = int.Parse(petSaveInfo.commonIndex[0..^1]);
            p = Instantiate(petsData.GetPetByIndex(petIndex).itemInfo.itemPrefab, skinParent);
            p.transform.localScale *= 1.1f;

            int petIndexInter = petIndex;
            totalCoinMultiplier += petsData.GetStatBonusByIndex(petIndexInter).coin_multiplier * 1.5f;
        }
        else
        {
            petIndex = int.Parse(petSaveInfo.commonIndex);
            p = Instantiate(petsData.GetPetByIndex(petIndex).itemInfo.itemPrefab, skinParent);

            int petIndexInter = petIndex;
            totalCoinMultiplier += petsData.GetStatBonusByIndex(petIndexInter).coin_multiplier;
        }
        
        if(!pets.ContainsKey(petSaveInfo.uniqueID))
            pets.Add(petSaveInfo.uniqueID, p);
        p.SetActive(true);

        
    }


    public void DespawnPet(PetSaveInfo petSaveInfo)
    {
        if(pets.ContainsKey(petSaveInfo.uniqueID))
            Destroy(pets[petSaveInfo.uniqueID]);

        pets.Remove(petSaveInfo.uniqueID);

        int petIndex = 0;
        if (petSaveInfo.commonIndex.Contains("*"))
        {
            petIndex = int.Parse(petSaveInfo.commonIndex[0..^1]);
            totalCoinMultiplier -= petsData.GetStatBonusByIndex(petIndex).coin_multiplier * 1.5f;
        }
        else
        {
            petIndex = int.Parse(petSaveInfo.commonIndex);
            totalCoinMultiplier -= petsData.GetStatBonusByIndex(petIndex).coin_multiplier;
        }
    }

    void ArrangePets()
    {
        float totalWidth = (pets.Count - 1) * petSpacing;
        LogUtils.Log("Petcount" + pets.Count);
        LogUtils.Log("SkinParentlocalPos" + skinParent.localPosition);

        float startX = skinParent.localPosition.x - totalWidth / 2f;

        foreach (KeyValuePair<string, GameObject> pet in pets)
        {
            var i = pets.Keys.ToList().IndexOf(pet.Key);
            float xPos = startX + i * petSpacing;

            Vector3 newPos = new Vector3(
                xPos,
                skinParent.localPosition.y,
                skinParent.localPosition.z
            );

            LogUtils.Log("SkinPetlocalPos" + pet.Value.transform.localPosition);
            pet.Value.transform.localPosition = newPos;
        }
    }

    protected override void SetStat()
    {
        //playerStat.SetCoinMultiplierPet(totalCoinMultiplier);
    }

    protected override void SetStatBonus()
    {
        //var playerStat = GetComponent<PlayerStat>();
        //playerStat.SetCoinMultiplierPet(totalCoinMultiplier);
    }

    private async void OnEquipPet(PetSaveInfo petSaveInfo)
    {
        SpawnPet(petSaveInfo);
        await UniTask.Delay(100);
        ArrangePets();
        SetStat();
    }

    private async void OnUnequipPet(PetSaveInfo petSaveInfo)
    {
        DespawnPet(petSaveInfo);
        await UniTask.Delay(100);
        ArrangePets();
        SetStat();
    }
}
