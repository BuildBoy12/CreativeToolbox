namespace CreativeToolbox.Commands.Locate
{
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using System;

    public class Room : ICommand
    {
        public string Command => "room";

        public string[] Aliases => Array.Empty<string>();

        public string Description => "Returns the room name a user is in";

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

            Player ply = Player.Get(arguments.At(0));
            if (ply == null)
            {
                response = $"Player \"{arguments.At(0)}\" not found";
                return false;
            }

            response = $"Player \"{ply.Nickname}\" is located at room: {ply.CurrentRoom.Name}";
            return true;
        }
    }
}