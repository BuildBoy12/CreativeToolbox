namespace CreativeToolbox.Commands.InfAmmo
{
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using System;
    using static CreativeToolbox;

    public class Clear : ICommand
    {
        public string Command => "clear";

        public string[] Aliases => new string[0];

        public string Description => "Clears infinite ammo from everyone";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender as CommandSender).CheckPermission("ct.infammo"))
            {
                response = "You do not have permission to run this command! Missing permission: \"ct.infammo\"";
                return false;
            }

            if (arguments.Count != 0)
            {
                response = "Usage: infammo clear";
                return false;
            }

            foreach (Player ply in Player.List)
                if (ply.ReferenceHub.TryGetComponent(out InfiniteAmmoComponent infiniteAmmoComponent))
                    UnityEngine.Object.Destroy(infiniteAmmoComponent);

            CreativeToolboxEventHandler.PlayersWithInfiniteAmmo.Clear();
            if (!Instance.Config.PreventCtBroadcasts)
                Map.Broadcast(5, "Infinite ammo is taken away from everyone now!");
            response = "Infinite ammo has been taken away from everyone";
            return true;
        }
    }
}