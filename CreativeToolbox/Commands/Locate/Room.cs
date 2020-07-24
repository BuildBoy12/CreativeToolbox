using System;
using CommandSystem;
using Exiled.Permissions.Extensions;
using Exiled.API.Features;

namespace CreativeToolbox.Commands.Locate
{
    public class Room : ICommand
    {
        public string Command { get; } = "room";

        public string[] Aliases { get; } = new string[] { };

        public string Description { get; } = "Returns the room name a user is in";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender as CommandSender).CheckPermission("ct.locate"))
            {
                response = "You do not have permission to run this command! Missing permission: \"ct.locate\"";
                return false;
            }

            if (arguments.Count != 1)
            {
                response = "Usage: locate room (player id / name)";
                return false;
            }

            Player Ply = Player.Get(arguments.At(0));
            if (Ply == null)
            {
                response = $"Player \"{arguments.At(0)}\" not found";
                return false;
            }

            response = $"Player \"{Ply.Nickname}\" is located at room: {Ply.CurrentRoom.Name}";
            return true;
        }
    }
}
