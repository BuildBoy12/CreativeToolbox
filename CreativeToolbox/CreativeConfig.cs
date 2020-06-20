using Exiled.API.Features;
using Exiled.API.Interfaces;
using Exiled.Loader;
using System;

namespace CreativeToolbox
{
    public class CreativeConfig : IConfig
    {
        public static bool IsToolboxEnabled = true;
        public static float MedkitAHPHealthValue;
        public static float PainkillerAHPHealthValue;
        public static float AdrenalineAHPHealthValue;
        public static float SCP500AHPHealthValue;
        public static float SCP207AHPHealthValue;
        public static float RandomRespawnTimer;
        public static float FragGrenadeFuseTimer;
        public static float FlashGrenadeFuseTimer;
        public static float HPRegenerationTimer;
        public static float HPRegenerationValue;
        public static float HPRegenerationIfHit;
        public static float AutoScaleValue;
        public static float GrenadeDeathTimer;
        public static float SCP096EnrageJumpHeight;
        public static float SCP096EnrageMoveSpeed;
        public static float AHPValueLimit;
        public static int SCP207DrinkLimit;
        public static int SCP096AHP;
        public static bool EnableGrenadeTimeMod = false;
        public static bool EnableMedicalItemMod = false;
        public static bool EnableFallDamagePrevent = false;
        public static bool EnableGrenadeDamagePrevent = false;
        public static bool EnableAutoScaling = false;
        public static bool EnableRetainingScaling = false;
        public static bool EnableGrenadeSpawnOnDeath = false;
        public static bool EnableGrenadeRangeMod = false;
        public static bool EnableDoorMessages = false;
        public static bool EnableDamageMessage = false;
        public static bool EnableExplodingAfterTooMuchSCP207 = false;
        public static bool EnableSCP018WarheadBounce = false;
        public static bool EnableRespawnsAsSameClass = false;
        public static bool EnableSCP096ShieldModify = false;
        public static bool EnableSCP096StatsModify = false;
        public static bool EnableSCP106AdvancedGodmode = false;
        public static bool DisableAutoScalingMessage = false;
        public static bool LockFallDamageMod = false;
        public static bool KeepAHPShieldForAllUsers = false;
        public static string LockedDoorMessage;
        public static string UnlockedDoorMessage;
        public static string NeedKeycardMessage;
        public static string BypassKeycardMessage;
        public static string BypassWithKeycardInHandMessage;
        public static string PryGatesMessage;
        public static string PryGatesBypassMessage;
        public static string DamageMessage;

        public bool IsEnabled { get; set; }

        public string Prefix => "creativetoolbox_";

        public void Reload()
        {
            IsEnabled = PluginManager.YamlConfig.GetBool("creativetoolbox_enable", false);
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
            EnableSCP018WarheadBounce = PluginManager.YamlConfig.GetBool("ct_enable_scp018_warhead_bounce", false);
            EnableExplodingAfterTooMuchSCP207 = PluginManager.YamlConfig.GetBool("ct_enable_exploding_after_drinking_scp207", false);
            EnableRespawnsAsSameClass = PluginManager.YamlConfig.GetBool("ct_enable_same_class_respawn", false);
            KeepAHPShieldForAllUsers = PluginManager.YamlConfig.GetBool("ct_enable_ahp_shield", false);
            EnableSCP106AdvancedGodmode = PluginManager.YamlConfig.GetBool("ct_enable_scp106_advanced_god", false);
            DisableAutoScalingMessage = PluginManager.YamlConfig.GetBool("ct_disable_autoscale_messages", false);
            LockFallDamageMod = PluginManager.YamlConfig.GetBool("ct_disable_fall_modification", false);
            LockedDoorMessage = PluginManager.YamlConfig.GetString("ct_locked_door_message", "you need a better keycard to open this door!");
            UnlockedDoorMessage = PluginManager.YamlConfig.GetString("ct_unlocked_door_message", "you held the keycard next to the reader");
            NeedKeycardMessage = PluginManager.YamlConfig.GetString("ct_need_keycard_message", "this door requires a keycard");
            BypassKeycardMessage = PluginManager.YamlConfig.GetString("ct_bypass_keycard_message", "you bypassed the reader");
            BypassWithKeycardInHandMessage = PluginManager.YamlConfig.GetString("ct_bypass_with_keycard_message", "you bypassed the reader, but you did not need a keycard");
            PryGatesMessage = PluginManager.YamlConfig.GetString("ct_pry_gate_message", "you pried the gate open");
            PryGatesBypassMessage = PluginManager.YamlConfig.GetString("ct_pry_gate_bypass_message", "you pried the gate open, but you could bypass it");
            RandomRespawnTimer = Math.Abs(PluginManager.YamlConfig.GetFloat("ct_random_respawn_timer", 0.05f)) >= 0.05f ? RandomRespawnTimer : 0.05f;
            FragGrenadeFuseTimer = Math.Abs(PluginManager.YamlConfig.GetFloat("ct_frag_grenade_fuse_timer", 5f)) >= 0.01f ? FragGrenadeFuseTimer : 5f;
            FlashGrenadeFuseTimer = Math.Abs(PluginManager.YamlConfig.GetFloat("ct_flash_grenade_fuse_timer", 3f)) >= 0.01f ? FlashGrenadeFuseTimer : 3f;
            HPRegenerationTimer = Math.Abs(PluginManager.YamlConfig.GetFloat("ct_regeneration_time", 5f));
            HPRegenerationValue = Math.Abs(PluginManager.YamlConfig.GetFloat("ct_regeneration_value", 5f));
            PainkillerAHPHealthValue = Math.Abs(PluginManager.YamlConfig.GetFloat("ct_painkillers_ahp_healing", 0f));
            MedkitAHPHealthValue = Math.Abs(PluginManager.YamlConfig.GetFloat("ct_medkit_ahp_healing", 0f));
            AdrenalineAHPHealthValue = Math.Abs(PluginManager.YamlConfig.GetFloat("ct_adrenaline_ahp_healing", 0f));
            SCP500AHPHealthValue = Math.Abs(PluginManager.YamlConfig.GetFloat("ct_scp500_ahp_healing", 0f));
            SCP207AHPHealthValue = Math.Abs(PluginManager.YamlConfig.GetFloat("ct_scp207_ahp_healing", 0f));
            AutoScaleValue = PluginManager.YamlConfig.GetFloat("ct_auto_scale_value", 1f);
            GrenadeDeathTimer = Math.Abs(PluginManager.YamlConfig.GetFloat("ct_grenade_timer_on_death", 5f)) >= 0.01f ? GrenadeDeathTimer : 5f;
            SCP096AHP = Math.Abs(PluginManager.YamlConfig.GetInt("ct_sco096_enrage_shield", 250));
            SCP096EnrageJumpHeight = Math.Abs(PluginManager.YamlConfig.GetFloat("ct_scp096_enrage_jump_height", 10f));
            SCP096EnrageMoveSpeed = Math.Abs(PluginManager.YamlConfig.GetFloat("ct_scp096_enrage_move_speed", 12f));
            SCP207DrinkLimit = Math.Abs(PluginManager.YamlConfig.GetInt("ct_scp207_drink_limit", 5));
            AHPValueLimit = Math.Abs(PluginManager.YamlConfig.GetFloat("ct_ahp_max_limit", 150f));
        }
    }
}
