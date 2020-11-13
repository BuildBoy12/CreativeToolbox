namespace CreativeToolbox.Commands.Nuke
{
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using System;

    public class Instant : ICommand
    {
        public string Command => "instant";

        public string[] Aliases => new string[0];

        public string Description => "Sets off the warhead instantly";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender as CommandSender).CheckPermission("ct.nuke"))
            {
                response = "You do not have permission to run this command! Missing permission: \"ct.nuke\"";
                return false;
            }

            if (arguments.Count != 0)
            {
                response = "Usage: nuke instant";
                return false;
            }

            Warhead.Start();
            Warhead.DetonationTimer = 0.05f;
            response = "The warhead has exploded now";
            return true;
        }
    }
}