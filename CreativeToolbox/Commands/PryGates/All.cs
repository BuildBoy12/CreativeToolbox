namespace CreativeToolbox.Commands.PryGates
{
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using System;
    using static CreativeToolbox;

    public class All : ICommand
    {
        public string Command => "all";

        public string[] Aliases => new[] {"*"};

        public string Description => "Lets everyone pry gates open";

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

            foreach (Player ply in Player.List)
            {
                if (!CreativeToolboxEventHandler.PlayersThatCanPryGates.Contains(ply))
                    CreativeToolboxEventHandler.PlayersThatCanPryGates.Add(ply);
            }

            if (!Instance.Config.PreventCtBroadcasts)
                Map.Broadcast(5, "Everyone has been given the ability to pry gates open!");
            response = "The ability to pry gates open is on for all players now";
            return true;
        }
    }
}