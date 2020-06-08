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
            if (CreativeToolbox.CT_Config.EnableGrenadeDamagePrevent)
                __instance.absoluteDamageFalloff = int.MaxValue;
            return true;
        }
    }
}