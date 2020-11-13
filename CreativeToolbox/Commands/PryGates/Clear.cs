namespace CreativeToolbox.Commands.PryGates
{
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using System;
    using static CreativeToolbox;

    public class Clear : ICommand
    {
        public string Command => "clear";

        public string[] Aliases => new string[0];

        public string Description => "Clears everyones ability to pry gates";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender as CommandSender).CheckPermission("ct.prygates"))
            {
                response = "You do not have permission to run this command! Missing permission: \"ct.prygates\"";
                return false;
            }

            if (arguments.Count != 0)
            {
                response = "Usage: prygates clear";
                return false;
            }

            CreativeToolboxEventHandler.PlayersThatCanPryGates.Clear();
            if (!Instance.Config.PreventCtBroadcasts)
                Map.Broadcast(5, "Everyone cannot pry gates open now!");
            response = "The ability to pry gates is cleared from all players now";
            return true;
        }
    }
}