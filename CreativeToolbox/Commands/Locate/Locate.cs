namespace CreativeToolbox.Commands.Locate
{
    using CommandSystem;
    using Exiled.Permissions.Extensions;
    using System;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class Locate : ParentCommand
    {
        public Locate() => LoadGeneratedCommands();

        public override string Command => "locate";

        public override string[] Aliases => new string[0];

        public override string Description => "Locates a user by their coordinates or which room they are in";

        public sealed override void LoadGeneratedCommands()
        {
            RegisterCommand(new Room());
            RegisterCommand(new Xyz());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender,
            out string response)
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