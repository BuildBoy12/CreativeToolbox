namespace CreativeToolbox.Commands.Locate
{
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using System;

    public class Xyz : ICommand
    {
        public string Command => "xyz";

        public string[] Aliases => new string[0];

        public string Description => "Returns the coordinates a user is at";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender as CommandSender).CheckPermission("ct.locate"))
            {
                response = "You do not have permission to run this command! Missing permission: \"ct.locate\"";
                return false;
            }

            if (arguments.Count != 1)
            {
                response = "Usage: locate xyz (player id / name)";
                return false;
            }

            Player ply = Player.Get(arguments.At(0));
            if (ply == null)
            {
                response = $"Player \"{arguments.At(0)}\" not found";
                return false;
            }

            response =
                $"Player \"{ply.Nickname}\" is located at X: {ply.Position.x}, Y: {ply.Position.y}, Z: {ply.Position.z}";
            return true;
        }
    }
}