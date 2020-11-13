namespace CreativeToolbox.Commands.InfAmmo
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

        public string Description => "Gives everyone infinite ammo";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender as CommandSender).CheckPermission("ct.infammo"))
            {
                response = "You do not have permission to run this command! Missing permission: \"ct.infammo\"";
                return false;
            }

            if (arguments.Count != 0)
            {
                response = "Usage: infammo all / *";
                return false;
            }

            foreach (Player ply in Player.List)
            {
                if (ply.ReferenceHub.GetComponent<InfiniteAmmoComponent>() != null)
                    continue;

                ply.ReferenceHub.gameObject.AddComponent<InfiniteAmmoComponent>();
                CreativeToolboxEventHandler.PlayersWithInfiniteAmmo.Add(ply);
            }

            if (!Instance.Config.PreventCtBroadcasts)
                Map.Broadcast(5, "Everyone has been given infinite ammo now!");
            response = "Everyone has been given infinite ammo";
            return true;
        }
    }
}