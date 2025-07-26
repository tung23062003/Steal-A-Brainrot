using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace KingCat.Base.Data
{
    [System.Serializable]
    public class UserProfile
    {
        public string id;
        public string name;
        public string avatar;
        public string country;
        public BigCurrency currency;
        public int cup;
        public long exp;
        public int petSlot;
        public int inventorySlot;
        public List<int> charSkins;
        public List<int> wingsSkins;
        public int wingsEquippedId; // add
        public List<PetSaveInfo> pets;
        public List<PetSaveInfo> petsEquiped;
        public List<int> petsIdUnlocked;
        public string data = "{}";
    }

    public class UserData : MonoSingleton<UserData>
    {
        private string LOCAL_KEY = "USER_DATA";
        public UserProfile userProfile;

        [HideInInspector]
        public UnityEvent<UserProfile> UserProfileChangedEvent = new UnityEvent<UserProfile>();

        //[SerializeField] private PetsDataSO petData;

#if UNITY_EDITOR
        [UnityEditor.MenuItem("User Data/Add 100 coins")]
#endif
        private static void TestAddCoin()
        {
            UserData.Instance.LoadUserProfile((userProfile) =>
            {
                userProfile.currency += new BigCurrency(100);
                UserData.Instance.SaveUserProfile("coin", userProfile.currency);
            });
        }

        public void AddCoin(BigCurrency coin)
        {
            var oldCoin = userProfile.currency;
            var newCoin = userProfile.currency + coin;
            //userProfile.currency += coin;
            UserData.Instance.SaveUserProfile("currency", newCoin);
            GameEvent.OnAddCoin.Invoke(oldCoin, newCoin);
        }

        //public void AddCup(int cup)
        //{
        //    var oldCup = userProfile.cup;
        //    var newCup = userProfile.cup + cup;

        //    UserData.Instance.SaveUserProfile("cup", newCup);
        //    GameEvent.OnAddCup.Invoke(oldCup, newCup);
        //}

        //public bool SubCoin(BigCurrency coin)
        //{
        //    if (userProfile.currency < coin) return false;
        //    userProfile.currency -= coin;
        //    UserData.Instance.SaveUserProfile("coin", userProfile.currency);
        //    return true;
        //}

        //public void AddPet(string commonIndex)
        //{
        //    if (userProfile.pets.Count >= userProfile.inventorySlot) return;
        //    string uniqueId = $"{commonIndex}-{MyUtils.GetRandomString()}";
        //    var newPet = new PetSaveInfo(commonIndex, uniqueId);
        //    userProfile.pets.Add(newPet);
        //    //UserData.Instance.SaveUserProfile("pets", new PetSaveInfo(commonIndex, uniqueId));
        //    PetDataWrapper petData = new() { pets = userProfile.pets };
        //    GameManager.Instance.SaveData("Data/pet_data.json", petData);
        //    //GameManager.Instance.SaveData("Data/pet_data.json", userProfile.pets);
        //    GameEvent.OnAddPet?.Invoke(newPet);
        //}

        //public bool DeletePet(PetSaveInfo petSaveInfo)
        //{
        //    var petRemoved = userProfile.pets.Find(item => item.uniqueID == petSaveInfo.uniqueID);
        //    if (petRemoved != null)
        //    {
        //        userProfile.pets.Remove(petRemoved);
        //        SetUserProfile(userProfile);
        //        UnEquipPet(petSaveInfo);
        //        PetDataWrapper petData = new() { pets = userProfile.pets };
        //        GameManager.Instance.SaveData("Data/pet_data.json", petData);

        //        return true;
        //    }
        //    return false;
        //}

        //public bool EquipPet(PetSaveInfo petSaveInfo)
        //{
        //    var pet = userProfile.pets.Find(item => item.uniqueID == petSaveInfo.uniqueID);
        //    if (pet != null)
        //    {
        //        UserData.Instance.SaveUserProfile("petsEquiped", pet);

        //        GameEvent.OnEquipPet?.Invoke(petSaveInfo);
        //        return true;
        //    }
        //    return false;
        //}

        //public bool UnEquipPet(PetSaveInfo petSaveInfo)
        //{
        //    //if (!userProfile.petsEquiped.Exists(item => item.uniqueID == petSaveInfo.uniqueID)) return false;

        //    var petUnequip = userProfile.petsEquiped.Find(item => item.uniqueID == petSaveInfo.uniqueID);
        //    if (petUnequip != null)
        //    {
        //        userProfile.petsEquiped.Remove(petUnequip);
        //        SetUserProfile(userProfile);

        //        GameEvent.OnUnequipPet?.Invoke(petSaveInfo);
        //        return true;
        //    }
        //    else
        //        return false;


        //}

        //public void UnEquipAllPet()
        //{
        //    var petEquiped = userProfile.petsEquiped;
        //    if (petEquiped.Count <= 0) return;

        //    foreach (var pet in petEquiped)
        //    {
        //        GameEvent.OnUnequipPet?.Invoke(pet);
        //    }

        //    userProfile.petsEquiped.Clear();
        //    SetUserProfile(userProfile);

        //    //GameEvent.OnChangePet?.Invoke();
        //}

        //public void CraftPet(PetSaveInfo petSaveInfo, UnityAction<List<PetSaveInfo>, PetSaveInfo> onComplete = null)
        //{
        //    var petRemoved = new List<PetSaveInfo>();

        //    for (int i = 0; i <= 2; i++)
        //    {
        //        var pet = userProfile.pets.FirstOrDefault(pet => pet.commonIndex == petSaveInfo.commonIndex && !pet.commonIndex.Contains("*"));
        //        userProfile.pets.Remove(pet);

        //        petRemoved.Add(pet);
        //        UnEquipPet(pet);
        //    }


        //    var upgradePetCommnetIndex = $"{petSaveInfo.commonIndex}*";

        //    var craftPet = new PetSaveInfo(upgradePetCommnetIndex, $"{upgradePetCommnetIndex}-{MyUtils.GetRandomString()}");
        //    userProfile.pets.Add(craftPet);
        //    PetDataWrapper petData = new() { pets = userProfile.pets };
        //    GameManager.Instance.SaveData("Data/pet_data.json", petData);

        //    onComplete?.Invoke(petRemoved, craftPet);
        //}

        //public void CraftPetADS(PetSaveInfo petSaveInfo, UnityAction<PetSaveInfo, PetSaveInfo> onComplete = null)
        //{
        //    var petRemoved = petSaveInfo;

        //    userProfile.pets.Remove(petSaveInfo);
        //    UnEquipPet(petSaveInfo);

        //    var upgradePetCommnetIndex = $"{petSaveInfo.commonIndex}*";

        //    var craftPet = new PetSaveInfo(upgradePetCommnetIndex, $"{upgradePetCommnetIndex}-{MyUtils.GetRandomString()}");
        //    userProfile.pets.Add(craftPet);
        //    PetDataWrapper petData = new() { pets = userProfile.pets };
        //    GameManager.Instance.SaveData("Data/pet_data.json", petData);

        //    onComplete?.Invoke(petRemoved, craftPet);
        //}

        //public async void EquipBest(UnityAction<List<PetSaveInfo>> onComplete = null)
        //{
        //    List<PetSaveInfo> bestPet = userProfile.pets
        //    .OrderByDescending(p =>
        //    {
        //        var baseValue = petData.GetPetByIndex(MyUtils.SubAsterisk(p.commonIndex)).itemInfo.statBonus.coin_multiplier.ToRawValue();

        //        return p.commonIndex.Contains("*") ? baseValue * 1.5f : baseValue;
        //    })
        //    .Take(userProfile.petSlot)
        //    .ToList();

        //    if (bestPet != null && bestPet.Count > 0)
        //    {
        //        UnEquipAllPet();
        //        foreach (var pet in bestPet)
        //        {
        //            EquipPet(pet);
        //            await UniTask.Yield();
        //        }
        //        await UniTask.Delay(100);
        //        onComplete?.Invoke(bestPet);
        //    }
        //}
        ////Kiểm tra xem pet đã mở khóa hay chưa
        //public bool IsPetUnlocked(int petId)
        //{
        //    return userProfile.petsIdUnlocked.Contains(petId);
        //}

        public void LoadUserProfile(UnityAction<UserProfile> callback)
        {
            var userProfileJson = "";
            if (!PlayerPrefs.HasKey(LOCAL_KEY))
            {
                var newUserProfile = new UserProfile();
                newUserProfile.id = "guest";
                newUserProfile.name = "Guest";
                string countryCode = "";
                string countryName = "";
                GameUtils.GetCountryName(out countryCode, out countryName);
                newUserProfile.country = countryCode;
                newUserProfile.avatar = "";
                newUserProfile.currency = GameConfigs.START_CURRENCY;
                newUserProfile.petSlot = 3;
                newUserProfile.inventorySlot = 500;
                newUserProfile.charSkins = new List<int> { 0 };
                newUserProfile.wingsSkins = new List<int> { };
                newUserProfile.wingsEquippedId = -1; // add
                newUserProfile.petsIdUnlocked = new List<int> { };

                //newUserProfile.pets = new List<PetSaveInfo> { };
                //newUserProfile.petsEquiped = new List<PetSaveInfo> { };
                userProfileJson = JsonUtility.ToJson(newUserProfile);
            }
            else userProfileJson = PlayerPrefs.GetString(LOCAL_KEY);

            try
            {
                userProfile = JsonUtility.FromJson<UserProfile>(userProfileJson);
                callback?.Invoke(userProfile);
            }
            catch
            {
                callback?.Invoke(null);
            }
        }

        public void SetUserProfile(UserProfile userProfile, UnityAction<UserProfile> callback = null)
        {
            //userProfile.pets.Clear();
            var userProfileJson = JsonUtility.ToJson(userProfile);
            PlayerPrefs.SetString(LOCAL_KEY, userProfileJson);
            PlayerPrefs.Save();

            callback?.Invoke(userProfile);
            UserProfileChangedEvent?.Invoke(userProfile);
        }

        public void SaveUserProfile(string type, object value, UnityAction<UserProfile> callback = null)
        {
            if (userProfile != null)
            {
                switch (type)
                {
                    case "id":
                        userProfile.id = (string)value;
                        break;
                    case "name":
                        userProfile.name = (string)value;
                        break;
                    case "avatar":
                        userProfile.avatar = (string)value;
                        break;
                    case "currency":
                        userProfile.currency = (BigCurrency)value;
                        break;
                    case "cup":
                        userProfile.cup = (int)value;
                        break;
                    case "exp":
                        userProfile.exp = (long)value;
                        break;
                    case "petSlot":
                        userProfile.petSlot = (int)value;
                        break;
                    case "inventorySlot":
                        userProfile.inventorySlot = (int)value;
                        break;
                    case "skin":
                        userProfile.charSkins.Add((int)value);
                        break;
                    case "wingsSkin":
                        userProfile.wingsSkins.Add((int)value);
                        break;
                    case "wingsEquippedId":    //add
                        userProfile.wingsEquippedId = (int)value;
                        break;
                    case "petsIdUnlocked":
                        userProfile.petsIdUnlocked.Add((int)value);
                        break;
                    //case "pets":
                    //    userProfile.pets.Add((PetSaveInfo)value);
                    //    break;
                    case "petsEquiped":
                        userProfile.petsEquiped.Add((PetSaveInfo)value);
                        break;
                    case "data":
                        userProfile.data = JsonUtility.ToJson(value);
                        break;
                }
                SetUserProfile(userProfile, callback);
            }
        }

        public T GetParam<T>(string key)
        {
            if (!string.IsNullOrEmpty(userProfile.data))
            {
                var jData = JObject.Parse(userProfile.data);
                if (jData != null)
                    if (jData.TryGetValue(key, out JToken value))
                    {
                        return value.ToObject<T>(); // Convert JToken to the specified type T
                    }
            }
            return default; // Return the default value for type T (null for reference types, 0 for int, etc.)
        }

        public void SetParam(string key, object value)
        {
            var jData = !string.IsNullOrEmpty(userProfile.data) ? JObject.Parse(userProfile.data) : new JObject();
            jData[key] = JToken.FromObject(value); // Add or update the key-value pair
            userProfile.data = jData.ToString(); // Update the original data string with the new JSON
            SetUserProfile(userProfile, null);
        }

    }
}

