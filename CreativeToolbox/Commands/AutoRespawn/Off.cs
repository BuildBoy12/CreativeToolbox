namespace CreativeToolbox.Commands.AutoRespawn
{
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using System;
    using static CreativeToolbox;

    public class Off : ICommand
    {
        public string Command => "off";

        public string[] Aliases => new string[0];

        public string Description => "Turns off automatic respawning for dead players";

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
            if (!Instance.Config.PreventCtBroadcasts)
                Map.Broadcast(5, "Auto respawning is now off!");
            response = "Auto respawning is now off";
            return true;
        }
    }
}