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

    private void Awake()
    {
       
        Rain = false;
        Day = false;
        //RenderSettings.fog = false;
        random = Random.Range(0, 99);

        if (random >= 0 && random <= 49)
        {
            ScriptPrincipal.profile = DayProfile;
            Day = true;
        }

        else if (random >= 50 && random <= 74)
        {
            ScriptPrincipal.profile = NightProfile;
            Night = true;
        }

        else if (random >= 75 && random <= 89)
        {
            ScriptPrincipal.profile = RainProfile;
            Instantiate(RainPS, RainPS.transform.position, RainPS.transform.rotation);
            Rain = true;
        }

        else
        {
            ScriptPrincipal.profile = FogProfile;
            RenderSettings.fog = true;
        }
    }
}
