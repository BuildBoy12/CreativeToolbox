using Exiled.API.Features;
using Exiled.API.Interfaces;
using Exiled.Loader;
using System;

namespace CreativeToolbox
{
    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool EnableCustomGrenadeTime { get; set; } = false;
        public bool EnableCustomHealing { get; set; } = false;
        public bool EnableFallDamagePrevention { get; set; } = false;
        public bool EnableGrenadeDamagePrevention { get; set; } = false;
        public bool EnableAutoScaling { get; set; } = false;
        public bool EnableKeepScale { get; set; } = false;
        public bool EnableGrenadeOnDeath { get; set; } = false;
        public bool EnableDoorMessages { get; set; } = false;
        public bool EnableKillMessages { get; set; } = false;
        public bool EnableCustomEffectsAfterDrinkingScp207 { get; set; } = false;
        public bool EnableScp018WarheadBounce { get; set; } = false;
        public bool EnableAhpShield { get; set; } = false;
        public bool EnableScp106AdvancedGod { get; set; } = false;
        public bool EnableCustomAnnouncements { get; set; } = false;
        public bool EnableCustomScp096Shield { get; set; } = false;
        public bool EnableReverseRoleRespawnWaves { get; set; } = false;
        public bool EnableDoorsDestroyedWithWarhead { get; set; } = false;
        public bool EnableRandomChaosInsurgencyAnnouncementChance { get; set; } = false;
        public bool EnableWarheadDetonationWhenCanceledChance { get; set; } = false;
        public bool DisableAutoScaleMessages { get; set; } = false;
        public bool UseXmasScpInAnnouncement { get; set; } = false;
        public bool PreventCtBroadcasts { get; set; } = false;
        public string LockedDoorMessage { get; set; } = "you need a better keycard to open this door!";
        public string UnlockedDoorMessage { get; set; } = "you held the keycard next to the reader";
        public string NeedKeycardMessage { get; set; } = "this door requires a keycard";
        public string BypassKeycardMessage { get; set; } = "you bypassed the reader";
        public string BypassWithKeycardMessage { get; set; } = "you bypassed the reader, but you did not need a keycard";
        public string PryGateMessage { get; set; } = "you pried the gate open";
        public string PryGateBypassMessage { get; set; } = "you pried the gate open, but you could bypass it";
        public string DrinkingScp207Message { get; set; } = "Number of drinks consumed: %counter";
        public string PryGatesWithScp207Message { get; set; } = "You can now pry gates open";
        public string ExplodeAfterScp207Message { get; set; } = "You drank too much and your body could not handle it";
        public string ChaosInsurgencyAnnouncement { get; set; } = "The ChaosInsurgency have entered the facility %scpnumber";
        public string NineTailedFoxAnnouncement { get; set; } = "MtfUnit Epsilon 11 Designated %unitname %unitnumber HasEntered AllRemaining %scpnumber";
        public int Scp096Ahp { get; set; } = 250;
        public float GrenadeTimerOnDeath { get; set; } = 5f;
        public float RegenerationTime { get; set; } = 1f;
        public float RegenerationValue { get; set; } = 5f;
        public float RandomRespawnTimer { get; set; } = 0.05f;
        public float FragGrenadeFuseTimer { get; set; } = 5f;
        public float FlashGrenadeFuseTimer { get; set; } = 3f;
        public float PainkillerAhpHealthValue { get; set; } = 0f;
        public float MedkitAhpHealthValue { get; set; } = 0f;
        public float AdrenalineAhpHealthValue { get; set; } = 0f;
        public float Scp500AhpHealthValue { get; set; } = 0f;
        public float Scp207AhpHealthValue { get; set; } = 0f;
        public int Scp207DrinkLimit { get; set; } = 5;
        public int Scp207PryGateLimit { get; set; } = 3;
        public float AutoScaleValue { get; set; } = 1f;
        public float AhpValueLimit { get; set; } = 75f;
        public float ChaosInsurgencyAnnouncementGlitchChance { get; set;  } = 0f;
        public float ChaosInsurgencyAnnouncementJamChance { get; set; } = 0f;
        public float NineTailedFoxAnnouncementGlitchChance { get; set; } = 0f;
        public float NineTailedFoxAnnouncementJamChance { get; set; } = 0f;
        public int ChaosInsurgencyAnnouncementChance { get; set; } = 50;
        public int InstantWarheadDetonationChance { get; set; } = 10;
    }
}
