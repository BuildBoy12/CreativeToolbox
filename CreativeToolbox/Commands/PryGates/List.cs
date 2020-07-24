using System;
using CommandSystem;
using Exiled.Permissions.Extensions;
using Exiled.API.Features;

namespace CreativeToolbox.Commands.PryGates
{
    public class List : ICommand
    {
        public string Command { get; } = "list";

        public string[] Aliases { get; } = new string[] { };

        public string Description { get; } = "Lists every player that can pry gates";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender as CommandSender).CheckPermission("ct.prygates"))
            {
                response = "You do not have permission to run this command! Missing permission: \"ct.prygates\"";
                return false;
            }

            if (arguments.Count != 0)
            {
                response = "Usage: prygates list";
                return false;
            }

            if (CreativeToolboxEventHandler.PlayersThatCanPryGates.Count > 0)
            {
                CreativeToolboxEventHandler.PlayerLister.Append("Players that can pry gates: ");
                foreach (Player Ply in CreativeToolboxEventHandler.PlayersThatCanPryGates)
                    CreativeToolboxEventHandler.PlayerLister.Append(Ply.Nickname + ", ");

                int length = CreativeToolboxEventHandler.PlayerLister.ToString().Length;
                response = CreativeToolboxEventHandler.PlayerLister.ToString().Substring(0, length - 2);
                CreativeToolboxEventHandler.PlayerLister.Clear();
                return true;
            }
            else
            {
                response = "There are no players currently online that can pry gates";
                CreativeToolboxEventHandler.PlayerLister.Clear();
                return true;
            }
        }
    }
}
