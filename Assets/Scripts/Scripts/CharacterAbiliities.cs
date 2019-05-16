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
        if (GameManager.totalCoins - 500 >= 0)
        {
            islandTier2 = true;
            GameManager.Instance.islandTier2 = true;
            GameManager.totalCoins -= 500;
            GameManager.Instance.AbilitesCoinsUpdate();
            SaveManager.Instance.Save();
            g.GetComponentInChildren<Button>().interactable = false;
            ButtonManager.disabledButtonsList.Add(g.name);
        }
    }

    public void UpgradeTitan(GameObject g)
    {
        if (GameManager.totalCoins - 50 >= 0)
        {
            Titan = true;
            GameManager.totalCoins -= 50;
            GameManager.Instance.AbilitesCoinsUpdate();
            SaveManager.Instance.Save();
            g.GetComponentInChildren<Button>().interactable = false;
            ButtonManager.disabledButtonsList.Add(g.name);
        }
    }

    public void UpgradeResourceSpeedMultiplier(GameObject g)
    {
        if (GameManager.totalCoins - 50 >= 0)
        {
            bootsMovementMultiplier = 1.25f;
            GameManager.totalCoins -= 50;
            GameManager.Instance.AbilitesCoinsUpdate();
            SaveManager.Instance.Save();
            g.GetComponentInChildren<Button>().interactable = false;
            ButtonManager.disabledButtonsList.Add(g.name);
        }
    }

    public void UpgradeCharacter(GameObject g)
    {
        if (GameManager.totalCoins - 250 >= 0)
        {
            dashActive = true;
            GameManager.totalCoins -= 250;
            GameManager.Instance.AbilitesCoinsUpdate();
            SaveManager.Instance.Save();
            g.GetComponentInChildren<Button>().interactable = false;
            ButtonManager.disabledButtonsList.Add(g.name);
        }
    }

    public void UnlockMarket(GameObject g)
    {
        if (GameManager.totalCoins - 150 >= 0)
        {
            market = true;
            GameManager.totalCoins -= 150;
            GameManager.Instance.AbilitesCoinsUpdate();
            SaveManager.Instance.Save();
            g.GetComponentInChildren<Button>().interactable = false;
            ButtonManager.disabledButtonsList.Add(g.name);
        }
    }

    public void UpgradeGoldMultiplier(GameObject g)
    {
        if (GameManager.totalCoins - 250 >= 0)
        {
            goldMultiplier = 2.0f;
            GameManager.totalCoins -= 250;
            GameManager.Instance.AbilitesCoinsUpdate();
            SaveManager.Instance.Save();
            g.GetComponentInChildren<Button>().interactable = false;
            ButtonManager.disabledButtonsList.Add(g.name);
        }
    }
}
