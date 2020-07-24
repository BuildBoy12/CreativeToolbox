using System;
using CommandSystem;
using Exiled.Permissions.Extensions;
using Exiled.API.Features;

namespace CreativeToolbox.Commands.PryGates
{
    public class Give : ICommand
    {
        public string Command { get; } = "give";

        public string[] Aliases { get; } = new string[] { };

        public string Description { get; } = "Gives a player the ability to pry gates open";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if(!(sender as CommandSender).CheckPermission("ct.prygates"))
            {
                response = "You do not have permission to run this command! Missing permission: \"ct.prygates\"";
                return false;
            }

            if (arguments.Count != 1)
            {
                response = "Usage: prygates give (player id / name)";
                return false;
            }

            Player Ply = Player.Get(arguments.At(0));
            if (Ply == null)
            {
                response = $"Player \"{arguments.At(0)}\" not found";
                return false;
            }

            if (!CreativeToolboxEventHandler.PlayersThatCanPryGates.Contains(Ply))
            {
                Ply.Broadcast(5, "You can pry gates open now!");
                CreativeToolboxEventHandler.PlayersThatCanPryGates.Add(Ply);
                response = $"Player \"{Ply.Nickname}\" can now pry gates open";
                return true;
            }
            else
            {
                Ply.Broadcast(5, "You cannot pry gates open now!");
                CreativeToolboxEventHandler.PlayersThatCanPryGates.Remove(Ply);
                response = $"Player \"{Ply.Nickname}\" cannot pry gates open now";
                return true;
            }
        }
    }
}
