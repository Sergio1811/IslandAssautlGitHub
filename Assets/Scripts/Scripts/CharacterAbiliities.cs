using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAbiliities : MonoBehaviour
{
    public static bool Titan = false;
    public static float bootsMovementMultiplier = 1.0f;
    public static bool market = false;
    public static bool dashActive = false;
    public static float goldMultiplier = 1.0f;

    public static bool islandTier2 = false;

    public void UpgradeRock()
    {
        islandTier2 = true;
    }

    public void UpgradeTitan()
    {
        Titan = true;
    }

    public void UpgradeResourceSpeedMultiplier()
    {
        bootsMovementMultiplier = 1.25f;
    }

    public void UpgradeCharacter()
    {
        dashActive = true;
    }

    public void UnlockMarket()
    {
        market = true;
    }

    public void UpgradeGoldMultiplier()
    {
        goldMultiplier = 2.0f;
    }
}
