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
    }

    public void UpgradePolivalente()
    {
        Polivalente = true;
    }

    public void UpgradeResourceSpeedMultiplier()
    {
        resourceSpeedMultiplier = 0.5f;
    }

    public void UpgradeSword()
    {
        swordTier2 = true;
    }

    public void UpgradeSwordSweep()
    {
        swordSweep = true;
    }

    public void UpgradeResourceMultiplier()
    {
        resourceMultiplier = 1.5f;
    }
}
