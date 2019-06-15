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
   
    private void Awake()
    {
       
        Rain = false;
        Day = false;
        //RenderSettings.fog = false;
        random = Random.Range(0, 4);
        switch (random)
        {
            case 0:
                ScriptPrincipal.profile = DayProfile;
                Day = true;
               
                break;

            case 1:
                ScriptPrincipal.profile = NightProfile;
                break;

            case 2:
                ScriptPrincipal.profile = RainProfile;
                Instantiate(RainPS,RainPS.transform.position, RainPS.transform.rotation);
                Rain = true;
                break;

            case 3:
                ScriptPrincipal.profile = FogProfile;
                RenderSettings.fog = true; 
                break;

        }

    }

}
