using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterAbiliities : MonoBehaviour
{
    public static CharacterAbiliities Instance { get; set; }

    public static bool Titan;
    public static float bootsMovementMultiplier;
    public static bool market;
    public static bool dashActive;
    public static float goldMultiplier;

    public static bool islandTier2;
    public static bool initialized;
    

    private void Awake()
    {
        Instance = this;
        if (!initialized)
            PlayerPrefsInitialization();
    }


    void PlayerPrefsInitialization()
    {
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "dash") == 1)
            dashActive = true;
        else
            dashActive = false;
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "comerciante") == 1)
            market = true;
        else
            market = false;
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "botas") == 1)
            bootsMovementMultiplier = 1.25f;
        else
            bootsMovementMultiplier = 1.0f;
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "exprimir") == 1)
            goldMultiplier = 2.0f;
        else
            goldMultiplier = 1.0f;
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "explorador") == 1)
            islandTier2 = true;
        else
            islandTier2 = false;
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "titan") == 1)
            Titan = true;
        else
            Titan = false;

        initialized = true;
    }
}