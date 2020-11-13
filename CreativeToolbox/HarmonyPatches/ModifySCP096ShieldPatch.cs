namespace CreativeToolbox
{
    using HarmonyLib;
    using PlayableScps;
    using UnityEngine;
    using static CreativeToolbox;

    [HarmonyPatch(typeof(Scp096), nameof(Scp096.AddTarget))]
    internal static class ModifyScp096ShieldPatch
    {
        private static bool Prefix(Scp096 __instance, GameObject target)
        {
            ReferenceHub hub = ReferenceHub.GetHub(target);
            if (!__instance.CanReceiveTargets || hub == null || __instance._targets.Contains(hub))
            {
                return false;
            }

            if (!__instance._targets.IsEmpty())
            {
                __instance.AddReset();
            }

            __instance._targets.Add(hub);
            __instance.AdjustShield(Instance.Config.EnableCustomScp096Shield
                ? Instance.Config.Scp096Ahp
                : 200);
            return false;
        }
    }
}