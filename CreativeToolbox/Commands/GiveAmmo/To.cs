namespace CreativeToolbox.Commands.GiveAmmo
{
    using CommandSystem;
    using Exiled.API.Enums;
    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using System;

    public class To : ICommand
    {
        public string Command => "to";

        public string[] Aliases => Array.Empty<string>();

        public string Description => "Gives the specified user a specified amount of ammo for a given ammo type";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender as CommandSender).CheckPermission("ct.giveammo"))
            {
                response = "You do not have permission to run this command! Missing permission: \"ct.giveammo\"";
                return false;
            }

            if (arguments.Count != 3)
            {
                response = "Usage: giveammo to (player id / name) (AmmoType) (amount)";
                return false;
            }

            Player ply = Player.Get(arguments.At(0));
            if (ply == null)
            {
                response = $"Player \"{arguments.At(0)}\" not found";
                return false;
            }

            if (ply.Role.GetTeam() == Team.SCP || ply.Role == RoleType.Spectator)
            {
                response = $"Player \"{arguments.At(0)}\" is not a valid class to give ammo to";
                return false;
            }

            if (!Enum.TryParse(arguments.At(1), true, out AmmoType ammo))
            {
                response = $"Invalid ammo type: {arguments.At(1)}";
                return false;
            }

            if (!uint.TryParse(arguments.At(2), out uint ammoAmount))
            {
                response = $"Invalid ammo amount: {arguments.At(2)}";
                return false;
            }

            ply.ReferenceHub.ammoBox[(int) ammo] = ply.ReferenceHub.ammoBox[(int) ammo] + ammoAmount;
            ply.Broadcast(5, $"You have been given {ammoAmount} of {ammo.ToString()} ammo!");
            response = $"Player \"{ply.Nickname}\" has been given {ammoAmount} of {ammo.ToString()} ammo";
            return true;
        }
    }
}