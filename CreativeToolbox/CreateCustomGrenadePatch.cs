using System;
using Grenades;
using HarmonyLib;

namespace CreativeToolbox
{
    [HarmonyPatch(typeof(Grenade), nameof(Grenade.Awake))]
    class CreateCustomGrenadePatch
    {
        public static bool Prefix(Grenade __instance)
        {
            if (CreativeConfig.EnableGrenadeTimeMod)
            {
                if (__instance.GetType() == typeof(FragGrenade))
                        __instance.fuseDuration = CreativeConfig.FragGrenadeFuseTimer;
                else if (__instance.GetType() == typeof(FlashGrenade))
                    __instance.fuseDuration = CreativeConfig.FlashGrenadeFuseTimer;
            }
            return true;
        }
    }
}
