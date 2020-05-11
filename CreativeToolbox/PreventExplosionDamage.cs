using System;
using Grenades;
using Harmony;
using UnityEngine;

namespace CreativeToolbox
{
    [HarmonyPatch(typeof(FragGrenade), nameof(FragGrenade.Awake))]
    class PreventExplosionDamage
    {
        public static bool Prefix(FragGrenade __instance)
        {
            if (CreativeToolbox.EnableGrenadeDamagePrevent)
            {
                __instance.absoluteDamageFalloff = int.MaxValue;
            }
            return true;
        }
    }
}
