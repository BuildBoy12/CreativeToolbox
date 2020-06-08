using Exiled.API.Features;
using Exiled.API.Interfaces;
using Exiled.Loader;
using System;

namespace CreativeToolbox
{
    public class CreativeConfig : IConfig
    {
        public bool IsToolboxEnabled = true;
        public float MedkitAHPHealthValue;
        public float PainkillerAHPHealthValue;
        public float AdrenalineAHPHealthValue;
        public float SCP500AHPHealthValue;
        public float RandomRespawnTimer;
        public float FragGrenadeFuseTimer;
        public float FlashGrenadeFuseTimer;
        public float HPRegenerationTimer;
        public float HPRegenerationValue;
        public float HPRegenerationIfHit;
        public float AutoScaleValue;
        public float GrenadeDeathTimer;
        public int SCP096AHP;
        public bool EnableGrenadeTimeMod = false;
        public bool EnableMedicalItemMod = false;
        public bool EnableFallDamagePrevent = false;
        public bool EnableGrenadeDamagePrevent = false;
        public bool EnableAutoScaling = false;
        public bool EnableRetainingScaling = false;
        public bool EnableGrenadeSpawnOnDeath = false;
        public bool EnableGrenadeRangeMod = false;
        public bool EnableDoorMessages = false;
        public bool EnableDamageMessage = false;
        public bool DisableAutoScalingMessage = false;
        public bool LockFallDamageMod = false;
        public bool SCP018WarheadBounce = false;
        public string LockedDoorMessage;
        public string UnlockedDoorMessage;
        public string NeedKeycardMessage;
        public string BypassKeycardMessage;
        public string BypassWithKeycardInHandMessage;
        public string PryGatesMessage;
        public string PryGatesBypassMessage;
        public string DamageMessage;

        public bool IsEnabled { get; set; } = PluginManager.YamlConfig.GetBool("creativetoolbox_enable", false);

        public string Prefix => "creativetoolbox_";

        public void Reload()
        {
            EnableGrenadeTimeMod = PluginManager.YamlConfig.GetBool("ct_enable_custom_grenade_time", false);
            EnableMedicalItemMod = PluginManager.YamlConfig.GetBool("ct_enable_custom_healing", false);
            EnableFallDamagePrevent = PluginManager.YamlConfig.GetBool("ct_enable_fall_damage_prevention", false);
            EnableGrenadeDamagePrevent = PluginManager.YamlConfig.GetBool("ct_enable_grenade_damage_prevention", false);
            EnableAutoScaling = PluginManager.YamlConfig.GetBool("ct_enable_auto_scaling", false);
            EnableRetainingScaling = PluginManager.YamlConfig.GetBool("ct_enable_keep_scale", false);
            EnableGrenadeSpawnOnDeath = PluginManager.YamlConfig.GetBool("ct_enable_grenade_on_death", false);
            EnableDoorMessages = PluginManager.YamlConfig.GetBool("ct_enable_door_messages", false);
            EnableDamageMessage = PluginManager.YamlConfig.GetBool("ct_enable_damage_message", false);
            EnableGrenadeRangeMod = PluginManager.YamlConfig.GetBool("ct_enable_custom_grenade_range", false);
            SCP018WarheadBounce = PluginManager.YamlConfig.GetBool("ct_enable_scp018_warhead_bounce", false);
            DisableAutoScalingMessage = PluginManager.YamlConfig.GetBool("ct_disable_autoscale_messages", false);
            LockFallDamageMod = PluginManager.YamlConfig.GetBool("ct_disable_fall_modification", false);
            LockedDoorMessage = PluginManager.YamlConfig.GetString("ct_locked_door_message", "you need a better keycard to open this door!");
            UnlockedDoorMessage = PluginManager.YamlConfig.GetString("ct_unlocked_door_message", "you held the keycard next to the reader");
            NeedKeycardMessage = PluginManager.YamlConfig.GetString("ct_need_keycard_message", "this door requires a keycard");
            BypassKeycardMessage = PluginManager.YamlConfig.GetString("ct_bypass_keycard_message", "you bypassed the reader");
            BypassWithKeycardInHandMessage = PluginManager.YamlConfig.GetString("ct_bypass_with_keycard_message", "you bypassed the reader, but you did not need a keycard");
            PryGatesMessage = PluginManager.YamlConfig.GetString("ct_pry_gate_message", "you pried the gate open");
            PryGatesBypassMessage = PluginManager.YamlConfig.GetString("ct_pry_gate_bypass_message", "you pried the gate open, but you could bypass it");
            if (EnableGrenadeRangeMod && EnableGrenadeDamagePrevent)
            {
                Log.Info("WARNING: You have both Grenade Damage Prevention and Grenade Damage Range Modification on");
                Log.Info("WARNING: Damage from modifying range will not take effect");
            }
            if (float.TryParse(PluginManager.YamlConfig.GetFloat("ct_random_respawn_timer").ToString(), out float respawn) && respawn > 0)
                RandomRespawnTimer = respawn;
            else
            {
                Log.Info("Detected invalid value in the configuration file for auto respawn timer! Using default value of 0.05");
                RandomRespawnTimer = 0.05f;
                PluginManager.YamlConfig.SetString("ct_random_respawn_timer", RandomRespawnTimer.ToString());
            }

            if (float.TryParse(PluginManager.YamlConfig.GetFloat("ct_frag_grenade_fuse_timer").ToString(), out float fgnade) && fgnade > 0)
                FragGrenadeFuseTimer = fgnade;
            else
            {
                Log.Info("Detected invalid value in the configuration file for frag grenade fuse! Using default value of 5");
                FragGrenadeFuseTimer = 5;
                PluginManager.YamlConfig.SetString("ct_frag_grenade_fuse_timer", FragGrenadeFuseTimer.ToString());
            }

            if (float.TryParse(PluginManager.YamlConfig.GetFloat("ct_flash_grenade_fuse_timer").ToString(), out float flnade) && flnade > 0)
                FlashGrenadeFuseTimer = flnade;
            else
            {
                Log.Info("Detected invalid value in the configuration file for flash grenade fuse! Using default value of 3");
                FlashGrenadeFuseTimer = 3;
                PluginManager.YamlConfig.SetString("ct_flash_grenade_fuse_timer", FlashGrenadeFuseTimer.ToString());
            }

            if (float.TryParse(PluginManager.YamlConfig.GetFloat("ct_regeneration_time").ToString(), out float rgn_t) && rgn_t > 0)
                HPRegenerationTimer = rgn_t;
            else
            {
                Log.Info("Detected invalid value in the configuration file for regeneration time! Using default value of 5");
                HPRegenerationTimer = 5;
                PluginManager.YamlConfig.SetString("ct_regeneration_time", HPRegenerationTimer.ToString());
            }

            if (float.TryParse(PluginManager.YamlConfig.GetFloat("ct_regeneration_value").ToString(), out float rgn_v) && rgn_v > 0)
                HPRegenerationValue = rgn_v;
            else
            {
                Log.Info("Detected invalid value in the configuration file for hp regeneration! Using default value of 5");
                HPRegenerationValue = 5;
                PluginManager.YamlConfig.SetString("ct_regeneration_value", HPRegenerationValue.ToString());
            }


            if (float.TryParse(PluginManager.YamlConfig.GetFloat("ct_painkillers_ahp_healing").ToString(), out float pain_ahpheal) && pain_ahpheal >= 0)
                PainkillerAHPHealthValue = pain_ahpheal;
            else
            {
                Log.Info("Detected invalid value in the configuration file for painkiller ahp healing! Using default value of 0");
                PainkillerAHPHealthValue = 0;
                PluginManager.YamlConfig.SetString("ct_painkillers_ahp_healing", PainkillerAHPHealthValue.ToString());
            }

            if (float.TryParse(PluginManager.YamlConfig.GetFloat("ct_medkit_ahp_healing").ToString(), out float med_ahpheal) && med_ahpheal >= 0)
                MedkitAHPHealthValue = med_ahpheal;
            else
            {
                Log.Info("Detected invalid value in the configuration file for medkit ahp healing! Using default value of 0");
                MedkitAHPHealthValue = 0;
                PluginManager.YamlConfig.SetString("ct_medkit_ahp_healing", MedkitAHPHealthValue.ToString());
            }

            if (float.TryParse(PluginManager.YamlConfig.GetFloat("ct_adrenaline_ahp_healing").ToString(), out float adr_ahpheal) && adr_ahpheal >= 0)
                AdrenalineAHPHealthValue = adr_ahpheal;
            else
            {
                Log.Info("Detected invalid value in the configuration file for adrenaline ahp healing! Using default value of 0");
                AdrenalineAHPHealthValue = 0;
                PluginManager.YamlConfig.SetString("ct_adrenaline_ahp_healing", AdrenalineAHPHealthValue.ToString());
            }

            if (float.TryParse(PluginManager.YamlConfig.GetFloat("ct_scp500_ahp_healing").ToString(), out float scp500_ahpheal) && scp500_ahpheal >= 0)
                SCP500AHPHealthValue = scp500_ahpheal;
            else
            {
                Log.Info("Detected invalid value in the configuration file for SCP-500 ahp healing! Using default value of 0");
                SCP500AHPHealthValue = 0;
                PluginManager.YamlConfig.SetString("ct_scp500_ahp_healing", SCP500AHPHealthValue.ToString());
            }

            if (float.TryParse(PluginManager.YamlConfig.GetFloat("ct_auto_scale_value").ToString(), out float as_val) && as_val != 0)
                AutoScaleValue = as_val;
            else
            {
                Log.Info("Detected invalid value in the configuration file for auto scaling! Using default value of 1");
                AutoScaleValue = 1;
                PluginManager.YamlConfig.SetString("ct_auto_scale_value", AutoScaleValue.ToString());
            }

            if (float.TryParse(PluginManager.YamlConfig.GetFloat("ct_grenade_timer_on_death").ToString(), out float gt_d) && gt_d > 0)
                GrenadeDeathTimer = gt_d;
            else
            {
                if (EnableGrenadeTimeMod)
                {
                    Log.Info("Detected invalid value in the configuration file for grenade timer on death! Using value of modified grenade timer");
                    GrenadeDeathTimer = Math.Abs(FragGrenadeFuseTimer);
                    PluginManager.YamlConfig.SetString("ct_grenade_timer_on_death", 5.ToString());
                }
                else
                {
                    Log.Info("Detected invalid value in the configuration file for grenade timer on death! Using default value of 5");
                    GrenadeDeathTimer = 5;
                    PluginManager.YamlConfig.SetString("ct_grenade_timer_on_death", GrenadeDeathTimer.ToString());
                }
            }

            if (int.TryParse(PluginManager.YamlConfig.GetInt("ct_grenade_timer_on_death").ToString(), out int s096_ahp) && s096_ahp > 0)
                SCP096AHP = s096_ahp;
            else
            {
                Log.Info("Detected invalid value in the configuration file for SCP-096 AHP health! Using default value of 250");
                SCP096AHP = 250;
                PluginManager.YamlConfig.SetString("ct_sco096_ahp", SCP096AHP.ToString());
            }
        }
    }
}
