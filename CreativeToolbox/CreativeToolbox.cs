namespace CreativeToolbox
{
    using Exiled.API.Features;
    using HarmonyLib;
    
    public sealed class CreativeToolbox : Plugin<Config>
    {
        private static readonly Harmony HarmonyInstance = new Harmony(nameof(CreativeToolbox).ToLowerInvariant());
        internal static CreativeToolbox Instance;
        private readonly CreativeToolboxEventHandler _handler = new CreativeToolboxEventHandler();
        
        public override void OnEnabled()
        {
            Instance = this;
            HarmonyInstance.PatchAll();

            Exiled.Events.Handlers.Player.InteractingDoor += _handler.RunWhenDoorIsInteractedWith;
            Exiled.Events.Handlers.Player.Joined += _handler.RunOnPlayerJoin;
            Exiled.Events.Handlers.Player.Left += _handler.RunOnPlayerLeave;
            Exiled.Events.Handlers.Player.MedicalItemUsed += _handler.RunOnMedItemUsed;
            Exiled.Events.Handlers.Player.Hurting += _handler.RunOnPlayerHurt;
            Exiled.Events.Handlers.Player.Died += _handler.RunOnPlayerDeath;
            Exiled.Events.Handlers.Server.RestartingRound += _handler.RunOnRoundRestart;
            Exiled.Events.Handlers.Server.RoundStarted += _handler.RunOnRoundStart;
            Exiled.Events.Handlers.Player.EnteringFemurBreaker += _handler.RunWhenPlayerEntersFemurBreaker;
            Exiled.Events.Handlers.Warhead.Detonated += _handler.RunWhenWarheadIsDetonated;
            Exiled.Events.Handlers.Server.RespawningTeam += _handler.RunWhenTeamRespawns;
            Exiled.Events.Handlers.Warhead.Stopping += _handler.RunWhenWarheadIsStopped;
            Exiled.Events.Handlers.Map.AnnouncingNtfEntrance += _handler.RunWhenNTFSpawns;
        }

        public override void OnDisabled()
        {
            HarmonyInstance?.UnpatchAll(nameof(CreativeToolbox).ToLowerInvariant());
            Instance = null;
            
            Exiled.Events.Handlers.Map.AnnouncingNtfEntrance -= _handler.RunWhenNTFSpawns;
            Exiled.Events.Handlers.Warhead.Stopping -= _handler.RunWhenWarheadIsStopped;
            Exiled.Events.Handlers.Server.RespawningTeam -= _handler.RunWhenTeamRespawns;
            Exiled.Events.Handlers.Warhead.Detonated -= _handler.RunWhenWarheadIsDetonated;
            Exiled.Events.Handlers.Player.EnteringFemurBreaker -= _handler.RunWhenPlayerEntersFemurBreaker;
            Exiled.Events.Handlers.Server.RoundStarted -= _handler.RunOnRoundStart;
            Exiled.Events.Handlers.Server.RestartingRound -= _handler.RunOnRoundRestart;
            Exiled.Events.Handlers.Player.Died -= _handler.RunOnPlayerDeath;
            Exiled.Events.Handlers.Player.Hurting -= _handler.RunOnPlayerHurt;
            Exiled.Events.Handlers.Player.MedicalItemUsed -= _handler.RunOnMedItemUsed;
            Exiled.Events.Handlers.Player.Left -= _handler.RunOnPlayerLeave;
            Exiled.Events.Handlers.Player.Joined -= _handler.RunOnPlayerJoin;
            Exiled.Events.Handlers.Player.InteractingDoor -= _handler.RunWhenDoorIsInteractedWith;
        }

        public override string Author => "KoukoCocoa";
        public override string Name => nameof(CreativeToolbox);
    }
}
