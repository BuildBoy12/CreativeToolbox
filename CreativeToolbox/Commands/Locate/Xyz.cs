using System;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace CreativeToolbox.Commands.Locate
{
    public class Xyz : ICommand
    {
        public string Command { get; } = "xyz";

        public string[] Aliases { get; } = new string[] { };

        public string Description { get; } = "Returns the coordinates a user is at";

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

            Player Ply = Player.Get(arguments.At(0));
            if (Ply == null)
            {
                response = $"Player \"{arguments.At(0)}\" not found";
                return false;
            }

            response = $"Player \"{Ply.Nickname}\" is located at X: {Ply.Position.x}, Y: {Ply.Position.y}, Z: {Ply.Position.z}";
            return true;
        }
    }
}
