using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMODUnity;

public class SoundManager : MonoBehaviour
{
    [HideInInspector]
    public string SwordSound, DashSound, BombDownSound, BombCountDownSound, BombExplosionSound, BombClickSound, HitSound, EnemyHurtSound, PlayerHurtSound;

    [HideInInspector]
    public string PortalOpenSound, PortalFluctuationSound, FireSound, DeathSound, HitEnemySword, SwordStoneSound, HitTree, BreakTree, HitAir;

    [HideInInspector]
    public string OpenChestSound, ButtonClicked,PassButton, CoinsSound, ObjectiveCompleteSound, FallingObject, UpgradeSkill;

    [HideInInspector]
    public string SunSound, AldeaEnemigos, FogSetting, RainSetting, WinIsland, NeutralIsland, LoseIsland, PickUpSound, NightSound;
    // Start is called before the first frame update
    void Awake()
    { 
        SwordSound = "event:/Vaiana/OnceSound/Sword/Sword";
        DashSound = "event:/BS/Character/moviments/velocitat/Dash";
        BombDownSound = "event:/";
        BombCountDownSound = "event:/";
        BombExplosionSound = "event:/IslandAssault/OnceSound/Explosion/Explosion-sound";
        BombClickSound = "";
        HitSound = "event:/IslandAssault/OnceSound/Axe_Enemy_Sound/Axe_Enemy_Sound";
        EnemyHurtSound = "event:/IslandAssault/OnceSound/Enem_hurt/Enemy_hurt";
        PlayerHurtSound = "event:/IslandAssault/OnceSound/Enem_hurt/Enemy_hurt";
        PortalOpenSound = "event:/BS/sfx/Portals/Portal_1";
        PortalFluctuationSound = "event:/BS/sfx/Portals/Portal_4_1seg";
        FireSound = "event:/IslandAssault/LongSound/Fire/Fire_Loop";
        DeathSound = "event:/IslandAssault/OnceSound/Death_Sound/Dead_Sound";
        HitEnemySword = "event:/IslandAssault/OnceSound/Enemy_hit/Enemy hit player";
        SwordStoneSound = "event:/IslandAssault/OnceSound/Enemy_hit/Enemy hit aldeano";
        HitTree = "event:/IslandAssault/OnceSound/Hit_Rock/Hit_Rock";
        BreakTree = "event:/BS/Character/moviments/pases/caminar/fulles";
        HitAir = "event:/BS/sfx/espases/hit_1";
        PlayerHurtSound = "event:/";

        OpenChestSound = "event:/IslandAssault/OnceSound/Open_Chest/Open_chest_sound";
        CoinsSound = "event:/IslandAssault/OnceSound/Coins/Trading_sound/Trading sound";
        ButtonClicked = "event:/BS/sfx/Botons/boto_2";
        ObjectiveCompleteSound = "event:/IslandAssault/UI/Objective_Completed/Objective_completed";
        FallingObject = "event:/BS/sfx/caida_objetos";
        PassButton = "event:/BS/sfx/Botons/boto_1";
        UpgradeSkill = "event:/BS/Eventuals/level_start";

        SunSound = "event:/BS/Ambient/Maritim/Mar";
        AldeaEnemigos = "event:/IslandAssault/AmbientSounds/Maori/Maori_Sound_Loop/Maori_Sound_Loop";
        FogSetting = "event:/IslandAssault/AmbientSounds/Wind/Wind_Blowing";
        RainSetting = "event:/IslandAssault/AmbientSounds/Maori/Maori_Percussion/Percussion sound2";
        WinIsland = "event:/BS/Eventuals/victory_level";
        NeutralIsland = "event:/BS/Eventuals/victory_game";
        LoseIsland = "event:/BS/Eventuals/game_over";
        PickUpSound = "event:/BS/sfx/agafarObjectes/pickup_4";
        NightSound = "event:/IslandAssault/AmbientSounds/Waves/Brave_Wave_Loop";
    }

    public void PlayOneShot(string ClipName, Vector3 Position)
    { 
        FMODUnity.RuntimeManager.PlayOneShot(ClipName, Position);
    }
}
