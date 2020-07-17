using System;
using Exiled.API.Features;
using Grenades;
using HarmonyLib;

namespace CreativeToolbox
{
    [HarmonyPatch(typeof(Scp018Grenade), nameof(Scp018Grenade.OnSpeedCollisionEnter))]
    static class SCP018EndgamePatch
    {
        public static void Postfix(Scp018Grenade __instance)
        {
            if (CreativeToolbox.ConfigRef.Config.EnableScp018WarheadBounce)
            {
                Warhead.Start();
                Warhead.DetonationTimer = 0.05f;
            }
        }
    }
}
