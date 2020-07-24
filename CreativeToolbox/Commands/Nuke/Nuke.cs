using System;
using CommandSystem;
using Exiled.Permissions.Extensions;

namespace CreativeToolbox.Commands.Nuke
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class Nuke : ParentCommand
    {
        public Nuke() => LoadGeneratedCommands();

        public override string Command { get; } = "nuke";

        public override string[] Aliases { get; } = new string[] { };

        public override string Description { get; } = "Sets off the warhead at a specified time";

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new Instant());
            RegisterCommand(new Start());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
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
