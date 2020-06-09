using System;
using HarmonyLib;
using PlayableScps;
using UnityEngine;

namespace CreativeToolbox
{
    [HarmonyPatch(typeof(Scp096), nameof(Scp096.AddTarget))]
    class AddTargetsOnClientPatch
    {
        public static bool Prefix(Scp096 __instance, GameObject target)
        {
			/*ReferenceHub hub = ReferenceHub.GetHub(target);
			if (__instance.CanReceiveTargets || hub != null || !__instance._targets.Contains(hub))
			{
				if (!__instance._targets.IsEmpty<ReferenceHub>())
				{
					__instance.EnrageTimeLeft += __instance.EnrageTimePerReset;
				}
				__instance._targets.Add(hub);
				__instance.AdjustShield(CreativeToolbox.CT_Config.SCP096AHP);
			}*/
			return true;
        }
    }
}
