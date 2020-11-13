namespace CreativeToolbox.Commands.AutoScale
{
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using System;
    using UnityEngine;
    using static CreativeToolbox;

    public class On : ICommand
    {
        public string Command => "on";

        public string[] Aliases => new string[0];

        public string Description => "Turns on auto scaling and sets players to a specified size";

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

            foreach (Player ply in Player.List)
            {
                ply.Scale = new Vector3(scalevar, scalevar, scalevar);
                CreativeToolboxEventHandler.PlayersWithRetainedScale.Add(ply.UserId);
            }

            if (!Instance.Config.DisableAutoScaleMessages)
                Map.Broadcast(5, $"Everyone has been set to {scalevar}x their normal size!");
            CreativeToolboxEventHandler.AutoScaleOn = true;
            response = $"Everyone has been set to {scalevar}x their normal size";
            return true;
        }
    }
}