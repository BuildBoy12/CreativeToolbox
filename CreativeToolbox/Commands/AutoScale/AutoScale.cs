﻿namespace CreativeToolbox.Commands.AutoScale
{
    using CommandSystem;
    using Exiled.Permissions.Extensions;
    using System;
    
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class AutoScale : ParentCommand
    {
        public AutoScale() => LoadGeneratedCommands();

        public override string Command => "autoscale";

        public override string[] Aliases => Array.Empty<string>();

        public override string Description => "Automatically scales players to a specified scale factor or resets their scale";

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new Off());
            RegisterCommand(new On());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender as CommandSender).CheckPermission("ct.autoscale"))
            {
                response = "You do not have permission to run this command! Missing permission: \"ct.autoscale\"";
                return false;
            }

            response = "Please enter a valid subcommand! Available ones: on, off";
            return false;
        }
    }
}
