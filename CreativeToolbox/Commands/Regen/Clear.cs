namespace CreativeToolbox.Commands.Regen
{
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using System;
    using static CreativeToolbox;

    public class Clear : ICommand
    {
        public string Command => "clear";

        public string[] Aliases => Array.Empty<string>();

        public string Description => "Clears regeneration from everyone";

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

            foreach (Player ply in Player.List)
                if (ply.ReferenceHub.TryGetComponent(out RegenerationComponent regenerationComponent))
                    UnityEngine.Object.Destroy(regenerationComponent);

            CreativeToolboxEventHandler.PlayersWithRegen.Clear();
            if (!Instance.Config.PreventCtBroadcasts)
                Map.Broadcast(5, "Regeneration is taken away from everyone now!");
            response = "Regeneration has been taken away from everyone";
            return true;
        }
    }
}