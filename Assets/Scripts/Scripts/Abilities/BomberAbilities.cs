using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BomberAbilities : MonoBehaviour
{
    public static BomberAbilities Instance { get; set; }

    public static bool Polivalente;
    public static float resourceSpeedMultiplier;
    public static bool explosiveTier2;
    public static bool bombKnockBack;
    public static float resourceMultiplier;

    public static bool rockTier2;
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
        else
            bombKnockBack = false;
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "c4") == 1)
            explosiveTier2 = true;
        else
            explosiveTier2 = false;
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "eficaciarocas") == 1)
            resourceMultiplier = 1.5f;
        else
            resourceMultiplier = 1.0f;
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "eficienciarocas") == 1)
            resourceSpeedMultiplier = 0.5f;
        else
            resourceSpeedMultiplier = 1.0f;
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "artificiero") == 1)
            rockTier2 = true;
        else
            rockTier2 = false;
        if (PlayerPrefs.GetInt(ButtonManager.boughtString + "polivalenterocas") == 1)
            Polivalente = true;
        else
            Polivalente = false;

        initialized = true;
    }
}