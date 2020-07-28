using System;
using HarmonyLib;
using PlayableScps;
using UnityEngine;

namespace CreativeToolbox
{
    [HarmonyPatch(typeof(Scp096), nameof(Scp096.AddTarget))]
    static class ModifySCP096ShieldPatch
    {
        public static bool Prefix(Scp096 __instance, GameObject target)
        {
			ReferenceHub hub = ReferenceHub.GetHub(target);
			if (__instance.CanReceiveTargets && hub != null && !__instance._targets.Contains(hub))
			{
				if (!__instance._targets.IsEmpty<ReferenceHub>())
				{
					__instance.AddReset();
				}
				__instance._targets.Add(hub);
				if (CreativeToolbox.ConfigRef.Config.EnableCustomScp096Shield)
					__instance.AdjustShield(CreativeToolbox.ConfigRef.Config.Scp096Ahp);
				else
					__instance.AdjustShield(200);
			}
			return false;
        }
    }
}
