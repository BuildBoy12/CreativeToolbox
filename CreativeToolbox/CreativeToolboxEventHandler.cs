using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Grenades;
using Hints;
using MEC;
using Mirror;
using UnityEngine;

namespace CreativeToolbox
{
    public sealed class CreativeToolboxEventHandler
    {
        public static StringBuilder PlayerLister = new StringBuilder();
        public static HashSet<Player> PlayersThatCanPryGates = new HashSet<Player>();
        public static HashSet<Player> PlayersWithRegen = new HashSet<Player>();
        public static HashSet<Player> PlayersWithInfiniteAmmo = new HashSet<Player>();
        public static HashSet<string> PlayersWithRetainedScale = new HashSet<string>();
        string[] DoorsThatAreLocked = { "012", "049_ARMORY", "079_FIRST", "079_SECOND", "096", "106_BOTTOM", 
            "106_PRIMARY", "106_SECONDARY", "173_ARMORY", "914", "CHECKPOINT_ENT", "CHECKPOINT_LCZ_A", "CHECKPOINT_LCZ_B",
            "GATE_A", "GATE_B", "HCZ_ARMORY", "HID", "INTERCOM", "LCZ_ARMORY", "NUKE_ARMORY", "NUKE_SURFACE" };
        string[] GatesThatExist = { "914", "GATE_A", "GATE_B", "079_FIRST", "079_SECOND" };
        System.Random RandNum = new System.Random();
        bool IsWarheadDetonated;
        bool IsDecontanimationActivated;
        public static bool AllowRespawning = false;
        public static bool PreventFallDamage = false;
        public static bool WasDeconCommandRun = false;
        public static bool AutoScaleOn = false;
        CoroutineHandle ChaosRespawnHandle;

        public CreativeToolbox plugin;
        public CreativeToolboxEventHandler(CreativeToolbox plugin) => this.plugin = plugin;

        public void RunOnRoundRestart()
        {
            AllowRespawning = false;
        }

        public void RunOnRoundStart()
        {
            AllowRespawning = false;
            PlayersWithRegen.Clear();
            PlayersWithInfiniteAmmo.Clear();
            PlayersThatCanPryGates.Clear();
            PlayersWithRetainedScale.Clear();
            PlayerLister.Clear();
            if (plugin.Config.EnableFallDamagePrevention)
                PreventFallDamage = true;
            if (plugin.Config.EnableAutoScaling)
            {
                foreach (Player Ply in Player.List)
                {
                    if (!plugin.Config.DisableAutoScaleMessages)
                        Map.Broadcast(5, $"Everyone who joined has their playermodel scale set to {plugin.Config.AutoScaleValue}x!");
                    Ply.Scale = new Vector3(plugin.Config.AutoScaleValue, plugin.Config.AutoScaleValue, plugin.Config.AutoScaleValue);
                    PlayersWithRetainedScale.Add(Ply.UserId);
                    AutoScaleOn = true;
                }
            }
            if (plugin.Config.EnableGrenadeOnDeath)
                Map.Broadcast(10, $"<color=red>Warning: Grenades spawn after you die, they explode after {plugin.Config.GrenadeTimerOnDeath} seconds of them spawning, be careful!</color>", Broadcast.BroadcastFlags.Normal);
            if (plugin.Config.EnableAhpShield)
            {
                foreach (Player Ply in Player.List)
                {
                    Ply.ReferenceHub.gameObject.AddComponent<KeepAHPShield>();
                }
                Map.Broadcast(10, $"<color=green>AHP will not go down naturally, only by damage, it can go up if you get more AHP through medical items. The AHP Limit is: {plugin.Config.AhpValueLimit}</color>", Broadcast.BroadcastFlags.Normal);
            }
        }

        public void RunOnPlayerJoin(JoinedEventArgs PlyJoin)
        {
            if (AutoScaleOn && plugin.Config.EnableKeepScale)
            {
                if (PlayersWithRetainedScale.Contains(PlyJoin.Player.UserId)) {
                    if (!plugin.Config.DisableAutoScaleMessages)
                        PlyJoin.Player.Broadcast(5, $"Your playermodel scale was set to {plugin.Config.AutoScaleValue}x their normal size!");
                    PlyJoin.Player.Scale = new Vector3(plugin.Config.AutoScaleValue, plugin.Config.AutoScaleValue, plugin.Config.AutoScaleValue);
                }
            }
            if (plugin.Config.EnableAhpShield)
                PlyJoin.Player.ReferenceHub.gameObject.AddComponent<KeepAHPShield>();
        }

        public void RunOnPlayerLeave(LeftEventArgs PlyLeave)
        {
            if (PlayersWithRegen.Contains(PlyLeave.Player))
                PlayersWithRegen.Remove(PlyLeave.Player);
            if (PlayersWithInfiniteAmmo.Contains(PlyLeave.Player))
                PlayersWithInfiniteAmmo.Remove(PlyLeave.Player);
            if (PlayersThatCanPryGates.Contains(PlyLeave.Player))
                PlayersThatCanPryGates.Remove(PlyLeave.Player);
        }

        public void RunOnPlayerDeath(DiedEventArgs PlyDeath)
        {
            if (AllowRespawning)
            {
                IsWarheadDetonated = Warhead.IsDetonated;
                IsDecontanimationActivated = Map.IsLCZDecontaminated;
                Timing.CallDelayed(plugin.Config.RandomRespawnTimer, () => RevivePlayer(PlyDeath.Target));
            }
            if (plugin.Config.EnableGrenadeOnDeath)
                SpawnGrenadeOnPlayer(PlyDeath.Target, true);
            if (plugin.Config.EnableKillMessages)
                if (PlyDeath.Killer != PlyDeath.Target)
                    PlyDeath.Killer.ReferenceHub.hints.Show(new TextHint($"\n\n\n\n\n\n\n\n\nYou killed player \"{PlyDeath.Target.Nickname}\" with \"{PlyDeath.Killer.CurrentItem.id}\"", new HintParameter[]
                    {
                        new StringHintParameter("")
                    }, HintEffectPresets.FadeInAndOut(0.25f, 1f, 0f)));
                else
                    PlyDeath.Killer.ReferenceHub.hints.Show(new TextHint($"\n\n\n\n\n\n\n\n\nYou game ended yourself", new HintParameter[]
                    {
                        new StringHintParameter("")
                    }, HintEffectPresets.FadeInAndOut(0.25f, 1f, 0f)));
        }

        public void RunOnPlayerHurt(HurtingEventArgs PlyHurt)
        {
            if (PreventFallDamage)
                if (PlyHurt.DamageType == DamageTypes.Falldown)
                    PlyHurt.Amount = 0;
        }

        public void RunOnMedItemUsed(UsedMedicalItemEventArgs MedUsed)
        {
            if (plugin.Config.EnableCustomHealing)
            {
                switch (MedUsed.Item)
                {
                    case ItemType.Painkillers:
                        MedUsed.Player.AdrenalineHealth += (int)plugin.Config.PainkillerAhpHealthValue;
                        break;
                    case ItemType.Medkit:
                        MedUsed.Player.AdrenalineHealth += (int)plugin.Config.MedkitAhpHealthValue;
                        break;
                    case ItemType.Adrenaline:
                        if (!(plugin.Config.AdrenalineAhpHealthValue <= 0))
                            MedUsed.Player.AdrenalineHealth += (int)plugin.Config.AdrenalineAhpHealthValue;
                        break;
                    case ItemType.SCP500:
                        MedUsed.Player.AdrenalineHealth += (int)plugin.Config.Scp500AhpHealthValue;
                        break;
                    case ItemType.SCP207:
                        MedUsed.Player.AdrenalineHealth += (int)plugin.Config.Scp207AhpHealthValue;
                        break;
                }
            }
            if (plugin.Config.EnableCustomEffectsAfterDrinkingScp207)
            {
                if (MedUsed.Item == ItemType.SCP207)
                {
                    if (!MedUsed.Player.ReferenceHub.TryGetComponent(out SCP207AbilityCounter ExplodeAfterDrinking))
                    {
                        MedUsed.Player.ReferenceHub.gameObject.AddComponent<SCP207AbilityCounter>();
                        return;
                    }
                    ExplodeAfterDrinking.Counter++;
                }
            }
        }

        public void RunWhenDoorIsInteractedWith(InteractingDoorEventArgs DoorInter)
        {
            if (plugin.Config.EnableDoorMessages)
            {
                if (PlayersThatCanPryGates.Contains(DoorInter.Player) && GatesThatExist.Contains(DoorInter.Door.DoorName))
                {
                    DoorInter.Door.PryGate();
                    if (!DoorInter.Player.IsBypassModeEnabled)
                    {
                        DoorInter.Player.ReferenceHub.hints.Show(new TextHint($"\n\n\n\n\n\n\n\n\n{plugin.Config.PryGateMessage}", new HintParameter[]
                        {
                            new StringHintParameter("")
                        }, HintEffectPresets.FadeInAndOut(0.25f, 1f, 0f)));
                    }
                    else
                    {
                        DoorInter.Player.ReferenceHub.hints.Show(new TextHint($"\n\n\n\n\n\n\n\n\n{plugin.Config.PryGateBypassMessage}", new HintParameter[]
                        {
                            new StringHintParameter("")
                        }, HintEffectPresets.FadeInAndOut(0.25f, 1f, 0f)));
                    }
                }
                else
                {
                    if (!DoorInter.Player.IsBypassModeEnabled)
                    {
                        if (DoorInter.Player.ReferenceHub.ItemInHandIsKeycard() && DoorsThatAreLocked.Contains(DoorInter.Door.DoorName))
                        {
                            if (DoorInter.IsAllowed)
                            {
                                DoorInter.Player.ReferenceHub.hints.Show(new TextHint($"\n\n\n\n\n\n\n\n\n{plugin.Config.UnlockedDoorMessage}", new HintParameter[]
                                {
                                    new StringHintParameter("")
                                }, HintEffectPresets.FadeInAndOut(0.25f, 1f, 0f)));
                            }
                            else
                            {
                                DoorInter.Player.ReferenceHub.hints.Show(new TextHint($"\n\n\n\n\n\n\n\n\n{plugin.Config.LockedDoorMessage}", new HintParameter[]
                                {
                                    new StringHintParameter("")
                                }, HintEffectPresets.FadeInAndOut(0.25f, 1f, 0f)));
                            }
                        }
                        else if (!DoorInter.Player.ReferenceHub.ItemInHandIsKeycard() && DoorsThatAreLocked.Contains(DoorInter.Door.DoorName))
                        {
                            DoorInter.Player.ReferenceHub.hints.Show(new TextHint($"\n\n\n\n\n\n\n\n\n{plugin.Config.NeedKeycardMessage}", new HintParameter[]
                            {
                                new StringHintParameter("")
                            }, HintEffectPresets.FadeInAndOut(0.25f, 1f, 0f)));
                        }
                    }
                    else if (DoorInter.Player.IsBypassModeEnabled && DoorsThatAreLocked.Contains(DoorInter.Door.DoorName))
                    {
                        if (DoorInter.Player.ReferenceHub.ItemInHandIsKeycard())
                        {
                            DoorInter.Player.ReferenceHub.hints.Show(new TextHint($"\n\n\n\n\n\n\n\n\n{plugin.Config.BypassWithKeycardMessage}", new HintParameter[]
                            {
                                new StringHintParameter("")
                            }, HintEffectPresets.FadeInAndOut(0.25f, 1f, 0f)));
                        }
                        else
                        {
                            DoorInter.Player.ReferenceHub.hints.Show(new TextHint($"\n\n\n\n\n\n\n\n\n{plugin.Config.BypassKeycardMessage}", new HintParameter[]
                            {
                                new StringHintParameter("")
                            }, HintEffectPresets.FadeInAndOut(0.25f, 1f, 0f)));
                        }
                    }
                }
            }
        }

        public void RunWhenPlayerEntersFemurBreaker(EnteringFemurBreakerEventArgs FemurBreaker)
        {
            if (plugin.Config.EnableScp106AdvancedGod)
            {
                foreach (Player Ply in Player.List)
                {
                    if (!(Ply.Role == RoleType.Scp106))
                        continue;

                    if (Ply.IsGodModeEnabled)
                    {
                        FemurBreaker.IsAllowed = false;
                        FemurBreaker.Player.Broadcast(2, "SCP-106 has advanced godmode, you cannot contain him", Broadcast.BroadcastFlags.Normal);
                        return;
                    }
                }
            }
        }

        public void RunWhenWarheadIsDetonated()
        {
            if (!IsWarheadDetonated && plugin.Config.EnableDoorsDestroyedWithWarhead)
            {
                foreach (Door door in UnityEngine.Object.FindObjectsOfType<Door>())
                {
                    door.Networkdestroyed = true;
                    door.Networklocked = true;
                }
            }
        }

        public void RunWhenWarheadIsStopped(StoppingEventArgs WarheadStop)
        {
            if (plugin.Config.EnableWarheadDetonationWhenCanceledChance && !IsWarheadDetonated)
            {
                int NewChance = RandNum.Next(0, 100);
                if (NewChance < plugin.Config.InstantWarheadDetonationChance)
                {
                    Warhead.Start();
                    Warhead.DetonationTimer = 0.05f;
                    Map.Broadcast(5, "<color=red>Someone tried to disable the warhead but pressed the wrong button</color>");
                }
            }
        }

        public void RunWhenTeamRespawns(RespawningTeamEventArgs TeamRspwn)
        {
            if (plugin.Config.EnableReverseRoleRespawnWaves)
                Timing.CallDelayed(0.1f, () => ChaosRespawnHandle = Timing.RunCoroutine(SpawnReverseOfWave(TeamRspwn.Players, TeamRspwn.NextKnownTeam == Respawning.SpawnableTeamType.ChaosInsurgency)));
        }

        public void RunWhenNTFSpawns(AnnouncingNtfEntranceEventArgs NTFAnnouncement)
        {
            NTFAnnouncement.IsAllowed = false;
            NineTailedFoxAnnouncer.singleton.ServerOnlyAddGlitchyPhrase(FormatMessage(CreativeToolbox.ConfigRef.Config.NineTailedFoxAnnouncement, NTFAnnouncement.UnitName, NTFAnnouncement.UnitNumber, NTFAnnouncement.ScpsLeft), CreativeToolbox.ConfigRef.Config.NineTailedFoxAnnouncementGlitchChance * 0.01f, CreativeToolbox.ConfigRef.Config.NineTailedFoxAnnouncementJamChance * 0.01f);
        }

        public void RevivePlayer(Player ply)
        {
            if (ply.Role != RoleType.Spectator) return;

            int num = RandNum.Next(0, 7);
            switch (num)
            {
                case 0:
                    ply.Role = RoleType.NtfCadet;
                    break;
                case 1:
                    if (!IsWarheadDetonated && !IsDecontanimationActivated)
                        ply.Role = RoleType.ClassD;
                    else
                        ply.Role = RoleType.ChaosInsurgency;
                    break;
                case 2:
                    if (!IsWarheadDetonated)
                        ply.Role = RoleType.FacilityGuard;
                    else
                        ply.Role = RoleType.NtfCommander;
                    break;
                case 3:
                    ply.Role = RoleType.NtfLieutenant;
                    break;
                case 4:
                    ply.Role = RoleType.NtfScientist;
                    break;
                case 5:
                    ply.Role = RoleType.ChaosInsurgency;
                    break;
                case 6:
                    if (!IsWarheadDetonated && !IsDecontanimationActivated)
                        ply.Role = RoleType.Scientist;
                    else
                        ply.Role = RoleType.NtfLieutenant;
                    break;
                case 7:
                    ply.Role = RoleType.NtfCommander;
                    break;
            }
        }

        public static void SpawnGrenadeOnPlayer(Player PlayerToSpawnGrenade, bool UseCustomTimer)
        {
            GrenadeManager gm = PlayerToSpawnGrenade.ReferenceHub.gameObject.GetComponent<GrenadeManager>();
            Grenade gnade = UnityEngine.Object.Instantiate(gm.availableGrenades[0].grenadeInstance.GetComponent<Grenade>());
            if (UseCustomTimer)
                gnade.fuseDuration = CreativeToolbox.ConfigRef.Config.GrenadeTimerOnDeath;
            else
                gnade.fuseDuration = 0.01f;
            gnade.FullInitData(gm, PlayerToSpawnGrenade.Position, Quaternion.Euler(gnade.throwStartAngle), gnade.throwLinearVelocityOffset, gnade.throwAngularVelocity);
            NetworkServer.Spawn(gnade.gameObject);
        }

        public IEnumerator<float> SpawnReverseOfWave(List<Player> RespawnedPlayers, bool Chaos)
        {
            List<Vector3> StoredPositions = new List<Vector3>();
            foreach (Player Ply in RespawnedPlayers)
            {
                StoredPositions.Add(Ply.Position);
                if (Chaos)
                    Ply.Role = (RoleType)Enum.Parse(typeof(RoleType), RandNum.Next(11, 13).ToString());
                else
                    Ply.Role = RoleType.ChaosInsurgency;
            }
            yield return Timing.WaitForSeconds(0.2f);
            int index = 0;
            foreach (Player Ply in RespawnedPlayers)
            {
                Ply.Position = StoredPositions[index];
                index++;
            }
            Timing.KillCoroutines(ChaosRespawnHandle);
        }

        public static string FormatMessage(string Input, string UnitName, int UnitNumber, int ScpsLeft)
        {
            return RogerFKTokenReplace.ReplaceAfterToken(Input, '%', new Tuple<string, object>[] { 
                new Tuple<string, object>("unitname", $"NATO_{UnitName.ElementAt(0)}"), 
                new Tuple<string, object>("unitnumber", UnitNumber), 
                new Tuple<string, object>("scpnumber", (ScpsLeft > 0) ? $"{(CreativeToolbox.ConfigRef.Config.UseXmasScpInAnnouncement ? $"{ScpsLeft.ToString()} XMAS_SCPSUBJECTS" : $"AwaitingRecontainment {ScpsLeft.ToString()} ScpSubjects")}" : $"{(CreativeToolbox.ConfigRef.Config.UseXmasScpInAnnouncement ? "0 XMAS_SCPSUBJECTS" : "NoSCPsLeft")}")});
        }

        public static string FormatMessage(string Input, int ScpsLeft)
        {
            return RogerFKTokenReplace.ReplaceAfterToken(Input, '%', new Tuple<string, object>[] { 
                new Tuple<string, object>("scpnumber", (ScpsLeft > 0) ? $"{(CreativeToolbox.ConfigRef.Config.UseXmasScpInAnnouncement ? $"{ScpsLeft.ToString()} XMAS_SCPSUBJECTS" : $"AwaitingRecontainment {ScpsLeft.ToString()} ScpSubjects")}" : $"{(CreativeToolbox.ConfigRef.Config.UseXmasScpInAnnouncement ? "0 XMAS_SCPSUBJECTS" : "NoSCPsLeft")}")});
        }
    }
}
