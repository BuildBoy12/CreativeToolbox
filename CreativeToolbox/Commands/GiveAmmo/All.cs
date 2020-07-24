using System;
using CommandSystem;
using Exiled.Permissions.Extensions;
using Exiled.API.Enums;
using Exiled.API.Features;

namespace CreativeToolbox.Commands.GiveAmmo
{
    class All : ICommand
    {
        public string Command { get; } = "all";

        public string[] Aliases { get; } = { "*" };

        public string Description { get; } = "Gives all users a specified amount of a given ammo type";

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

            if (!Enum.TryParse(arguments.At(0), out AmmoType Ammo))
            {
                response = $"Invalid ammo type: {arguments.At(0)}";
                return false;
            }

            if (!int.TryParse(arguments.At(1), out int AmmoAmount) || AmmoAmount < 0)
            {
                response = $"Invalid ammo amount: {arguments.At(1)}";
                return false;
            }

            foreach (Player Ply in Player.List)
            {
                if (Ply.Role.IsNotHuman(false))
                    continue;
                
                Ply.SetAmmo(Ammo, (uint)(Ply.GetAmmo(Ammo) + AmmoAmount));
            }

            Map.Broadcast(5, $"Everyone has been given {AmmoAmount} of {Ammo.ToString()} ammo!");
            response = $"Everyone has been given {AmmoAmount} of {Ammo.ToString()} ammo";
            return true;
        }
    }
}
