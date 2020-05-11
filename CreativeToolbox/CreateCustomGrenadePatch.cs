using EXILED.Extensions;
using Grenades;
using Harmony;
using Mirror;
using UnityEngine;

namespace CreativeToolbox
{
    [HarmonyPatch(typeof(Grenade), nameof(Grenade.Awake))]
    class CreateCustomGrenadePatch
    {
        public static bool Prefix(Grenade __instance)
        {
            if (CreativeToolbox.EnableGrenadeTimeMod)
            {
                if (__instance.GetType() == typeof(FragGrenade))
                        __instance.fuseDuration = CreativeToolbox.FragGrenadeFuseTimer;
                else if (__instance.GetType() == typeof(FlashGrenade))
                    __instance.fuseDuration = CreativeToolbox.FlashGrenadeFuseTimer;
            }
            return true;
        }
    }
}
