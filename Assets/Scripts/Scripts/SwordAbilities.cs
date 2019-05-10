using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAbilities : MonoBehaviour
{
    public static bool Polivalente = false;
    public static float resourceSpeedMultiplier = 1.0f;
    public static bool swordTier2 = false;
    public static bool swordSweep = false;
    public static float resourceMultiplier = 1.0f;

    public static bool enemyTier2 = false;

    public void UpgradeEnemy()
    {
        enemyTier2 = true;
        GameManager.totalCoins -= 150;
        GameManager.Instance.AbilitesCoinsUpdate();
        SaveManager.Instance.Save();
    }

    public void UpgradePolivalente()
    {
        Polivalente = true;
        GameManager.totalCoins -= 50;
        GameManager.Instance.AbilitesCoinsUpdate();
        SaveManager.Instance.Save();
    }

    public void UpgradeResourceSpeedMultiplier()
    {
        resourceSpeedMultiplier = 0.5f;
        GameManager.totalCoins -= 50;
        GameManager.Instance.AbilitesCoinsUpdate();
        SaveManager.Instance.Save();
    }

    public void UpgradeSword()
    {
        swordTier2 = true;
        GameManager.totalCoins -= 100;
        GameManager.Instance.AbilitesCoinsUpdate();
        SaveManager.Instance.Save();
    }

    public void UpgradeSwordSweep()
    {
        swordSweep = true;
        GameManager.totalCoins -= 150;
        GameManager.Instance.AbilitesCoinsUpdate();
        SaveManager.Instance.Save();
    }

    public void UpgradeResourceMultiplier()
    {
        resourceMultiplier = 1.5f;
        GameManager.totalCoins -= 100;
        GameManager.Instance.AbilitesCoinsUpdate();
        SaveManager.Instance.Save();
    }
}
