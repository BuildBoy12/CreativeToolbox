using System;
using CommandSystem;
using Exiled.Permissions.Extensions;

namespace CreativeToolbox.Commands.Locate
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class Locate : ParentCommand
    {
        public Locate() => LoadGeneratedCommands();

        public override string Command { get; } = "locate";

        public override string[] Aliases { get; } = new string[] { };

        public override string Description { get; } = "Locates a user by their coordinates or which room they are in";

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new Room());
            RegisterCommand(new Xyz());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender as CommandSender).CheckPermission("ct.locate"))
            {
                response = "You do not have permission to run this command! Missing permission: \"ct.locate\"";
                return false;
            }

            response = "Please enter a valid subcommand! Available ones: xyz, room";
            return false;
        }
    }
}
