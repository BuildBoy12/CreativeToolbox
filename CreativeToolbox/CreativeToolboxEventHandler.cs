using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EXILED;
using EXILED.Extensions;
using Grenades;
using Harmony;
using MEC;
using Mirror;
using UnityEngine;

namespace CreativeToolbox
{
    public class CreativeToolboxEventHandler
    {
        HashSet<ReferenceHub> PlayersWith207 = new HashSet<ReferenceHub>();
        HashSet<ReferenceHub> PlayersWithRegen = new HashSet<ReferenceHub>();
        HashSet<ReferenceHub> PlayersWithInfiniteAmmo = new HashSet<ReferenceHub>();
        HashSet<ReferenceHub> PlayersWithInvisiblity = new HashSet<ReferenceHub>();
        System.Random randNum = new System.Random();
        bool IsWarheadDetonated;
        bool IsDecontanimationActivated;
        bool AllowRespawning = false;
        bool PreventFallDamage = false;

        public void RunOnRoundRestart()
        {
            AllowRespawning = false;
        }

        public void RunOnRoundStart()
        {
            AllowRespawning = false;
            PlayersWith207.Clear();
            PlayersWithRegen.Clear();
            if (CreativeToolbox.EnableFallDamagePrevent)
            {
                PreventFallDamage = true;
            }
        }

        public void RunOnPlayerLeave(PlayerLeaveEvent le)
        {
            if (PlayersWith207.Contains(le.Player))
                PlayersWith207.Remove(le.Player);
            if (PlayersWithRegen.Contains(le.Player))
                PlayersWithRegen.Remove(le.Player);
            if (PlayersWithInfiniteAmmo.Contains(le.Player))
                PlayersWithInfiniteAmmo.Remove(le.Player);
            if (PlayersWithInvisiblity.Contains(le.Player))
                PlayersWithInvisiblity.Remove(le.Player);
        }

        public void RunOnPlayerSpawn(PlayerSpawnEvent plySpwn)
        {
            if (PlayersWith207.Contains(plySpwn.Player))
                Timing.CallDelayed(1f, () => plySpwn.Player.effectsController.EnableEffect("SCP-207"));
        }

        public void RunOnPlayerDeath(ref PlayerDeathEvent d)
        {
            if (AllowRespawning)
            {
                ReferenceHub hub = d.Player;
                IsWarheadDetonated = Map.IsNukeDetonated;
                IsDecontanimationActivated = Map.IsLCZDecontaminated;
                Timing.CallDelayed(CreativeToolbox.RandomRespawnTimer, () => RevivePlayer(hub));
            }
        }

        public void RunOnPlayerHurt(ref PlayerHurtEvent h)
        {
            if (PreventFallDamage)
                if (h.DamageType == DamageTypes.Falldown)
                    h.Amount = 0;
        }

        public void RunOnMedItemUsed(UsedMedicalItemEvent m)
        {
            if (CreativeToolbox.EnableMedicalItemMod)
            {
                switch (m.ItemType)
                {
                    case ItemType.Painkillers:
                        m.Player.AddAdrenalineHealth((byte)CreativeToolbox.PainkillerAHPHealthValue);
                        break;
                    case ItemType.Medkit:
                        m.Player.AddAdrenalineHealth((byte)CreativeToolbox.MedkitAHPHealthValue);
                        break;
                    case ItemType.Adrenaline:
                        if (!(CreativeToolbox.AdrenalineAHPHealthValue <= 0))
                            m.Player.AddAdrenalineHealth((byte)CreativeToolbox.AdrenalineAHPHealthValue);
                        break;
                    case ItemType.SCP500:
                        m.Player.AddAdrenalineHealth((byte)CreativeToolbox.SCP500AHPHealthValue);
                        break;
                }
            }
        }

        public void RunOnRemoteAdminCommand(ref RACommandEvent RAComEv)
        {
            try
            {
                string[] Arguments = RAComEv.Command.Split(' ');
                ReferenceHub Sender = RAComEv.Sender.SenderId == "SERVER CONSOLE" || RAComEv.Sender.SenderId == "GAME CONSOLE" ? PlayerManager.localPlayer.GetPlayer() : Player.GetPlayer(RAComEv.Sender.SenderId);
                switch (Arguments[0].ToLower())
                {
                    case "arspawn":
                        RAComEv.Allow = false;
                        if (!Sender.CheckPermission("ct.arspawn"))
                        {
                            RAComEv.Sender.RAMessage("You are not authorized to use this command");
                            return;
                        }

                        if (Arguments.Length < 2)
                        {
                            RAComEv.Sender.RAMessage("Invalid parameters! Syntax: arspawn (on/off/value) (value (if choosing \"time\"))");
                            return;
                        }

                        switch (Arguments.Length)
                        {
                            case 2:
                                switch (Arguments[1].ToLower())
                                {
                                    case "on":
                                        if (!AllowRespawning)
                                        {
                                            RAComEv.Sender.RAMessage("Auto respawning enabled!");
                                            Map.Broadcast("<color=green>Random auto respawning enabled!</color>", 5);
                                            AllowRespawning = true;
                                            return;
                                        }
                                        RAComEv.Sender.RAMessage("Auto respawning is already on!");
                                        break;
                                    case "off":
                                        if (AllowRespawning)
                                        {
                                            RAComEv.Sender.RAMessage("Auto respawning disabled!");
                                            Map.Broadcast("<color=red>Random auto respawning disabled!</color>", 5);
                                            AllowRespawning = false;
                                            return;
                                        }
                                        RAComEv.Sender.RAMessage("Auto respawning is already off!");
                                        break;
                                    case "time":
                                        RAComEv.Sender.RAMessage("Missing value for time!");
                                        break;
                                    default:
                                        RAComEv.Sender.RAMessage("Please enter either \"on\" or \"off\" (If # of arguments is 2)!");
                                        break;
                                }
                                break;
                            case 3:
                                switch (Arguments[1].ToLower())
                                {
                                    case "time":
                                        if (float.TryParse(Arguments[2].ToLower(), out float rspwn) && rspwn > 0)
                                        {
                                            CreativeToolbox.RandomRespawnTimer = rspwn;
                                            RAComEv.Sender.RAMessage($"Auto respawning timer is now set to {rspwn} seconds!");
                                            Map.Broadcast($"Auto respawning timer is now set to {rspwn} seconds!", 5);
                                            return;
                                        }
                                        RAComEv.Sender.RAMessage($"Invalid value for auto respawn timer! Value: {Arguments[2].ToLower()}");
                                        break;
                                    default:
                                        RAComEv.Sender.RAMessage("Please enter only \"time\"!");
                                        break;
                                }
                                break;
                            default:
                                RAComEv.Sender.RAMessage($"Invalid number of parameters! Value: {Arguments.Length}, Expected 3");
                                break;
                        }
                        break;
                    case "fdamage":
                        RAComEv.Allow = false;
                        if (!Sender.CheckPermission("ct.fdamage"))
                        {
                            RAComEv.Sender.RAMessage("You are not authorized to use this command!");
                            return;
                        }

                        if (CreativeToolbox.EnableFallDamagePrevent)
                        {
                            RAComEv.Sender.RAMessage("Fall damage cannot be modified!");
                            return;
                        }

                        if (Arguments.Length < 2)
                        {
                            RAComEv.Sender.RAMessage("Invalid parameters! Syntax: fdamage (on/off)");
                            return;
                        }

                        switch (Arguments.Length)
                        {
                            case 2:
                                switch (Arguments[1].ToLower())
                                {
                                    case "on":
                                        if (!PreventFallDamage)
                                        {
                                            PreventFallDamage = true;
                                            RAComEv.Sender.RAMessage("Fall damage enabled!");
                                            Map.Broadcast("<color=green>Fall damage enabled!</color>", 5);
                                            return;
                                        }
                                        RAComEv.Sender.RAMessage("Fall damage is already on!");
                                        break;
                                    case "off":
                                        if (PreventFallDamage)
                                        {
                                            PreventFallDamage = false;
                                            RAComEv.Sender.RAMessage("Fall damage disabled!");
                                            Map.Broadcast("<color=red>Fall damage disabled!</color>", 5);
                                            return;
                                        }
                                        RAComEv.Sender.RAMessage("Auto respawning is already off!");
                                        break;
                                    default:
                                        RAComEv.Sender.RAMessage("Please enter either \"on\" or \"off\"!");
                                        break;
                                }
                                break;
                            default:
                                RAComEv.Sender.RAMessage($"Invalid number of parameters! Value: {Arguments.Length}");
                                break;
                        }
                        break;
                    case "fspeed":
                        RAComEv.Allow = false;
                        if (!Sender.CheckPermission("ct.fspeed"))
                        {
                            RAComEv.Sender.RAMessage("You are not authorized to use this command!");
                            return;
                        }

                        if (Arguments.Length < 2)
                        {
                            RAComEv.Sender.RAMessage("Invalid parameters! Syntax: fspeed (clear/list/*/all/(id or name))");
                            return;
                        }

                        switch (Arguments.Length)
                        {
                            case 2:
                                switch (Arguments[1].ToLower())
                                {
                                    case "clear":
                                        foreach (ReferenceHub hub in Player.GetHubs())
                                        {
                                            if (hub.TryGetComponent(out SpeedComponent speed))
                                            {
                                                UnityEngine.Object.Destroy(speed);
                                                PlayersWith207.Remove(hub);
                                            }
                                        }
                                        RAComEv.Sender.RAMessage("Faster speed is off for all players now");
                                        Map.Broadcast("Faster speed is cleared from all players now!", 5);
                                        break;
                                    case "list":
                                        if (PlayersWith207.Count != 0)
                                        {
                                            string playerLister = "Players with Fast Speed on: ";
                                            foreach (ReferenceHub hub in PlayersWith207)
                                            {
                                                playerLister += hub.nicknameSync.MyNick + ", ";
                                            }
                                            playerLister = playerLister.Substring(0, playerLister.Length - 2);
                                            RAComEv.Sender.RAMessage(playerLister);
                                            return;
                                        }
                                        RAComEv.Sender.RAMessage("There are no players currently online with Fast Speed on");
                                        break;
                                    case "*":
                                    case "all":
                                        foreach (ReferenceHub hub in Player.GetHubs())
                                        {
                                            if (!hub.TryGetComponent(out SpeedComponent speed))
                                            {
                                                speed = hub.gameObject.AddComponent<SpeedComponent>();
                                                PlayersWith207.Add(hub);
                                            }
                                        }
                                        RAComEv.Sender.RAMessage("Fast Speed is on for all players now");
                                        Map.Broadcast("Everyone has been given faster speed!", 5);
                                        break;
                                    default:
                                        ReferenceHub ChosenPlayer = Player.GetPlayer(Arguments[1]);
                                        if (ChosenPlayer == null)
                                        {
                                            RAComEv.Sender.RAMessage($"Player \"{Arguments[1]}\" not found");
                                            return;
                                        }
                                        if (!ChosenPlayer.TryGetComponent(out SpeedComponent spd))
                                        {
                                            RAComEv.Sender.RAMessage($"Faster speed enabled for \"{ChosenPlayer.nicknameSync.MyNick}\"!");
                                            Player.GetPlayer(Arguments[1])?.Broadcast(3, "Faster speed is enabled for you!", false);
                                            PlayersWith207.Add(ChosenPlayer);
                                            ChosenPlayer.gameObject.AddComponent<SpeedComponent>();
                                        }
                                        else
                                        {
                                            RAComEv.Sender.RAMessage($"Faster speed disabled for \"{ChosenPlayer.nicknameSync.MyNick}\"!");
                                            Player.GetPlayer(Arguments[1])?.Broadcast(3, "Faster speed is disabled for you!", false);
                                            PlayersWith207.Remove(ChosenPlayer);
                                            UnityEngine.Object.Destroy(spd);
                                        }
                                        break;
                                }
                                break;
                            default:
                                RAComEv.Sender.RAMessage($"Invalid number of parameters! Value: {Arguments.Length}");
                                break;
                        }
                        break;
                    case "giveammo":
                        RAComEv.Allow = false;
                        if (!Sender.CheckPermission("ct.giveammo"))
                        {
                            RAComEv.Sender.RAMessage("You are not authorized to use this command");
                            return;
                        }

                        if (Arguments.Length < 4)
                        {
                            RAComEv.Sender.RAMessage("Invalid parameters! Syntax: giveam (*/all/(id or name)) (5/7/9) (amount)");
                            return;
                        }

                        switch (Arguments.Length)
                        {
                            case 4:
                                switch (Arguments[1].ToLower())
                                {
                                    case "*":
                                    case "all":
                                        switch (Arguments[2].ToLower())
                                        {
                                            case "5":
                                                if (int.TryParse(Arguments[3].ToLower(), out int FiveMM) && FiveMM >= 0)
                                                {
                                                    foreach (ReferenceHub hub in Player.GetHubs())
                                                    {
                                                        if (hub.GetRole() != RoleType.None)
                                                            hub.SetAmmo(EXILED.ApiObjects.AmmoType.Dropped5, hub.GetAmmo(EXILED.ApiObjects.AmmoType.Dropped5) + FiveMM);
                                                    }
                                                    RAComEv.Sender.RAMessage($"{FiveMM} 5.56mm ammo given to everyone!");
                                                    Map.Broadcast($"Everyone has been given {FiveMM} 5.56mm ammo!", 3);
                                                    return;
                                                }
                                                RAComEv.Sender.RAMessage($"Invalid value for ammo count! Value: {Arguments[3]}");
                                                break;
                                            case "7":
                                                if (int.TryParse(Arguments[3].ToLower(), out int SevenMM) && SevenMM >= 0)
                                                {
                                                    foreach (ReferenceHub hub in Player.GetHubs())
                                                    {
                                                        if (hub.GetRole() != RoleType.None)
                                                            hub.SetAmmo(EXILED.ApiObjects.AmmoType.Dropped7, hub.GetAmmo(EXILED.ApiObjects.AmmoType.Dropped7) + SevenMM);
                                                    }
                                                    RAComEv.Sender.RAMessage($"{SevenMM} 7.62mm ammo given to everyone!");
                                                    Map.Broadcast($"Everyone has been given {SevenMM} 7.62mm ammo!", 3);
                                                    return;
                                                }
                                                RAComEv.Sender.RAMessage($"Invalid value for ammo count! Value: {Arguments[3]}");
                                                break;
                                            case "9":
                                                if (int.TryParse(Arguments[3].ToLower(), out int NineMM) && NineMM >= 0)
                                                {
                                                    foreach (ReferenceHub hub in Player.GetHubs())
                                                    {
                                                        if (hub.GetRole() != RoleType.None)
                                                            hub.SetAmmo(EXILED.ApiObjects.AmmoType.Dropped9, hub.GetAmmo(EXILED.ApiObjects.AmmoType.Dropped9) + NineMM);
                                                    }
                                                    RAComEv.Sender.RAMessage($"{NineMM} 9.00mm ammo given to everyone!");
                                                    Map.Broadcast($"Everyone has been given {NineMM} 9mm ammo!", 3);
                                                    return;
                                                }
                                                RAComEv.Sender.RAMessage($"Invalid value for ammo count! Value: {Arguments[3]}");
                                                break;
                                            default:
                                                RAComEv.Sender.RAMessage($"Please enter \"5\" (5.56mm), \"7\" (7.62mm), or \"9\" (9.00mm)!");
                                                break;
                                        }
                                        break;
                                    default:
                                        ReferenceHub ChosenPlayer = Player.GetPlayer(Arguments[1]);
                                        ChosenPlayer.HideTag();
                                        if (ChosenPlayer == null)
                                        {
                                            RAComEv.Sender.RAMessage($"Player \"{Arguments[1]}\" not found");
                                            return;
                                        }
                                        else if (ChosenPlayer.GetRole() == RoleType.None)
                                        {
                                            RAComEv.Sender.RAMessage("You cannot give ammo to a person with no role!");
                                            return;
                                        }
                                        switch (Arguments[2].ToLower())
                                        {
                                            case "5":
                                                if (int.TryParse(Arguments[3].ToLower(), out int FiveMM) && FiveMM >= 0)
                                                {
                                                    ChosenPlayer.SetAmmo(EXILED.ApiObjects.AmmoType.Dropped5, ChosenPlayer.GetAmmo(EXILED.ApiObjects.AmmoType.Dropped5) + FiveMM);
                                                    RAComEv.Sender.RAMessage($"{FiveMM} 5.56mm ammo given to \"{ChosenPlayer.nicknameSync.MyNick}\"!");
                                                    Player.GetPlayer(Arguments[1])?.Broadcast(3, $"You were given {FiveMM} of 5.56mm ammo!", false);
                                                    return;
                                                }
                                                RAComEv.Sender.RAMessage($"Invalid value for ammo count! Value: {Arguments[3]}");
                                                break;
                                            case "7":
                                                if (int.TryParse(Arguments[3].ToLower(), out int SevenMM) && SevenMM >= 0)
                                                {
                                                    ChosenPlayer.SetAmmo(EXILED.ApiObjects.AmmoType.Dropped7, ChosenPlayer.GetAmmo(EXILED.ApiObjects.AmmoType.Dropped7) + SevenMM);
                                                    RAComEv.Sender.RAMessage($"{SevenMM} 7.62mm ammo given to \"{ChosenPlayer.nicknameSync.MyNick}\"!");
                                                    Player.GetPlayer(Arguments[1])?.Broadcast(3, $"You were given {SevenMM} of 7.62mm ammo!", false);
                                                    return;
                                                }
                                                RAComEv.Sender.RAMessage($"Invalid value for ammo count! Value: {Arguments[3]}");
                                                break;
                                            case "9":
                                                if (int.TryParse(Arguments[3].ToLower(), out int NineMM) && NineMM >= 0)
                                                {
                                                    ChosenPlayer.SetAmmo(EXILED.ApiObjects.AmmoType.Dropped9, ChosenPlayer.GetAmmo(EXILED.ApiObjects.AmmoType.Dropped9) + NineMM);
                                                    RAComEv.Sender.RAMessage($"{NineMM} 9.00mm ammo given to \"{ChosenPlayer.nicknameSync.MyNick}\"!");
                                                    Player.GetPlayer(Arguments[1])?.Broadcast(3, $"You were given {NineMM} of 9.00mm ammo!", false);
                                                    return;
                                                }
                                                RAComEv.Sender.RAMessage($"Invalid value for ammo count! Value: {Arguments[3]}");
                                                break;
                                            default:
                                                RAComEv.Sender.RAMessage($"Please enter \"5\" (5.62mm), \"7\" (7mm), or \"9\" (9mm)!");
                                                break;
                                        }
                                        break;
                                }
                                break;
                            default:
                                RAComEv.Sender.RAMessage($"Invalid number of parameters! Value: {Arguments.Length}, Expected 4");
                                break;
                        }
                        break;
                    case "gnade":
                        RAComEv.Allow = false;
                        if (!Sender.CheckPermission("ct.gnade"))
                        {
                            RAComEv.Sender.RAMessage("You are not authorized to use this command");
                            return;
                        }

                        if (!CreativeToolbox.EnableGrenadeTimeMod)
                        {
                            RAComEv.Sender.RAMessage("You cannot modify grenades as it is disabled!");
                            return;
                        }

                        if (Arguments.Length < 3)
                        {
                            RAComEv.Sender.RAMessage("Invalid parameters! Syntax: gnade (frag/flash) (value)");
                            return;
                        }

                        switch (Arguments.Length)
                        {
                            case 3:
                                switch (Arguments[1].ToLower())
                                {
                                    case "frag":
                                        if (float.TryParse(Arguments[2].ToLower(), out float value) && value > 0)
                                        {
                                            CreativeToolbox.FragGrenadeFuseTimer = value;
                                            RAComEv.Sender.RAMessage($"Frag grenade fuse timer set to {value}");
                                            return;
                                        }
                                        RAComEv.Sender.RAMessage($"Invalid value for fuse timer! Value: {Arguments[2]}");
                                        break;
                                    case "flash":
                                        if (float.TryParse(Arguments[2].ToLower(), out float val) && val > 0)
                                        {
                                            CreativeToolbox.FlashGrenadeFuseTimer = val;
                                            RAComEv.Sender.RAMessage($"Flash grenade fuse timer set to {val}");
                                            return;
                                        }
                                        RAComEv.Sender.RAMessage($"Invalid value for fuse timer! Value: {Arguments[2]}");
                                        break;
                                    default:
                                        RAComEv.Sender.RAMessage("Please enter either \"frag\" or \"flash\"!");
                                        break;
                                }
                                break;
                            default:
                                RAComEv.Sender.RAMessage($"Invalid number of parameters! Value: {Arguments.Length}, Expected 3");
                                break;
                        }
                        break;
                    case "infammo":
                        RAComEv.Allow = false;
                        if (!Sender.CheckPermission("ct.infammo") || !Sender.CheckPermission("ct.*"))
                        {
                            RAComEv.Sender.RAMessage("You are not authorized to use this command");
                            return;
                        }

                        if (Arguments.Length < 2)
                        {
                            RAComEv.Sender.RAMessage("Invalid parameters! Syntax: infam (clear/list/*/all/(id or name))");
                            return;
                        }

                        switch (Arguments.Length)
                        {
                            case 2:
                                switch (Arguments[1].ToLower())
                                {
                                    case "clear":
                                        foreach (ReferenceHub hub in Player.GetHubs())
                                        {
                                            if (hub.TryGetComponent(out InfiniteAmmoComponent infComponent))
                                            {
                                                UnityEngine.Object.Destroy(infComponent);
                                                PlayersWithInfiniteAmmo.Remove(hub);
                                            }
                                        }
                                        RAComEv.Sender.RAMessage("Infinite ammo is cleared from all players now!");
                                        Map.Broadcast("Infinite ammo is cleared from all players now!", 5);
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
                                            RAComEv.Sender.RAMessage(playerLister);
                                            return;
                                        }
                                        RAComEv.Sender.RAMessage("There are no players currently online with Infinite Ammo on");
                                        break;
                                    case "*":
                                    case "all":
                                        foreach (ReferenceHub hub in Player.GetHubs())
                                        {
                                            if (!hub.TryGetComponent(out InfiniteAmmoComponent infComponent))
                                            {
                                                hub.gameObject.AddComponent<InfiniteAmmoComponent>();
                                                PlayersWithInfiniteAmmo.Add(hub);
                                            }
                                        }
                                        RAComEv.Sender.RAMessage("Infinite ammo is on for all players now!");
                                        Map.Broadcast("Everyone has been given infinite ammo!", 3);
                                        break;
                                    default:
                                        ReferenceHub ChosenPlayer = Player.GetPlayer(Arguments[1]);
                                        if (ChosenPlayer == null)
                                        {
                                            RAComEv.Sender.RAMessage($"Player \"{Arguments[1]}\" not found");
                                            return;
                                        }
                                        if (!ChosenPlayer.TryGetComponent(out InfiniteAmmoComponent inf))
                                        {
                                            RAComEv.Sender.RAMessage($"Infinite ammo enabled for \"{ChosenPlayer.nicknameSync.MyNick}\"!");
                                            Player.GetPlayer(Arguments[1])?.Broadcast(3, "Infinite ammo is enabled for you!", false);
                                            PlayersWithInfiniteAmmo.Add(ChosenPlayer);
                                            ChosenPlayer.gameObject.AddComponent<InfiniteAmmoComponent>();
                                        }
                                        else
                                        {
                                            RAComEv.Sender.RAMessage($"Infinite ammo disabled for \"{ChosenPlayer.nicknameSync.MyNick}\"!");
                                            Player.GetPlayer(Arguments[1])?.Broadcast(3, "Infinite ammo is disabled for you!", false);
                                            PlayersWithInfiniteAmmo.Remove(ChosenPlayer);
                                            UnityEngine.Object.Destroy(inf);
                                        }
                                        break;
                                }
                                break;
                            default:
                                RAComEv.Sender.RAMessage($"Invalid number of parameters! Value: {Arguments.Length}, Expected 2");
                                break;
                        }
                        break;
                    case "invis":
                        RAComEv.Allow = false;
                        if (!Sender.CheckPermission("ct.invis"))
                        {
                            RAComEv.Sender.RAMessage("You are not authorized to use this command");
                            return;
                        }

                        if (Arguments.Length < 2)
                        {
                            RAComEv.Sender.RAMessage("Invalid parameters! Syntax: invis (clear/list/*/all/(id or name))");
                            return;
                        }

                        switch (Arguments.Length)
                        {
                            case 2:
                                switch (Arguments[1].ToLower())
                                {
                                    case "clear":
                                        foreach (ReferenceHub hub in Player.GetHubs())
                                        {
                                            if (hub.TryGetComponent(out InvisibleComponent invComponent))
                                            {
                                                UnityEngine.Object.Destroy(invComponent);
                                                PlayersWithInvisiblity.Remove(hub);
                                            }
                                        }
                                        RAComEv.Sender.RAMessage("Invisibility is cleared from all players now!");
                                        Map.Broadcast("Invisibility is cleared from all players now!", 5);
                                        break;
                                    case "list":
                                        if (PlayersWithInfiniteAmmo.Count != 0)
                                        {
                                            string playerLister = "Players with Invisibility on: ";
                                            foreach (ReferenceHub hub in PlayersWithInvisiblity)
                                            {
                                                playerLister += hub.nicknameSync.MyNick + ", ";
                                            }
                                            playerLister = playerLister.Substring(0, playerLister.Count() - 2);
                                            RAComEv.Sender.RAMessage(playerLister);
                                            return;
                                        }
                                        RAComEv.Sender.RAMessage("There are no players currently online with Invisibility on");
                                        break;
                                    case "*":
                                    case "all":
                                        foreach (ReferenceHub hub in Player.GetHubs())
                                        {
                                            if (!hub.TryGetComponent(out InvisibleComponent infComponent))
                                            {
                                                hub.gameObject.AddComponent<InvisibleComponent>();
                                                PlayersWithInvisiblity.Add(hub);
                                            }
                                        }
                                        RAComEv.Sender.RAMessage("Invisibility is on for all players now!");
                                        Map.Broadcast("Everyone has been given invisibility!", 3);
                                        break;
                                    default:
                                        ReferenceHub ChosenPlayer = Player.GetPlayer(Arguments[1]);
                                        if (ChosenPlayer == null)
                                        {
                                            RAComEv.Sender.RAMessage($"Player \"{Arguments[1]}\" not found");
                                            return;
                                        }
                                        if (!ChosenPlayer.TryGetComponent(out InvisibleComponent inv))
                                        {
                                            RAComEv.Sender.RAMessage($"Invisibility enabled for \"{ChosenPlayer.nicknameSync.MyNick}\"!");
                                            Player.GetPlayer(Arguments[1])?.Broadcast(3, "Invisibility is enabled for you!", false);
                                            PlayersWithInvisiblity.Add(ChosenPlayer);
                                            ChosenPlayer.gameObject.AddComponent<InvisibleComponent>();
                                        }
                                        else
                                        {
                                            RAComEv.Sender.RAMessage($"Invisibility disabled for \"{ChosenPlayer.nicknameSync.MyNick}\"!");
                                            Player.GetPlayer(Arguments[1])?.Broadcast(3, "Invisibility is disabled for you!", false);
                                            PlayersWithInvisiblity.Remove(ChosenPlayer);
                                            UnityEngine.Object.Destroy(inv);
                                        }
                                        break;
                                }
                                break;
                            default:
                                RAComEv.Sender.RAMessage($"Invalid number of parameters! Value: {Arguments.Length}, Expected 2");
                                break;
                        }
                        break;
                    case "locate":
                        RAComEv.Allow = false;
                        if (!Sender.CheckPermission("ct.locate"))
                        {
                            RAComEv.Sender.RAMessage("You are not authorized to use this command");
                            return;
                        }

                        if (Arguments.Length < 3)
                        {
                            RAComEv.Sender.RAMessage("Invalid parameters! Syntax: locate (xyz/room) (id or name)");
                            return;
                        }

                        switch (Arguments.Length)
                        {
                            case 3:
                                switch (Arguments[1].ToLower())
                                {
                                    case "room":
                                        ReferenceHub ChosenPlayer = Player.GetPlayer(Arguments[2]);
                                        if (ChosenPlayer == null)
                                        {
                                            RAComEv.Sender.RAMessage($"Player \"{Arguments[2]}\" not found");
                                            return;
                                        }
                                        RAComEv.Sender.RAMessage($"Player \"{Arguments[2]}\" is located at room: {ChosenPlayer.GetCurrentRoom().Name}");
                                        break;
                                    case "xyz":
                                        ChosenPlayer = Player.GetPlayer(Arguments[2]);
                                        if (ChosenPlayer == null)
                                        {
                                            RAComEv.Sender.RAMessage($"Player \"{Arguments[2]}\" not found");
                                            return;
                                        }
                                        RAComEv.Sender.RAMessage($"Player \"{Arguments[2]}\" is located at X: {ChosenPlayer.GetPosition().x}, Y: {ChosenPlayer.GetPosition().y}, Z: {ChosenPlayer.GetPosition().z}");
                                        break;
                                    default:
                                        RAComEv.Sender.RAMessage("Please enter either \"room\" or \"xyz\"!");
                                        break;
                                }
                                break;
                            default:
                                RAComEv.Sender.RAMessage($"Invalid number of parameters! Value: {Arguments.Length}, Expected 3");
                                break;
                        }
                        break;
                    case "regen":
                        RAComEv.Allow = false;
                        if (!Sender.CheckPermission("ct.regen"))
                        {
                            RAComEv.Sender.RAMessage("You are not authorized to use this command");
                            return;
                        }

                        if (Arguments.Length < 2)
                        {
                            RAComEv.Sender.RAMessage("Invalid parameters! Syntax: regen (clear/list/*/all/id)");
                            return;
                        }

                        switch (Arguments.Length)
                        {
                            case 2:
                                switch (Arguments[1].ToLower())
                                {
                                    case "clear":
                                        foreach (ReferenceHub hub in Player.GetHubs())
                                        {
                                            if (hub.TryGetComponent(out RegenerationComponent rgnComponent))
                                            {
                                                UnityEngine.Object.Destroy(rgnComponent);
                                                PlayersWithRegen.Remove(hub);
                                            }
                                        }
                                        RAComEv.Sender.RAMessage("Regeneration is cleared from all players now!");
                                        Map.Broadcast("Regeneration is cleared from all players now!", 5);
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
                                            RAComEv.Sender.RAMessage(playerLister);
                                            return;
                                        }
                                        RAComEv.Sender.RAMessage("There are no players currently online with Regeneration on");
                                        break;
                                    case "*":
                                    case "all":
                                        foreach (ReferenceHub hub in Player.GetHubs())
                                        {
                                            if (!hub.TryGetComponent(out RegenerationComponent rgnComponent))
                                            {
                                                hub.gameObject.AddComponent<RegenerationComponent>();
                                                PlayersWithRegen.Add(hub);
                                            }
                                        }
                                        RAComEv.Sender.RAMessage("Regeneration is on for all players now!");
                                        Map.Broadcast("Regeneration is on for all players now!", 5);
                                        break;
                                    case "time":
                                        RAComEv.Sender.RAMessage("Missing value for seconds!");
                                        break;
                                    case "value":
                                        RAComEv.Sender.RAMessage("Missing value for health!");
                                        break;
                                    default:
                                        ReferenceHub ChosenPlayer = Player.GetPlayer(Arguments[1]);
                                        if (ChosenPlayer == null)
                                        {
                                            RAComEv.Sender.RAMessage($"Player \"{Arguments[1]}\" not found");
                                            return;
                                        }
                                        if (!ChosenPlayer.TryGetComponent(out RegenerationComponent rgn))
                                        {
                                            RAComEv.Sender.RAMessage($"Regeneration enabled for \"{ChosenPlayer.nicknameSync.MyNick}\"!");
                                            Player.GetPlayer(Arguments[1])?.Broadcast(3, "Regeneration is enabled for you!", false);
                                            PlayersWithRegen.Add(ChosenPlayer);
                                            ChosenPlayer.gameObject.AddComponent<RegenerationComponent>();
                                        }
                                        else
                                        {
                                            RAComEv.Sender.RAMessage($"Regeneration disabled for \"{ChosenPlayer.nicknameSync.MyNick}\"!");
                                            Player.GetPlayer(Arguments[1])?.Broadcast(3, "Regeneration is disabled for you!", false);
                                            PlayersWithRegen.Remove(ChosenPlayer);
                                            UnityEngine.Object.Destroy(rgn);
                                        }
                                        break;
                                }
                                break;
                            case 3:
                                switch (Arguments[1].ToLower())
                                {
                                    case "time":
                                        if (float.TryParse(Arguments[2].ToLower(), out float rgn_t) && rgn_t > 0)
                                        {
                                            CreativeToolbox.HPRegenerationTimer = rgn_t;
                                            RAComEv.Sender.RAMessage($"Players with regeneration gain health every {rgn_t} seconds!");
                                            return;
                                        }
                                        RAComEv.Sender.RAMessage($"Invalid value for regeneration timer! Value: {Arguments[2].ToLower()}");
                                        break;
                                    case "value":
                                        if (float.TryParse(Arguments[2].ToLower(), out float rgn_v) && rgn_v > 0)
                                        {
                                            CreativeToolbox.HPRegenerationValue = rgn_v;
                                            RAComEv.Sender.RAMessage($"Players with regeneration gain {rgn_v} health every {CreativeToolbox.HPRegenerationTimer} seconds!");
                                            return;
                                        }
                                        RAComEv.Sender.RAMessage($"Invalid value for regeneration healing! Value: {Arguments[2].ToLower()}");
                                        break;
                                    default:
                                        RAComEv.Sender.RAMessage("Please enter either \"time\" or \"value\"!");
                                        break;
                                }
                                break;
                            default:
                                RAComEv.Sender.RAMessage($"Invalid number of parameters! Value: {Arguments.Length}, Expected 3");
                                break;
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                Log.Info($"Error handling command: {e}");
                RAComEv.Sender.RAMessage("There was an error handling this command, check console for details", false);
                return;
            }
        }

        public void RevivePlayer(ReferenceHub rh)
        {
            if (rh.GetRole() != RoleType.Spectator) return;
            int num = randNum.Next(0, 7);
            switch (num)
            {
                case 0:
                    rh.characterClassManager.SetPlayersClass(RoleType.NtfCadet, rh.gameObject);
                    break;
                case 1:
                    if (!IsWarheadDetonated && !IsDecontanimationActivated)
                        rh.characterClassManager.SetPlayersClass(RoleType.ClassD, rh.gameObject);
                    else
                        rh.characterClassManager.SetPlayersClass(RoleType.ChaosInsurgency, rh.gameObject);
                    break;
                case 2:
                    if (!IsWarheadDetonated)
                        rh.characterClassManager.SetPlayersClass(RoleType.FacilityGuard, rh.gameObject);
                    else
                        rh.characterClassManager.SetPlayersClass(RoleType.NtfCommander, rh.gameObject);
                    break;
                case 3:
                    rh.characterClassManager.SetPlayersClass(RoleType.NtfLieutenant, rh.gameObject);
                    break;
                case 4:
                    rh.characterClassManager.SetPlayersClass(RoleType.NtfScientist, rh.gameObject);
                    break;
                case 5:
                    rh.characterClassManager.SetPlayersClass(RoleType.ChaosInsurgency, rh.gameObject);
                    break;
                case 6:
                    if (!IsWarheadDetonated && !IsDecontanimationActivated)
                        rh.characterClassManager.SetPlayersClass(RoleType.Scientist, rh.gameObject);
                    else
                        rh.characterClassManager.SetPlayersClass(RoleType.NtfLieutenant, rh.gameObject);
                    break;
                case 7:
                    rh.characterClassManager.SetPlayersClass(RoleType.NtfCommander, rh.gameObject);
                    break;
            }
        }
    }
}
