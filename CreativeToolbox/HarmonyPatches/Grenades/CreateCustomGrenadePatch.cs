namespace CreativeToolbox
{
    using Grenades;
    using HarmonyLib;
    using static CreativeToolbox;

    [HarmonyPatch(typeof(Grenade), nameof(Grenade.Awake))]
    internal static class CreateCustomGrenadePatch
    {
        private static bool Prefix(Grenade __instance)
        {
            if (!Instance.Config.EnableCustomGrenadeTime)
                return true;

            if (__instance.GetType() == typeof(FragGrenade))
                __instance.fuseDuration = Instance.Config.FragGrenadeFuseTimer;
            else if (__instance.GetType() == typeof(FlashGrenade))
                __instance.fuseDuration = Instance.Config.FlashGrenadeFuseTimer;
            return true;
        }
    }
}