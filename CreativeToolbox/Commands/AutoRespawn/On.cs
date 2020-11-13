namespace CreativeToolbox.Commands.AutoRespawn
{
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using System;
    using static CreativeToolbox;

    public class On : ICommand
    {
        public string Command => "on";

        public string[] Aliases => new string[0];

        public string Description => "Turns on automatic respawning for dead players";

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
            if (!Instance.Config.PreventCtBroadcasts)
                Map.Broadcast(5, "Auto respawning is now on!");
            response = "Auto respawning is now on";
            return true;
        }
    }
}