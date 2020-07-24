using System;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace CreativeToolbox.Commands.AutoRespawn
{
    public class Off : ICommand
    {
        public string Command { get; } = "off";

        public string[] Aliases { get; } = new string[] { };

        public string Description { get; } = "Turns off automatic respawning for dead players";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender as CommandSender).CheckPermission("ct.autorespawn"))
            {
                response = "You do not have permission to run this command! Missing permission: \"ct.autorespawn\"";
                return false;
            }

            if (arguments.Count != 0)
            {
                response = "Usage: arspawn off";
                return false;
            }

            if (!CreativeToolboxEventHandler.AllowRespawning)
            {
                response = "Auto respawning is already off";
                return false;
            }

            CreativeToolboxEventHandler.AllowRespawning = false;
            Map.Broadcast(5, "Auto respawning is now off!");
            response = "Auto respawning is now off";
            return true;
        }
    }
}
