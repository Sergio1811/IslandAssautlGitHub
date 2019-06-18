using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class ClimaRandom : MonoBehaviour
{
    public PostProcessingBehaviour ScriptPrincipal;

    public PostProcessingProfile DayProfile;
    public PostProcessingProfile NightProfile;
    public PostProcessingProfile RainProfile;
    public PostProcessingProfile FogProfile;

    public ParticleSystem RainPS;
    int random;
    public static bool Rain;
    public static bool Day;
    public static bool Night;
    public static bool Fog;



    private void Awake()
    {
       
        Rain = false;
        Day = false;
        Night = false;
        Fog = false;
        random = Random.Range(0, 101);

        if (random >= 0 && random <= 49)
        {
            ScriptPrincipal.profile = DayProfile;
            Day = true;
        }
        else if (random >= 50 && random <= 74)
        {
            ScriptPrincipal.profile = NightProfile;
            Night = true;
            GameObject.FindGameObjectWithTag("MainLight").GetComponent<Light>().shadowStrength = 0;
        }
        else if (random >= 75 && random <= 89)
        {
            ScriptPrincipal.profile = RainProfile;
            Instantiate(RainPS, RainPS.transform.position, RainPS.transform.rotation);
            Rain = true;
            GameObject.FindGameObjectWithTag("MainLight").GetComponent<Light>().shadowStrength = 0;
        }
        else
        {
            ScriptPrincipal.profile = FogProfile;
            RenderSettings.fog = true;
            Fog = true;
            GameObject.FindGameObjectWithTag("MainLight").GetComponent<Light>().shadowStrength = 0;
        }
    }
}
