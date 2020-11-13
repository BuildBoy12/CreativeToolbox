namespace CreativeToolbox.Commands.AutoRespawn
{
    using CommandSystem;
    using Exiled.Permissions.Extensions;
    using System;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class AutoRespawn : ParentCommand
    {
        public AutoRespawn() => LoadGeneratedCommands();

        public override string Command => "autorespawn";

        public override string[] Aliases => new string[0];

        public override string Description =>
            "Lets you turn on/off respawning dead players as a random class and change respawn timers";

        public sealed override void LoadGeneratedCommands()
        {
            RegisterCommand(new Off());
            RegisterCommand(new On());
            RegisterCommand(new Time());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender,
            out string response)
        {
            if (!(sender as CommandSender).CheckPermission("ct.autorespawn"))
            {
                response = "You do not have permission to run this command! Missing permission: \"ct.autorespawn\"";
                return false;
            }

            response = "Please enter a valid subcommand! Available ones: on, off, time";
            return false;
        }
    }
}