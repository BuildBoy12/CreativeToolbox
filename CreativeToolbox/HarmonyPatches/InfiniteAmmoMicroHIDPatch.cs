using HarmonyLib;

namespace CreativeToolbox
{
    [HarmonyPatch(typeof(MicroHID), nameof(MicroHID.UpdateServerside))]
    static class InfiniteAmmoMicrioHIDPatch
    {
        public static bool Prefix(MicroHID __instance)
        {
            if (__instance.refHub.TryGetComponent(out InfiniteAmmoComponent infAmmo) && __instance.refHub.inventory.curItem == ItemType.MicroHID)
            {
                __instance.ChangeEnergy(1);
                __instance.NetworkEnergy = 1;
            }
            return true;
        }
    }
}
