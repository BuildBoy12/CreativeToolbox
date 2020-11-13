namespace CreativeToolbox.Commands.Nuke
{
    using CommandSystem;
    using Exiled.Permissions.Extensions;
    using System;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class Nuke : ParentCommand
    {
        public Nuke() => LoadGeneratedCommands();

        public override string Command => "nuke";

        public override string[] Aliases => Array.Empty<string>();

        public override string Description => "Sets off the warhead at a specified time";

        public sealed override void LoadGeneratedCommands()
        {
            RegisterCommand(new Instant());
            RegisterCommand(new Start());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender,
            out string response)
        {
            if (!(sender as CommandSender).CheckPermission("ct.nuke"))
            {
                response = "You do not have permission to run this command! Missing permission: \"ct.nuke\"";
                return false;
            }

            response = "Please enter a valid subcommand! Available ones: instant, start";
            return false;
        }
    }
}