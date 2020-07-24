using System;
using CommandSystem;
using Exiled.Permissions.Extensions;
using Exiled.API.Features;

namespace CreativeToolbox.Commands.Explode
{
    public class Person : ICommand
    {
        public string Command { get; } = "person";

        public string[] Aliases { get; } = new string[] { };

        public string Description { get; } = "Explodes a specified player";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender as CommandSender).CheckPermission("ct.explode"))
            {
                response = "You do not have permission to run this command! Missing permission: \"ct.explode\"";
                return false;
            }

            if (arguments.Count != 1)
            {
                response = "Usage: explode person (player id / name)";
                return false;
            }

            Player Ply = Player.Get(arguments.At(0));
            if (Ply == null)
            {
                response = $"Invalid target to explode: {arguments.At(0)}";
                return false;
            }

            if (Ply.Role == RoleType.Spectator || Ply.Role == RoleType.None)
            {
                response = $"Player \"{Ply.Nickname}\" is not a valid class to explode";
                return false;
            }

            Ply.Kill();
            CreativeToolboxEventHandler.SpawnGrenadeOnPlayer(Ply, false);
            response = $"Player \"{Ply.Nickname}\" game ended (exploded)";
            return true;
        }
    }
}
