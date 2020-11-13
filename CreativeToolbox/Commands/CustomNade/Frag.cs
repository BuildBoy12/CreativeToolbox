namespace CreativeToolbox.Commands.CustomNade
{
    using CommandSystem;
    using Exiled.Permissions.Extensions;
    using Exiled.API.Features;
    using System;
    using static CreativeToolbox;

    public class Frag : ICommand
    {
        public string Command => "frag";

        public string[] Aliases => Array.Empty<string>();

        public string Description => "Modifies the fuse timer for frag grenades";

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

            if (!float.TryParse(arguments.At(0), out float time) || time < 0.05)
            {
                response = $"Invalid value for flash grenade timer: {arguments.At(0)}";
                return false;
            }

            Instance.Config.FragGrenadeFuseTimer = time;
            if (!Instance.Config.PreventCtBroadcasts)
                Map.Broadcast(5, $"Frag grenades will now explode after {time} seconds!");
            response = $"Frag grenades will now explode after {time} seconds";
            return true;
        }
    }
}