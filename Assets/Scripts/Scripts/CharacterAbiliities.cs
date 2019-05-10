using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterAbiliities : MonoBehaviour
{
    public static bool Titan = false;
    public static float bootsMovementMultiplier = 1.0f;
    public static bool market = false;
    public static bool dashActive = false;
    public static float goldMultiplier = 1.0f;

    public static bool islandTier2 = false;

    public void UpgradeRock(GameObject g)
    {
        islandTier2 = true;
        GameManager.totalCoins -= 150;
        GameManager.Instance.AbilitesCoinsUpdate();
        SaveManager.Instance.Save();
        g.GetComponentInChildren<Button>().interactable = false;
        ButtonManager.disabledButtonsList.Add(g.name);
    }

    public void UpgradeTitan(GameObject g)
    {
        Titan = true;
        GameManager.totalCoins -= 50;
        GameManager.Instance.AbilitesCoinsUpdate();
        SaveManager.Instance.Save();
        g.GetComponentInChildren<Button>().interactable = false;
        ButtonManager.disabledButtonsList.Add(g.name);
    }

    public void UpgradeResourceSpeedMultiplier(GameObject g)
    {
        bootsMovementMultiplier = 1.25f;
        GameManager.totalCoins -= 50;
        GameManager.Instance.AbilitesCoinsUpdate();
        SaveManager.Instance.Save();
        g.GetComponentInChildren<Button>().interactable = false;
        ButtonManager.disabledButtonsList.Add(g.name);
    }

    public void UpgradeCharacter(GameObject g)
    {
        dashActive = true;
        GameManager.totalCoins -= 150;
        GameManager.Instance.AbilitesCoinsUpdate();
        SaveManager.Instance.Save();
        g.GetComponentInChildren<Button>().interactable = false;
        ButtonManager.disabledButtonsList.Add(g.name);
    }

    public void UnlockMarket(GameObject g)
    {
        market = true;
        GameManager.totalCoins -= 100;
        GameManager.Instance.AbilitesCoinsUpdate();
        SaveManager.Instance.Save();
        g.GetComponentInChildren<Button>().interactable = false;
        ButtonManager.disabledButtonsList.Add(g.name);
    }

    public void UpgradeGoldMultiplier(GameObject g)
    {
        goldMultiplier = 2.0f;
        GameManager.totalCoins -= 150;
        GameManager.Instance.AbilitesCoinsUpdate();
        SaveManager.Instance.Save();
        g.GetComponentInChildren<Button>().interactable = false;
        ButtonManager.disabledButtonsList.Add(g.name);
    }
}
