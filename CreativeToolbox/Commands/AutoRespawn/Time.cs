using System;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;


namespace CreativeToolbox.Commands.AutoRespawn
{
    public class Time : ICommand
    {
        public string Command { get; } = "time";

        public string[] Aliases { get; } = new string[] { };

        public string Description { get; } = "Turns on automatic respawning for dead players";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender as CommandSender).CheckPermission("ct.autorespawn"))
            {
                response = "You do not have permission to run this command! Missing permission: \"ct.autorespawn\"";
                return false;
            }

            if (arguments.Count != 1)
            {
                response = "Usage: arspawn time (value)";
                return false;
            }

            if (!float.TryParse(arguments.At(0), out float respawn))
            {
                response = $"Invalid value for respawn timer: {arguments.At(0)}";
                return false;
            }

            CreativeToolbox.ConfigRef.Config.RandomRespawnTimer = respawn;
            if (!CreativeToolbox.ConfigRef.Config.PreventCtBroadcasts)
                Map.Broadcast(5, $"Auto respawning timer is now set to {respawn}!");
            response = "Auto respawning timer is now set to {respawn}";
            return true;
        }
    }
}
