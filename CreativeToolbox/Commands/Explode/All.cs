namespace CreativeToolbox.Commands.Explode
{
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using System;

    public class All : ICommand
    {
        public string Command => "all";

        public string[] Aliases => new[] {"*"};

        public string Description => "Explodes everyone instantly";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender as CommandSender).CheckPermission("ct.explode"))
            {
                response = "You do not have permission to run this command! Missing permission: \"ct.explode\"";
                return false;
            }

            if (arguments.Count != 0)
            {
                response = "Usage: explode all / *";
                return false;
            }

            foreach (Player ply in Player.List)
            {
                if (ply.Role == RoleType.Spectator || ply.Role == RoleType.None)
                    continue;

                ply.Kill();
                CreativeToolboxEventHandler.SpawnGrenadeOnPlayer(ply, false);
            }

            response = "Everyone exploded, Hubert cannot believe you have done this";
            return true;
        }
    }
}