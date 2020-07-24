using System;
using CommandSystem;
using Exiled.Permissions.Extensions;
using Exiled.API.Features;

namespace CreativeToolbox.Commands.Regen
{
    public class HealTime : ICommand
    {
        public string Command { get; } = "healtime";

        public string[] Aliases { get; } = new string[] { };

        public string Description { get; } = "Sets the interval users are given health (in seconds)";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender as CommandSender).CheckPermission("ct.regen"))
            {
                response = "You do not have permission to run this command! Missing permission: \"ct.regen\"";
                return false;
            }

            if (arguments.Count != 1)
            {
                response = "Usage: regen healtime (value)";
                return false;
            }

            if (!float.TryParse(arguments.At(0), out float healtime) || healtime < 0.05)
            {
                response = $"Invalid value for healing time interval: {arguments.At(0)}";
                return false;
            }

            CreativeToolbox.ConfigRef.Config.RegenerationValue = healtime;
            Map.Broadcast(5, $"Players with regeneration will heal every {healtime} seconds!");
            response = $"Players with regeneration will heal every {healtime} seconds";
            return true;
        }
    }
}
