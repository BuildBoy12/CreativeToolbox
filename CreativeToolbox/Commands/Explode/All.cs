using System;
using CommandSystem;
using Exiled.Permissions.Extensions;

namespace CreativeToolbox.Commands.Explode
{
    public class All : ICommand
    {
        public string Command { get; } = "all";

        public string[] Aliases { get; } = new string[] { "*" };

        public string Description { get; } = "Explodes everyone instantly";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender as CommandSender).CheckPermission("ct.explode"))
            {
                response = "You do not have permission to run this command! Missing permission: \"ct.explode\"";
                return false;
            }

            if (arguments.Count != 0)
            {
                response = "Usage: explode all / *";
                return false;
            }

            foreach (Exiled.API.Features.Player Ply in Exiled.API.Features.Player.List)
            {
                if (Ply.Role == RoleType.Spectator || Ply.Role == RoleType.None)
                    continue;

                Ply.Kill();
                CreativeToolboxEventHandler.SpawnGrenadeOnPlayer(Ply, false);
            }
            response = "Everyone exploded, Hubert cannot believe you have done this";
            return true;
        }
    }
}
