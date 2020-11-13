namespace CreativeToolbox.Commands.GiveAmmo
{
    using CommandSystem;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using System;

    public class All : ICommand
    {
        public string Command => "all";

        public string[] Aliases => new[] {"*"};

        public string Description => "Gives all users a specified amount of a given ammo type";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender as CommandSender).CheckPermission("ct.giveammo"))
            {
                response = "You do not have permission to run this command! Missing permission: \"ct.giveammo\"";
                return false;
            }

            if (arguments.Count != 2)
            {
                response = "Usage: giveammo all / * (AmmoType) (amount)";
                return false;
            }

            if (!Enum.TryParse(arguments.At(0), true, out AmmoType ammo))
            {
                response = $"Invalid ammo type: {arguments.At(0)}";
                return false;
            }

            if (!uint.TryParse(arguments.At(1), out uint ammoAmount))
            {
                response = $"Invalid ammo amount: {arguments.At(1)}";
                return false;
            }

            foreach (Player ply in Player.List)
            {
                if (ply.Team == Team.SCP || ply.Team == Team.RIP)
                    continue;

                ply.ReferenceHub.ammoBox[(int) ammo] = ply.ReferenceHub.ammoBox[(int) ammo] + ammoAmount;
            }

            Map.Broadcast(5, $"Everyone has been given {ammoAmount} of {ammo.ToString()} ammo!");
            response = $"Everyone has been given {ammoAmount} of {ammo.ToString()} ammo";
            return true;
        }
    }
}