using System;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using UnityEngine;

namespace CreativeToolbox.Commands.AutoScale
{
    public class On : ICommand
    {
        public string Command { get; } = "on";

        public string[] Aliases { get; } = new string[] { };

        public string Description { get; } = "Turns on auto scaling and sets players to a specified size";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender as CommandSender).CheckPermission("ct.autoscale"))
            {
                response = "You do not have permission to run this command! Missing permission: \"ct.autoscale\"";
                return false;
            }

            if (arguments.Count != 1)
            {
                response = "Usage: autoscale on (value)";
                return false;
            }

            if (!float.TryParse(arguments.At(0), out float scalevar))
            {
                response = $"Invalid value for auto scaling value: {arguments.At(0)}";
                return false;
            }

            foreach (Player Ply in Player.List)
            {
                Ply.Scale = new Vector3(scalevar, scalevar, scalevar);
                CreativeToolboxEventHandler.PlayersWithRetainedScale.Add(Ply.UserId);
            }

            if (!CreativeToolbox.ConfigRef.Config.DisableAutoScaleMessages)
                Map.Broadcast(5, $"Everyone has been set to {scalevar}x their normal size!");
            CreativeToolboxEventHandler.AutoScaleOn = true;
            response = $"Everyone has been set to {scalevar}x their normal size";
            return true;
        }
    }
}
