using System;
using CommandSystem;
using Exiled.Permissions.Extensions;
using Exiled.API.Features;

namespace CreativeToolbox.Commands.Regen
{
    public class HealValue : ICommand
    {
        public string Command { get; } = "healvalue";

        public string[] Aliases { get; } = new string[] { };

        public string Description { get; } = "Sets the amount of health that is gained per interval";

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

            if (!float.TryParse(arguments.At(0), out float healvalue) || healvalue < 0.05)
            {
                response = $"Invalid value for healing: {arguments.At(0)}";
                return false;
            }

            CreativeToolbox.ConfigRef.Config.RegenerationValue = healvalue;
            if (!CreativeToolbox.ConfigRef.Config.PreventCtBroadcasts)
                Map.Broadcast(5, $"Players with regeneration will heal {healvalue} HP per interval!");
            response = $"Players with regeneration will heal {healvalue} HP per interval";
            return true;
        }
    }
}
