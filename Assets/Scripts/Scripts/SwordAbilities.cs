using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwordAbilities : MonoBehaviour
{
    public static bool Polivalente = false;
    public static float resourceSpeedMultiplier = 1.0f;
    public static bool swordTier2 = false;
    public static bool swordSweep = false;
    public static float resourceMultiplier = 1.0f;

    public static bool enemyTier2 = false;

    public void UpgradeEnemy(GameObject g)
    {
        enemyTier2 = true;
        GameManager.totalCoins -= 150;
        GameManager.Instance.AbilitesCoinsUpdate();
        SaveManager.Instance.Save();
        g.GetComponentInChildren<Button>().interactable = false;
        ButtonManager.disabledButtonsList.Add(g.name);
    }

    public void UpgradePolivalente(GameObject g)
    {
        Polivalente = true;
        GameManager.totalCoins -= 50;
        GameManager.Instance.AbilitesCoinsUpdate();
        SaveManager.Instance.Save();
        g.GetComponentInChildren<Button>().interactable = false;
        ButtonManager.disabledButtonsList.Add(g.name);
    }

    public void UpgradeResourceSpeedMultiplier(GameObject g)
    {
        resourceSpeedMultiplier = 0.5f;
        GameManager.totalCoins -= 50;
        GameManager.Instance.AbilitesCoinsUpdate();
        SaveManager.Instance.Save();
        g.GetComponentInChildren<Button>().interactable = false;
        ButtonManager.disabledButtonsList.Add(g.name);
    }

    public void UpgradeSword(GameObject g)
    {
        swordTier2 = true;
        GameManager.totalCoins -= 100;
        GameManager.Instance.AbilitesCoinsUpdate();
        SaveManager.Instance.Save();
        g.GetComponentInChildren<Button>().interactable = false;
        ButtonManager.disabledButtonsList.Add(g.name);
    }

    public void UpgradeSwordSweep(GameObject g)
    {
        swordSweep = true;
        GameManager.totalCoins -= 150;
        GameManager.Instance.AbilitesCoinsUpdate();
        SaveManager.Instance.Save();
        g.GetComponentInChildren<Button>().interactable = false;
        ButtonManager.disabledButtonsList.Add(g.name);
    }

    public void UpgradeResourceMultiplier(GameObject g)
    {
        resourceMultiplier = 1.5f;
        GameManager.totalCoins -= 100;
        GameManager.Instance.AbilitesCoinsUpdate();
        SaveManager.Instance.Save();
        g.GetComponentInChildren<Button>().interactable = false;
        ButtonManager.disabledButtonsList.Add(g.name);
    }
}
