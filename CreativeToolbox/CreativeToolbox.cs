using Exiled.API.Features;
using HarmonyLib;

namespace CreativeToolbox
{
    public sealed class CreativeToolbox : Plugin<Config>
    {
        public static CreativeToolbox ConfigRef { get; private set; }
        public static Harmony HarmonyInstance { get; private set; }

        public override string Author => "KoukoCocoa";
        public override string Name => nameof(CreativeToolbox);

        private static int harmonyCounter;
        public CreativeToolboxEventHandler Handler { get; private set; }

        public CreativeToolbox()
        {
            ConfigRef = this;
        }

        public override void OnEnabled()
        {
            if (Handler == null)
                Handler = new CreativeToolboxEventHandler(this);

            HarmonyInstance = new Harmony($"koukococoa.creativetoolbox_{++harmonyCounter}");
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
            HarmonyInstance?.UnpatchAll();

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
        }

        public override void OnReloaded() { }
    }
}
