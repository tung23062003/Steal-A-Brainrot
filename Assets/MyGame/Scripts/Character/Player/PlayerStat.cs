using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    //[ReadOnly] public BigCurrency powerBonus = new(0);
    //[ReadOnly] public BigCurrency coinMultiplier = new(1);

    //private PlayerClimbLadder playerClimbLadder;
    //private float originalClimbSpeed;
    //private BigCurrency originWingCoinMultiplier = new(0);
    //private BigCurrency originPetCoinMultiplier = new(0);
    //private BigCurrency originCoinMultiplier = new(0);

    //private BigCurrency boosterOriginWingCoinMultiplier = new(0);
    //private BigCurrency boosterOriginPetCoinMultiplier = new(0);
    //private float boosterClimbSpeed;

    //private void Awake()
    //{
    //    playerClimbLadder = GetComponent<PlayerClimbLadder>();

    //    originalClimbSpeed = playerClimbLadder.climbSpeed;
    //    originCoinMultiplier = coinMultiplier;
    //}

    //public void ExchangePowerToSpeedAndCoin()
    //{
    //    originWingCoinMultiplier = originCoinMultiplier;
    //    playerClimbLadder.climbSpeed = originalClimbSpeed;
    //    var speedBoostPercent = BigCurrency.Clamp(powerBonus * 0.02f, new BigCurrency(0f), new BigCurrency(100f));
    //    var coinBoostPercent = powerBonus * 0.05f;

    //    var currentSpeed = playerClimbLadder.climbSpeed;
    //    var newSpeed = (speedBoostPercent + 1) * currentSpeed;

    //    originWingCoinMultiplier *= (1.0f + coinBoostPercent);
    //    playerClimbLadder.climbSpeed = (float)newSpeed.ToRawValue();

    //    playerClimbLadder.ResetClimbSpeed();
    //    //originalClimbSpeed = playerClimbLadder.climbSpeed;
    //    //originWingCoinMultiplier = coinMultiplier;
    //    SetCoinMultiplier();
    //}

    //public void SetPower(BigCurrency bonus)
    //{
    //    powerBonus = bonus;

    //    ExchangePowerToSpeedAndCoin();
    //}

    //public void SetSpeedBonus(float bonus)
    //{
    //    boosterClimbSpeed = playerClimbLadder.climbSpeed;
    //    playerClimbLadder.climbSpeed = boosterClimbSpeed * (bonus + 1);
    //    //playerClimbLadder.climbSpeed = (float)newSpeed.ToRawValue();
    //    playerClimbLadder.ResetClimbSpeed();
    //}

    //public void SetSpeedPenalty(float penalty)
    //{
    //    //var newSpeed = originalClimbSpeed - (penalty * originalClimbSpeed);
    //    playerClimbLadder.climbSpeed = playerClimbLadder.climbSpeed - boosterClimbSpeed * penalty;
    //    playerClimbLadder.ResetClimbSpeed();
    //}

    //public void ResetToOriginSpeed()
    //{
    //    playerClimbLadder.climbSpeed = originalClimbSpeed;
    //    playerClimbLadder.ResetClimbSpeed();
    //}

    //public void SetCoinMultiplier()
    //{
    //    coinMultiplier = originWingCoinMultiplier + originPetCoinMultiplier;
    //}

    //public void SetCoinMultiplierBonus(BigCurrency bonus)
    //{
    //    //coinMultiplier *= (bonus + 1);
    //    boosterOriginWingCoinMultiplier = originWingCoinMultiplier;
    //    boosterOriginPetCoinMultiplier = originPetCoinMultiplier;

    //    originWingCoinMultiplier = boosterOriginWingCoinMultiplier * (bonus + 1);
    //    originPetCoinMultiplier = boosterOriginPetCoinMultiplier * (bonus + 1);
    //    coinMultiplier = originWingCoinMultiplier + originPetCoinMultiplier;

    //    if (coinMultiplier < new BigCurrency(1.5f))
    //        coinMultiplier = new BigCurrency(1.5f);
    //}


    //public void SetCoinMultiplierPenalty(BigCurrency penalty)
    //{
    //    //coinMultiplier -= coinMultiplier * bonus;
    //    originWingCoinMultiplier = originWingCoinMultiplier - boosterOriginWingCoinMultiplier * penalty;
    //    originPetCoinMultiplier = originPetCoinMultiplier - boosterOriginPetCoinMultiplier * penalty;
    //    coinMultiplier = originWingCoinMultiplier + originPetCoinMultiplier;

    //    if (coinMultiplier < new BigCurrency(1.5f))
    //        coinMultiplier = new BigCurrency(1.0f);
    //}

    //public void SetCoinMultiplierPet(BigCurrency bonus)
    //{
    //    originPetCoinMultiplier = bonus;
    //    coinMultiplier = originWingCoinMultiplier + bonus;
    //}

    //public void SetCoinMultiplier(BigCurrency coin)
    //{
    //    if (coin <= new BigCurrency(1)) coin = new(1);
    //    coinMultiplier = coin;
    //}
}
