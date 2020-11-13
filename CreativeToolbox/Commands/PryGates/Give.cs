namespace CreativeToolbox.Commands.PryGates
{
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using System;
    using static CreativeToolbox;

    public class Give : ICommand
    {
        public string Command => "give";

        public string[] Aliases => new string[0];

        public string Description => "Gives a player the ability to pry gates open";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender as CommandSender).CheckPermission("ct.prygates"))
            {
                response = "You do not have permission to run this command! Missing permission: \"ct.prygates\"";
                return false;
            }

            if (arguments.Count != 1)
            {
                response = "Usage: prygates give (player id / name)";
                return false;
            }

            Player ply = Player.Get(arguments.At(0));
            if (ply == null)
            {
                response = $"Player \"{arguments.At(0)}\" not found";
                return false;
            }

            if (!CreativeToolboxEventHandler.PlayersThatCanPryGates.Contains(ply))
            {
                if (!Instance.Config.PreventCtBroadcasts)
                    ply.Broadcast(5, "You can pry gates open now!");
                CreativeToolboxEventHandler.PlayersThatCanPryGates.Add(ply);
                response = $"Player \"{ply.Nickname}\" can now pry gates open";
                return true;
            }

            if (!Instance.Config.PreventCtBroadcasts)
                ply.Broadcast(5, "You cannot pry gates open now!");
            CreativeToolboxEventHandler.PlayersThatCanPryGates.Remove(ply);
            response = $"Player \"{ply.Nickname}\" cannot pry gates open now";
            return true;
        }
    }
}