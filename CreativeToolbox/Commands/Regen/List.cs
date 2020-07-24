using System;
using System.Text;
using CommandSystem;
using Exiled.Permissions.Extensions;
using Exiled.API.Features;

namespace CreativeToolbox.Commands.Regen
{
    public class List : ICommand
    {
        public string Command { get; } = "list";

        public string[] Aliases { get; } = new string[] { };

        public string Description { get; } = "Lists every player who has regeneration";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender as CommandSender).CheckPermission("ct.regen"))
            {
                response = "You do not have permission to run this command! Missing permission: \"ct.regen\"";
                return false;
            }

            if (arguments.Count != 0)
            {
                response = "Usage: regen list";
                return false;
            }

            if (CreativeToolboxEventHandler.PlayersWithRegen.Count > 0)
            {
                CreativeToolboxEventHandler.PlayerLister.Append("Players with regeneration: ");
                foreach (Player Ply in CreativeToolboxEventHandler.PlayersWithRegen)
                    CreativeToolboxEventHandler.PlayerLister.Append(Ply.Nickname + ", ");

                int length = CreativeToolboxEventHandler.PlayerLister.ToString().Length;
                response = CreativeToolboxEventHandler.PlayerLister.ToString().Substring(0, length - 2);
                CreativeToolboxEventHandler.PlayerLister.Clear();
                return true;
            }
            else
            {
                response = "There are no players currently online with regeneration on";
                CreativeToolboxEventHandler.PlayerLister.Clear();
                return true;
            }
        }
    }
}
