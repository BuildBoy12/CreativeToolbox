using Exiled.API.Features;
using Exiled.API.Interfaces;
using Exiled.Events.Handlers;
using Exiled.Loader;
using HarmonyLib;
using System;

namespace CreativeToolbox
{
    public class CreativeToolbox : Plugin
    {
        public static Harmony HarmonyInstance { private set; get; }

        public override IConfig Config { get; } = new CreativeConfig();

        public static int harmonyCounter;
        public CreativeToolboxEventHandler Handler;

        public override void OnEnabled()
        {
            Handler = new CreativeToolboxEventHandler();
            harmonyCounter++;
            HarmonyInstance = new Harmony($"defytherush.creativetoolbox_{harmonyCounter}");
            HarmonyInstance.PatchAll();
            Exiled.Events.Handlers.Player.InteractingDoor += Handler.RunWhenDoorIsInteractedWith;
            Exiled.Events.Handlers.Player.Joined += Handler.RunOnPlayerJoin;
            Exiled.Events.Handlers.Player.Left += Handler.RunOnPlayerLeave;
            Exiled.Events.Handlers.Player.MedicalItemUsed += Handler.RunOnMedItemUsed;
            Exiled.Events.Handlers.Player.Hurting += Handler.RunOnPlayerHurt;
            Exiled.Events.Handlers.Player.Died += Handler.RunOnPlayerDeath;
            //Events.PlayerSpawnEvent += Handler.RunOnPlayerSpawn;
            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand += Handler.RunOnRemoteAdminCommand;
            Exiled.Events.Handlers.Server.RestartingRound += Handler.RunOnRoundRestart;
            Exiled.Events.Handlers.Server.RoundStarted += Handler.RunOnRoundStart;
            Exiled.Events.Handlers.Player.EnteringFemurBreaker += Handler.RunWhenPlayerEntersFemurBreaker;
        }

        public override void OnDisabled()
        {
            Log.Info("Disabling \"CreativeToolbox\"");
            if (HarmonyInstance != null || HarmonyInstance != default)
            {
                HarmonyInstance.UnpatchAll();
            }
            Exiled.Events.Handlers.Player.EnteringFemurBreaker -= Handler.RunWhenPlayerEntersFemurBreaker;
            Exiled.Events.Handlers.Server.RoundStarted -= Handler.RunOnRoundStart;
            Exiled.Events.Handlers.Server.RestartingRound -= Handler.RunOnRoundRestart;
            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand -= Handler.RunOnRemoteAdminCommand;
            //Events.PlayerSpawnEvent -= Handler.RunOnPlayerSpawn;
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
