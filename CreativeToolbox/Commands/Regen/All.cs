using System;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace CreativeToolbox.Commands.Regen
{
    public class All : ICommand
    {
        public string Command { get; } = "all";

        public string[] Aliases { get; } = new string[] { "*" };

        public string Description { get; } = "Gives everyone regeneration";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender as CommandSender).CheckPermission("ct.regen"))
            {
                response = "You do not have permission to run this command! Missing permission: \"ct.regen\"";
                return false;
            }

            if (arguments.Count != 0)
            {
                response = "Usage: regen all / *";
                return false;
            }

            foreach (Player Ply in Player.List)
            {
                if (!Ply.ReferenceHub.TryGetComponent(out RegenerationComponent Regen))
                {
                    Ply.ReferenceHub.gameObject.AddComponent<RegenerationComponent>();
                    CreativeToolboxEventHandler.PlayersWithRegen.Add(Ply);
                }
            }

            if (!CreativeToolbox.ConfigRef.Config.PreventCtBroadcasts)
                Map.Broadcast(5, "Everyone has been given regeneration now!");
            response = "Everyone has been given regeneration";
            return true;
        }
    }
}
