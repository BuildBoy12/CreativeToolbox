using System;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using UnityEngine;

namespace CreativeToolbox.Commands.AutoScale
{
    public class Off : ICommand
    {
        public string Command { get; } = "off";

        public string[] Aliases { get; } = new string[] { };

        public string Description { get; } = "Turns off auto scaling and resets players to their normal size";

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

            foreach (Player Ply in Player.List)
                Ply.Scale = Vector3.one;

            if (!CreativeToolbox.ConfigRef.Config.DisableAutoScaleMessages)
                Map.Broadcast(5, "Everyone has been restored to their normal size!");
            CreativeToolboxEventHandler.PlayersWithRetainedScale.Clear();
            CreativeToolboxEventHandler.AutoScaleOn = false;
            response = "Everyone has been restored to their normal size";
            return true;
        }
    }
}
