using System;
using Grenades;
using HarmonyLib;

namespace CreativeToolbox
{
    [HarmonyPatch(typeof(Grenade), nameof(Grenade.Awake))]
    static class CreateCustomGrenadePatch
    {
        public static bool Prefix(Grenade __instance)
        {
            if (CreativeToolbox.ConfigRef.Config.EnableCustomGrenadeTime)
            {
                if (__instance.GetType() == typeof(FragGrenade))
                        __instance.fuseDuration = CreativeToolbox.ConfigRef.Config.FragGrenadeFuseTimer;
                else if (__instance.GetType() == typeof(FlashGrenade))
                    __instance.fuseDuration = CreativeToolbox.ConfigRef.Config.FlashGrenadeFuseTimer;
            }
            return true;
        }
    }
}
