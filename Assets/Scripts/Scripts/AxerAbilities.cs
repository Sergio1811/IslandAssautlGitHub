using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxerAbilities : MonoBehaviour
{
    public static bool Polivalente = false;
    public static float resourceSpeedMultiplier = 1.0f;
    public static bool axerTier2 = false;
    public static bool axeStunt = false;
    public static float resourceMultiplier = 1.0f;

    public static bool treeTier2 = false;

    public void UpgradeTree()
    {
        treeTier2 = true;
    }

    public void UpgradePolivalente()
    {
        Polivalente = true;
    }

    public void UpgradeResourceSpeedMultiplier()
    {
        resourceSpeedMultiplier = 0.5f;
    }

    public void UpgradeAxe()
    {
        axerTier2 = true;
    }

    public void UpgradeStun()
    {
        axeStunt = true;
    }

    public void UpgradeResourceMultiplier()
    {
        resourceMultiplier = 1.5f;
    }
}
