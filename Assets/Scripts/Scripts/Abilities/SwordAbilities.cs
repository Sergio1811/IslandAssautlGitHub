using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwordAbilities : MonoBehaviour
{
    public static SwordAbilities Instance { get; set; }

    public static bool Polivalente;
    public static float resourceSpeedMultiplier;
    public static bool swordTier2;
    public static bool swordSweep;
    public static float resourceMultiplier;

    public static bool enemyTier2;
    public static bool initialized;

    
    private void Awake()
    {
        Instance = this;
        if (!initialized)
            PlayerPrefsInitialization();
    }


    void PlayerPrefsInitialization()
    {
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "barrido") == 1)
            swordSweep = true;
        else
            swordSweep = false;
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "espadon") == 1)
            swordTier2 = true;
        else
            swordTier2 = false;
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "eficaciaataque") == 1)
            resourceMultiplier = 1.5f;
        else
            resourceMultiplier = 1.0f;
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "eficienciaataque") == 1)
            resourceSpeedMultiplier = 0.5f;
        else
            resourceSpeedMultiplier = 1.0f;
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "gladiador") == 1)
            enemyTier2 = true;
        else
            enemyTier2 = false;
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "polivalenteataque") == 1)
            Polivalente = true;
        else
            Polivalente = false;

        initialized = true;
    }
}