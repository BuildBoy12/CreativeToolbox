using System;
using Respawning;
using HarmonyLib;
using Exiled.API.Features;

namespace CreativeToolbox
{
    [HarmonyPatch(typeof(RespawnManager), nameof(RespawnManager.ForceSpawnTeam))]
    static class AddCustomCIAnnouncement
    {
        static Random CIAnnouncementChance = new Random();
        static double NewChance;
        public static void Postfix(RespawnManager __instance, SpawnableTeamType teamToSpawn)
        {
            int counter = 0;
            foreach (Player Ply in Player.List)
            {
                if (Ply.Role.IsNotHuman(true))
                    counter++;
            }
            if (CreativeToolbox.ConfigRef.Config.EnableRandomChaosInsurgencyAnnouncementChance)
            {
                NewChance = CIAnnouncementChance.Next(0, 100);
                if (NewChance < CreativeToolbox.ConfigRef.Config.ChaosInsurgencyAnnouncementChance)
                    if (CreativeToolbox.ConfigRef.Config.EnableCustomAnnouncements && teamToSpawn == SpawnableTeamType.ChaosInsurgency)
                        NineTailedFoxAnnouncer.singleton.ServerOnlyAddGlitchyPhrase(CreativeToolboxEventHandler.FormatMessage(CreativeToolbox.ConfigRef.Config.ChaosInsurgencyAnnouncement, counter), CreativeToolbox.ConfigRef.Config.ChaosInsurgencyAnnouncementGlitchChance * 0.01f, CreativeToolbox.ConfigRef.Config.ChaosInsurgencyAnnouncementJamChance * 0.01f);
            }
            else
                if (CreativeToolbox.ConfigRef.Config.EnableCustomAnnouncements && teamToSpawn == SpawnableTeamType.ChaosInsurgency)
                    NineTailedFoxAnnouncer.singleton.ServerOnlyAddGlitchyPhrase(CreativeToolboxEventHandler.FormatMessage(CreativeToolbox.ConfigRef.Config.ChaosInsurgencyAnnouncement, counter), CreativeToolbox.ConfigRef.Config.ChaosInsurgencyAnnouncementGlitchChance * 0.01f, CreativeToolbox.ConfigRef.Config.ChaosInsurgencyAnnouncementJamChance * 0.01f);
        }
    }
}
