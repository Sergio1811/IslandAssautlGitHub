using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AxerAbilities : MonoBehaviour
{
    public static AxerAbilities Instance { get; set; }

    public static bool Polivalente;
    public static float resourceSpeedMultiplier;
    public static bool axerTier2;
    public static bool axeStunt;
    public static float resourceMultiplier;

    public static bool treeTier2;
    public static bool initialized;
    
    
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
        else
            axeStunt = false;
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "doblefilo") == 1)
            axerTier2 = true;
        else
            axerTier2 = false;
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "eficaciamadera") == 1)
            resourceMultiplier = 1.5f;
        else
            resourceMultiplier = 1.0f;
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "eficienciamadera") == 1)
            resourceSpeedMultiplier = 0.5f;
        else
            resourceSpeedMultiplier = 1.0f;
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "maestrodelhacha") == 1)
            treeTier2 = true;
        else
            treeTier2 = false;
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "polivalentemadera") == 1)
            Polivalente = true;
        else
            Polivalente = false;

        initialized = true;
    }
}