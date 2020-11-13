namespace CreativeToolbox.Commands.Regen
{
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using System;
    using static CreativeToolbox;

    public class HealValue : ICommand
    {
        public string Command => "healvalue";

        public string[] Aliases => new string[0];

        public string Description => "Sets the amount of health that is gained per interval";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender as CommandSender).CheckPermission("ct.regen"))
            {
                response = "You do not have permission to run this command! Missing permission: \"ct.regen\"";
                return false;
            }

            if (arguments.Count != 1)
            {
                response = "Usage: regen healvalue (value)";
                return false;
            }

            if (!float.TryParse(arguments.At(0), out float healValue) || healValue < 0.05)
            {
                response = $"Invalid value for healing: {arguments.At(0)}";
                return false;
            }

            Instance.Config.RegenerationValue = healValue;
            if (!Instance.Config.PreventCtBroadcasts)
                Map.Broadcast(5, $"Players with regeneration will heal {healValue} HP per interval!");
            response = $"Players with regeneration will heal {healValue} HP per interval";
            return true;
        }
    }
}