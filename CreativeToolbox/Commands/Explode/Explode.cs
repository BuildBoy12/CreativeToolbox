﻿namespace CreativeToolbox.Commands.Explode
{
    using CommandSystem;
    using Exiled.Permissions.Extensions;
    using System;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class Explode : ParentCommand
    {
        public Explode() => LoadGeneratedCommands();

        public override string Command => "explode";

        public override string[] Aliases => Array.Empty<string>();

        public override string Description => "Explodes a specified user or everyone instantly";

        public sealed override void LoadGeneratedCommands()
        {
            RegisterCommand(new All());
            RegisterCommand(new Person());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender,
            out string response)
        {
            if (!(sender as CommandSender).CheckPermission("ct.explode"))
            {
                response = "You do not have permission to run this command! Missing permission: \"ct.explode\"";
                return false;
            }

            response = "Please enter a valid subcommand! Available ones: all, person";
            return false;
        }
    }
}