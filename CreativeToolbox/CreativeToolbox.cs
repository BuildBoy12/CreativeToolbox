using Exiled.API.Features;
using Exiled.API.Interfaces;
using Exiled.Events.Handlers;
using Exiled.Loader;
using HarmonyLib;
using System;

namespace CreativeToolbox
{
    public class CreativeToolbox : Plugin<Config>
    {
        private static readonly Lazy<CreativeToolbox> LazyInstance = new Lazy<CreativeToolbox>(() => new CreativeToolbox());
        private CreativeToolbox() { }
        public static CreativeToolbox ConfigRef => LazyInstance.Value;
        public static Harmony HarmonyInstance { private set; get; }

        public static int harmonyCounter;
        public CreativeToolboxEventHandler Handler;

        public override void OnEnabled()
        {
            base.OnEnabled();
            Handler = new CreativeToolboxEventHandler(this);
            harmonyCounter++;
            HarmonyInstance = new Harmony($"koukococoa.creativetoolbox_{harmonyCounter}");
            HarmonyInstance.PatchAll();
            Exiled.Events.Handlers.Player.InteractingDoor += Handler.RunWhenDoorIsInteractedWith;
            Exiled.Events.Handlers.Player.Joined += Handler.RunOnPlayerJoin;
            Exiled.Events.Handlers.Player.Left += Handler.RunOnPlayerLeave;
            Exiled.Events.Handlers.Player.MedicalItemUsed += Handler.RunOnMedItemUsed;
            Exiled.Events.Handlers.Player.Hurting += Handler.RunOnPlayerHurt;
            Exiled.Events.Handlers.Player.Died += Handler.RunOnPlayerDeath;
            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand += Handler.RunOnRemoteAdminCommand;
            Exiled.Events.Handlers.Server.RestartingRound += Handler.RunOnRoundRestart;
            Exiled.Events.Handlers.Server.RoundStarted += Handler.RunOnRoundStart;
            Exiled.Events.Handlers.Player.EnteringFemurBreaker += Handler.RunWhenPlayerEntersFemurBreaker;
            Exiled.Events.Handlers.Warhead.Detonated += Handler.RunWhenWarheadIsDetonated;
            Exiled.Events.Handlers.Server.RespawningTeam += Handler.RunWhenTeamRespawns;
        }

        public override void OnDisabled()
        {
            Log.Info("Disabling \"CreativeToolbox\"");
            if (HarmonyInstance != null || HarmonyInstance != default)
            {
                HarmonyInstance.UnpatchAll();
            }
            Exiled.Events.Handlers.Server.RespawningTeam -= Handler.RunWhenTeamRespawns;
            Exiled.Events.Handlers.Warhead.Detonated -= Handler.RunWhenWarheadIsDetonated;
            Exiled.Events.Handlers.Player.EnteringFemurBreaker -= Handler.RunWhenPlayerEntersFemurBreaker;
            Exiled.Events.Handlers.Server.RoundStarted -= Handler.RunOnRoundStart;
            Exiled.Events.Handlers.Server.RestartingRound -= Handler.RunOnRoundRestart;
            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand -= Handler.RunOnRemoteAdminCommand;
            Exiled.Events.Handlers.Player.Died -= Handler.RunOnPlayerDeath;
            Exiled.Events.Handlers.Player.Hurting -= Handler.RunOnPlayerHurt;
            Exiled.Events.Handlers.Player.MedicalItemUsed -= Handler.RunOnMedItemUsed;
            Exiled.Events.Handlers.Player.Left -= Handler.RunOnPlayerLeave;
            Exiled.Events.Handlers.Player.Joined -= Handler.RunOnPlayerJoin;
            Exiled.Events.Handlers.Player.InteractingDoor -= Handler.RunWhenDoorIsInteractedWith;
            Handler = null;
        }

        public override void OnReloaded() { }
    }
}
