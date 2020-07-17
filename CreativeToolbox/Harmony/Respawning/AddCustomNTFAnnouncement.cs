using System;
using HarmonyLib;
using Respawning.NamingRules;

namespace CreativeToolbox
{
    [HarmonyPatch(typeof(UnitNamingRule), nameof(UnitNamingRule.ConfirmAnnouncement), typeof(string))]
    static class AddCustomNTFAnnouncement
    {
        public static bool Prefix(UnitNamingRule __instance, string completeAnnouncement)
        {
            if (!String.IsNullOrWhiteSpace(CreativeToolbox.ConfigRef.Config.NineTailedFoxAnnouncement))
            {
                NineTailedFoxAnnouncer.singleton.ServerOnlyAddGlitchyPhrase(CreativeToolbox.ConfigRef.Config.NineTailedFoxAnnouncement, CreativeToolbox.ConfigRef.Config.NineTailedFoxAnnouncementGlitchChance * 0.01f, CreativeToolbox.ConfigRef.Config.NineTailedFoxAnnouncementJamChance * 0.01f);
                return false;
            }
            return true;
        }
    }
}
