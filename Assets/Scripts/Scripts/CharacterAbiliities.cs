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
        GameManager.totalCoins -= 150;
        SaveManager.Instance.Save();
    }

    public void UpgradeTitan()
    {
        Titan = true;
        GameManager.totalCoins -= 50;
        SaveManager.Instance.Save();
    }

    public void UpgradeResourceSpeedMultiplier()
    {
        bootsMovementMultiplier = 1.25f;
        GameManager.totalCoins -= 50;
        SaveManager.Instance.Save();
    }

    public void UpgradeCharacter()
    {
        dashActive = true;
        GameManager.totalCoins -= 150;
        SaveManager.Instance.Save();
    }

    public void UnlockMarket()
    {
        market = true;
        GameManager.totalCoins -= 100;
        SaveManager.Instance.Save();
    }

    public void UpgradeGoldMultiplier()
    {
        goldMultiplier = 2.0f;
        GameManager.totalCoins -= 150;
        SaveManager.Instance.Save();
    }
}
