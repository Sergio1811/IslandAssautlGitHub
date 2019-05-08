using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberAbilities : MonoBehaviour
{
    public static bool Polivalente = false;
    public static float resourceSpeedMultiplier = 1.0f;
    public static bool explosiveTier2 = false;
    public static bool bombKnockBack = false;
    public static float resourceMultiplier = 1.0f;

    public static bool rockTier2 = false;

    public void UpgradeRock ()
    {
        rockTier2 = true;
        GameManager.totalCoins -= 150;
        SaveManager.Instance.Save();
    }

    public void UpgradePolivalente()
    {
        Polivalente = true;
        GameManager.totalCoins -= 50;
        SaveManager.Instance.Save();
    }

    public void UpgradeResourceSpeedMultiplier()
    {
        resourceSpeedMultiplier = 0.5f;
        GameManager.totalCoins -= 50;
        SaveManager.Instance.Save();
    }

    public void UpgradeExplosive()
    {
        explosiveTier2 = true;
        GameManager.totalCoins -= 100;
        SaveManager.Instance.Save();
    }

    public void UpgradeBombKnockBack()
    {
        bombKnockBack = true;
        GameManager.totalCoins -= 150;
        SaveManager.Instance.Save();
    }

    public void UpgradeResourceMultiplier()
    {
        resourceMultiplier = 1.5f;
        GameManager.totalCoins -= 100;
        SaveManager.Instance.Save();
    }

}
