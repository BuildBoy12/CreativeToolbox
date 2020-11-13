namespace CreativeToolbox
{
    using Grenades;
    using HarmonyLib;
    using static CreativeToolbox;

    [HarmonyPatch(typeof(Scp018Grenade), nameof(Scp018Grenade.Awake))]
    internal static class PreventScp018Damage
    {
        public static bool Prefix(Scp018Grenade __instance)
        {
            if (!Instance.Config.EnableGrenadeDamagePrevention)
                return true;

            __instance.damageHurt = 0;
            __instance.damageScpMultiplier = 0;
            return true;
        }
    }
}