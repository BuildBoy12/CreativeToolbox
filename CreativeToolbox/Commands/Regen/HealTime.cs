namespace CreativeToolbox.Commands.Regen
{
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using System;
    using static CreativeToolbox;

    public class HealTime : ICommand
    {
        public string Command => "healtime";

        public string[] Aliases => new string[0];

        public string Description => "Sets the interval users are given health (in seconds)";

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

            if (!float.TryParse(arguments.At(0), out float healTime) || healTime < 0.05)
            {
                response = $"Invalid value for healing time interval: {arguments.At(0)}";
                return false;
            }

            Instance.Config.RegenerationValue = healTime;
            if (!Instance.Config.PreventCtBroadcasts)
                Map.Broadcast(5, $"Players with regeneration will heal every {healTime} seconds!");
            response = $"Players with regeneration will heal every {healTime} seconds";
            return true;
        }
    }
}