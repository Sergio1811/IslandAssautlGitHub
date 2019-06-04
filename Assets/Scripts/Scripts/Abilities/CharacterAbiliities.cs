using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterAbiliities : MonoBehaviour
{
    public static CharacterAbiliities Instance { get; set; }

    public static bool Titan = false;
    public static float bootsMovementMultiplier = 1.0f;
    public static bool market = false;
    public static bool dashActive = false;
    public static float goldMultiplier = 1.0f;

    public static bool islandTier2 = false;
    public static bool initialized = false;
    

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
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "comerciante") == 1)
            market = true;
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "botas") == 1)
            bootsMovementMultiplier = 1.25f;
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "exprimir") == 1)
            goldMultiplier = 2.0f;
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "explorador") == 1)
            islandTier2 = true;
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "titan") == 1)
            Titan = true;

        initialized = true;
    }
}