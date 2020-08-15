using System;
using CommandSystem;
using Exiled.Permissions.Extensions;
using Exiled.API.Features;

namespace CreativeToolbox.Commands.PryGates
{
    public class All : ICommand
    {
        public string Command { get; } = "all";

        public string[] Aliases { get; } = new string[] { "*" };

        public string Description { get; } = "Lets everyone pry gates open";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender as CommandSender).CheckPermission("ct.prygates"))
            {
                response = "You do not have permission to run this command! Missing permission: \"ct.prygates\"";
                return false;
            }

            if (arguments.Count != 0)
            {
                response = "Usage: prygates all";
                return false;
            }

            foreach (Player Ply in Player.List)
            {
                if (!CreativeToolboxEventHandler.PlayersThatCanPryGates.Contains(Ply))
                    CreativeToolboxEventHandler.PlayersThatCanPryGates.Add(Ply);
            }

            if (!CreativeToolbox.ConfigRef.Config.PreventCtBroadcasts)
                Map.Broadcast(5, "Everyone has been given the ability to pry gates open!");
            response = "The ability to pry gates open is on for all players now";
            return true;
        }
    }
}
