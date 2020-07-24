using System;
using CommandSystem;
using Exiled.Permissions.Extensions;
using Exiled.API.Features;

namespace CreativeToolbox.Commands.Regen
{
    public class Clear : ICommand
    {
        public string Command { get; } = "clear";

        public string[] Aliases { get; } = new string[] { };

        public string Description { get; } = "Clears regeneration from everyone";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender as CommandSender).CheckPermission("ct.regen"))
            {
                response = "You do not have permission to run this command! Missing permission: \"ct.regen\"";
                return false;
            }

            if (arguments.Count != 0)
            {
                response = "Usage: regen clear";
                return false;
            }

            foreach (Player Ply in Player.List)
                if (Ply.ReferenceHub.TryGetComponent(out RegenerationComponent Regen))
                    UnityEngine.Object.Destroy(Regen);

            CreativeToolboxEventHandler.PlayersWithRegen.Clear();
            Map.Broadcast(5, "Regeneration is taken away from everyone now!");
            response = "Regeneration has been taken away from everyone";
            return true;
        }
    }
}
