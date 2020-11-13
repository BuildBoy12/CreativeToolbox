namespace CreativeToolbox
{
    using Exiled.API.Features;
    using HarmonyLib;
    using Respawning;
    using System;
    using static CreativeToolbox;

    [HarmonyPatch(typeof(RespawnManager), nameof(RespawnManager.ForceSpawnTeam))]
    internal static class AddCustomCiAnnouncement
    {
        private static readonly Random CiAnnouncementChance = new Random();
        private static double _newChance;

        private static void Postfix(RespawnManager __instance, SpawnableTeamType teamToSpawn)
        {
            int counter = 0;
            foreach (Player ply in Player.List)
            {
                if (ply.Team == Team.SCP)
                    counter++;
            }

            if (Instance.Config.EnableRandomChaosInsurgencyAnnouncementChance)
            {
                _newChance = CiAnnouncementChance.Next(0, 100);
                if (!(_newChance < Instance.Config.ChaosInsurgencyAnnouncementChance))
                    return;

                if (Instance.Config.EnableCustomAnnouncements && teamToSpawn == SpawnableTeamType.ChaosInsurgency)
                    NineTailedFoxAnnouncer.singleton.ServerOnlyAddGlitchyPhrase(
                        CreativeToolboxEventHandler.FormatMessage(Instance.Config.ChaosInsurgencyAnnouncement, counter),
                        Instance.Config.ChaosInsurgencyAnnouncementGlitchChance * 0.01f,
                        Instance.Config.ChaosInsurgencyAnnouncementJamChance * 0.01f);
            }
            else if (Instance.Config.EnableCustomAnnouncements && teamToSpawn == SpawnableTeamType.ChaosInsurgency)
                NineTailedFoxAnnouncer.singleton.ServerOnlyAddGlitchyPhrase(
                    CreativeToolboxEventHandler.FormatMessage(Instance.Config.ChaosInsurgencyAnnouncement, counter),
                    Instance.Config.ChaosInsurgencyAnnouncementGlitchChance * 0.01f,
                    Instance.Config.ChaosInsurgencyAnnouncementJamChance * 0.01f);
        }
    }
}