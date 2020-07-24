using System;
using CommandSystem;
using Exiled.Permissions.Extensions;
using Exiled.API.Features;

namespace CreativeToolbox.Commands.PryGates
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class PryGates : ParentCommand
    {
        public PryGates() => LoadGeneratedCommands();

        public override string Command { get; } = "prygates";

        public override string[] Aliases { get; } = new string[] { };

        public override string Description { get; } = "Gives the ability to pry gates to players, clear the ability from players, and shows who has the ability";

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new All());
            RegisterCommand(new Clear());
            RegisterCommand(new Give());
            RegisterCommand(new List());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender as CommandSender).CheckPermission("ct.prygates"))
            {
                response = "You do not have permission to run this command! Missing permission: \"ct.prygates\"";
                return false;
            }

            response = "Please enter a valid subcommand! Available ones: all, clear, give, list";
            return false;
        }
    }
}
