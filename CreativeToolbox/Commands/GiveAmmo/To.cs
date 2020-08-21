using System;
using CommandSystem;
using Exiled.Permissions.Extensions;
using Exiled.API.Enums;
using Exiled.API.Features;

namespace CreativeToolbox.Commands.GiveAmmo
{
    public class To : ICommand
    {
        public string Command { get; } = "to";

        public string[] Aliases { get; } = new string[] { };

        public string Description { get; } = "Gives the specified user a specified amount of ammo for a given ammo type";

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

            Player Plyr = Player.Get(arguments.At(0));
            if (Plyr == null)
            {
                response = $"Player \"{arguments.At(0)}\" not found";
                return false;
            }
            else if (Plyr.Role.IsNotHuman(false))
            {
                response = $"Player \"{arguments.At(0)}\" is not a valid class to give ammo to";
                return false;
            }

            if (!Enum.TryParse(arguments.At(1), true, out AmmoType Ammo))
            {
                response = $"Invalid ammo type: {arguments.At(1)}";
                return false;
            }

            if (!uint.TryParse(arguments.At(2), out uint AmmoAmount))
            {
                response = $"Invalid ammo amount: {arguments.At(2)}";
                return false;
            }

            Plyr.ReferenceHub.ammoBox[(int)Ammo] = Plyr.ReferenceHub.ammoBox[(int)Ammo] + AmmoAmount;
            Plyr.Broadcast(5, $"You have been given {AmmoAmount} of {Ammo.ToString()} ammo!");
            response = $"Player \"{Plyr.Nickname}\" has been given {AmmoAmount} of {Ammo.ToString()} ammo";
            return true;
        }
    }
}
