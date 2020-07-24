using System;
using CommandSystem;
using Exiled.Permissions.Extensions;
using Exiled.API.Features;

namespace CreativeToolbox.Commands.CustomNade
{
    public class Frag : ICommand
    {
        public string Command { get; } = "frag";

        public string[] Aliases { get; } = new string[] { };

        public string Description { get; } = "Modifies the fuse timer for frag grenades";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender as CommandSender).CheckPermission("ct.customnade"))
            {
                response = "You do not have permission to run this command! Missing permission: \"ct.customnade\"";
                return false;
            }

            if (arguments.Count != 1)
            {
                response = "Usage: customnade frag (value)";
                return false;
            }

            if (!float.TryParse(arguments.At(0), out float fragtime) || fragtime < 0.05)
            {
                response = $"Invalid value for flash grenade timer: {arguments.At(0)}";
                return false;
            }

            CreativeToolbox.ConfigRef.Config.FragGrenadeFuseTimer = fragtime;
            Map.Broadcast(5, $"Frag grenades will now explode after {fragtime} seconds!");
            response = $"Frag grenades will now explode after {fragtime} seconds";
            return true;
        }
    }
}
