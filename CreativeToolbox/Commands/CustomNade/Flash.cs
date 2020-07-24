using System;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace CreativeToolbox.Commands.CustomNade
{
    public class Flash : ICommand
    {
        public string Command { get; } = "flash";

        public string[] Aliases { get; } = new string[] { };

        public string Description { get; } = "Modifies the fuse timer for flash grenades";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender as CommandSender).CheckPermission("ct.customnade"))
            {
                response = "You do not have permission to run this command! Missing permission: \"ct.customnade\"";
                return false;
            }

            if (arguments.Count != 1)
            {
                response = "Usage: customnade flash (value)";
                return false;
            }

            if (!float.TryParse(arguments.At(0), out float flashtime) || flashtime < 0.05)
            {
                response = $"Invalid value for flash grenade timer: {arguments.At(0)}";
                return false;
            }

            CreativeToolbox.ConfigRef.Config.FlashGrenadeFuseTimer = flashtime;
            Map.Broadcast(5, $"Flash grenades will now explode after {flashtime} seconds!");
            response = $"Flash grenades will now explode after {flashtime} seconds";
            return true;
        }
    }
}
