namespace CreativeToolbox.Commands.PryGates
{
    using CommandSystem;
    using Exiled.Permissions.Extensions;
    using System;

    public class List : ICommand
    {
        public string Command => "list";

        public string[] Aliases => Array.Empty<string>();

        public string Description => "Lists every player that can pry gates";

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
                response =
                    $"Players that can pry gates: {string.Join(", ", CreativeToolboxEventHandler.PlayersThatCanPryGates)}";
                return true;
            }

            response = "There are no players currently online that can pry gates";
            return true;
        }
    }
}