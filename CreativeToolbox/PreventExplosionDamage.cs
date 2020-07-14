using System;
using Grenades;
using HarmonyLib;

namespace CreativeToolbox
{
    [HarmonyPatch(typeof(FragGrenade), nameof(FragGrenade.Awake))]
    class PreventExplosionDamage
    {
        public static bool Prefix(FragGrenade __instance)
        {
            if (CreativeToolbox.ConfigRef.Config.EnableGrenadeDamagePrevention)
                __instance.absoluteDamageFalloff = int.MaxValue;
            return true;
        }
    }
}