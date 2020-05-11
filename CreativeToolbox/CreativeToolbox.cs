using System;
using EXILED;
using Harmony;

namespace CreativeToolbox
{
    public class CreativeToolbox : EXILED.Plugin
    {

        public static float MedkitAHPHealthValue;
        public static float PainkillerAHPHealthValue;
        public static float AdrenalineAHPHealthValue;
        public static float SCP500AHPHealthValue;
        public static float RandomRespawnTimer;
        public static float FragGrenadeFuseTimer;
        public static float FlashGrenadeFuseTimer;
        public static float HPRegenerationTimer;
        public static float HPRegenerationValue;
        public static float HPRegenerationIfHit;
        bool IsToolboxEnabled = true;
        public static bool EnableGrenadeTimeMod = false;
        public static bool EnableMedicalItemMod = false;
        public static bool EnableFallDamagePrevent = false;
        public static bool EnableGrenadeDamagePrevent = false;
        internal const string pluginVersion = "1.0";
        internal const string pluginPrefix = "CT";
        public static HarmonyInstance HarmonyInstance { private set; get; }
        public static int harmonyCounter;
        public CreativeToolboxEventHandler Handler;
        public override string getName => "CreativeToolbox";

        public void ReloadConfig()
        {
            CheckValues();
            CheckPluginFeatures();
            if (!IsToolboxEnabled)
                Log.Info("Plugin disabled!");
        }

        public void CheckPluginFeatures()
        {
            IsToolboxEnabled = Config.GetBool("creativetoolbox_enable", true);
            EnableGrenadeTimeMod = Config.GetBool("ct_enable_custom_grenade_time", false);
            EnableMedicalItemMod = Config.GetBool("ct_enable_custom_healing", false);
            EnableFallDamagePrevent = Config.GetBool("ct_enable_fall_damage_prevention", false);
            EnableGrenadeDamagePrevent = Config.GetBool("ct_enable_grenade_damage_prevention", false);
        }

        public void CheckValues()
        {
            if (float.TryParse(Config.GetFloat("ct_random_respawn_timer").ToString(), out float respawn) && respawn > 0)
                RandomRespawnTimer = respawn;
            else
            {
                Log.Info("Detected invalid value in the configuration file for auto respawn timer! Using default value of 0.05");
                RandomRespawnTimer = 0.05f;
                Config.SetString("ct_random_respawn_timer", RandomRespawnTimer.ToString());
            }

            if (float.TryParse(Config.GetFloat("ct_frag_grenade_fuse_timer").ToString(), out float fgnade) && fgnade > 0)
                FragGrenadeFuseTimer = fgnade;
            else
            {
                Log.Info("Detected invalid value in the configuration file for frag grenade fuse! Using default value of 5");
                FragGrenadeFuseTimer = 5;
                Config.SetString("ct_frag_grenade_fuse_timer", FragGrenadeFuseTimer.ToString());
            }

            if (float.TryParse(Config.GetFloat("ct_flash_grenade_fuse_timer").ToString(), out float flnade) && flnade > 0)
                FlashGrenadeFuseTimer = flnade;
            else
            {
                Log.Info("Detected invalid value in the configuration file for flash grenade fuse! Using default value of 3");
                FlashGrenadeFuseTimer = 3;
                Config.SetString("ct_flash_grenade_fuse_timer", FlashGrenadeFuseTimer.ToString());
            }

            if (float.TryParse(Config.GetFloat("ct_regeneration_time").ToString(), out float rgn_t) && rgn_t > 0)
                HPRegenerationTimer = rgn_t;
            else
            {
                Log.Info("Detected invalid value in the configuration file for regeneration time! Using default value of 5");
                HPRegenerationTimer = 5;
                Config.SetString("ct_regeneration_time", HPRegenerationTimer.ToString());
            }

            if (float.TryParse(Config.GetFloat("ct_regeneration_value").ToString(), out float rgn_v) && rgn_v > 0)
                HPRegenerationValue = rgn_v;
            else
            {
                Log.Info("Detected invalid value in the configuration file for hp regeneration! Using default value of 5");
                HPRegenerationValue = 5;
                Config.SetString("ct_regeneration_value", HPRegenerationValue.ToString());
            }


            if (float.TryParse(Config.GetFloat("ct_painkillers_ahp_healing").ToString(), out float pain_ahpheal) && pain_ahpheal >= 0)
                PainkillerAHPHealthValue = pain_ahpheal;
            else
            {
                Log.Info("Detected invalid value in the configuration file for painkiller ahp healing! Using default value of 0");
                PainkillerAHPHealthValue = 0;
            }

            if (float.TryParse(Config.GetFloat("ct_medkit_ahp_healing").ToString(), out float med_ahpheal) && med_ahpheal >= 0)
                MedkitAHPHealthValue = med_ahpheal;
            else
            {
                Log.Info("Detected invalid value in the configuration file for medkit ahp healing! Using default value of 0");
                MedkitAHPHealthValue = 0;
            }

            if (float.TryParse(Config.GetFloat("ct_adrenaline_ahp_healing").ToString(), out float adr_ahpheal) && adr_ahpheal >= 0)
                AdrenalineAHPHealthValue = adr_ahpheal;
            else
            {
                Log.Info("Detected invalid value in the configuration file for adrenaline ahp healing! Using default value of 0");
                AdrenalineAHPHealthValue = 0;
            }

            if (float.TryParse(Config.GetFloat("ct_scp500_ahp_healing").ToString(), out float scp500_ahpheal) && scp500_ahpheal >= 0)
                SCP500AHPHealthValue = scp500_ahpheal;
            else
            {
                Log.Info("Detected invalid value in the configuration file for SCP-500 ahp healing! Using default value of 0");
                SCP500AHPHealthValue = 0;
            }

            if (float.TryParse(Config.GetFloat("ct_bloodthirst_hp_healing").ToString(), out float bt_heal) && bt_heal >= 0)
                HPRegenerationIfHit = bt_heal;
            else
            {
                Log.Info("Detected invalid value in the configuration file for bloodthirsty healing! Using default value of 0");
                HPRegenerationIfHit = 0;
            }
        }

        public override void OnDisable()
        {
            Log.Info("Disabling \"CreativeToolbox\"");
            if (HarmonyInstance != null || HarmonyInstance != default)
            {
                HarmonyInstance.UnpatchAll();
            }
            Events.RoundStartEvent -= Handler.RunOnRoundStart;
            Events.RoundRestartEvent -= Handler.RunOnRoundRestart;
            Events.RemoteAdminCommandEvent -= Handler.RunOnRemoteAdminCommand;
            Events.PlayerSpawnEvent -= Handler.RunOnPlayerSpawn;
            Events.PlayerDeathEvent -= Handler.RunOnPlayerDeath;
            Events.PlayerHurtEvent -= Handler.RunOnPlayerHurt;
            Events.UsedMedicalItemEvent -= Handler.RunOnMedItemUsed;
            Events.PlayerLeaveEvent -= Handler.RunOnPlayerLeave;
            Handler = null;
        }

        public override void OnEnable()
        {
            ReloadConfig();
            if (!IsToolboxEnabled)
                return;

            Log.Info("Starting up \"CreativeToolbox\"! (Created by DefyTheRush)");
            Handler = new CreativeToolboxEventHandler();
            harmonyCounter++;
            HarmonyInstance = HarmonyInstance.Create($"defytherush.creativetoolbox_{harmonyCounter}");
            HarmonyInstance.PatchAll();
            Events.PlayerLeaveEvent += Handler.RunOnPlayerLeave;
            Events.UsedMedicalItemEvent += Handler.RunOnMedItemUsed;
            Events.PlayerHurtEvent += Handler.RunOnPlayerHurt;
            Events.PlayerDeathEvent += Handler.RunOnPlayerDeath;
            Events.PlayerSpawnEvent += Handler.RunOnPlayerSpawn;
            Events.RemoteAdminCommandEvent += Handler.RunOnRemoteAdminCommand;
            Events.RoundRestartEvent += Handler.RunOnRoundRestart;
            Events.RoundStartEvent += Handler.RunOnRoundStart;
        }

        public override void OnReload() { }
    }
}
