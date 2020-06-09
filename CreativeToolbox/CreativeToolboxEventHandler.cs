using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Exiled.Permissions.Extensions;
using Grenades;
using Hints;
using MEC;
using Mirror;
using PlayableScps;
using Targeting;
using UnityEngine;

namespace CreativeToolbox
{
    public class CreativeToolboxEventHandler
    {
        //HashSet<ReferenceHub> PlayersWith207 = new HashSet<ReferenceHub>();
        //HashSet<ReferenceHub> PlayersWithInvisibility = new HashSet<ReferenceHub>();
        public static HashSet<ReferenceHub> PlayersWithAdvancedGodmode = new HashSet<ReferenceHub>();
        HashSet<ReferenceHub> PlayersThatCanPryGates = new HashSet<ReferenceHub>();
        HashSet<ReferenceHub> PlayersWithRegen = new HashSet<ReferenceHub>();
        HashSet<ReferenceHub> PlayersWithInfiniteAmmo = new HashSet<ReferenceHub>();
        HashSet<String> PlayersWithRetainedScale = new HashSet<String>();
        string[] DoorsThatAreLocked = { "012", "049_ARMORY", "079_FIRST", "079_SECOND", "096", "106_BOTTOM", 
            "106_PRIMARY", "106_SECONDARY", "173_ARMORY", "914", "CHECKPOINT_ENT", "CHECKPOINT_LCZ_A", "CHECKPOINT_LCZ_B",
            "GATE_A", "GATE_B", "HCZ_ARMORY", "HID", "INTERCOM", "LCZ_ARMORY", "NUKE_ARMORY" };
        string[] GatesThatExist = { "914", "GATE_A", "GATE_B", "079_FIRST", "079_SECOND" };
        System.Random RandNum = new System.Random();
        Item[] AvailableItems;
        bool IsWarheadDetonated;
        bool IsDecontanimationActivated;
        bool AllowRespawning = false;
        bool PreventFallDamage = false;
        bool WasDeconCommandRun = false;
        bool AutoScaleOn = false;

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
            if (CreativeConfig.EnableFallDamagePrevent)
                PreventFallDamage = true;
            if (CreativeConfig.EnableAutoScaling)
            {
                foreach (Player Ply in Player.List)
                {
                    if (!CreativeConfig.DisableAutoScalingMessage)
                        Map.Broadcast(5, $"Everyone who joined has their playermodel scale set to {CreativeConfig.AutoScaleValue}x!", Broadcast.BroadcastFlags.Normal);
                    Ply.Scale = new Vector3(CreativeConfig.AutoScaleValue, CreativeConfig.AutoScaleValue, CreativeConfig.AutoScaleValue);
                    PlayersWithRetainedScale.Add(Ply.UserId);
                    AutoScaleOn = true;
                }
            }
            if (CreativeConfig.EnableGrenadeSpawnOnDeath)
                Map.Broadcast(10, $"<color=red>Warning: Grenades spawn after you die, they explode after {CreativeConfig.GrenadeDeathTimer} seconds of them spawning, be careful!</color>", Broadcast.BroadcastFlags.Normal);
        }

        public void RunOnPlayerJoin(JoinedEventArgs PlyJoin)
        {
            if (CreativeConfig.EnableAutoScaling && CreativeConfig.EnableRetainingScaling)
            {
                if (PlayersWithRetainedScale.Contains(PlyJoin.Player.UserId)) {
                    if (!CreativeConfig.DisableAutoScalingMessage)
                        PlyJoin.Player.Broadcast(5, $"Your playermodel scale was set to {CreativeConfig.AutoScaleValue}x!", Broadcast.BroadcastFlags.Normal);
                    PlyJoin.Player.Scale = new Vector3(CreativeConfig.AutoScaleValue, CreativeConfig.AutoScaleValue, CreativeConfig.AutoScaleValue);
                }
            }
        }

        public void RunOnPlayerLeave(LeftEventArgs PlyLeave)
        {
            if (PlayersWithRegen.Contains(PlyLeave.Player.ReferenceHub))
                PlayersWithRegen.Remove(PlyLeave.Player.ReferenceHub);
            if (PlayersWithInfiniteAmmo.Contains(PlyLeave.Player.ReferenceHub))
                PlayersWithInfiniteAmmo.Remove(PlyLeave.Player.ReferenceHub);
            if (PlayersThatCanPryGates.Contains(PlyLeave.Player.ReferenceHub))
                PlayersThatCanPryGates.Remove(PlyLeave.Player.ReferenceHub);
            if (PlayersWithAdvancedGodmode.Contains(PlyLeave.Player.ReferenceHub))
                PlayersWithAdvancedGodmode.Remove(PlyLeave.Player.ReferenceHub);
        }

        /*public void RunOnPlayerSpawn(PlayerSpawnEvent PlySpwn)
        {
            if (PlayersWith207.Contains(PlySpwn.Player))
                Timing.CallDelayed(1f, () => PlySpwn.Player.playerEffectsController.EnableEffect(new Scp207(PlySpwn.Player)));
        }*/

        public void RunOnPlayerDeath(DiedEventArgs PlyDeath)
        {
            if (AllowRespawning)
            {
                IsWarheadDetonated = Warhead.IsDetonated;
                IsDecontanimationActivated = Map.IsLCZDecontaminated;
                Timing.CallDelayed(CreativeConfig.RandomRespawnTimer, () => RevivePlayer(PlyDeath.Target));
            }
            if (CreativeConfig.EnableGrenadeSpawnOnDeath)
            {
                SpawnGrenadeOnPlayer(PlyDeath.Target, true);
            }
        }

        public void RunOnPlayerHurt(HurtingEventArgs PlyHurt)
        {
            if (PreventFallDamage)
                if (PlyHurt.DamageType == DamageTypes.Falldown)
                    PlyHurt.Amount = 0;
        }

        public void RunOnMedItemUsed(UsedMedicalItemEventArgs MedUsed)
        {
            if (CreativeConfig.EnableMedicalItemMod)
            {
                switch (MedUsed.Item)
                {
                    case ItemType.Painkillers:
                        MedUsed.Player.AdrenalineHealth += (int)CreativeConfig.PainkillerAHPHealthValue;
                        break;
                    case ItemType.Medkit:
                        MedUsed.Player.AdrenalineHealth += (int)CreativeConfig.MedkitAHPHealthValue;
                        break;
                    case ItemType.Adrenaline:
                        if (!(CreativeConfig.AdrenalineAHPHealthValue <= 0))
                            MedUsed.Player.AdrenalineHealth += (int)CreativeConfig.AdrenalineAHPHealthValue;
                        break;
                    case ItemType.SCP500:
                        MedUsed.Player.AdrenalineHealth += (int)CreativeConfig.SCP500AHPHealthValue;
                        break;
                    case ItemType.SCP207:
                        MedUsed.Player.AdrenalineHealth += (int)CreativeConfig.SCP207AHPHealthValue;
                        break;
                }
            }
            if (CreativeConfig.EnableExplodingAfterTooMuchSCP207)
            {
                if (MedUsed.Item == ItemType.SCP207)
                {
                    if (!MedUsed.Player.ReferenceHub.TryGetComponent(out SCP207Counter ExplodeAfterDrinking))
                    {
                        MedUsed.Player.ReferenceHub.gameObject.AddComponent<SCP207Counter>();
                        return;
                    }
                    ExplodeAfterDrinking.Counter++;
                }
            }
        }

        public void RunWhenDoorIsInteractedWith(InteractingDoorEventArgs DoorInter)
        {
            if (CreativeConfig.EnableDoorMessages)
            {
                if (PlayersThatCanPryGates.Contains(DoorInter.Player.ReferenceHub) && GatesThatExist.Contains(DoorInter.Door.DoorName))
                {
                    DoorInter.Door.PryGate();
                    if (!DoorInter.Player.IsBypassModeEnabled)
                    {
                        DoorInter.Player.ReferenceHub.hints.Show(new TextHint($"\n\n\n\n\n\n\n\n\n{CreativeConfig.PryGatesMessage}", new HintParameter[]
                        {
                            new StringHintParameter("")
                        }, HintEffectPresets.FadeInAndOut(0.25f, 1f, 0f)));
                    }
                    else
                    {
                        DoorInter.Player.ReferenceHub.hints.Show(new TextHint($"\n\n\n\n\n\n\n\n\n{CreativeConfig.PryGatesBypassMessage}", new HintParameter[]
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
                                DoorInter.Player.ReferenceHub.hints.Show(new TextHint($"\n\n\n\n\n\n\n\n\n{CreativeConfig.UnlockedDoorMessage}", new HintParameter[]
                                {
                                    new StringHintParameter("")
                                }, HintEffectPresets.FadeInAndOut(0.25f, 1f, 0f)));
                            }
                            else
                            {
                                DoorInter.Player.ReferenceHub.hints.Show(new TextHint($"\n\n\n\n\n\n\n\n\n{CreativeConfig.LockedDoorMessage}", new HintParameter[]
                                {
                                    new StringHintParameter("")
                                }, HintEffectPresets.FadeInAndOut(0.25f, 1f, 0f)));
                            }
                        }
                        else if (!DoorInter.Player.ReferenceHub.ItemInHandIsKeycard() && DoorsThatAreLocked.Contains(DoorInter.Door.DoorName))
                        {
                            DoorInter.Player.ReferenceHub.hints.Show(new TextHint($"\n\n\n\n\n\n\n\n\n{CreativeConfig.NeedKeycardMessage}", new HintParameter[]
                            {
                                new StringHintParameter("")
                            }, HintEffectPresets.FadeInAndOut(0.25f, 1f, 0f)));
                        }
                    }
                    else if (DoorInter.Player.IsBypassModeEnabled && DoorsThatAreLocked.Contains(DoorInter.Door.DoorName))
                    {
                        if (DoorInter.Player.ReferenceHub.ItemInHandIsKeycard())
                        {
                            DoorInter.Player.ReferenceHub.hints.Show(new TextHint($"\n\n\n\n\n\n\n\n\n{CreativeConfig.BypassWithKeycardInHandMessage}", new HintParameter[]
                            {
                                new StringHintParameter("")
                            }, HintEffectPresets.FadeInAndOut(0.25f, 1f, 0f)));
                        }
                        else
                        {
                            DoorInter.Player.ReferenceHub.hints.Show(new TextHint($"\n\n\n\n\n\n\n\n\n{CreativeConfig.BypassKeycardMessage}", new HintParameter[]
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
            if (PlayersWithAdvancedGodmode.Count == 0)
            {
                FemurBreaker.IsAllowed = false;
                FemurBreaker.Player.Broadcast((ushort)2, "SCP-106 has advanced godmode, you cannot contain him", Broadcast.BroadcastFlags.Normal);
            }
        }

        public void RunOnRemoteAdminCommand(SendingRemoteAdminCommandEventArgs RAComEv)
        {
            try
            {
                switch (RAComEv.Name.ToLower())
                {
                    case "advgod":
                        RAComEv.IsAllowed = false;
                        if (!RAComEv.Sender.CheckPermission("ct.advgod"))
                        {
                            RAComEv.Sender.RemoteAdminMessage("You are not authorized to use this command");
                            return;
                        }

                        if (RAComEv.Arguments.Count < 2)
                        {
                            RAComEv.Sender.RemoteAdminMessage("Invalid parameters! Syntax: advgod ((id/name)/*/all/clear/list) (Note: This only will be given to SCP-106 roles)");
                            return;
                        }

                        switch (RAComEv.Arguments.Count)
                        {
                            case 2:
                                switch (RAComEv.Arguments[0].ToLower())
                                {
                                    case "*":
                                    case "all":
                                        foreach (Player Ply in Player.List)
                                        {
                                            if (!(Ply.Role == RoleType.Scp106) || PlayersWithAdvancedGodmode.Contains(Ply.ReferenceHub))
                                                continue;

                                            Ply.ReferenceHub.gameObject.AddComponent<SCP106AdvancedGodComponent>();
                                            PlayersWithAdvancedGodmode.Add(Ply.ReferenceHub);
                                        }
                                        Map.Broadcast(5, "Everyone who is SCP-106 has Advanced Godmode!", Broadcast.BroadcastFlags.Normal);
                                        break;
                                    case "clear":
                                        foreach (Player Ply in Player.List)
                                        {
                                            if (!PlayersWithAdvancedGodmode.Contains(Ply.ReferenceHub) || Ply.ReferenceHub.TryGetComponent(out SCP106AdvancedGodComponent SCPAdvGod))
                                                continue;

                                            UnityEngine.Object.Destroy(SCPAdvGod);
                                        }
                                        PlayersWithAdvancedGodmode.Clear();
                                        Map.Broadcast(5, "Everyone who is SCP-106 does not have Advanced Godmode anymore!", Broadcast.BroadcastFlags.Normal);
                                        break;
                                    case "list":
                                        if (PlayersWithAdvancedGodmode.Count != 0)
                                        {
                                            string playerLister = "Players with Advanced Godmode on: ";
                                            foreach (ReferenceHub hub in PlayersWithAdvancedGodmode)
                                            {
                                                playerLister += hub.nicknameSync.MyNick + ", ";
                                            }
                                            playerLister = playerLister.Substring(0, playerLister.Count() - 2);
                                            RAComEv.Sender.RemoteAdminMessage(playerLister);
                                            return;
                                        }
                                        RAComEv.Sender.RemoteAdminMessage("There are no players currently online with Advanced Godmode on");
                                        break;
                                    default:
                                        Player ChosenPlayer = Player.Get(RAComEv.Arguments[1]);
                                        if (ChosenPlayer == null)
                                        {
                                            RAComEv.Sender.RemoteAdminMessage($"Player \"{RAComEv.Arguments[1]}\" not found");
                                            return;
                                        }

                                        if (!(ChosenPlayer.Role == RoleType.Scp106))
                                        {
                                            RAComEv.Sender.RemoteAdminMessage($"Player \"{ChosenPlayer.Nickname}\" is not SCP-106!");
                                            return;
                                        }

                                        if (ChosenPlayer.ReferenceHub.TryGetComponent(out SCP106AdvancedGodComponent AdvGod))
                                        {
                                            RAComEv.Sender.RemoteAdminMessage($"Player \"{ChosenPlayer.Nickname}\" already has Advanced Godmode!");
                                            return;
                                        }

                                        ChosenPlayer.ReferenceHub.gameObject.AddComponent<SCP106AdvancedGodComponent>();
                                        PlayersWithAdvancedGodmode.Add(ChosenPlayer.ReferenceHub);
                                        Player.Get(RAComEv.Arguments[1])?.Broadcast(3, "Advanced Godmode is enabled for you!", Broadcast.BroadcastFlags.Normal);
                                        break;
                                }
                                break;
                            default:
                                RAComEv.Sender.RemoteAdminMessage($"Invalid number of parameters! Value: {RAComEv.Arguments.Count}, Expected 2");
                                break;
                        }
                        break;
                    case "arspawn":
                        RAComEv.IsAllowed = false;
                        if (!RAComEv.Sender.CheckPermission("ct.arspawn"))
                        {
                            RAComEv.Sender.RemoteAdminMessage("You are not authorized to use this command");
                            return;
                        }

                        if (RAComEv.Arguments.Count < 1)
                        {
                            RAComEv.Sender.RemoteAdminMessage("Invalid parameters! Syntax: arspawn (on/off/value) (value (if choosing \"time\"))");
                            return;
                        }

                        switch (RAComEv.Arguments.Count)
                        {
                            case 1:
                                switch (RAComEv.Arguments[0].ToLower())
                                {
                                    case "on":
                                        if (!AllowRespawning)
                                        {
                                            RAComEv.Sender.RemoteAdminMessage("Auto respawning enabled!");
                                            Map.Broadcast(5, "<color=green>Random auto respawning enabled!</color>", Broadcast.BroadcastFlags.Normal);
                                            AllowRespawning = true;
                                            return;
                                        }
                                        RAComEv.Sender.RemoteAdminMessage("Auto respawning is already on!");
                                        break;
                                    case "off":
                                        if (AllowRespawning)
                                        {
                                            RAComEv.Sender.RemoteAdminMessage("Auto respawning disabled!");
                                            Map.Broadcast(5, "<color=red>Random auto respawning disabled!</color>", Broadcast.BroadcastFlags.Normal);
                                            AllowRespawning = false;
                                            return;
                                        }
                                        RAComEv.Sender.RemoteAdminMessage("Auto respawning is already off!");
                                        break;
                                    case "time":
                                        RAComEv.Sender.RemoteAdminMessage("Missing value for time!");
                                        break;
                                    default:
                                        RAComEv.Sender.RemoteAdminMessage("Please enter either \"on\" or \"off\" (If # of arguments is 1)!");
                                        break;
                                }
                                break;
                            case 2:
                                switch (RAComEv.Arguments[0].ToLower())
                                {
                                    case "time":
                                        if (float.TryParse(RAComEv.Arguments[1].ToLower(), out float rspwn) && rspwn > 0)
                                        {
                                            CreativeConfig.RandomRespawnTimer = rspwn;
                                            RAComEv.Sender.RemoteAdminMessage($"Auto respawning timer is now set to {rspwn} seconds!");
                                            Map.Broadcast(5, $"Auto respawning timer is now set to {rspwn} seconds!", Broadcast.BroadcastFlags.Normal);
                                            return;
                                        }
                                        RAComEv.Sender.RemoteAdminMessage($"Invalid value for auto respawn timer! Value: {RAComEv.Arguments[1].ToLower()}");
                                        break;
                                    default:
                                        RAComEv.Sender.RemoteAdminMessage("Please enter only \"time\"!");
                                        break;
                                }
                                break;
                            default:
                                RAComEv.Sender.RemoteAdminMessage($"Invalid number of parameters! Value: {RAComEv.Arguments.Count}, Expected 2");
                                break;
                        }
                        break;
                    case "autoscale":
                        RAComEv.IsAllowed = false;
                        if (!RAComEv.Sender.CheckPermission("ct.autoscale"))
                        {
                            RAComEv.Sender.RemoteAdminMessage("You are not authorized to use this command!");
                            return;
                        }

                        if (!CreativeConfig.EnableAutoScaling)
                        {
                            RAComEv.Sender.RemoteAdminMessage("Auto scaling cannot be modified!");
                            return;
                        }

                        if (AutoScaleOn)
                        {
                            foreach (Player Ply in Player.List)
                            {
                                Ply.Scale = Vector3.one;
                            }
                            if (!CreativeConfig.DisableAutoScalingMessage)
                                Map.Broadcast(5, "Everyone has been restored to their normal size!", Broadcast.BroadcastFlags.Normal);
                            PlayersWithRetainedScale.Clear();
                            AutoScaleOn = false;
                        }
                        else
                        {
                            foreach (Player Ply in Player.List)
                            {
                                Ply.Scale = new Vector3(CreativeConfig.AutoScaleValue, CreativeConfig.AutoScaleValue, CreativeConfig.AutoScaleValue);
                                PlayersWithRetainedScale.Add(Ply.UserId);
                            }
                            if (!CreativeConfig.DisableAutoScalingMessage)
                                Map.Broadcast(5, $"Everyone has their playermodel scale set to {CreativeConfig.AutoScaleValue}x!", Broadcast.BroadcastFlags.Normal);
                            AutoScaleOn = true;
                        }
                        break;
                    case "explode":
                        RAComEv.IsAllowed = false;
                        if (!RAComEv.Sender.CheckPermission("ct.explode"))
                        {
                            RAComEv.Sender.RemoteAdminMessage("You are not authorized to use this command!");
                            return;
                        }

                        if (RAComEv.Arguments.Count < 1)
                        {
                            RAComEv.Sender.RemoteAdminMessage("Invalid parameters! Syntax: explode ((id/name)/*/all)");
                            return;
                        }

                        switch (RAComEv.Arguments[0].ToLower())
                        {
                            case "all":
                            case "*":
                                foreach (Player Ply in Player.List)
                                {
                                    switch (Ply.Role)
                                    {
                                        case RoleType.Spectator:
                                        case RoleType.None:
                                            break;
                                        default:
                                            Ply.Kill();
                                            SpawnGrenadeOnPlayer(Ply, false);
                                            break;
                                    }
                                }
                                RAComEv.Sender.RemoteAdminMessage($"Everyone exploded, Hubert cannot believe you did this");
                                break;
                            default:
                                Player ChosenPlayer = Player.Get(RAComEv.Arguments[0]);
                                if (ChosenPlayer == null)
                                {
                                    RAComEv.Sender.RemoteAdminMessage($"Player \"{RAComEv.Arguments[0]}\" not found");
                                    return;
                                }
                                
                                switch (ChosenPlayer.Role)
                                {
                                    case RoleType.Spectator:
                                    case RoleType.None:
                                        RAComEv.Sender.RemoteAdminMessage($"Player \"{ChosenPlayer.ReferenceHub.nicknameSync.MyNick}\" is not a valid class to explode, not this time!");
                                        break;
                                    default:
                                        RAComEv.Sender.RemoteAdminMessage($"Player \"{ChosenPlayer.ReferenceHub.nicknameSync.MyNick}\" game ended (exploded)");
                                        ChosenPlayer.Kill();
                                        SpawnGrenadeOnPlayer(ChosenPlayer, false);
                                        break;
                                }
                                break;
                        }
                        break;
                    case "fdamage":
                        RAComEv.IsAllowed = false;
                        if (!RAComEv.Sender.CheckPermission("ct.fdamage"))
                        {
                            RAComEv.Sender.RemoteAdminMessage("You are not authorized to use this command!");
                            return;
                        }

                        if (CreativeConfig.LockFallDamageMod)
                        {
                            RAComEv.Sender.RemoteAdminMessage("Fall damage cannot be modified!");
                            return;
                        }

                        if (RAComEv.Arguments.Count < 1)
                        {
                            RAComEv.Sender.RemoteAdminMessage("Invalid parameters! Syntax: fdamage (on/off)");
                            return;
                        }

                        switch (RAComEv.Arguments.Count)
                        {
                            case 1:
                                switch (RAComEv.Arguments[0].ToLower())
                                {
                                    case "on":
                                        if (PreventFallDamage)
                                        {
                                            PreventFallDamage = false;
                                            RAComEv.Sender.RemoteAdminMessage("Fall damage enabled!");
                                            Map.Broadcast(5, "<color=green>Fall damage enabled!</color>", Broadcast.BroadcastFlags.Normal);
                                            return;
                                        }
                                        RAComEv.Sender.RemoteAdminMessage("Fall damage is already on!");
                                        break;
                                    case "off":
                                        if (!PreventFallDamage)
                                        {
                                            PreventFallDamage = true;
                                            RAComEv.Sender.RemoteAdminMessage("Fall damage disabled!");
                                            Map.Broadcast(5, "<color=red>Fall damage disabled!</color>", Broadcast.BroadcastFlags.Normal);
                                            return;
                                        }
                                        RAComEv.Sender.RemoteAdminMessage("Fall damage is already off!");
                                        break;
                                    default:
                                        RAComEv.Sender.RemoteAdminMessage("Please enter either \"on\" or \"off\"!");
                                        break;
                                }
                                break;
                            default:
                                RAComEv.Sender.RemoteAdminMessage($"Invalid number of parameters! Value: {RAComEv.Arguments.Count}");
                                break;
                        }
                        break;
                    case "giveammo":
                        RAComEv.IsAllowed = false;
                        if (!RAComEv.Sender.CheckPermission("ct.giveammo"))
                        {
                            RAComEv.Sender.RemoteAdminMessage("You are not authorized to use this command");
                            return;
                        }

                        if (RAComEv.Arguments.Count < 3)
                        {
                            RAComEv.Sender.RemoteAdminMessage("Invalid parameters! Syntax: giveam (*/all/(id or name)) (5/7/9) (amount)");
                            return;
                        }

                        switch (RAComEv.Arguments.Count)
                        {
                            case 3:
                                switch (RAComEv.Arguments[0].ToLower())
                                {
                                    case "*":
                                    case "all":
                                        switch (RAComEv.Arguments[1].ToLower())
                                        {
                                            case "5":
                                                if (int.TryParse(RAComEv.Arguments[2].ToLower(), out int FiveMM) && FiveMM >= 0)
                                                {
                                                    foreach (Player Ply in Player.List)
                                                    {
                                                        if (Ply.Role != RoleType.None)
                                                            Ply.SetAmmo(Exiled.API.Enums.AmmoType.Nato556, (uint) (Ply.GetAmmo(Exiled.API.Enums.AmmoType.Nato556) + FiveMM));
                                                    }
                                                    RAComEv.Sender.RemoteAdminMessage($"{FiveMM} 5.56mm ammo given to everyone!");
                                                    Map.Broadcast(3, $"Everyone has been given {FiveMM} 5.56mm ammo!", Broadcast.BroadcastFlags.Normal);
                                                    return;
                                                }
                                                RAComEv.Sender.RemoteAdminMessage($"Invalid value for ammo count! Value: {RAComEv.Arguments[3]}");
                                                break;
                                            case "7":
                                                if (int.TryParse(RAComEv.Arguments[2].ToLower(), out int SevenMM) && SevenMM >= 0)
                                                {
                                                    foreach (Player Ply in Player.List)
                                                    {
                                                        if (Ply.Role != RoleType.None)
                                                            Ply.SetAmmo(Exiled.API.Enums.AmmoType.Nato762, (uint) (Ply.GetAmmo(Exiled.API.Enums.AmmoType.Nato762) + SevenMM));
                                                    }
                                                    RAComEv.Sender.RemoteAdminMessage($"{SevenMM} 7.62mm ammo given to everyone!");
                                                    Map.Broadcast(3, $"Everyone has been given {SevenMM} 7.62mm ammo!", Broadcast.BroadcastFlags.Normal);
                                                    return;
                                                }
                                                RAComEv.Sender.RemoteAdminMessage($"Invalid value for ammo count! Value: {RAComEv.Arguments[3]}");
                                                break;
                                            case "9":
                                                if (int.TryParse(RAComEv.Arguments[2].ToLower(), out int NineMM) && NineMM >= 0)
                                                {
                                                    foreach (Player Ply in Player.List)
                                                    {
                                                        if (Ply.Role != RoleType.None)
                                                            Ply.SetAmmo(Exiled.API.Enums.AmmoType.Nato9, (uint) (Ply.GetAmmo(Exiled.API.Enums.AmmoType.Nato9) + NineMM));
                                                    }
                                                    RAComEv.Sender.RemoteAdminMessage($"{NineMM} 9.00mm ammo given to everyone!");
                                                    Map.Broadcast(3, $"Everyone has been given {NineMM} 9mm ammo!", Broadcast.BroadcastFlags.Normal);
                                                    return;
                                                }
                                                RAComEv.Sender.RemoteAdminMessage($"Invalid value for ammo count! Value: {RAComEv.Arguments[3]}");
                                                break;
                                            default:
                                                RAComEv.Sender.RemoteAdminMessage($"Please enter \"5\" (5.56mm), \"7\" (7.62mm), or \"9\" (9.00mm)!");
                                                break;
                                        }
                                        break;
                                    default:
                                        Player ChosenPlayer = Player.Get(RAComEv.Arguments[0]);
                                        if (ChosenPlayer == null)
                                        {
                                            RAComEv.Sender.RemoteAdminMessage($"Player \"{RAComEv.Arguments[0]}\" not found");
                                            return;
                                        }
                                        else if (ChosenPlayer.Role == RoleType.None)
                                        {
                                            RAComEv.Sender.RemoteAdminMessage("You cannot give ammo to a person with no role!");
                                            return;
                                        }
                                        switch (RAComEv.Arguments[1].ToLower())
                                        {
                                            case "5":
                                                if (int.TryParse(RAComEv.Arguments[2].ToLower(), out int FiveMM) && FiveMM >= 0)
                                                {
                                                    ChosenPlayer.SetAmmo(Exiled.API.Enums.AmmoType.Nato556, (uint) (ChosenPlayer.GetAmmo(Exiled.API.Enums.AmmoType.Nato556) + FiveMM));
                                                    RAComEv.Sender.RemoteAdminMessage($"{FiveMM} 5.56mm ammo given to \"{ChosenPlayer.ReferenceHub.nicknameSync.MyNick}\"!");
                                                    Player.Get(RAComEv.Arguments[0])?.Broadcast(3, $"You were given {FiveMM} of 5.56mm ammo!", Broadcast.BroadcastFlags.Normal);
                                                    return;
                                                }
                                                RAComEv.Sender.RemoteAdminMessage($"Invalid value for ammo count! Value: {RAComEv.Arguments[3]}");
                                                break;
                                            case "7":
                                                if (int.TryParse(RAComEv.Arguments[2].ToLower(), out int SevenMM) && SevenMM >= 0)
                                                {
                                                    ChosenPlayer.SetAmmo(Exiled.API.Enums.AmmoType.Nato762, (uint) (ChosenPlayer.GetAmmo(Exiled.API.Enums.AmmoType.Nato762) + SevenMM));
                                                    RAComEv.Sender.RemoteAdminMessage($"{SevenMM} 7.62mm ammo given to \"{ChosenPlayer.ReferenceHub.nicknameSync.MyNick}\"!");
                                                    Player.Get(RAComEv.Arguments[0])?.Broadcast(3, $"You were given {SevenMM} of 7.62mm ammo!", Broadcast.BroadcastFlags.Normal);
                                                    return;
                                                }
                                                RAComEv.Sender.RemoteAdminMessage($"Invalid value for ammo count! Value: {RAComEv.Arguments[3]}");
                                                break;
                                            case "9":
                                                if (int.TryParse(RAComEv.Arguments[2].ToLower(), out int NineMM) && NineMM >= 0)
                                                {
                                                    ChosenPlayer.SetAmmo(Exiled.API.Enums.AmmoType.Nato9, (uint) (ChosenPlayer.GetAmmo(Exiled.API.Enums.AmmoType.Nato9) + NineMM));
                                                    RAComEv.Sender.RemoteAdminMessage($"{NineMM} 9.00mm ammo given to \"{ChosenPlayer.ReferenceHub.nicknameSync.MyNick}\"!");
                                                    Player.Get(RAComEv.Arguments[0])?.Broadcast(3, $"You were given {NineMM} of 9.00mm ammo!", Broadcast.BroadcastFlags.Normal);
                                                    return;
                                                }
                                                RAComEv.Sender.RemoteAdminMessage($"Invalid value for ammo count! Value: {RAComEv.Arguments[3]}");
                                                break;
                                            default:
                                                RAComEv.Sender.RemoteAdminMessage($"Please enter \"5\" (5.62mm), \"7\" (7mm), or \"9\" (9mm)!");
                                                break;
                                        }
                                        break;
                                }
                                break;
                            default:
                                RAComEv.Sender.RemoteAdminMessage($"Invalid number of parameters! Value: {RAComEv.Arguments.Count}, Expected 4");
                                break;
                        }
                        break;
                    case "gnade":
                        RAComEv.IsAllowed = false;
                        if (!RAComEv.Sender.CheckPermission("ct.gnade"))
                        {
                            RAComEv.Sender.RemoteAdminMessage("You are not authorized to use this command");
                            return;
                        }

                        if (!CreativeConfig.EnableGrenadeTimeMod)
                        {
                            RAComEv.Sender.RemoteAdminMessage("You cannot modify grenades as it is disabled!");
                            return;
                        }

                        if (RAComEv.Arguments.Count < 2)
                        {
                            RAComEv.Sender.RemoteAdminMessage("Invalid parameters! Syntax: gnade (frag/flash) (value)");
                            return;
                        }

                        switch (RAComEv.Arguments.Count)
                        {
                            case 2:
                                switch (RAComEv.Arguments[0].ToLower())
                                {
                                    case "frag":
                                        if (float.TryParse(RAComEv.Arguments[1].ToLower(), out float value) && value > 0)
                                        {
                                            CreativeConfig.FragGrenadeFuseTimer = value;
                                            RAComEv.Sender.RemoteAdminMessage($"Frag grenade fuse timer set to {value}");
                                            return;
                                        }
                                        RAComEv.Sender.RemoteAdminMessage($"Invalid value for fuse timer! Value: {RAComEv.Arguments[1]}");
                                        break;
                                    case "flash":
                                        if (float.TryParse(RAComEv.Arguments[1].ToLower(), out float val) && val > 0)
                                        {
                                            CreativeConfig.FlashGrenadeFuseTimer = val;
                                            RAComEv.Sender.RemoteAdminMessage($"Flash grenade fuse timer set to {val}");
                                            return;
                                        }
                                        RAComEv.Sender.RemoteAdminMessage($"Invalid value for fuse timer! Value: {RAComEv.Arguments[1]}");
                                        break;
                                    default:
                                        RAComEv.Sender.RemoteAdminMessage("Please enter either \"frag\" or \"flash\"!");
                                        break;
                                }
                                break;
                            default:
                                RAComEv.Sender.RemoteAdminMessage($"Invalid number of parameters! Value: {RAComEv.Arguments.Count}, Expected 3");
                                break;
                        }
                        break;
                    case "infammo":
                        RAComEv.IsAllowed = false;
                        if (!RAComEv.Sender.CheckPermission("ct.infammo") || !RAComEv.Sender.CheckPermission("ct.*"))
                        {
                            RAComEv.Sender.RemoteAdminMessage("You are not authorized to use this command");
                            return;
                        }

                        if (RAComEv.Arguments.Count < 1)
                        {
                            RAComEv.Sender.RemoteAdminMessage("Invalid parameters! Syntax: infam (clear/list/*/all/(id or name))");
                            return;
                        }

                        switch (RAComEv.Arguments.Count)
                        {
                            case 1:
                                switch (RAComEv.Arguments[0].ToLower())
                                {
                                    case "clear":
                                        foreach (Player Ply in Player.List)
                                        {
                                            if (Ply.ReferenceHub.TryGetComponent(out InfiniteAmmoComponent infComponent))
                                            {
                                                UnityEngine.Object.Destroy(infComponent);
                                            }
                                            PlayersWithInfiniteAmmo.Clear();
                                        }
                                        RAComEv.Sender.RemoteAdminMessage("Infinite ammo is cleared from all players now!");
                                        Map.Broadcast(5, "Infinite ammo is cleared from all players now!", Broadcast.BroadcastFlags.Normal);
                                        break;
                                    case "list":
                                        if (PlayersWithInfiniteAmmo.Count != 0)
                                        {
                                            string playerLister = "Players with Infinite Ammo on: ";
                                            foreach (ReferenceHub hub in PlayersWithInfiniteAmmo)
                                            {
                                                playerLister += hub.nicknameSync.MyNick + ", ";
                                            }
                                            playerLister = playerLister.Substring(0, playerLister.Count() - 2);
                                            RAComEv.Sender.RemoteAdminMessage(playerLister);
                                            return;
                                        }
                                        RAComEv.Sender.RemoteAdminMessage("There are no players currently online with Infinite Ammo on");
                                        break;
                                    case "*":
                                    case "all":
                                        foreach (Player Ply in Player.List)
                                        {
                                            if (!Ply.ReferenceHub.TryGetComponent(out InfiniteAmmoComponent infComponent))
                                            {
                                                Ply.ReferenceHub.gameObject.AddComponent<InfiniteAmmoComponent>();
                                                PlayersWithInfiniteAmmo.Add(Ply.ReferenceHub);
                                            }
                                        }
                                        RAComEv.Sender.RemoteAdminMessage("Infinite ammo is on for all players now!");
                                        Map.Broadcast(3, "Everyone has been given infinite ammo!", Broadcast.BroadcastFlags.Normal);
                                        break;
                                    default:
                                        Player ChosenPlayer = Player.Get(RAComEv.Arguments[0]);
                                        if (ChosenPlayer == null)
                                        {
                                            RAComEv.Sender.RemoteAdminMessage($"Player \"{RAComEv.Arguments[1]}\" not found");
                                            return;
                                        }
                                        if (!ChosenPlayer.ReferenceHub.TryGetComponent(out InfiniteAmmoComponent inf))
                                        {
                                            RAComEv.Sender.RemoteAdminMessage($"Infinite ammo enabled for \"{ChosenPlayer.ReferenceHub.nicknameSync.MyNick}\"!");
                                            Player.Get(RAComEv.Arguments[0])?.Broadcast(3, "Infinite ammo is enabled for you!", Broadcast.BroadcastFlags.Normal);
                                            PlayersWithInfiniteAmmo.Add(ChosenPlayer.ReferenceHub);
                                            ChosenPlayer.ReferenceHub.gameObject.AddComponent<InfiniteAmmoComponent>();
                                        }
                                        else
                                        {
                                            RAComEv.Sender.RemoteAdminMessage($"Infinite ammo disabled for \"{ChosenPlayer.ReferenceHub.nicknameSync.MyNick}\"!");
                                            Player.Get(RAComEv.Arguments[0])?.Broadcast(3, "Infinite ammo is disabled for you!", Broadcast.BroadcastFlags.Normal);
                                            PlayersWithInfiniteAmmo.Remove(ChosenPlayer.ReferenceHub);
                                            UnityEngine.Object.Destroy(inf);
                                        }
                                        break;
                                }
                                break;
                            default:
                                RAComEv.Sender.RemoteAdminMessage($"Invalid number of parameters! Value: {RAComEv.Arguments.Count}, Expected 2");
                                break;
                        }
                        break;
                    case "locate":
                        RAComEv.IsAllowed = false;
                        if (!RAComEv.Sender.CheckPermission("ct.locate"))
                        {
                            RAComEv.Sender.RemoteAdminMessage("You are not authorized to use this command");
                            return;
                        }

                        if (RAComEv.Arguments.Count < 2)
                        {
                            RAComEv.Sender.RemoteAdminMessage("Invalid parameters! Syntax: locate (xyz/room) (id or name)");
                            return;
                        }

                        switch (RAComEv.Arguments.Count)
                        {
                            case 2:
                                switch (RAComEv.Arguments[0].ToLower())
                                {
                                    case "room":
                                        Player ChosenPlayer = Player.Get(RAComEv.Arguments[1]);
                                        if (ChosenPlayer == null)
                                        {
                                            RAComEv.Sender.RemoteAdminMessage($"Player \"{RAComEv.Arguments[1]}\" not found");
                                            return;
                                        }
                                        RAComEv.Sender.RemoteAdminMessage($"Player \"{ChosenPlayer.Nickname}\" is located at room: {ChosenPlayer.CurrentRoom.Name}");
                                        break;
                                    case "xyz":
                                        ChosenPlayer = Player.Get(RAComEv.Arguments[1]);
                                        if (ChosenPlayer == null)
                                        {
                                            RAComEv.Sender.RemoteAdminMessage($"Player \"{RAComEv.Arguments[1]}\" not found");
                                            return;
                                        }
                                        RAComEv.Sender.RemoteAdminMessage($"Player \"{ChosenPlayer.Nickname}\" is located at X: {ChosenPlayer.Position.x}, Y: {ChosenPlayer.Position.y}, Z: {ChosenPlayer.Position.z}");
                                        break;
                                    default:
                                        RAComEv.Sender.RemoteAdminMessage("Please enter either \"room\" or \"xyz\"!");
                                        break;
                                }
                                break;
                            default:
                                RAComEv.Sender.RemoteAdminMessage($"Invalid number of parameters! Value: {RAComEv.Arguments.Count}, Expected 2");
                                break;
                        }
                        break;
                    case "nuke":
                        RAComEv.IsAllowed = false;
                        if (!RAComEv.Sender.CheckPermission("ct.nuke"))
                        {
                            RAComEv.Sender.RemoteAdminMessage("You are not authorized to use this command");
                            return;
                        }

                        if (RAComEv.Arguments.Count < 1)
                        {
                            RAComEv.Sender.RemoteAdminMessage("Invalid syntax! Syntax: nuke (start/stop/instant) (value (if using \"start\"))");
                            return;
                        }

                        switch (RAComEv.Arguments.Count)
                        {
                            case 1:
                                switch (RAComEv.Arguments[0].ToLower())
                                {
                                    case "instant":
                                        Warhead.Start();
                                        Warhead.DetonationTimer = 0.05f;
                                        break;
                                    default:
                                        RAComEv.Sender.RemoteAdminMessage("Please enter only \"instant\" or \"start (value)\"!");
                                        break;
                                }
                                break;
                            case 2:
                                switch (RAComEv.Arguments[0].ToLower())
                                {
                                    case "start":
                                        if (!float.TryParse(RAComEv.Arguments[1].ToLower(), out float timer) || (timer >= 143 || timer < 0.05))
                                        {
                                            RAComEv.Sender.RemoteAdminMessage($"Invalid value for timer: {RAComEv.Arguments[1]}, highest is 143, lowest is 0.05");
                                            return;
                                        }
                                        Warhead.Start();
                                        Warhead.DetonationTimer = timer;
                                        break;
                                    default:
                                        RAComEv.Sender.RemoteAdminMessage("Please enter only \"start (value)\"!");
                                        break;
                                }
                                break;
                            default:
                                RAComEv.Sender.RemoteAdminMessage($"Invalid number of parameters! Value: {RAComEv.Arguments.Count}, Expected 1-2");
                                break;
                        }
                        break;
                    case "prygates":
                        RAComEv.IsAllowed = false;
                        if (!RAComEv.Sender.CheckPermission("ct.prygates"))
                        {
                            RAComEv.Sender.RemoteAdminMessage("You are not authorized to use this command");
                            return;
                        }

                        if (RAComEv.Arguments.Count < 1)
                        {
                            RAComEv.Sender.RemoteAdminMessage("Invalid parameters! Syntax: prygates (clear/list/*/all/id)");
                            return;
                        }

                        switch (RAComEv.Arguments.Count)
                        {
                            case 1:
                                switch (RAComEv.Arguments[0].ToLower())
                                {
                                    case "clear":
                                        PlayersThatCanPryGates.Clear();
                                        RAComEv.Sender.RemoteAdminMessage("Ability to pry gates is cleared from all players now!");
                                        Map.Broadcast(5, "The ability to pry gates is cleared from all players now!", Broadcast.BroadcastFlags.Normal);
                                        break;
                                    case "list":
                                        if (PlayersThatCanPryGates.Count != 0)
                                        {
                                            string playerLister = "Players with Pry Gates on: ";
                                            foreach (ReferenceHub hub in PlayersThatCanPryGates)
                                            {
                                                playerLister += hub.nicknameSync.MyNick + ", ";
                                            }
                                            playerLister = playerLister.Substring(0, playerLister.Count() - 2);
                                            RAComEv.Sender.RemoteAdminMessage(playerLister);
                                            return;
                                        }
                                        RAComEv.Sender.RemoteAdminMessage("There are no players currently online with Pry Gates on");
                                        break;
                                    case "*":
                                    case "all":
                                        foreach (Player Ply in Player.List)
                                        {
                                            if (!PlayersThatCanPryGates.Contains(Ply.ReferenceHub))
                                                PlayersThatCanPryGates.Add(Ply.ReferenceHub);
                                        }
                                        RAComEv.Sender.RemoteAdminMessage("Ability to pry gates is on for all players now!");
                                        Map.Broadcast(5, "Everyone has been given the pry gates ability!", Broadcast.BroadcastFlags.Normal);
                                        break;
                                    default:
                                        Player ChosenPlayer = Player.Get(RAComEv.Arguments[0]);
                                        if (ChosenPlayer == null)
                                        {
                                            RAComEv.Sender.RemoteAdminMessage($"Player \"{RAComEv.Arguments[0]}\" not found");
                                            return;
                                        }
                                        if (!PlayersThatCanPryGates.Contains(ChosenPlayer.ReferenceHub))
                                        {
                                            RAComEv.Sender.RemoteAdminMessage($"Pry gates ability enabled for \"{ChosenPlayer.ReferenceHub.nicknameSync.MyNick}\"!");
                                            Player.Get(RAComEv.Arguments[0])?.Broadcast(3, "Pry gates ability is enabled for you!", Broadcast.BroadcastFlags.Normal);
                                            PlayersThatCanPryGates.Add(ChosenPlayer.ReferenceHub);
                                        }
                                        else
                                        {
                                            RAComEv.Sender.RemoteAdminMessage($"Pry gates ability disabled for \"{ChosenPlayer.ReferenceHub.nicknameSync.MyNick}\"!");
                                            Player.Get(RAComEv.Arguments[0])?.Broadcast(3, "Pry gates ability is disabled for you!", Broadcast.BroadcastFlags.Normal);
                                            PlayersThatCanPryGates.Remove(ChosenPlayer.ReferenceHub);
                                        }
                                        break;
                                }
                                break;
                            default:
                                RAComEv.Sender.RemoteAdminMessage($"Invalid number of parameters! Value: {RAComEv.Arguments.Count}, Expected 2");
                                break;
                        }
                        break;
                    case "regen":
                        RAComEv.IsAllowed = false;
                        if (!RAComEv.Sender.CheckPermission("ct.regen"))
                        {
                            RAComEv.Sender.RemoteAdminMessage("You are not authorized to use this command");
                            return;
                        }

                        if (RAComEv.Arguments.Count < 1)
                        {
                            RAComEv.Sender.RemoteAdminMessage("Invalid parameters! Syntax: regen (clear/list/*/all/id)");
                            return;
                        }

                        switch (RAComEv.Arguments.Count)
                        {
                            case 1:
                                switch (RAComEv.Arguments[0].ToLower())
                                {
                                    case "clear":
                                        foreach (Player Ply in Player.List)
                                        {
                                            if (Ply.ReferenceHub.TryGetComponent(out RegenerationComponent rgnComponent))
                                            {
                                                UnityEngine.Object.Destroy(rgnComponent);
                                            }
                                            PlayersWithRegen.Clear();

                                        }
                                        RAComEv.Sender.RemoteAdminMessage("Regeneration is cleared from all players now!");
                                        Map.Broadcast(5, "Regeneration is cleared from all players now!", Broadcast.BroadcastFlags.Normal);
                                        break;
                                    case "list":
                                        if (PlayersWithRegen.Count != 0)
                                        {
                                            string playerLister = "Players with Regeneration on: ";
                                            foreach (ReferenceHub hub in PlayersWithRegen)
                                            {
                                                playerLister += hub.nicknameSync.MyNick + ", ";
                                            }
                                            playerLister = playerLister.Substring(0, playerLister.Count() - 2);
                                            RAComEv.Sender.RemoteAdminMessage(playerLister);
                                            return;
                                        }
                                        RAComEv.Sender.RemoteAdminMessage("There are no players currently online with Regeneration on");
                                        break;
                                    case "*":
                                    case "all":
                                        foreach (Player ply in Player.List)
                                        {
                                            if (!ply.ReferenceHub.TryGetComponent(out RegenerationComponent rgnComponent))
                                            {
                                                ply.ReferenceHub.gameObject.AddComponent<RegenerationComponent>();
                                                PlayersWithRegen.Add(ply.ReferenceHub);
                                            }
                                        }
                                        RAComEv.Sender.RemoteAdminMessage("Regeneration is on for all players now!");
                                        Map.Broadcast(5, "Regeneration is on for all players now!", Broadcast.BroadcastFlags.Normal);
                                        break;
                                    case "time":
                                        RAComEv.Sender.RemoteAdminMessage("Missing value for seconds!");
                                        break;
                                    case "value":
                                        RAComEv.Sender.RemoteAdminMessage("Missing value for health!");
                                        break;
                                    default:
                                        ReferenceHub ChosenPlayer = Player.Get(RAComEv.Arguments[0]).ReferenceHub;
                                        if (ChosenPlayer == null)
                                        {
                                            RAComEv.Sender.RemoteAdminMessage($"Player \"{RAComEv.Arguments[0]}\" not found");
                                            return;
                                        }
                                        if (!ChosenPlayer.TryGetComponent(out RegenerationComponent rgn))
                                        {
                                            RAComEv.Sender.RemoteAdminMessage($"Regeneration enabled for \"{ChosenPlayer.nicknameSync.MyNick}\"!");
                                            Player.Get(RAComEv.Arguments[0])?.Broadcast(3, "Regeneration is enabled for you!", Broadcast.BroadcastFlags.Normal);
                                            PlayersWithRegen.Add(ChosenPlayer);
                                            ChosenPlayer.gameObject.AddComponent<RegenerationComponent>();
                                        }
                                        else
                                        {
                                            RAComEv.Sender.RemoteAdminMessage($"Regeneration disabled for \"{ChosenPlayer.nicknameSync.MyNick}\"!");
                                            Player.Get(RAComEv.Arguments[0])?.Broadcast(3, "Regeneration is disabled for you!", Broadcast.BroadcastFlags.Normal);
                                            PlayersWithRegen.Remove(ChosenPlayer);
                                            UnityEngine.Object.Destroy(rgn);
                                        }
                                        break;
                                }
                                break;
                            case 2:
                                switch (RAComEv.Arguments[0].ToLower())
                                {
                                    case "time":
                                        if (float.TryParse(RAComEv.Arguments[1].ToLower(), out float rgn_t) && rgn_t > 0)
                                        {
                                            CreativeConfig.HPRegenerationTimer = rgn_t;
                                            RAComEv.Sender.RemoteAdminMessage($"Players with regeneration gain health every {rgn_t} seconds!");
                                            return;
                                        }
                                        RAComEv.Sender.RemoteAdminMessage($"Invalid value for regeneration timer! Value: {RAComEv.Arguments[1].ToLower()}");
                                        break;
                                    case "value":
                                        if (float.TryParse(RAComEv.Arguments[1].ToLower(), out float rgn_v) && rgn_v > 0)
                                        {
                                            CreativeConfig.HPRegenerationValue = rgn_v;
                                            RAComEv.Sender.RemoteAdminMessage($"Players with regeneration gain {rgn_v} health every {CreativeConfig.HPRegenerationTimer} seconds!");
                                            return;
                                        }
                                        RAComEv.Sender.RemoteAdminMessage($"Invalid value for regeneration healing! Value: {RAComEv.Arguments[1].ToLower()}");
                                        break;
                                    default:
                                        RAComEv.Sender.RemoteAdminMessage("Please enter either \"time\" or \"value\"!");
                                        break;
                                }
                                break;
                            default:
                                RAComEv.Sender.RemoteAdminMessage($"Invalid number of parameters! Value: {RAComEv.Arguments.Count}, Expected 3");
                                break;
                        }
                        break;
                    case "sdecon":
                        RAComEv.IsAllowed = false;
                        if (!RAComEv.Sender.CheckPermission("ct.sdecon"))
                        {
                            RAComEv.Sender.RemoteAdminMessage("You are not authorized to use this command");
                            return;
                        }
                        if (!Map.IsLCZDecontaminated || !WasDeconCommandRun)
                        {
                            RAComEv.Sender.RemoteAdminMessage("Light Contaimnent Zone Decontamination is on!");
                            Map.StartDecontamination();
                            WasDeconCommandRun = true;
                            return;
                        }
                        RAComEv.Sender.RemoteAdminMessage("Light Contaimnent Zone Decontamination is already active!");
                        break;
                }
            }
            catch (Exception e)
            {
                Log.Info($"Error handling command: {e}");
                RAComEv.Sender.RemoteAdminMessage("There was an error handling this command, check console for details", false);
                return;
            }
        }

        public void RevivePlayer(Player ply)
        {
            if (ply.Role != RoleType.Spectator) return;

            int num = RandNum.Next(0, 7);
            switch (num)
            {
                case 0:
                    ply.ReferenceHub.characterClassManager.SetPlayersClass(RoleType.NtfCadet, ply.ReferenceHub.gameObject);
                    break;
                case 1:
                    if (!IsWarheadDetonated && !IsDecontanimationActivated)
                        ply.ReferenceHub.characterClassManager.SetPlayersClass(RoleType.ClassD, ply.ReferenceHub.gameObject);
                    else
                        ply.ReferenceHub.characterClassManager.SetPlayersClass(RoleType.ChaosInsurgency, ply.ReferenceHub.gameObject);
                    break;
                case 2:
                    if (!IsWarheadDetonated)
                        ply.ReferenceHub.characterClassManager.SetPlayersClass(RoleType.FacilityGuard, ply.ReferenceHub.gameObject);
                    else
                        ply.ReferenceHub.characterClassManager.SetPlayersClass(RoleType.NtfCommander, ply.ReferenceHub.gameObject);
                    break;
                case 3:
                    ply.ReferenceHub.characterClassManager.SetPlayersClass(RoleType.NtfLieutenant, ply.ReferenceHub.gameObject);
                    break;
                case 4:
                    ply.ReferenceHub.characterClassManager.SetPlayersClass(RoleType.NtfScientist, ply.ReferenceHub.gameObject);
                    break;
                case 5:
                    ply.ReferenceHub.characterClassManager.SetPlayersClass(RoleType.ChaosInsurgency, ply.ReferenceHub.gameObject);
                    break;
                case 6:
                    if (!IsWarheadDetonated && !IsDecontanimationActivated)
                        ply.ReferenceHub.characterClassManager.SetPlayersClass(RoleType.Scientist, ply.ReferenceHub.gameObject);
                    else
                        ply.ReferenceHub.characterClassManager.SetPlayersClass(RoleType.NtfLieutenant, ply.ReferenceHub.gameObject);
                    break;
                case 7:
                    ply.ReferenceHub.characterClassManager.SetPlayersClass(RoleType.NtfCommander, ply.ReferenceHub.gameObject);
                    break;
            }
        }

        public static void SpawnGrenadeOnPlayer(Player PlayerToSpawnGrenade, bool UseCustomTimer)
        {
            GrenadeManager gm = PlayerToSpawnGrenade.ReferenceHub.gameObject.GetComponent<GrenadeManager>();
            Grenade gnade = UnityEngine.Object.Instantiate(gm.availableGrenades[0].grenadeInstance.GetComponent<Grenade>());
            if (UseCustomTimer)
                gnade.fuseDuration = CreativeConfig.GrenadeDeathTimer;
            else
            {
                gnade.fuseDuration = 0.01f;
            }
            gnade.FullInitData(gm, PlayerToSpawnGrenade.Position, Quaternion.Euler(gnade.throwStartAngle), gnade.throwLinearVelocityOffset, gnade.throwAngularVelocity);
            NetworkServer.Spawn(gnade.gameObject);
        }

        public Item[] GetItems()
        {
            if (AvailableItems == null)
                AvailableItems = GameObject.FindObjectOfType<Inventory>().availableItems;
            return AvailableItems;
        }
    }
}
