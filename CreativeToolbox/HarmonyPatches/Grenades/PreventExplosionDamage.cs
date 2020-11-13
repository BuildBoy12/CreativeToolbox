namespace CreativeToolbox
{
    using Grenades;
    using HarmonyLib;
    using static CreativeToolbox;

    [HarmonyPatch(typeof(FragGrenade), nameof(FragGrenade.Awake))]
    internal static class PreventExplosionDamage
    {
        private static bool Prefix(FragGrenade __instance)
        {
            if (Instance.Config.EnableGrenadeDamagePrevention)
                __instance.absoluteDamageFalloff = int.MaxValue;
            return true;
        }
    }
}