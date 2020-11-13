namespace CreativeToolbox.Commands.InfAmmo
{
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using System;

    public class List : ICommand
    {
        public string Command => "list";

        public string[] Aliases => Array.Empty<string>();

        public string Description => "Lists every player who has infinite ammo";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender as CommandSender).CheckPermission("ct.infammo"))
            {
                response = "You do not have permission to run this command! Missing permission: \"ct.infammo\"";
                return false;
            }

            if (arguments.Count != 0)
            {
                response = "Usage: infammo list";
                return false;
            }

            if (CreativeToolboxEventHandler.PlayersWithInfiniteAmmo.Count > 0)
            {
                CreativeToolboxEventHandler.PlayerLister.Append("Players with infinite ammo: ");
                foreach (Player ply in CreativeToolboxEventHandler.PlayersWithInfiniteAmmo)
                    CreativeToolboxEventHandler.PlayerLister.Append(ply.Nickname + ", ");

                int length = CreativeToolboxEventHandler.PlayerLister.ToString().Length;
                response = CreativeToolboxEventHandler.PlayerLister.ToString().Substring(0, length - 2);
                CreativeToolboxEventHandler.PlayerLister.Clear();
                return true;
            }

            response = "There are no players currently online with infinite ammo";
            CreativeToolboxEventHandler.PlayerLister.Clear();
            return true;
        }
    }
}