namespace CreativeToolbox.Commands.Regen
{
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using System;
    using static CreativeToolbox;

    public class Give : ICommand
    {
        public string Command => "give";

        public string[] Aliases => Array.Empty<string>();

        public string Description => "Gives a player the ability to regenerate health";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender as CommandSender).CheckPermission("ct.regen"))
            {
                response = "You do not have permission to run this command! Missing permission: \"ct.regen\"";
                return false;
            }

            if (arguments.Count != 1)
            {
                response = "Usage: regen give (player id / name)";
                return false;
            }

            Player ply = Player.Get(arguments.At(0));
            if (ply == null)
            {
                response = $"Player \"{arguments.At(0)}\" not found";
                return false;
            }

            if (!ply.ReferenceHub.TryGetComponent(out RegenerationComponent regenerationComponent))
            {
                CreativeToolboxEventHandler.PlayersWithRegen.Add(ply);
                ply.GameObject.AddComponent<RegenerationComponent>();
                if (!Instance.Config.PreventCtBroadcasts)
                    ply.Broadcast(5, "Regeneration is enabled for you!");
                response = $"Regeneration enabled for Player \"{ply.Nickname}\"";
                return true;
            }

            CreativeToolboxEventHandler.PlayersWithRegen.Remove(ply);
            UnityEngine.Object.Destroy(regenerationComponent);
            if (!Instance.Config.PreventCtBroadcasts)
                ply.Broadcast(5, "Regeneration is disabled for you!");
            response = $"Regeneration disabled for Player \"{ply.Nickname}\"";
            return true;
        }
    }
}