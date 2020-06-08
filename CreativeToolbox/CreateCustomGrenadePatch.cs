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
            if (CreativeToolbox.CT_Config.EnableGrenadeTimeMod)
            {
                if (__instance.GetType() == typeof(FragGrenade))
                        __instance.fuseDuration = CreativeToolbox.CT_Config.FragGrenadeFuseTimer;
                else if (__instance.GetType() == typeof(FlashGrenade))
                    __instance.fuseDuration = CreativeToolbox.CT_Config.FlashGrenadeFuseTimer;
            }
            return true;
        }
    }
}
