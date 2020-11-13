namespace CreativeToolbox.Commands.InfAmmo
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

        public string Description => "Gives the player infinite ammo";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender as CommandSender).CheckPermission("ct.infammo"))
            {
                response = "You do not have permission to run this command! Missing permission: \"ct.infammo\"";
                return false;
            }

            if (arguments.Count != 1)
            {
                response = "Usage: infammo give (player id / name)";
                return false;
            }

            Player ply = Player.Get(arguments.At(0));
            if (ply == null)
            {
                response = $"Player \"{arguments.At(0)}\" not found";
                return false;
            }

            if (!ply.ReferenceHub.TryGetComponent(out InfiniteAmmoComponent infiniteAmmoComponent))
            {
                CreativeToolboxEventHandler.PlayersWithInfiniteAmmo.Add(ply);
                ply.GameObject.AddComponent<InfiniteAmmoComponent>();
                if (!Instance.Config.PreventCtBroadcasts)
                    ply.Broadcast(5, "Infinite ammo is enabled for you!");
                response = $"Infinite ammo enabled for Player \"{ply.Nickname}\"";
                return true;
            }

            CreativeToolboxEventHandler.PlayersWithInfiniteAmmo.Remove(ply);
            UnityEngine.Object.Destroy(infiniteAmmoComponent);
            if (!Instance.Config.PreventCtBroadcasts)
                ply.Broadcast(5, "Infinite ammo is disabled for you!");
            response = $"Infinite ammo disabled for Player \"{ply.Nickname}\"";
            return true;
        }
    }
}