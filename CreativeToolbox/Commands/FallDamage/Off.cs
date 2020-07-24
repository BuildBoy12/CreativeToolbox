using System;
using CommandSystem;
using Exiled.Permissions.Extensions;
using Exiled.API.Features;

namespace CreativeToolbox.Commands.FallDamage
{
    public class Off : ICommand
    {
        public string Command { get; } = "off";

        public string[] Aliases { get; } = new string[] { };

        public string Description { get; } = "Turns off fall damage";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender as CommandSender).CheckPermission("ct.falldamage"))
            {
                response = "You do not have permission to run this command! Missing permission: \"ct.falldamage\"";
                return false;
            }

            if (CreativeToolboxEventHandler.PreventFallDamage)
            {
                response = "Fall damage is already off";
                return false;
            }

            CreativeToolboxEventHandler.PreventFallDamage = true;
            Map.Broadcast(5, "Fall damage is off now!");
            response = "Fall damage is off";
            return true;
        }
    }
}
