using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public static class GameConstants
{
}


public class GameEvent
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Init()
    {
        MyUtils.ResetUnityEvent<GameEvent>();
    }

    public static UnityEvent RemoveSplashScreen = new();

    public static UnityEvent<GameObject> OnPlayerSpawn = new();
    public static UnityEvent OnWinLevel = new();

    public static UnityEvent<float> OnChangeSkin = new();
    public static UnityEvent OnDecreaseHP = new();

    public static UnityEvent<BigCurrency, BigCurrency> OnAddCoin = new();
    public static UnityEvent<int, int> OnAddCup = new();
    public static UnityEvent OnRevive = new();

    public static UnityEvent<EntityType, int> OnChangeWeapon = new();

    public static UnityEvent OnCheckpoint = new();

    public static UnityEvent OnUpJetpack = new();
    public static UnityEvent OnDownJetpack = new();

    public static UnityEvent OnDead = new();
    public static UnityEvent OnUnlockSkin = new();
    public static UnityEvent OnSuggestShield = new();
    public static UnityEvent OnAcceptShield = new();
    public static UnityEvent OnRejectShield = new();
    public static UnityEvent<string> OnAdsBreak = new();
    public static UnityEvent OnToggleInteractSkipBtn = new();
    public static UnityEvent<bool> OnToggleCanvas = new();

    public static UnityEvent<bool> OnInteractLadder = new();
    public static UnityEvent OnJumpOutFromLadder = new();
    public static UnityEvent OnAutoClimbClicked = new();
    public static UnityEvent OnPlayerGettingTired = new();
    public static UnityEvent OnPlayerNoLongerClimb = new();
    public static UnityEvent OnPlayerInputChange = new();
    public static UnityEvent<string> OnPlayerCoinGainChanged = new();
    public static UnityEvent OnInTopTower = new();
    public static UnityEvent OnSpawnCup = new();
    public static UnityEvent<PetSaveInfo> OnEquipPet = new();
    public static UnityEvent<PetSaveInfo> OnUnequipPet = new();
    public static UnityEvent<PetSaveInfo> OnAddPet = new();

    public static UnityEvent OnEquipWings = new();

    public static UnityEvent OnCoinBoosterEffect = new();
    public static UnityEvent OnSpeedBoosterEffect = new();
    public static UnityEvent OnLuckyBoosterEffect = new();

    public static UnityEvent OnCoinBooster = new();
    public static UnityEvent OnSpeedBooster = new();
    public static UnityEvent OnLuckyBooster = new();

    public static UnityEvent<string> OnGetPet = new();

    public static UnityEvent OnArrangePet = new();
    public static UnityEvent<RewardType, BigCurrency> OnGetReward = new();
    public static UnityEvent<TargetDirectionType> OnDirectionTutorial = new();

    public static UnityEvent OnFreezePlayer = new();
    public static UnityEvent OnUnFreezePlayer = new();

    public static UnityEvent OnLoadDataDone = new();
}

public enum RewardType
{
    COIN = 0,
    CUP = 1,
}

public enum EntityType
{
    NONE = 0,
    Player = 1,
    NPC = 2,
}

public enum EnemyType
{
    NONE = 0,
    NORMAL = 1,
    HERO = 2
}

public enum EndLevelType
{
    NONE = 0,
    WIN = 1,
    LOSE = 2,
}

public enum CountdownType
{
    NONE = 0,
    BEFORE_START = 1,
    DURING_GAME = 2,
    GREEN_LIGHT = 3,
    RED_LIGHT = 4,
}


public enum Direction { Left, Right, Forward }

public enum Map 
{
    LONDON = 0,
    NEWYORK = 1,
    HANOI = 2,
    HONGKONG = 3,
}

public enum BackOption
{
    Minigame = 0,
    Home = 1,
}

public enum GameMode
{
    Challenge = 0,
    Minigame = 1,
}

public enum LoseType
{
    Fall = 0,
    Timeout = 1,
}

public enum PlayerType
{
    Default = 0,
    Skate = 1,
}

public enum Rarity
{
    RARE = 0,
    EPIC = 1,
    LEGENDARY = 2,
    MYSTERIOUS = 3,
    IMMORTAL = 4,
}

public enum UnlockType
{
    Default,
    Coin,
    Ads,
    Both, // Coin + Ads
}


public enum GiftType
{
    Coin,
    BoosterCoin,
    BoosterSpeed,
    Lucky,
    Pet
}

public enum TargetDirectionType
{
    NONE = 0,
    WING = 1,
    EGG = 2,
}