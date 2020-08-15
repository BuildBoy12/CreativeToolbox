using System;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace CreativeToolbox.Commands.AutoRespawn
{
    public class On : ICommand
    {
        public string Command { get; } = "on";

        public string[] Aliases { get; } = new string[] { };

        public string Description { get; } = "Turns on automatic respawning for dead players";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender as CommandSender).CheckPermission("ct.autorespawn"))
            {
                response = "You do not have permission to run this command! Missing permission: \"ct.autorespawn\"";
                return false;
            }

            if (arguments.Count != 0)
            {
                response = "Usage: arspawn on";
                return false;
            }

            if (CreativeToolboxEventHandler.AllowRespawning)
            {
                response = "Auto respawning is already on";
                return false;
            }

            CreativeToolboxEventHandler.AllowRespawning = true;
            if (!CreativeToolbox.ConfigRef.Config.PreventCtBroadcasts)
                Map.Broadcast(5, "Auto respawning is now on!");
            response = "Auto respawning is now on";
            return true;
        }
    }
}
