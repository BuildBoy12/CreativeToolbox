using System;
using CommandSystem;
using Exiled.Permissions.Extensions;


namespace CreativeToolbox.Commands.FallDamage
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class FallDamage : ParentCommand
    {
        public FallDamage() => LoadGeneratedCommands();

        public override string Command { get; } = "falldamage";

        public override string[] Aliases { get; } = new string[] { };

        public override string Description { get; } = "Turns on/off fall damage";

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new Off());
            RegisterCommand(new On());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender as CommandSender).CheckPermission("ct.falldamage"))
            {
                response = "You do not have permission to run this command! Missing permission: \"ct.falldamage\"";
                return false;
            }

            if (!CreativeToolbox.ConfigRef.Config.EnableFallDamagePrevention)
            {
                response = "You cannot modify fall damage as the setting is disabled";
                return false;
            }

            response = "Please enter a valid subcommand! Available ones: frag, flash";
            return false;
        }
    }
}
