using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AxerAbilities : MonoBehaviour
{
    public static AxerAbilities Instance { get; set; }

    public static bool Polivalente = false;
    public static float resourceSpeedMultiplier = 1.0f;
    public static bool axerTier2 = false;
    public static bool axeStunt = false;
    public static float resourceMultiplier = 1.0f;

    public static bool treeTier2 = false;
    public static bool initialized = false;
    
    
    private void Awake()
    {
        Instance = this;
        if (!initialized)
            PlayerPrefsInitialization();
    }


    void PlayerPrefsInitialization()
    {
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "conmocion") == 1)
            axeStunt = true;
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "doblefilo") == 1)
            axerTier2 = true;
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "eficaciamadera") == 1)
            resourceMultiplier = 1.5f;
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "eficienciamadera") == 1)
            resourceSpeedMultiplier = 0.5f;
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "maestrodelhacha") == 1)
            treeTier2 = true;
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "polivalentemadera") == 1)
            Polivalente = true;

        initialized = true;
    }
}