namespace CreativeToolbox
{
    using HarmonyLib;

    [HarmonyPatch(typeof(MicroHID), nameof(MicroHID.UpdateServerside))]
    internal static class InfiniteAmmoMicroHidPatch
    {
        private static bool Prefix(MicroHID __instance)
        {
            if (!__instance.refHub.TryGetComponent(out InfiniteAmmoComponent infAmmo) ||
                __instance.refHub.inventory.curItem != ItemType.MicroHID)
                return true;

            __instance.ChangeEnergy(1);
            __instance.NetworkEnergy = 1;
            return true;
        }
    }
}