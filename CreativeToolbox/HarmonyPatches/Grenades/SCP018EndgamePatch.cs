namespace CreativeToolbox
{
    using Exiled.API.Features;
    using Grenades;
    using HarmonyLib;
    using static CreativeToolbox;

    [HarmonyPatch(typeof(Scp018Grenade), nameof(Scp018Grenade.OnSpeedCollisionEnter))]
    static class SCP018EndgamePatch
    {
        public static void Postfix(Scp018Grenade __instance)
        {
            if (!Instance.Config.EnableScp018WarheadBounce)
                return;

            Warhead.Start();
            Warhead.DetonationTimer = 0.05f;
        }
    }
}