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
    private void Awake()
    {
        Rain = false;
        random = Random.Range(0, 3);
        switch (random)
        {
            case 0:
                ScriptPrincipal.profile = DayProfile;
                break;

            case 1:
                ScriptPrincipal.profile = NightProfile;
                break;

            case 2:
                ScriptPrincipal.profile = RainProfile;
                Instantiate(RainPS, Vector3.zero, RainPS.transform.rotation);
                Rain = true;
                break;

            case 3:
                ScriptPrincipal.profile = FogProfile;
                break;

        }

    }

}
