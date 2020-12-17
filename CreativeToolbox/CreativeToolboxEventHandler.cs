using Exiled.API.Features;

namespace CreativeToolbox
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Exiled.Events.EventArgs;
    using Grenades;
    using Hints;
    using MEC;
    using Mirror;
    using UnityEngine;
    using static CreativeToolbox;

    public sealed class CreativeToolboxEventHandler
    {
        public static readonly StringBuilder PlayerLister = new StringBuilder();
        public static readonly HashSet<Player> PlayersThatCanPryGates = new HashSet<Player>();
        public static readonly HashSet<Player> PlayersWithRegen = new HashSet<Player>();
        public static readonly HashSet<Player> PlayersWithInfiniteAmmo = new HashSet<Player>();
        public static readonly HashSet<string> PlayersWithRetainedScale = new HashSet<string>();
        private readonly System.Random _randNum = new System.Random();
        private bool _isWarheadDetonated;
        private bool _isDecontaminationActivated;
        public static bool AllowRespawning;
        public static bool PreventFallDamage;
        public static bool WasDecontaminationCommandRun;
        public static bool AutoScaleOn;
        private CoroutineHandle _chaosRespawnHandle;

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
            if (Instance.Config.EnableFallDamagePrevention)
                PreventFallDamage = true;
            if (Instance.Config.EnableAutoScaling)
            {
                foreach (Player ply in Player.List)
                {
                    if (!Instance.Config.DisableAutoScaleMessages)
                        Map.Broadcast(5,
                            $"Everyone who joined has their player model scale set to {Instance.Config.AutoScaleValue}x!");
                    ply.Scale = new Vector3(Instance.Config.AutoScaleValue, Instance.Config.AutoScaleValue,
                        Instance.Config.AutoScaleValue);
                    PlayersWithRetainedScale.Add(ply.UserId);
                    AutoScaleOn = true;
                }
            }

            if (Instance.Config.EnableGrenadeOnDeath && !Instance.Config.PreventCtBroadcasts)
                Map.Broadcast(10,
                    $"<color=red>Warning: Grenades spawn after you die, they explode after {Instance.Config.GrenadeTimerOnDeath} seconds of them spawning, be careful!</color>");
            if (!Instance.Config.EnableAhpShield)
                return;

            foreach (Player ply in Player.List)
                ply.ReferenceHub.gameObject.AddComponent<KeepAhpShield>();


            if (!Instance.Config.PreventCtBroadcasts)
                Map.Broadcast(10,
                    $"<color=green>AHP will not go down naturally, only by damage, it can go up if you get more AHP through medical items. The AHP Limit is: {Instance.Config.AhpValueLimit}</color>");
        }

        public void RunOnPlayerJoin(JoinedEventArgs ev)
        {
            if (AutoScaleOn && Instance.Config.EnableKeepScale)
            {
                if (PlayersWithRetainedScale.Contains(ev.Player.UserId))
                {
                    if (!Instance.Config.DisableAutoScaleMessages)
                        ev.Player.Broadcast(5,
                            $"Your player model scale was set to {Instance.Config.AutoScaleValue}x their normal size!");
                    ev.Player.Scale = new Vector3(Instance.Config.AutoScaleValue, Instance.Config.AutoScaleValue,
                        Instance.Config.AutoScaleValue);
                }
            }

            if (Instance.Config.EnableAhpShield)
                ev.Player.ReferenceHub.gameObject.AddComponent<KeepAhpShield>();
        }

        public void RunOnPlayerLeave(LeftEventArgs ev)
        {
            if (PlayersWithRegen.Contains(ev.Player))
                PlayersWithRegen.Remove(ev.Player);
            if (PlayersWithInfiniteAmmo.Contains(ev.Player))
                PlayersWithInfiniteAmmo.Remove(ev.Player);
            if (PlayersThatCanPryGates.Contains(ev.Player))
                PlayersThatCanPryGates.Remove(ev.Player);
        }

        public void RunOnPlayerDeath(DiedEventArgs ev)
        {
            if (AllowRespawning)
            {
                _isWarheadDetonated = Warhead.IsDetonated;
                _isDecontaminationActivated = Map.IsLCZDecontaminated;
                Timing.CallDelayed(Instance.Config.RandomRespawnTimer, () => RevivePlayer(ev.Target));
            }

            if (Instance.Config.EnableGrenadeOnDeath)
                SpawnGrenadeOnPlayer(ev.Target, true);
            if (!Instance.Config.EnableKillMessages)
                return;
            if (ev.Killer != ev.Target)
                ev.Killer.ReferenceHub.hints.Show(new TextHint(
                    $"\n\n\n\n\n\n\n\n\nYou killed player \"{ev.Target.Nickname}\" with \"{ev.Killer.CurrentItem.id}\"",
                    new HintParameter[]
                    {
                        new StringHintParameter("")
                    }, HintEffectPresets.FadeInAndOut(0.25f, 1f, 0f)));
            else
                ev.Killer.ReferenceHub.hints.Show(new TextHint($"\n\n\n\n\n\n\n\n\nYou game ended yourself",
                    new HintParameter[]
                    {
                        new StringHintParameter("")
                    }, HintEffectPresets.FadeInAndOut(0.25f, 1f, 0f)));
        }

        public void RunOnPlayerHurt(HurtingEventArgs ev)
        {
            if (!PreventFallDamage)
                return;

            if (ev.DamageType == DamageTypes.Falldown)
                ev.Amount = 0;
        }

        public void RunOnMedItemUsed(UsedMedicalItemEventArgs ev)
        {
            if (Instance.Config.EnableCustomHealing)
            {
                switch (ev.Item)
                {
                    case ItemType.Painkillers:
                        ev.Player.AdrenalineHealth += (int) Instance.Config.PainkillerAhpHealthValue;
                        break;
                    case ItemType.Medkit:
                        ev.Player.AdrenalineHealth += (int) Instance.Config.MedkitAhpHealthValue;
                        break;
                    case ItemType.Adrenaline:
                        if (!(Instance.Config.AdrenalineAhpHealthValue <= 0))
                            ev.Player.AdrenalineHealth += (int) Instance.Config.AdrenalineAhpHealthValue;
                        break;
                    case ItemType.SCP500:
                        ev.Player.AdrenalineHealth += (int) Instance.Config.Scp500AhpHealthValue;
                        break;
                    case ItemType.SCP207:
                        ev.Player.AdrenalineHealth += (int) Instance.Config.Scp207AhpHealthValue;
                        break;
                }
            }

            if (!Instance.Config.EnableCustomEffectsAfterDrinkingScp207 || ev.Item != ItemType.SCP207) 
                return;

            if (!ev.Player.ReferenceHub.TryGetComponent(out Scp207AbilityCounter scp207AbilityCounter))
            {
                ev.Player.ReferenceHub.gameObject.AddComponent<Scp207AbilityCounter>();
                return;
            }

            scp207AbilityCounter.counter++;
        }

        public void RunWhenDoorIsInteractedWith(InteractingDoorEventArgs ev)
        {
            if (ev.Player.Team == Team.SCP || ev.Player.Role == RoleType.Spectator)
                return;
            if (PlayersThatCanPryGates.Contains(ev.Player))
                ev.Door.PryGate();
            if (!Instance.Config.EnableDoorMessages) 
                return;
            
            if (PlayersThatCanPryGates.Contains(ev.Player) && ev.Door.doorType == Door.DoorTypes.HeavyGate)
            {
                if (!ev.Player.IsBypassModeEnabled)
                {
                    ev.Player.ReferenceHub.hints.Show(new TextHint(
                        $"\n\n\n\n\n\n\n\n\n{Instance.Config.PryGateMessage}", new HintParameter[]
                        {
                            new StringHintParameter("")
                        }, HintEffectPresets.FadeInAndOut(0.25f, 1f, 0f)));
                }
                else
                {
                    ev.Player.ReferenceHub.hints.Show(new TextHint(
                        $"\n\n\n\n\n\n\n\n\n{Instance.Config.PryGateBypassMessage}", new HintParameter[]
                        {
                            new StringHintParameter("")
                        }, HintEffectPresets.FadeInAndOut(0.25f, 1f, 0f)));
                }
            }
            else
            {
                if (!ev.Player.IsBypassModeEnabled)
                {
                    if (ev.Player.ItemInHandIsKeycard()
                        && ev.Door.PermissionLevels != 0)
                    {
                        if (ev.IsAllowed)
                        {
                            ev.Player.ReferenceHub.hints.Show(new TextHint(
                                $"\n\n\n\n\n\n\n\n\n{Instance.Config.UnlockedDoorMessage}", new HintParameter[]
                                {
                                    new StringHintParameter("")
                                }, HintEffectPresets.FadeInAndOut(0.25f, 1f, 0f)));
                        }
                        else
                        {
                            ev.Player.ReferenceHub.hints.Show(new TextHint(
                                $"\n\n\n\n\n\n\n\n\n{Instance.Config.LockedDoorMessage}", new HintParameter[]
                                {
                                    new StringHintParameter("")
                                }, HintEffectPresets.FadeInAndOut(0.25f, 1f, 0f)));
                        }
                    }
                    else if (!ev.Player.ItemInHandIsKeycard() && ev.Door.PermissionLevels != 0)
                    {
                        ev.Player.ReferenceHub.hints.Show(new TextHint(
                            $"\n\n\n\n\n\n\n\n\n{Instance.Config.NeedKeycardMessage}", new HintParameter[]
                            {
                                new StringHintParameter("")
                            }, HintEffectPresets.FadeInAndOut(0.25f, 1f, 0f)));
                    }
                }
                else if (ev.Player.IsBypassModeEnabled && ev.Door.PermissionLevels != 0)
                {
                    if (ev.Player.ItemInHandIsKeycard())
                    {
                        ev.Player.ReferenceHub.hints.Show(new TextHint(
                            $"\n\n\n\n\n\n\n\n\n{Instance.Config.BypassWithKeycardMessage}", new HintParameter[]
                            {
                                new StringHintParameter("")
                            }, HintEffectPresets.FadeInAndOut(0.25f, 1f, 0f)));
                    }
                    else
                    {
                        ev.Player.ReferenceHub.hints.Show(new TextHint(
                            $"\n\n\n\n\n\n\n\n\n{Instance.Config.BypassKeycardMessage}", new HintParameter[]
                            {
                                new StringHintParameter("")
                            }, HintEffectPresets.FadeInAndOut(0.25f, 1f, 0f)));
                    }
                }
            }
        }

        public void RunWhenPlayerEntersFemurBreaker(EnteringFemurBreakerEventArgs ev)
        {
            if (!Instance.Config.EnableScp106AdvancedGod)
                return;

            foreach (Player ply in Player.List)
            {
                if (ply.Role != RoleType.Scp106 || !ply.IsGodModeEnabled)
                    continue;

                ev.IsAllowed = false;
                if (!Instance.Config.PreventCtBroadcasts)
                    ev.Player.Broadcast(2, "SCP-106 has advanced godmode, you cannot contain him",
                        Broadcast.BroadcastFlags.Normal);
                return;
            }
        }

        public void RunWhenWarheadIsDetonated()
        {
            if (_isWarheadDetonated || !Instance.Config.EnableDoorsDestroyedWithWarhead)
                return;

            foreach (Door door in UnityEngine.Object.FindObjectsOfType<Door>())
            {
                door.Networkdestroyed = true;
                door.Networklocked = true;
            }
        }

        public void RunWhenWarheadIsStopped(StoppingEventArgs ev)
        {
            if (!Instance.Config.EnableWarheadDetonationWhenCanceledChance || _isWarheadDetonated)
                return;

            int newChance = _randNum.Next(0, 100);
            if (newChance >= Instance.Config.InstantWarheadDetonationChance)
                return;

            Warhead.Start();
            Warhead.DetonationTimer = 0.05f;
            if (!Instance.Config.PreventCtBroadcasts)
                Map.Broadcast(5,
                    "<color=red>Someone tried to disable the warhead but pressed the wrong button</color>");
        }

        public void RunWhenTeamRespawns(RespawningTeamEventArgs ev)
        {
            if (Instance.Config.EnableReverseRoleRespawnWaves)
                Timing.CallDelayed(0.1f,
                    () => _chaosRespawnHandle = Timing.RunCoroutine(SpawnReverseOfWave(ev.Players,
                        ev.NextKnownTeam == Respawning.SpawnableTeamType.ChaosInsurgency)));
        }

        public void RunWhenNTFSpawns(AnnouncingNtfEntranceEventArgs ev)
        {
            ev.IsAllowed = false;
            NineTailedFoxAnnouncer.singleton.ServerOnlyAddGlitchyPhrase(
                FormatMessage(Instance.Config.NineTailedFoxAnnouncement, ev.UnitName, ev.UnitNumber, ev.ScpsLeft),
                Instance.Config.NineTailedFoxAnnouncementGlitchChance * 0.01f,
                Instance.Config.NineTailedFoxAnnouncementJamChance * 0.01f);
        }

        private void RevivePlayer(Player ply)
        {
            if (ply.Role != RoleType.Spectator) return;

            int num = _randNum.Next(0, 7);
            switch (num)
            {
                case 0:
                    ply.Role = RoleType.NtfCadet;
                    break;
                case 1:
                    if (!_isWarheadDetonated && !_isDecontaminationActivated)
                        ply.Role = RoleType.ClassD;
                    else
                        ply.Role = RoleType.ChaosInsurgency;
                    break;
                case 2:
                    ply.Role = !_isWarheadDetonated ? RoleType.FacilityGuard : RoleType.NtfCommander;
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
                    if (!_isWarheadDetonated && !_isDecontaminationActivated)
                        ply.Role = RoleType.Scientist;
                    else
                        ply.Role = RoleType.NtfLieutenant;
                    break;
                case 7:
                    ply.Role = RoleType.NtfCommander;
                    break;
            }
        }

        public static void SpawnGrenadeOnPlayer(Player playerToSpawnGrenade, bool useCustomTimer)
        {
            GrenadeManager gm = playerToSpawnGrenade.ReferenceHub.gameObject.GetComponent<GrenadeManager>();
            Grenade grenade =
                UnityEngine.Object.Instantiate(gm.availableGrenades[0].grenadeInstance.GetComponent<Grenade>());
            grenade.fuseDuration = useCustomTimer ? Instance.Config.GrenadeTimerOnDeath : 0.01f;
            grenade.FullInitData(gm, playerToSpawnGrenade.Position, Quaternion.Euler(grenade.throwStartAngle),
                grenade.throwLinearVelocityOffset, grenade.throwAngularVelocity, playerToSpawnGrenade.Team);
            NetworkServer.Spawn(grenade.gameObject);
        }

        private IEnumerator<float> SpawnReverseOfWave(List<Player> respawnedPlayers, bool chaos)
        {
            List<Vector3> storedPositions = new List<Vector3>();
            foreach (Player ply in respawnedPlayers)
            {
                storedPositions.Add(ply.Position);
                if (chaos)
                    ply.Role = (RoleType) Enum.Parse(typeof(RoleType), _randNum.Next(11, 13).ToString());
                else
                    ply.Role = RoleType.ChaosInsurgency;
            }

            yield return Timing.WaitForSeconds(0.2f);
            int index = 0;
            foreach (Player ply in respawnedPlayers)
            {
                ply.Position = storedPositions[index];
                index++;
            }

            Timing.KillCoroutines(_chaosRespawnHandle);
        }

        private static string FormatMessage(string input, string unitName, int unitNumber, int scpsLeft)
        {
            return input.ReplaceAfterToken('%', new[]
            {
                new Tuple<string, object>("unitname", $"NATO_{unitName.ElementAt(0)}"),
                new Tuple<string, object>("unitnumber", unitNumber),
                new Tuple<string, object>("scpnumber",
                    (scpsLeft > 0)
                        ? $"{(Instance.Config.UseXmasScpInAnnouncement ? $"{scpsLeft.ToString()} XMAS_SCPSUBJECTS" : $"AwaitingRecontainment {scpsLeft.ToString()} ScpSubjects")}"
                        : "NoSCPsLeft")
            });
        }

        public static string FormatMessage(string input, int scpsLeft)
        {
            return input.ReplaceAfterToken('%', new[]
            {
                new Tuple<string, object>("scpnumber",
                    (scpsLeft > 0)
                        ? $"{(Instance.Config.UseXmasScpInAnnouncement ? $"{scpsLeft.ToString()} XMAS_SCPSUBJECTS" : $"AwaitingRecontainment {scpsLeft.ToString()} ScpSubjects")}"
                        : "NoSCPsLeft")
            });
        }
    }
}