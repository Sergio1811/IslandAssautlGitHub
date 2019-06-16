using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMODUnity;

public class SoundManager : MonoBehaviour
{
    [HideInInspector]
    public static string SwordSound, DashSound, BombDownSound, BombCountDownSound, BombExplosionSound, BombClickSound, HitSound, EnemyHurtSound, PlayerHurtSound;

    [HideInInspector]
    public static string PortalOpenSound, PortalFluctuationSound, FireSound, DeathSound, HitEnemySword, SwordStoneSound, HitTree, BreakTree, HitAir;

    [HideInInspector]
    public static string OpenChestSound, ButtonClicked,PassButton, CoinsSound, ObjectiveCompleteSound, FallingObject, UpgradeSkill;

    [HideInInspector]
    public static string SunSound, AldeaEnemigos, FogSetting, RainSetting, WinIsland, NeutralIsland, LoseIsland, PickUpSound, NightSound, TimeSound;

    static bool created = false;

    
    // Start is called before the first frame update
    void Awake()
    { 
        SwordSound = "event:/Vaiana/OnceSound/Sword/Sword";
        DashSound = "event:/BS/Character/moviments/velocitat/Dash";//Done
        BombDownSound = "event:/";
        BombCountDownSound = "event:/";
        BombExplosionSound = "event:/IslandAssault/OnceSound/Explosion/Explosion-sound";//Done
        BombClickSound = "";
        HitSound = "event:/IslandAssault/OnceSound/Axe_Enemy_Sound/Axe_Enemy_Sound";//done
        EnemyHurtSound = "event:/IslandAssault/OnceSound/Enem_hurt/Enemy_hurt";//done
        PlayerHurtSound = "event:/IslandAssault/OnceSound/Enem_hurt/Enemy_hurt";//done
        PortalOpenSound = "event:/BS/sfx/Portals/Portal_1";//done
        PortalFluctuationSound = "event:/BS/sfx/Portals/Portal_4_1seg";//done
        FireSound = "event:/IslandAssault/LongSound/Fire/Fire_Loop";//done
        DeathSound = "event:/IslandAssault/OnceSound/Death_Sound/Dead_Sound";//done
        HitEnemySword = "event:/IslandAssault/OnceSound/Enemy_hit/Enemy hit player";//done
        SwordStoneSound = "event:/IslandAssault/OnceSound/Enemy_hit/Enemy hit aldeano";//done
        HitTree = "event:/IslandAssault/OnceSound/Hit_Rock/Hit_Rock";//done
        BreakTree = "event:/BS/Character/moviments/pases/caminar/fulles";//done
        HitAir = "event:/BS/sfx/espases/hit_1";//done
        PlayerHurtSound = "event:/";

        OpenChestSound = "event:/IslandAssault/OnceSound/Open_Chest/Open_chest_sound";//done
        CoinsSound = "event:/IslandAssault/OnceSound/Coins/Trading_sound/Trading sound";//done
        ButtonClicked = "event:/BS/sfx/Botons/boto_2";//done
        ObjectiveCompleteSound = "event:/IslandAssault/UI/Objective_Completed/Objective_completed";//done
        FallingObject = "event:/BS/sfx/caida_objetos";//done
        PassButton = "event:/BS/sfx/Botons/boto_1";//done
        UpgradeSkill = "event:/BS/Eventuals/level_start";//done

        SunSound = "event:/BS/Ambient/Maritim/Mar";
        AldeaEnemigos = "event:/IslandAssault/AmbientSounds/Maori/Maori_Sound_Loop/Maori_Sound_Loop";
        FogSetting = "event:/IslandAssault/AmbientSounds/Wind/Wind_Blowing";
        RainSetting = "event:/IslandAssault/AmbientSounds/Maori/Maori_Percussion/Percussion sound2";
        WinIsland = "event:/BS/Eventuals/victory_level";//done
        NeutralIsland = "event:/BS/Eventuals/victory_game";
        LoseIsland = "event:/BS/Eventuals/game_over";//done
        PickUpSound = "event:/BS/sfx/agafarObjectes/pickup_2";//done
        NightSound = "event:/IslandAssault/AmbientSounds/Waves/Brave_Wave_Loop";
        TimeSound = "";

        if (!created)
        {
            DontDestroyOnLoad(gameObject);
            created = true;
        }

    }

    public static void PlayOneShot(string ClipName, Vector3 Position)
    { 
        FMODUnity.RuntimeManager.PlayOneShot(ClipName, Position);
    }
}
