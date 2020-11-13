namespace CreativeToolbox.Commands.Nuke
{
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using System;

    public class Start : ICommand
    {
        public string Command => "start";

        public string[] Aliases => Array.Empty<string>();

        public string Description => "Sets off the warhead at a specified time";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender as CommandSender).CheckPermission("ct.nuke"))
            {
                response = "You do not have permission to run this command! Missing permission: \"ct.nuke\"";
                return false;
            }

            if (arguments.Count != 1)
            {
                response = "Usage: nuke start (value)";
                return false;
            }

            if (!float.TryParse(arguments.At(0), out float nukeTimer) || (nukeTimer < 0.05 || nukeTimer > 142))
            {
                response = $"Invalid value for nuke timer: {arguments.At(0)}";
                return false;
            }

            Warhead.Start();
            Warhead.DetonationTimer = nukeTimer;
            response = $"The warhead has started at {nukeTimer} seconds";
            return true;
        }
    }
}