using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwordAbilities : MonoBehaviour
{
    public static SwordAbilities Instance { get; set; }

    public static bool Polivalente = false;
    public static float resourceSpeedMultiplier = 1.0f;
    public static bool swordTier2 = false;
    public static bool swordSweep = false;
    public static float resourceMultiplier = 1.0f;

    public static bool enemyTier2 = false;
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
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "espadon") == 1)
            swordTier2 = true;
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "eficaciaataque") == 1)
            resourceMultiplier = 1.5f;
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "eficienciaataque") == 1)
            resourceSpeedMultiplier = 0.5f;
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "gladiador") == 1)
            enemyTier2 = true;
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "polivalenteataque") == 1)
            Polivalente = true;

        initialized = true;
    }
}