using System;
using CommandSystem;
using Exiled.Permissions.Extensions;
using Exiled.API.Features;

namespace CreativeToolbox.Commands.Regen
{
    class Give : ICommand
    {
        public string Command { get; } = "give";

        public string[] Aliases { get; } = new string[] { };

        public string Description { get; } = "Gives a player the ability to regenerate health";

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

            Player Ply = Player.Get(arguments.At(0));
            if (Ply == null)
            {
                response = $"Player \"{arguments.At(0)}\" not found";
                return false;
            }

            if (!Ply.ReferenceHub.TryGetComponent(out RegenerationComponent Regen))
            {
                CreativeToolboxEventHandler.PlayersWithRegen.Add(Ply);
                Ply.GameObject.AddComponent<RegenerationComponent>();
                if (!CreativeToolbox.ConfigRef.Config.PreventCtBroadcasts)
                    Ply.Broadcast(5, "Regeneration is enabled for you!");
                response = $"Regeneration enabled for Player \"{Ply.Nickname}\"";
                return true;
            }
            else
            {
                CreativeToolboxEventHandler.PlayersWithRegen.Remove(Ply);
                UnityEngine.Object.Destroy(Regen);
                if (!CreativeToolbox.ConfigRef.Config.PreventCtBroadcasts)
                    Ply.Broadcast(5, "Regeneration is disabled for you!");
                response = $"Regeneration disabled for Player \"{Ply.Nickname}\"";
                return true;
            }
        }
    }
}
