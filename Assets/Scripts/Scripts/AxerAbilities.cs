using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxerAbilities : MonoBehaviour
{
    public static bool Polivalente = false;
    public static float resourceSpeedMultiplier = 1.0f;
    public static bool explosiveTier2 = false;
    public static bool bombKnockBack = false;
    public static float resourceMultiplier = 1.0f;

    public static bool rockTier2 = false;

    public void UpgradeRock()
    {
        rockTier2 = true;
    }

    public void UpgradePolivalente()
    {
        Polivalente = true;
    }

    public void UpgradeResourceSpeedMultiplier()
    {
        resourceSpeedMultiplier = 0.5f;
    }

    public void UpgradeExplosive()
    {
        explosiveTier2 = true;
    }

    public void UpgradeBombKnockBack()
    {
        bombKnockBack = true;
    }

    public void UpgradeResourceMultiplier()
    {
        resourceMultiplier = 1.5f;
    }
}
