using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BomberAbilities : MonoBehaviour
{
    public static BomberAbilities Instance { get; set; }

    public static bool Polivalente = false;
    public static float resourceSpeedMultiplier = 1.0f;
    public static bool explosiveTier2 = false;
    public static bool bombKnockBack = false;
    public static float resourceMultiplier = 1.0f;

    public static bool rockTier2 = false;
    public static bool initialized;
    

    private void Awake()
    {
        Instance = this;
        if (!initialized)
            PlayerPrefsInitialization();
    }


    void PlayerPrefsInitialization()
    {
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "empujon") == 1)
            bombKnockBack = true;
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "c4") == 1)
            explosiveTier2 = true;
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "eficaciarocas") == 1)
            resourceMultiplier = 1.5f;
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "eficienciarocas") == 1)
            resourceSpeedMultiplier = 0.5f;
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "artificiero") == 1)
            rockTier2 = true;
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "polivalenterocas") == 1)
            Polivalente = true;

        initialized = true;
    }
}