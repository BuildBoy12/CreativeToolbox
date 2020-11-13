namespace CreativeToolbox.Commands.CustomNade
{
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using System;
    using static CreativeToolbox;

    public class Flash : ICommand
    {
        public string Command => "flash";

        public string[] Aliases => new string[0];

        public string Description => "Modifies the fuse timer for flash grenades";

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

            if (!float.TryParse(arguments.At(0), out float time) || time < 0.05)
            {
                response = $"Invalid value for flash grenade timer: {arguments.At(0)}";
                return false;
            }

            Instance.Config.FlashGrenadeFuseTimer = time;
            if (!Instance.Config.PreventCtBroadcasts)
                Map.Broadcast(5, $"Flash grenades will now explode after {time} seconds!");
            response = $"Flash grenades will now explode after {time} seconds";
            return true;
        }
    }
}