namespace CreativeToolbox
{
    using Exiled.API.Interfaces;

    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool EnableCustomGrenadeTime { get; private set; } = false;
        public bool EnableCustomHealing { get; private set; } = false;
        public bool EnableFallDamagePrevention { get; private set; } = false;
        public bool EnableGrenadeDamagePrevention { get; private set; } = false;
        public bool EnableAutoScaling { get; private set; } = false;
        public bool EnableKeepScale { get; private set; } = false;
        public bool EnableGrenadeOnDeath { get; private set; } = false;
        public bool EnableDoorMessages { get; private set; } = false;
        public bool EnableKillMessages { get; private set; } = false;
        public bool EnableCustomEffectsAfterDrinkingScp207 { get; private set; } = false;
        public bool EnableScp018WarheadBounce { get; private set; } = false;
        public bool EnableAhpShield { get; private set; } = false;
        public bool EnableScp106AdvancedGod { get; private set; } = false;
        public bool EnableCustomAnnouncements { get; private set; } = false;
        public bool EnableCustomScp096Shield { get; private set; } = false;
        public bool EnableReverseRoleRespawnWaves { get; private set; } = false;
        public bool EnableDoorsDestroyedWithWarhead { get; private set; } = false;
        public bool EnableRandomChaosInsurgencyAnnouncementChance { get; private set; } = false;
        public bool EnableWarheadDetonationWhenCanceledChance { get; private set; } = false;
        public bool DisableAutoScaleMessages { get; private set; } = false;
        public bool UseXmasScpInAnnouncement { get; private set; } = false;
        public bool PreventCtBroadcasts { get; private set; } = false;
        public string LockedDoorMessage { get; private set; } = "you need a better keycard to open this door!";
        public string UnlockedDoorMessage { get; private set; } = "you held the keycard next to the reader";
        public string NeedKeycardMessage { get; private set; } = "this door requires a keycard";
        public string BypassKeycardMessage { get; private set; } = "you bypassed the reader";

        public string BypassWithKeycardMessage { get; private set; } =
            "you bypassed the reader, but you did not need a keycard";

        public string PryGateMessage { get; private set; } = "you pried the gate open";
        public string PryGateBypassMessage { get; private set; } = "you pried the gate open, but you could bypass it";
        public string DrinkingScp207Message { get; private set; } = "Number of drinks consumed: %counter";
        public string PryGatesWithScp207Message { get; private set; } = "You can now pry gates open";

        public string ExplodeAfterScp207Message { get; private set; } =
            "You drank too much and your body could not handle it";

        public string ChaosInsurgencyAnnouncement { get; private set; } =
            "The ChaosInsurgency have entered the facility %scpnumber";

        public string NineTailedFoxAnnouncement { get; private set; } =
            "MtfUnit Epsilon 11 Designated %unitname %unitnumber HasEntered AllRemaining %scpnumber";

        public int Scp096Ahp { get; private set; } = 250;
        public float GrenadeTimerOnDeath { get; private set; } = 5f;
        public float RegenerationTime { get; private set; } = 1f;
        public float RegenerationValue { get; set; } = 5f;
        public float RandomRespawnTimer { get; set; } = 0.05f;
        public float FragGrenadeFuseTimer { get; set; } = 5f;
        public float FlashGrenadeFuseTimer { get; set; } = 3f;
        public float PainkillerAhpHealthValue { get; private set; } = 0f;
        public float MedkitAhpHealthValue { get; private set; } = 0f;
        public float AdrenalineAhpHealthValue { get; private set; } = 0f;
        public float Scp500AhpHealthValue { get; private set; } = 0f;
        public float Scp207AhpHealthValue { get; private set; } = 0f;
        public int Scp207DrinkLimit { get; private set; } = 5;
        public int Scp207PryGateLimit { get; private set; } = 3;
        public float AutoScaleValue { get; private set; } = 1f;
        public float AhpValueLimit { get; private set; } = 75f;
        public float ChaosInsurgencyAnnouncementGlitchChance { get; private set; } = 0f;
        public float ChaosInsurgencyAnnouncementJamChance { get; private set; } = 0f;
        public float NineTailedFoxAnnouncementGlitchChance { get; private set; } = 0f;
        public float NineTailedFoxAnnouncementJamChance { get; private set; } = 0f;
        public int ChaosInsurgencyAnnouncementChance { get; private set; } = 50;
        public int InstantWarheadDetonationChance { get; private set; } = 10;
    }
}