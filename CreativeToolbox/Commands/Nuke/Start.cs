using System;
using CommandSystem;
using Exiled.Permissions.Extensions;
using Exiled.API.Features;

namespace CreativeToolbox.Commands.Nuke
{
    public class Start : ICommand
    {
        public string Command { get; } = "start";

        public string[] Aliases { get; } = new string[] { };

        public string Description { get; } = "Sets off the warhead at a specified time";

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

            if (!float.TryParse(arguments.At(0), out float nuketimer) || (nuketimer < 0.05 || nuketimer > 142))
            {
                response = $"Invalid value for nuke timer: {arguments.At(0)}";
                return false;
            }

            Warhead.Start();
            Warhead.DetonationTimer = nuketimer;
            response = $"The warhead has started at {nuketimer} seconds";
            return true;
        }
    }
}
