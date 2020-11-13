namespace CreativeToolbox.Commands.Regen
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

        public string Description => "Gives everyone regeneration";

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

            foreach (Player ply in Player.List)
            {
                if (ply.ReferenceHub.GetComponent<RegenerationComponent>() != null)
                    continue;

                ply.ReferenceHub.gameObject.AddComponent<RegenerationComponent>();
                CreativeToolboxEventHandler.PlayersWithRegen.Add(ply);
            }

            if (!Instance.Config.PreventCtBroadcasts)
                Map.Broadcast(5, "Everyone has been given regeneration now!");
            response = "Everyone has been given regeneration";
            return true;
        }
    }
}