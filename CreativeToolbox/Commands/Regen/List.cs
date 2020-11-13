namespace CreativeToolbox.Commands.Regen
{
    using CommandSystem;
    using Exiled.Permissions.Extensions;
    using System;

    public class List : ICommand
    {
        public string Command => "list";

        public string[] Aliases => new string[0];

        public string Description => "Lists every player who has regeneration";

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
                response =
                    $"Players with regeneration: {string.Join(", ", CreativeToolboxEventHandler.PlayersWithRegen)}";
                return true;
            }

            response = "There are no players currently online with regeneration on";
            return true;
        }
    }
}