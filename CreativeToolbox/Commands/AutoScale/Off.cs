namespace CreativeToolbox.Commands.AutoScale
{
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using System;
    using UnityEngine;
    using static CreativeToolbox;

    public class Off : ICommand
    {
        public string Command => "off";

        public string[] Aliases => Array.Empty<string>();

        public string Description => "Turns off auto scaling and resets players to their normal size";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender as CommandSender).CheckPermission("ct.autoscale"))
            {
                response = "You do not have permission to run this command! Missing permission: \"ct.autoscale\"";
                return false;
            }

            if (arguments.Count != 0)
            {
                response = "Usage: autoscale off";
                return false;
            }

            if (!CreativeToolboxEventHandler.AutoScaleOn)
            {
                response = "Auto scaling is already off";
                return false;
            }

            foreach (Player ply in Player.List)
                ply.Scale = Vector3.one;

            if (!Instance.Config.DisableAutoScaleMessages)
                Map.Broadcast(5, "Everyone has been restored to their normal size!");
            CreativeToolboxEventHandler.PlayersWithRetainedScale.Clear();
            CreativeToolboxEventHandler.AutoScaleOn = false;
            response = "Everyone has been restored to their normal size";
            return true;
        }
    }
}