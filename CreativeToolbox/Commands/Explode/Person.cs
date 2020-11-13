namespace CreativeToolbox.Commands.Explode
{
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using System;

    public class Person : ICommand
    {
        public string Command => "person";

        public string[] Aliases => new string[0];

        public string Description => "Explodes a specified player";

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

            Player ply = Player.Get(arguments.At(0));
            if (ply == null)
            {
                response = $"Invalid target to explode: {arguments.At(0)}";
                return false;
            }

            if (ply.Role == RoleType.Spectator || ply.Role == RoleType.None)
            {
                response = $"Player \"{ply.Nickname}\" is not a valid class to explode";
                return false;
            }

            ply.Kill();
            CreativeToolboxEventHandler.SpawnGrenadeOnPlayer(ply, false);
            response = $"Player \"{ply.Nickname}\" game ended (exploded)";
            return true;
        }
    }
}