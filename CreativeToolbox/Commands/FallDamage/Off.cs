namespace CreativeToolbox.Commands.FallDamage
{
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using System;
    using static CreativeToolbox;

    public class Off : ICommand
    {
        public string Command => "off";

        public string[] Aliases => new string[0];

        public string Description => "Turns off fall damage";

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
            if (!Instance.Config.PreventCtBroadcasts)
                Map.Broadcast(5, "Fall damage is off now!");
            response = "Fall damage is off";
            return true;
        }
    }
}