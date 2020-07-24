using System;
using CommandSystem;
using Exiled.Permissions.Extensions;
using Exiled.API.Features;

namespace CreativeToolbox.Commands.Regen
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class Regen : ParentCommand
    {
        public Regen() => LoadGeneratedCommands();

        public override string Command { get; } = "regen";

        public override string[] Aliases { get; } = new string[] { };

        public override string Description { get; } = "Gives regeneration to players, clears regeneration from players, and shows who has regeneration";

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new All());
            RegisterCommand(new Clear());
            RegisterCommand(new Give());
            RegisterCommand(new HealTime());
            RegisterCommand(new HealValue());
            RegisterCommand(new List());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender as CommandSender).CheckPermission("ct.regen"))
            {
                response = "You do not have permission to run this command! Missing permission: \"ct.regen\"";
                return false;
            }

            response = "Please enter a valid subcommand! Available ones: all, clear, give, healtime, healvalue, list";
            return false;
        }
    }
}
